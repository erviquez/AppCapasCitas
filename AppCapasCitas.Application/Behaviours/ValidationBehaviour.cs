
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using AppCapasCitas.Transversal.Common; // Asumiendo que tienes un Response<T> definido aquí

namespace AppCapasCitas.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                return HandleValidationErrors(failures);
            }

            return await next();
        }

        private static TResponse HandleValidationErrors(IEnumerable<ValidationFailure> failures)
        {
            // Si TResponse es Response<T>
            if (typeof(TResponse).IsGenericType && 
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Response<>))
            {
                var responseType = typeof(Response<>);
                var genericType = typeof(TResponse).GetGenericArguments()[0];
                var constructedType = responseType.MakeGenericType(genericType);
                
                var response = Activator.CreateInstance(constructedType) as dynamic;
                
                response!.IsSuccess = false;
                response.Message = "Errores de validación";
                response.Errors = failures
                    .Select(f => new ValidationFailure
                    {
                        PropertyName = f.PropertyName,
                        ErrorMessage = f.ErrorMessage,
                        ErrorCode = f.ErrorCode
                    })
                    .ToList();
                
                return response;
            }
            
            // Si no es Response<T>, lanzamos la excepción
            throw new ValidationException(failures);
        }
    }
}


// using System;
// using FluentValidation;
// using MediatR;

// namespace AppCapasCitas.Application.Behaviours;

//     public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
//     {
//         private readonly IEnumerable<IValidator<TRequest>> _validators;

//         public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
//         {
//             _validators = validators;
//         }

//         public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//         {
//             if (_validators.Any())
//             { 
//                 var context = new ValidationContext<TRequest>(request);
//                 var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
//                 var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

//                 if (failures.Count != 0)
//                 {
//                     throw new ValidationException(failures);
//                 }
            
//             }

//             return await next();
//         }


//     }
