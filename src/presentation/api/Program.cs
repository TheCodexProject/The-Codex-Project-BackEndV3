using entityFrameworkCore;
using entityFrameworkCore.extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// # --------------------- #
// #     CONTROLLERS       #
// # --------------------- #

// Registers all of our controllers.
builder.Services.AddControllers();

// # --------------------- #
// #     DATABASE SETUP    #
// # --------------------- #

// NOTE: This needs to have the "weird" path to the database file.
// This is because the database file is created in the root of the project, and the project is run from the bin folder.
// So I need to find an alternative way to get the path to the database file in a more reliable way.
builder.Services.AddDbContext<DbContext, LocalDbContext>();

// Register repositories.
builder.Services.RegisterRepositories();

// Register unit of work.
builder.Services.RegisterUnitOfWork();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
