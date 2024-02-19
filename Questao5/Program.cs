using MediatR;
using Microsoft.OpenApi.Models;
using Questao5.Application.Interfaces;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Sqlite;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do MediatR
builder.Services.AddMediatR(typeof(Program).Assembly);

// Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Conta Corrente", Version = "v1" });
});


builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddScoped<IMovimentoRepository, MovimentoRepository>();
builder.Services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

//Configuração do Swagger
    app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Conta Corrente");
    c.RoutePrefix = string.Empty;
});


app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


