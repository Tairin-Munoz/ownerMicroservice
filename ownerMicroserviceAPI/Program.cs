using System.Text;
using ownerMicroservice.Application.Services;
using ownerMicroservice.Domain.Ports;
using ownerMicroservice.Domain.Services;
using ownerMicroservice.Infrastructure.Connection;
using ownerMicroservice.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgreSql");
var connectionManager = DatabaseConnectionManager.GetInstance(connectionString!);
builder.Services.AddSingleton(connectionManager);


//Inyeccion de Capas
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<OwnerService>();
builder.Services.AddScoped<IDbConnectionFactory, PostgreSqlConnection>();

// Inyección de Validadores
builder.Services.AddScoped<IValidator<ownerMicroservice.Domain.Entities.Owner>, OwnerValidator>();



builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();