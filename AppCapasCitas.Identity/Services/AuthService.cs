using AppCapasCitas.Application.Contracts.Persistence.Identity;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.Identity.Data;
using AppCapasCitas.Identity.Models;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.Transversal.Common.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ValidationFailureFluent = FluentValidation.Results.ValidationFailure;

namespace AppCapasCitas.Identity.Services;

/// <summary>
/// Servicio de autenticación que implementa la interfaz IAuthService.
/// Proporciona funcionalidades para registro, login, gestión de tokens JWT,
/// manejo de roles y operaciones CRUD de usuarios.
/// </summary>
public class AuthService : IAuthService
{
    // Managers de Identity para operaciones con usuarios y roles
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    // Configuración para generación de tokens JWT
    private readonly JwtSettings _jwtSettings;

    // Contexto de base de datos para operaciones con refresh tokens
    private readonly CleanArchitectureIdentityDbContext _context;

    // Parámetros para validación de tokens
    private readonly TokenValidationParameters _tokenValidationParameters;
    // Logger para registrar eventos y errores
    private readonly IAppLogger<AuthService> _appLogger;


    /// <summary>
    /// Constructor del servicio de autenticación
    /// </summary>
    /// <param name="userManager">Manager para operaciones con usuarios</param>
    /// <param name="signInManager">Manager para procesos de login</param>
    /// <param name="jwtSettings">Configuración de JWT</param>
    /// <param name="context">Contexto de base de datos</param>
    /// <param name="tokenValidationParameters">Parámetros para validación de tokens</param>
    /// <param name="roleManager">Manager para operaciones con roles</param>
    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<JwtSettings> jwtSettings,
        CleanArchitectureIdentityDbContext context,
        TokenValidationParameters tokenValidationParameters,
        RoleManager<ApplicationRole> roleManager,
        IAppLogger<AuthService> appLogger)


    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
        _context = context;
        _tokenValidationParameters = tokenValidationParameters;
        _roleManager = roleManager;
        _appLogger = appLogger;

        // Verifica y crea los roles básicos al inicializar el servicio
        VerifyRoles().Wait();
    }


    /// <summary>
    /// Autentica a un usuario y genera tokens de acceso
    /// </summary>
    /// <param name="request">Credenciales del usuario (email y password)</param>
    /// <returns>Respuesta con datos del usuario y tokens</returns>
    /// <exception cref="Exception">Cuando las credenciales son inválidas</exception>
    public async Task<Response<AuthResponse>> Login(AuthRequest request)
    {
        var response = new Response<AuthResponse>();
        try
        {
            // Busca usuario por email
            var applicationUser = await _userManager.FindByEmailAsync(request.Email);

            if (applicationUser == null)
            {
                response.IsSuccess = false;
                response.Message = "El usuario no existe";
                return response;
            }
            if (applicationUser.Active == false)
            {
                response.IsSuccess = false;
                response.Message = "El usuario está inactivo";
                return response;
            }
            //recuperar Roles del usuario
            var roles = await _userManager.GetRolesAsync(applicationUser);

            // Intenta autenticar al usuario
            var resultado = await _signInManager.PasswordSignInAsync(
                applicationUser.UserName!,
                request.Password,
                request.RememberMe,
                lockoutOnFailure: false
                );
            //agregar fecha de último login
            applicationUser.LastLogin = DateTime.UtcNow;
            _context.Users!.Update(applicationUser);
            await _context.SaveChangesAsync();

            if (!resultado.Succeeded)
            {
                response.IsSuccess = false;
                response.Message = "Las credenciales son inválidas";
                return response;
            }

            // Genera tokens JWT y refresh token
            var token = await GenerateToken(applicationUser);

            // Construye respuesta con datos del usuario

            response.IsSuccess = true;
            response.Message = "Login exitoso";
            response.Data = new AuthResponse
            {
                Id = Guid.Parse(applicationUser.Id),
                Token = token.Item1,      // Nuevo token JWT
                Email = applicationUser.Email!,
                Username = applicationUser.UserName!,
                RefreshToken = token.Item2, // Nuevo refresh token
                Active = applicationUser.Active,
                Roles = roles.ToList() // Roles del usuario,
            };
        }
        catch (Exception ex)
        {
            var st = new System.Diagnostics.StackTrace(true);
            var frame = st.GetFrame(0); // Frame actual
            var className = frame!.GetMethod()!.DeclaringType!.FullName;
            var lineNumber = frame.GetFileLineNumber();
            string errorMessage = $"Error en la Linea: {lineNumber} -> {ex.Message} ";

            _appLogger.LogError(ex.Message, ex);
            response.Message = "Ocurrió un error, revisar detalle.";
            response.IsSuccess = false;
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent(className, errorMessage)
            };
        }
        return response;
    }

    /// <summary>
    /// Renueva los tokens de acceso usando un refresh token válido
    /// </summary>
    /// <param name="tokenRequest">Token JWT expirado y refresh token</param>
    /// <returns>Nuevos tokens o errores si la validación falla</returns>
    public async Task<Response<AuthResponse>> RefreshToken(TokenRequest tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var response = new Response<AuthResponse>();

        // Clona los parámetros de validación desactivando la validación de tiempo
        var tokenValidationParamsClone = _tokenValidationParameters.Clone();
        tokenValidationParamsClone.ValidateLifetime = false;

        try
        {
            // Valida el token JWT (aunque esté expirado)
            var tokenVerification = jwtTokenHandler.ValidateToken(
                tokenRequest.Token,
                tokenValidationParamsClone,
                out var validatedToken);

            // Verifica el algoritmo de encriptación
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);

                if (!result)
                {
                    response.IsSuccess = false;
                    response.Message = "El token tiene errores de encriptación";
                    return response;
                }
            }

            // Obtiene fecha de expiración del token
            var utcExpiryDate = long.Parse(tokenVerification.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            // Verifica que el token esté expirado (no debería poder refrescarse si aún es válido)
            if (expiryDate > DateTime.UtcNow)
            {

                response.IsSuccess = false;
                response.Message = "El token no ha expirado";
                return response;
            }

            // Busca el refresh token en la base de datos
            var storedToken = await _context.RefreshTokens!
                .FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedToken is null)
            {
                response.IsSuccess = false;
                response.Message = "El refresh token no existe";
                return response;

            }

            // Verifica que el refresh token no haya sido usado
            if (storedToken.IsUsed)
            {
                response.IsSuccess = false;
                response.Message = "El token ya fue usado";
                return response;
            }

            // Verifica que el refresh token no haya sido revocado
            if (storedToken.IsRevoked)
            {
                response.IsSuccess = false;
                response.Message = "El token fue revocado";
                return response;
            }

            // Verifica que el JWT ID coincida con el almacenado en el refresh token
            var jti = tokenVerification.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;

            if (storedToken.JwtId != jti)
            {
                response.IsSuccess = false;
                response.Message = "El token no concuerda con el valor inicial";
                return response;

            }

            // Verifica que el refresh token no haya expirado
            if (storedToken.ExpireDate < DateTime.UtcNow)
            {
                response.IsSuccess = false;
                response.Message = "El refresh token ha expirado";
                return response;
            }

            // Marca el refresh token como usado
            storedToken.IsUsed = true;
            _context.RefreshTokens!.Update(storedToken);
            await _context.SaveChangesAsync();

            // Genera nuevos tokens para el usuario
            var user = await _userManager.FindByIdAsync(storedToken.UserId!);
            var token = await GenerateToken(user!);


            response.IsSuccess = true;
            response.Data = new AuthResponse
            {
                Id = Guid.Parse(user!.Id),
                Token = token.Item1,      // Nuevo token JWT
                Email = user.Email!,
                Username = user.Email!,
                RefreshToken = token.Item2, // Nuevo refresh token
            };

        }
        catch (Exception ex)
        {
            // Manejo específico para tokens expirados
            if (ex.Message.Contains("Lifetime validation failed. The token is expired"))
            {
                _appLogger.LogError(ex.Message, ex);
                response.IsSuccess = false;
                response.Message = "El token ha expirado por favor tienes que realizar otra vez el login.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "El token tiene errores tienes que realizar otra vez el login.";
            }
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent("Token", ex.Message)
            };
        }
        return response;
    }

    /// <summary>
    /// Convierte un timestamp Unix a DateTime
    /// </summary>
    /// <param name="unixTimeStamp">Timestamp en formato Unix</param>
    /// <returns>Fecha y hora correspondiente</returns>
    private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
        return dateTimeVal;
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema
    /// </summary>
    /// <param name="request">Datos del nuevo usuario</param>
    /// <returns>Respuesta con datos del usuario registrado</returns>
    /// <exception cref="Exception">Cuando el username/email ya existe o hay errores en el registro</exception>
    public async Task<Response<RegistrationResponse>> Register(RegistrationRequest request)
    {
        var response = new Response<RegistrationResponse>();
        try
        {
            // Verifica que el username no esté en uso
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
            {
                response.IsSuccess = false;
                response.Message = "El username ya fue tomado por otra cuenta";
                return response;
            }
            // Verifica que el email no esté en uso
            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                response.IsSuccess = false;
                response.Message = "El email ya fue tomado por otra cuenta";
                return response;
            }
            //verifica si el telefono ya existe
            // var existingPhone = await _userManager.Users
            //     .FirstOrDefaultAsync(u => u.PhoneNumber == request.Celular);
            // if (existingPhone != null)
            // {
            //     response.IsSuccess = false;
            //     response.Message = "El Celular ya está asociado a otra cuenta";
            //     return response;
            // }
            ApplicationRole? role = new();
            // Asigna rol específico si se proporcionó, o rol por defecto "Registrado"
            if (!string.IsNullOrEmpty(request.RoleId))
            {
                role = await _roleManager.FindByIdAsync(request.RoleId);
                if (role == null)
                {
                    response.IsSuccess = false;
                    response.Message = "El rol con id {request.RoleId} no existe";
                    return response;
                }
            }
            else
            {
                role = await _roleManager.FindByNameAsync("Registrado");
                if (role == null)
                {
                    response.IsSuccess = false;
                    response.Message = "El rol con nombre Registrado no existe";
                    return response;
                }
            }
            // Crea el nuevo usuario
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Username,
                EmailConfirmed = false, // Por defecto, el email no está confirmado
                Active = true, // Por defecto, el usuario está activo
                PhoneNumber = request.Celular

            };
            // Intenta crear el usuario con la contraseña proporcionada
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                // Asigna el rol al usuario
                var roleResult = await _userManager.AddToRoleAsync(user, role.Name!);

                if (!roleResult.Succeeded)
                {
                    response.IsSuccess = false;
                    response.Message = $"Error al asignar el rol al usuario: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}";
                    response.Errors = roleResult.Errors.Select(e => new ValidationFailureFluent("Role", e.Description));
                    return response;
                }
                // Genera tokens para el nuevo usuario
                var token = await GenerateToken(user);
                response.IsSuccess = true;
                response.Data = new RegistrationResponse
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    Token = token.Item1,          // Token JWT
                    Username = user.UserName,
                    RefreshToken = token.Item2,     // Refresh token
                    RoleId = role.Id,
                    RoleName = role.Name!,
                };
            }
        }
        catch (Exception ex)
        {
            var st = new System.Diagnostics.StackTrace(true);
            var frame = st.GetFrame(0); // Frame actual
            var className = frame!.GetMethod()!.DeclaringType!.FullName;
            var lineNumber = frame.GetFileLineNumber();
            string errorMessage = $"Error en la Linea: {lineNumber} -> {ex.InnerException!.Message} ";
            _appLogger.LogError(ex.Message, ex);
            response.Message = "Ocurrió un error, revisar detalle.";
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent(className, errorMessage)
            };
        }
        return response;
    }
    /// <summary>
    /// Reset password de un usuario
    /// <param name="request">Datos del usuario y nueva contraseña</param>
    /// <returns>Respuesta con éxito o error</returns>
    /// <exception cref="Exception">Cuando el usuario no existe o hay errores al cambiar la contraseña</exception>
    /// </summary>
    
    public async Task<Response<bool>> ResetPassword(AuthResetPasswordRequest request)
    {
        var response = new Response<bool>();
        try
        {
            // Busca el usuario por ID
            var user = await _userManager.FindByIdAsync(request.UsuarioId.ToString());
            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "El usuario no existe";
                return response;
            }
            // Verifica que la contraseña antigua sea correcta
            var passwordCheck = await _userManager.CheckPasswordAsync(user, request.OldPassword);
            if (!passwordCheck)
            {
                response.IsSuccess = false;
                response.Message = "La contraseña antigua es incorrecta";
                return response;
            }

            // Verifica que la nueva contraseña sea válida
            if (string.IsNullOrEmpty(request.NewPassword) || request.NewPassword.Length < 6)
            {
                response.IsSuccess = false;
                response.Message = "La nueva contraseña debe tener al menos 6 caracteres";
                return response;
            }

            // Verifica que las contraseñas coincidan
            if (request.NewPassword != request.ConfirmNewPassword)
            {
                response.IsSuccess = false;
                response.Message = "Las contraseñas no coinciden";
                return response;
            }
            if (!string.IsNullOrEmpty(request.OldPassword))
            {
                var changeResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                if (!changeResult.Succeeded)
                {
                    response.IsSuccess = false;
                    response.Message = $"Error al cambiar la contraseña: {string.Join(", ", changeResult.Errors.Select(e => e.Description))}";
                    return response;
                }
           }
            else
            {
                //Validar si se mantiene funcionalidad
                // Si no se proporciona contraseña antigua, se establece la nueva directamente
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    response.IsSuccess = false;
                    response.Message = $"Error al actualizar el usuario: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}";
                    return response;
                }
            }
            // Actualiza el usuario con la nueva contraseña
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);
            await _userManager.UpdateAsync(user);

            response.IsSuccess = true;
            response.Data = true; // Indica que la operación fue exitosa
        }
        catch (Exception ex)
        {
            var st = new System.Diagnostics.StackTrace(true);
            var frame = st.GetFrame(0); // Frame actual
            var className = frame!.GetMethod()!.DeclaringType!.FullName;
            var lineNumber = frame.GetFileLineNumber();
            string errorMessage = $"Error en la Linea: {lineNumber} -> {ex.InnerException?.Message} ";
            _appLogger.LogError(ex.Message, ex);
            response.Message = "Ocurrió un error, revisar detalle.";
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent(className, errorMessage)
            };
        }
        return response;
    }

    /// <summary>
    /// Asigna un rol a un usuario existente
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="roleId">ID del rol a asignar</param>
    /// <returns>True si la operación fue exitosa</returns>
    /// <exception cref="Exception">Cuando el usuario o rol no existen</exception>
    public async Task<Response<bool>> AssignRoleToUser(string userId, string roleId)
    {
        var response = new Response<bool>();
        try
        {
            // Busca el usuario
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception($"El usuario con id {userId} no existe");
            }

            // Busca el rol
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new Exception($"El rol con id {roleId} no existe");
            }

            // Asigna el rol al usuario
            var resultAgregarRol = await _userManager.AddToRoleAsync(user, role.Name!);

            response.IsSuccess = resultAgregarRol.Succeeded ? true : false;
            response.Data = resultAgregarRol.Succeeded;
            response.Message = resultAgregarRol.Succeeded ? "Rol asignado correctamente" : "Error al asignar el rol";

        }
        catch (Exception ex)
        {
            var st = new System.Diagnostics.StackTrace(true);
            var frame = st.GetFrame(0); // Frame actual
            var className = frame!.GetMethod()!.DeclaringType!.FullName;
            var lineNumber = frame.GetFileLineNumber();
            string errorMessage = $"Error en la Linea: {lineNumber} -> {ex.Message} ";
            _appLogger.LogError(ex.Message, ex);
            response.Message = "Ocurrió un error, revisar detalle.";
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent(className, errorMessage)
            };
        }
        return response;

    }

    /// <summary>
    /// Elimina un usuario del sistema
    /// </summary>
    /// <param name="userId">ID del usuario a eliminar</param>
    /// <returns>True si la operación fue exitosa</returns>
    /// <exception cref="Exception">Cuando el usuario no existe o hay errores al eliminarlo</exception>
    public async Task<Response<bool>> DeleteUser(string userId)
    {
        var response = new Response<bool>();
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception($"El usuario con id {userId} no existe");
            }

            // Eliminar el refresh token asociado al usuario
            var refreshTokens = await _context.RefreshTokens!
                .Where(x => x.UserId == userId)
                .ToListAsync();
            // Eliminar los refresh tokens asociados al usuario
            if (refreshTokens != null)
            {
                _context.RefreshTokens!.RemoveRange(refreshTokens);
                await _context.SaveChangesAsync();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                response.IsSuccess = true;
                response.Message = "Usuario eliminado correctamente";
            }
        }
        catch (Exception ex)
        {
            var st = new System.Diagnostics.StackTrace(true);
            var frame = st.GetFrame(0); // Frame actual
            var className = frame!.GetMethod()!.DeclaringType!.FullName;
            var lineNumber = frame.GetFileLineNumber();
            string errorMessage = $"Error en la Linea: {lineNumber} -> {ex.Message} ";
            _appLogger.LogError(ex.Message, ex);
            response.Message = "Ocurrió un error, revisar detalle.";
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent(className, errorMessage)
            };
        }
        return response;
    }



    /// <summary>
    /// Cierra la sesión de un usuario revocando su refresh token
    /// </summary>
    /// <param name="request">Datos del token a revocar</param>
    /// <returns>Resultado de la operación</returns>

    public async Task<Response<bool>> Logout(LogoutRequest request)
    {
        var response = new Response<bool>();
        try
        {
            // 1. Buscar el refresh token en la base de datos
            var storedToken = await _context.RefreshTokens!
                .FirstOrDefaultAsync(x => x.Token == request.Token &&
                                    x.UserId == request.UserId.ToString() &&
                                    x.IsRevoked == false
                                    );

            if (storedToken == null)
            {
                response.IsSuccess = false;
                response.Message = "Token de refresco no encontrado o ya fue revocado.";
                return response;
            }

            // 2. Revocar el refresh token (marcar como usado y revocado)
            storedToken.IsUsed = true;
            storedToken.IsRevoked = true;
            _context.RefreshTokens!.Update(storedToken);
            await _context.SaveChangesAsync();

            // 3. Cerrar sesión del usuario (opcional, depende de tu flujo)
            await _signInManager.SignOutAsync();
            response.IsSuccess = true;
            response.Data = true;
            response.Message = "Logout exitoso. Token revocado.";
        }
        catch (Exception ex)
        {
            var st = new System.Diagnostics.StackTrace(true);
            var frame = st.GetFrame(0); // Frame actual
            var className = frame!.GetMethod()!.DeclaringType!.FullName;
            var lineNumber = frame.GetFileLineNumber();
            string errorMessage = $"Error en la Linea: {lineNumber} -> {ex.Message} ";
            _appLogger.LogError(ex.Message, ex);
            response.Message = "Ocurrió un error, revisar detalle.";
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent(className, errorMessage)
            };
        }
        return response;

    }

    /// <summary>
    /// Genera un token JWT y un refresh token para un usuario
    /// </summary>
    /// <param name="user">Usuario para el que se generarán los tokens</param>
    /// <returns>Tupla con token JWT y refresh token</returns>

    private async Task<Tuple<string, string>> GenerateToken(ApplicationUser user)
    {
        // 1. INICIALIZACIÓN
        // Manejador para crear y escribir tokens JWT
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        // Crear clave de seguridad usando la clave secreta de configuración
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        // 2. OBTENER CLAIMS (ATRIBUTOS) DEL USUARIO
        // Obtener todos los claims personalizados del usuario
        var userClaims = await _userManager.GetClaimsAsync(user);
        // Obtener todos los roles del usuario
        var roles = await _userManager.GetRolesAsync(user);


        // 3. PREPARAR CLAIMS DE ROLES
        var roleClaims = new List<Claim>();
        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role)); // Convertir roles a claims
        }

        // 4. CONSTRUIR EL TOKEN JWT
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            // Combinar tres tipos de claims:
            Subject = new ClaimsIdentity(new[]
                {
                    new Claim("uid", user.Id),
                    new Claim("email", user.Email!),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),  // Email
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email!),  // Subject
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  // ID único del token
                    new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
           
                }
                .Union(userClaims)  // Claims personalizados
                .Union(roleClaims)),  // Claims de roles

            // Tiempo de expiración (de configuración)
            Expires = DateTime.UtcNow.Add(_jwtSettings.ExpireTime),
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            // Credenciales de firma (algoritmo HMAC-SHA256)
            SigningCredentials = new SigningCredentials(
                symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256)
        };

        // 5. GENERAR TOKEN DE ACCESO (JWT)
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token); // Token como string

        // 6. GENERAR REFRESH TOKEN
        var refreshToken = new RefreshToken
        {
            JwtId = token.Id,  // Relación con el JWT
            IsUsed = false,    // No usado aún
            IsRevoked = false, // No revocado
            UserId = user.Id,  // Dueño del token
            CreateDate = DateTime.UtcNow,  // Fecha creación
            ExpireDate = DateTime.UtcNow.AddMonths(6),  // Expira en 6 meses
            // Token complejo (caracteres aleatorios + GUID)
            Token = $"{GenerateRandomTokenCharacters(35)}{Guid.NewGuid()}"
        };

        // 7. GUARDAR REFRESH TOKEN EN BD
        await _context.RefreshTokens!.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        // 8. RETORNAR AMBOS TOKENS
        return new Tuple<string, string>(jwtToken, refreshToken.Token);
    }


    /// <summary>
    /// Genera una cadena aleatoria de caracteres para usar en tokens
    /// </summary>
    /// <param name="length">Longitud de la cadena a generar</param>
    /// <returns>Cadena aleatoria</returns>
    private string GenerateRandomTokenCharacters(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// Guarda los cambios en el contexto de base de datos
    /// </summary>
    /// <returns>Número de registros afectados</returns>
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Obtiene todos los roles disponibles en el sistema
    /// </summary>
    /// <returns>Lista de roles</returns>
    public async Task<Response<List<Role>>> GetRoles()
    {
        var response = new Response<List<Role>>();
        var rolesBd = await _roleManager.Roles.ToListAsync();
        var roles = rolesBd.Select(role => new Role
        {
            Id = Guid.Parse(role.Id),
            Name = role.Name,
            Prioridad = role.Prioridad,
            Orden = role.Orden,
            Activo = role.Activo
        })
        .Where(role => role.Activo == true)
        .OrderBy(role => role.Orden)
        .ThenBy(role => role.Prioridad)
        .ToList();
        response.IsSuccess = true;
        response.Data = roles;
        response.Message = "Roles obtenidos correctamente";
        return response;
    }

    /// <summary>
    /// Verifica y crea los roles básicos del sistema si no existen
    /// </summary>
    private async Task VerifyRoles()
    {
        // Lista de roles básicos que deben existir
        var basicRoles = new[]
            {
                new { Name = "Registrado", Prioridad = 2, Orden = 1, Activo = true },
                new { Name = "Medico", Prioridad = 3, Orden = 2, Activo = true },
                new { Name = "Paciente", Prioridad = 3, Orden = 3, Activo = true },
                new { Name = "Administrador", Prioridad = 1, Orden = 4, Activo = true },
                new { Name = "Visor", Prioridad = 4, Orden = 5, Activo = true },
                new { Name = "Operador", Prioridad = 4, Orden = 6, Activo = true },
                new { Name = "Usuario", Prioridad = 5, Orden = 7, Activo = false },
                new { Name = "Inactivo", Prioridad = 5, Orden = 8, Activo = false }
            };

            foreach (var role in basicRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = role.Name,
                        NormalizedName = role.Name.ToUpper(),
                        Prioridad = role.Prioridad,
                        Orden = role.Orden,
                        Activo = role.Activo
                    });
                }
            }
    }

    /// <summary>
    /// Obtiene el ID de un rol por su nombre
    /// </summary>
    /// <param name="roleName">Nombre del rol</param>
    /// <returns>ID del rol</returns>
    /// <exception cref="Exception">Cuando el rol no existe</exception>
    public async Task<Guid> GetRoleIdByName(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new Exception($"El rol con nombre {roleName} no existe");
        }
        return Guid.Parse(role.Id);
    }

    public async Task<Response<string>> GetRoleIdByRoleId(string roleId)
    {
        var response = new Response<string>();
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null && role!.Name != "")
        {
            response.IsSuccess = false;
            response.Message = $"El rol con id {roleId} no existe";
            return response;
        }
        response.IsSuccess = true;
        response.Data = role.Name;
        response.Message = $"ID del rol con id {roleId} obtenido correctamente";
        return response;
    }

    //Obtener Roles del usuario
    public async Task<Response<List<string>>> GetRolesByUserId(string userId)
    {
        var response = new Response<List<string>>();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            response.IsSuccess = false;
            response.Message = $"El usuario con id {userId} no existe";
            return response;
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null || roles.Count == 0)
        {
            response.IsSuccess = false;
            response.Message = $"El usuario con id {userId} no tiene roles asignados";
            return response;
        }
        response.IsSuccess = true;
        response.Data = roles.ToList();
        response.Message = $"Roles del usuario con id {userId} obtenidos correctamente";
        return response;
    }

    /// <summary>
    /// Elimina todos los roles asignados a un usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>True si la operación fue exitosa</returns>
    /// <exception cref="Exception">Cuando el usuario no existe o hay errores</exception>
    private async Task<bool> DeleteUserRole(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"El usuario con id {userId} no existe");
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count > 0)
        {
            foreach (var role in roles)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    throw new Exception($"{result.Errors}");
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Elimina un rol específico de un usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="roleId">ID del rol a eliminar</param>
    /// <returns>True si la operación fue exitosa</returns>
    /// <exception cref="Exception">Cuando el usuario o rol no existen</exception>
    public async Task<bool> RemoveRoleFromUser(string userId, string roleId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"El usuario con id {userId} no existe");
        }

        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            throw new Exception($"El rol con id {roleId} no existe");
        }

        var result = await _userManager.RemoveFromRoleAsync(user, role.Name!);
        return result.Succeeded;
    }

    public async Task<bool> UserExists(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null;
    }
    public async Task<Response<AuthResponse>> GetApplicationUser(string userId)
    {
        var response = new Response<AuthResponse>();

        var user = await _userManager.FindByIdAsync(userId);
        if (user is not null)
        {
            var authResponse = new AuthResponse()
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName!,
                Email = user.Email!
            };
            response.Data = authResponse;
            response.IsSuccess = true;
        }
        else
        {
            response.IsSuccess = false;
            response.Message = "No se encontro el usuario";
        }
        return response;
    }
    //obtiene todos los usuarios
    public async Task<Response<IReadOnlyList<AuthResponse>>> GetAllApplicationUser()
    {
        var response = new Response<IReadOnlyList<AuthResponse>>();

        var users = await _userManager.Users.ToListAsync();
        if (users != null && users.Count > 0)
        {
            var authResponses = users.Select(user => new AuthResponse
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName!,
                Email = user.Email!,
                Active = user.Active,
                LastLogin = user.LastLogin

            })
            //.Where(x => x.Active == true)
            .ToList();

            response = new Response<IReadOnlyList<AuthResponse>>
            {
                Data = authResponses,
                IsSuccess = true
            };
            return response;
        }
        response.IsSuccess = false;
        response.Message = "No se encontraron usuarios";
        return response;

    }



    public async Task<Response<IReadOnlyList<AuthResponse>>> GetAllApplicationUserActive(bool active = true)
    {
        var response = new Response<IReadOnlyList<AuthResponse>>();

        var users = await _userManager.Users.ToListAsync();
        if (users != null && users.Count > 0)
        {
            var authResponses = users.Select(user => new AuthResponse
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName!,
                Email = user.Email!,
                Active = user.Active,
                LastLogin = user.LastLogin
            })
            .Where(x => x.Active == active)
            .ToList();

            response = new Response<IReadOnlyList<AuthResponse>>
            {
                Data = authResponses,
                IsSuccess = true
            };
            return response;
        }
        response.IsSuccess = false;
        response.Message = $"No se encontraron usuarios " + (active ? "activos" : "inactivos");
        return response;

    }
    public async Task<Response<bool>> UpdateApplicationUser(AuthRequest authRequest)
    {
        var response = new Response<bool>();
        try
        {
            var userIdentity = await _userManager.FindByIdAsync(authRequest.UsuarioId.ToString());
            if (userIdentity is not null)
            {
                if (authRequest.Email != "") userIdentity.Email = authRequest.Email;
                if (authRequest.Username != "") userIdentity.UserName = authRequest.Username;
                if (authRequest.Password != "") userIdentity.UserName = authRequest.Password;
                if (userIdentity.Active != authRequest.Active) userIdentity.Active = authRequest.Active;
                await _userManager.UpdateAsync(userIdentity);
                response.IsSuccess = true;
                response.Data = true;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No se encontro el usuario a actualizar";
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }
    /// <summary>
    /// Elimina todos los roles asignados a un usuario
    ///     /// </summary>
    /// <param name="userId">ID del usuario</param>
    ///     /// <returns>True si la operación fue exitosa</returns>
    public async Task<bool> RemoveAllRolesFromUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }
        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        return true;
    }

    /// <summary>
    /// Confirma el email de un usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="token">Token de confirmación</param>
    /// <returns>True si la confirmación fue exitosa</returns>
    public async Task<Response<Guid>> ConfirmEmail(string userId, string refreshToken)
    {
        var response = new Response<Guid>();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            response.IsSuccess = false;
            response.Message = "Usuario no encontrado";
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent("User", "Usuario no encontrado")
            };
            return response;
        }
        //validar si el email ya está confirmado
        if (user.EmailConfirmed)
        {
            response.IsSuccess = false;
            response.Message = "El email ya está confirmado";
            return response;
        }

        var refreshTokenBd = await _context.RefreshTokens!
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Token == refreshToken && x.IsUsed == false && x.IsRevoked == false);
        if (refreshTokenBd == null)
        {
            response.IsSuccess = false;
            response.Message = "No se encontró un token válido para el usuario";
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent("RefreshToken", "No se encontró un refresh token válido para el usuario")
            };
            return response;
        }
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var result = await _userManager.ConfirmEmailAsync(user, token);
        response.IsSuccess = result.Succeeded;
        if (!result.Succeeded)
        {
            response.Message = "Error al confirmar el email";
            response.Errors = result.Errors.Select(e => new ValidationFailureFluent("Email", e.Description)).ToList();
        }
        else
        {
            response.IsSuccess = true;
            response.Data = Guid.Parse(user.Id);
            response.Message = "Email confirmado correctamente";
        }
        return response;
    }
    public async Task<Response<Guid>> ConfirmPhoneNumber(string userId, string refreshToken)
    {
        var response = new Response<Guid>();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            response.IsSuccess = false;
            response.Message = "Usuario no encontrado";
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent("User", "Usuario no encontrado")
            };
            return response;
        }
        // Validar si el teléfono ya está confirmado
        if (user.PhoneNumberConfirmed) 
        {
            response.IsSuccess = false;
            _appLogger.LogWarning($"El usuario {user.UserName} ya tiene el teléfono {user.PhoneNumber} confirmado, ");
            response.Message = "El teléfono ya está confirmado";
            return response;
        }
        //
        if (string.IsNullOrEmpty(user.PhoneNumber))
        {
            response.IsSuccess = false;
            response.Message = "El usuario no tiene un número de teléfono asociado";
            _appLogger.LogWarning("El usuario {UserId} no tiene un número de teléfono asociado", userId);
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent("PhoneNumber", "El usuario no tiene un número de teléfono asociado")
            };
        }
        var refreshTokenBd = await _context.RefreshTokens!.FirstOrDefaultAsync(x => x.UserId == userId && x.Token == refreshToken && x.IsUsed == false && x.IsRevoked == false);
        if (refreshTokenBd == null)
        {
            response.IsSuccess = false;
            response.Message = "No se encontró un token válido para el usuario";
            response.Errors = new List<ValidationFailureFluent>
            {
                new ValidationFailureFluent("RefreshToken", "No se encontró un refresh token válido para el usuario")
            };
            return response;
        }
        // Confirmar el teléfono usando el token
        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user,user.PhoneNumber!);
        var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber!, token);
        response.IsSuccess = result.Succeeded;
        if (!result.Succeeded)
        {
            response.Message = "Error al confirmar el teléfono";
            response.Errors = result.Errors.Select(e => new ValidationFailureFluent("PhoneNumber", e.Description)).ToList();
        }
        else
        {
            response.IsSuccess = true;
            response.Data = Guid.Parse(user.Id);
            response.Message = "Teléfono confirmado correctamente";
        }
        return response;
    }


}