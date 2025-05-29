using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AppCapasCitas.FrontEnd;
using AppCapasCitas.FrontEnd.Proxy.Interfaces;
using AppCapasCitas.FrontEnd.Proxy.Implementaciones;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
string url = builder.Configuration.GetValue<string>("UrlBackend:Url")!;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url) });

//Inyeccion de dependencias
builder.Services.AddScoped<IUsuarioProxy,UsuarioProxy>();
builder.Services.AddBlazorBootstrap();

await builder.Build().RunAsync();
