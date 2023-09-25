using Microsoft.EntityFrameworkCore;
using ProductsInfo;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

//Carregar os valores das váriaveis ambiente
DotNetEnv.Env.Load();

//Obter o valor da variável de ambiente ACESS_DATABASE
string databaseConnection = Environment.GetEnvironmentVariable("ACESS_DATABASE");

//Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ProductsDbContext>(opt => opt.UseSqlServer(databaseConnection));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
