using Microsoft.EntityFrameworkCore;
using InvestmentApp.Data;
using InvestmentApp.User.Repository;
using InvestmentApp.User.Service;
using InvestmentApp.User.Dao;
using InvestmentApp.Investimento.Repository;
using InvestmentApp.Investimento.Service;
using InvestmentApp.Common.Repository;
using InvestmentApp.Common.Services;
using InvestmentApp.Common.Mapping;
using InvestmentApp.Common.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<InvestmentAppDbContext>(options =>
    options.UseOracle("Data Source=oracle.fiap.com.br:1521/ORCL;User Id=RM98047;Password=201104;"));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInvestmentRepository, InvestmentRepository>();
builder.Services.AddScoped<IUserInvestmentRepository, UserInvestmentRepository>();

// Add services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInvestmentService, InvestmentService>();
builder.Services.AddScoped<IUserInvestmentService, UserInvestmentService>();

// Add DAOs
builder.Services.AddScoped<IUserDao, UserDao>();

// Add controllers
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Investment App API",
        Version = "v1",
        Description = "API para gerenciamento de usuários, investimentos e a relação usuário-investimento",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Investment App Team",
            Email = "contato@investmentapp.com"
        }
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investment App API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseCors();

app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InvestmentAppDbContext>();
    try
    {
        await context.Database.EnsureCreatedAsync();
        await DataSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
