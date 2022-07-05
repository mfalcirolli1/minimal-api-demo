// Atrav�s desse builder h� o acesso ao Container, Seri�os que ser�o configurados, forma que ser� configurada a request (m�e)
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conex�o com o Banco de Dados
builder.Services.AddDbContext<MinimalContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Ap�s adicionar migration
//      -> add-migration Initial
//      -> update-database

// Sempre por �ltimo
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapear Endpoints
app.MapGet("/fornecedor", async (MinimalContextDb contextDb) =>
    await contextDb.Fornecedores.ToListAsync())
    .WithName("GetFornecedores")
    .WithTags("Fornecedores");

                                            // Par�metros
app.MapGet("/fornecedor/{id}", async (Guid id, MinimalContextDb contextDb) =>

    // Execu��o
    await contextDb.Fornecedores.FindAsync(id)

    // Valida��o
    is Fornecedor fornecedor ? Results.Ok(fornecedor) : Results.NotFound()) 

    // Produz
    .Produces<Fornecedor>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)

    .WithName("GetFornecedorById")
    .WithTags("Fornecedor");


app.MapPost("/fornecedor", async (MinimalContextDb contextDb, Fornecedor fornecedor) =>
{

    contextDb.Fornecedores.Add(fornecedor);
    var result = await contextDb.SaveChangesAsync();

})
.Produces<Fornecedor>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status404NotFound)
.WithName("PostFornecedor")
.WithTags("Fornecedor");


// Executa a API
app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}