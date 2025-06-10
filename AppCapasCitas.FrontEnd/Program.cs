using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AppCapasCitas.FrontEnd;
using AppCapasCitas.FrontEnd.Proxy.Interfaces;
using AppCapasCitas.FrontEnd.Proxy.Implementaciones;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using AppCapasCitas.FrontEnd.Security;
using Blazored.Toast;
using CurrieTechnologies.Razor.SweetAlert2;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
string url = builder.Configuration.GetValue<string>("UrlBackend:Url")!;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url) });
builder.Services.AddScoped<AuthenticationStateProvider,AuthenticationService>();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddAuthorizationCore();

//Inyeccion de dependencias
builder.Services.AddScoped<IUsuarioProxy,UsuarioProxy>();
builder.Services.AddScoped<ILoginProxy,LoginProxy>();
builder.Services.AddScoped<IPacienteProxy,PacienteProxy>();
builder.Services.AddScoped<IMedicoProxy,MedicoProxy>();

builder.Services.AddBlazorBootstrap();
builder.Services.AddBlazoredToast();
// builder.Services.AddSweetAlert2();
// builder.Services.AddSweetAlert2(options => {
//  options.SetThemeForColorSchemePreference(ColorScheme.Dark, SweetAlertTheme.Dark);
// });
builder.Services.AddSweetAlert2(options => {
 options.Theme = SweetAlertTheme.Dark;
 options.SetThemeForColorSchemePreference(ColorScheme.Light, SweetAlertTheme.Bootstrap4);
});

await builder.Build().RunAsync();
