/*
 * Esta es la clase a ejecutar cuando se crea el programa/ejecuta
 */

using CustomersApi.Repositories;
using CustomersApi.UseCases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); // añade los controladores propios y los nuevos
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // expone los endpoints
builder.Services.AddSwaggerGen(); // inyecta el servicio de swagger para documentar y testear la api desde navegador
builder.Services.AddRouting(r=>r.LowercaseUrls = true); // a todas nuestras urls seran en minuscula COMO DEBE SER
builder.Services.AddDbContext<CustomerDatabaseContext>(mysqlBuilder => {
    mysqlBuilder.UseMySQL(builder.Configuration.GetConnectionString("Connection1"));
    // similar a Nodejs .env variables, usamos el appsetings.json como archivo de entorno, y ahi ponemos la cadena de conexion
});
builder.Services.AddScoped<IUpdateCustomerUseCase, UpdateCustomerUseCase>();
/* antes de ejecutar, añadimos la interfaz IUpdate a nuestro contenedor de dependencias. Las dependencias pueden ser:
 * scope: cada vez que llega una request
 * transient: cada vez que inyectamos en un servicio
 * singleton: una unica instancia
  */

var app = builder.Build(); // toma todo lo anterior y lo construye la aplicacion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // en desarrollo exponemos swagger
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // habilitar las funciones de redireccion por lado del server

app.UseAuthorization(); // habilitar las autenticaciones

// app.MapGet("/ejemplo/{id}", (long id) => { return "net 6, "+id; }); // Minimal API: es una manera simplificada pero no ordenada de exponer endpoints de nuestra APIREST
/* El problema con esta forma de trabajar es que es menos ordenada, y no sigue los principios SOLID como querriamos, 
 * En este caso en particular falla al primer principio Single Responsability Principle, el cual dice que una clase debe hacer una cosa y una unica cosa.
 * Pero si vemos bien, aqui no usamos clases, ni controladores, nada. por ende no es tan recomendable el uso de minimal api 
 * Ademas lo idal es que Program.cs no tenga logica de negocio, por ello es controversial y no se recomienda su uso a gran escala
 
 */

app.MapControllers(); // los mapeos de controladores

app.Run(); // paso FINAL que ejecuta lo construido hasta aqui
