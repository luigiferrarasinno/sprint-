using InvestmentApp.Data;
using InvestmentApp.User.Model;
using InvestmentApp.Investimento.Model;
using Microsoft.EntityFrameworkCore;

namespace InvestmentApp.Data
{
    public static class DataSeeder
    {
    public static async Task SeedAsync(InvestmentAppDbContext context)
    {
        // Check if data already exists using Count instead of Any() to avoid Oracle boolean issues
        var userCount = await context.Users.CountAsync();
        var investmentCount = await context.Investments.CountAsync();
        
        if (userCount > 0 || investmentCount > 0)
        {
            return; // Database has been seeded
        }            // Create admin user
            var adminUser = new User.Model.User
            {
                Id = Guid.NewGuid(),
                Nome = "Administrador",
                Email = "admin@investmentapp.com",
                Senha = "admin123",
                CPF = "12345678900",
                DataNascimento = new DateTime(1990, 1, 1),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Create test user
            var testUser = new User.Model.User
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao@teste.com",
                Senha = "usuario123",
                CPF = "98765432100",
                DataNascimento = new DateTime(1985, 5, 15),
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(adminUser, testUser);

            // Create sample investments
            var investments = new List<Investment>
            {
                new Investment
                {
                    Id = Guid.NewGuid(),
                    Name = "Tesouro Direto - Selic 2029",
                    Type = "Renda Fixa",
                    BaseValue = 100.00m,
                    ExpectedYieldPercent = 12.50m,
                    RiskLevel = "Baixo",
                    Description = "Título do Tesouro Nacional indexado à taxa Selic",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Investment
                {
                    Id = Guid.NewGuid(),
                    Name = "Fundo Multimercado XP",
                    Type = "Fundo",
                    BaseValue = 50.00m,
                    ExpectedYieldPercent = 15.80m,
                    RiskLevel = "Médio",
                    Description = "Fundo de investimento multimercado com estratégia diversificada",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Investment
                {
                    Id = Guid.NewGuid(),
                    Name = "FII Kinea Renda Imobiliária",
                    Type = "Fundo Imobiliário",
                    BaseValue = 150.00m,
                    ExpectedYieldPercent = 8.20m,
                    RiskLevel = "Médio",
                    Description = "Fundo de investimento imobiliário focado em imóveis comerciais",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Investment
                {
                    Id = Guid.NewGuid(),
                    Name = "Ações VALE3",
                    Type = "Ação",
                    BaseValue = 75.50m,
                    ExpectedYieldPercent = 18.50m,
                    RiskLevel = "Alto",
                    Description = "Ações da Vale S.A. - Mineração e logística",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Investments.AddRange(investments);

            await context.SaveChangesAsync();
        }
    }
}
