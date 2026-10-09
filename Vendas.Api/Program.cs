using Microsoft.EntityFrameworkCore;
using MediatR;
using Vendas.Application.Validators;
using Vendas.Api.Endpoints;
using Vendas.Application.Services;
using Vendas.Domain.Repositories;
using Vendas.Infrastructure.Data;
using Vendas.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<RegistrarVendaService>();
builder.Services.AddScoped<VendaValidator>();
builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(typeof(ListarVendasQueryHandler).Assembly));

builder.Services.AddCors(options => 
{
    options.AddPolicy("Frontend", policy => 
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Vendas API v1");
    });
}
app.UseCors("Frontend");
app.UseHttpsRedirection();
app.MapVendasEndpoints();


app.Run();

