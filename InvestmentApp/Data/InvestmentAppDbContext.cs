using Microsoft.EntityFrameworkCore;
using InvestmentApp.User.Model;
using InvestmentApp.Investimento.Model;
using InvestmentApp.Common.Models;

namespace InvestmentApp.Data
{
    public class InvestmentAppDbContext : DbContext
    {
        public InvestmentAppDbContext(DbContextOptions<InvestmentAppDbContext> options) : base(options)
        {
        }

        public DbSet<User.Model.User> Users { get; set; }
        public DbSet<Investment> Investments { get; set; }
        public DbSet<UserInvestment> UserInvestments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle("Data Source=oracle.fiap.com.br:1521/ORCL;User Id=RM98047;Password=201104;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User.Model.User>(entity =>
            {
                entity.ToTable("USERS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Nome).HasColumnName("NOME").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasColumnName("EMAIL").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Senha).HasColumnName("SENHA").IsRequired().HasMaxLength(255);
                entity.Property(e => e.CPF).HasColumnName("CPF").IsRequired().HasMaxLength(11);
                entity.Property(e => e.DataNascimento).HasColumnName("DATA_NASCIMENTO").IsRequired();
                entity.Property(e => e.Role).HasColumnName("ROLE").IsRequired().HasMaxLength(20);
                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").IsRequired();
                entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").IsRequired();
                
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.CPF).IsUnique();
            });

            // Investment configuration
            modelBuilder.Entity<Investment>(entity =>
            {
                entity.ToTable("INVESTMENTS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Name).HasColumnName("NAME").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).HasColumnName("TYPE").IsRequired().HasMaxLength(50);
                entity.Property(e => e.BaseValue).HasColumnName("BASE_VALUE").IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.ExpectedYieldPercent).HasColumnName("EXPECTED_YIELD_PERCENT").IsRequired().HasColumnType("decimal(5,2)");
                entity.Property(e => e.RiskLevel).HasColumnName("RISK_LEVEL").IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(500);
                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").IsRequired();
                entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").IsRequired();
            });

            // UserInvestment configuration
            modelBuilder.Entity<UserInvestment>(entity =>
            {
                entity.ToTable("USER_INVESTMENTS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.UserId).HasColumnName("USER_ID").IsRequired();
                entity.Property(e => e.InvestmentId).HasColumnName("INVESTMENT_ID").IsRequired();
                entity.Property(e => e.AmountInvested).HasColumnName("AMOUNT_INVESTED").IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Units).HasColumnName("UNITS").IsRequired().HasColumnType("decimal(18,6)");
                entity.Property(e => e.PurchaseDate).HasColumnName("PURCHASE_DATE").IsRequired();
                entity.Property(e => e.CurrentValue).HasColumnName("CURRENT_VALUE").IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasColumnName("STATUS").IsRequired().HasMaxLength(20);
                entity.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").IsRequired();
                entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").IsRequired();

                // Foreign keys
                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserInvestments)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Investment)
                    .WithMany(i => i.UserInvestments)
                    .HasForeignKey(e => e.InvestmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
