
using AppCapasCitas.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
string connetionStringSql =  builder.Configuration.GetConnectionString("SqlConnectionString")!;
builder.Services.AddDbContext<CitasDbContext>(options =>
                options.UseSqlServer(connetionStringSql)
                       .UseSnakeCaseNamingConvention()
                )
                ;
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AppCapasCitas API",
        Version = "v1",
        Description = "API for the AppCapasCitas application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ernesto De la O",
            Email = "erviquez@gmail.com"
        }
    });      
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppCapasCitas API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.UseAuthorization();

app.MapControllers();
app.Run();
