using application.extensions;
using entityFrameworkCore;
using entityFrameworkCore.extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// # --------------------- #
// #     CONTROLLERS       #
// # --------------------- #

// Registers all of our controllers.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 10; // Adjust the depth limit as needed
    });

// # --------------------- #
// #    COMMAND HANDLERS   #
// #    AND DISPATCHER     #
// # --------------------- #

builder.Services.RegisterCommandHandlers();
builder.Services.RegisterCommandDispatcher();

// # --------------------- #
// #     DATABASE SETUP    #
// # --------------------- #

// NOTE: This needs to have the "weird" path to the database file.
// This is because the database file is created in the root of the project, and the project is run from the bin folder.
// So I need to find an alternative way to get the path to the database file in a more reliable way.
builder.Services.AddDbContext<LocalDbContext>(options =>
{
    options.UseSqlite("Data Source=../../../src/infrastructure/entityFrameworkCore/localdb.db");
});

// Register repositories.
builder.Services.RegisterRepositories();

// Register unit of work.
builder.Services.RegisterUnitOfWork();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();