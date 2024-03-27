using Microsoft.EntityFrameworkCore;
using Streaming.API;
using Streaming.Application.Account;
using Streaming.Repository;
using Streaming.Repository.Account;
using Streaming.Repository.Streaming;
using Streaming.Repository.Transaction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("musicaApiServer", client =>
{
    client.BaseAddress = new Uri("http://localhost:8080");
}).AddPolicyHandler(RetryPolicyConfiguration.GetRetryPolicy());


builder.Services.AddDbContext<StreamingContext>(c =>
{
    c.UseInMemoryDatabase("Streaming");
});

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<BandaRepository>();
builder.Services.AddScoped<PlanoRepository>();

builder.Services.AddScoped<UsuarioService>();

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

app.Run();
