using application.extensions;
using entityFrameworkCore;
using entityFrameworkCore.extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    // Add multiple Swagger documents for each group (e.g., version or logical group)
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "OVERVIEW API",
        Version = "v1"
    });

    c.SwaggerDoc("Users", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "User API",
        Version = "v1",
        Description = "API endpoints for managing users and their related entities."
    });

    c.SwaggerDoc("Organizations", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Organization API",
        Version = "v1",
        Description = "API endpoints for managing organizations and their related entities."
    });

    c.SwaggerDoc("Workspaces", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Workspace API",
        Version = "v1",
        Description = "API endpoints for managing workspaces and their related entities."
    });

    c.SwaggerDoc("Projects", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Project API",
        Version = "v1",
        Description = "API endpoints for managing projects and their related entities."
    });

    c.SwaggerDoc("WorkItems", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "WorkItem API",
        Version = "v1",
        Description = "API endpoints for managing work items and their related entities."
    });


    // Add annotations to all actions
    c.EnableAnnotations();
    // Optional: If you want all actions tagged with their group name
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (docName == "v1")
            return true;  // Show all endpoints in 'v1'

        var groupName = apiDesc.GroupName ?? string.Empty;
        return docName.Equals(groupName, StringComparison.OrdinalIgnoreCase);
    });

    // Add XML comments, if available, to Swagger UI (optional)
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // c.IncludeXmlComments(xmlPath);
});


// # --------------------- #
// #     CONTROLLERS       #
// # --------------------- #

// Registers all of our controllers.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 64; // Adjust the depth limit as needed
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
    app.UseSwaggerUI(c =>
    {
        // Add separate pages for each API group
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Overview");
        c.SwaggerEndpoint("/swagger/Users/swagger.json", "Users API");
        c.SwaggerEndpoint("/swagger/Organizations/swagger.json", "Organizations API");
        c.SwaggerEndpoint("/swagger/Workspaces/swagger.json", "Workspaces API");
        c.SwaggerEndpoint("/swagger/Projects/swagger.json", "Projects API");
        c.SwaggerEndpoint("/swagger/WorkItems/swagger.json", "WorkItems API");
    });
}

app.UseHttpsRedirection();

app.Run();