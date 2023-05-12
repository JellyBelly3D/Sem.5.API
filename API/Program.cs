using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Sem._5.API;
using Sem._5.API.MockDB;
using Sem._5.API.SQLiteDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddSingleton<IDataAccess, DataAccessMock>();
builder.Services.AddSingleton<IDataAccess, DataAccess>();
builder.Services.AddSingleton<IDbConnectionFactory, SqLiteConnectionFactory>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>((options) =>
    // null = Pascal Case
    options.SerializerOptions.PropertyNamingPolicy = null
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddEndpoints();

app.Run();