using InvestmentApp.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentApp.Investimento.Model
{
    public class Investment : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // "Renda Fixa", "Fundo", etc.
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseValue { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ExpectedYieldPercent { get; set; }
        
        [Required]
        [StringLength(20)]
        public string RiskLevel { get; set; } = string.Empty; // "Baixo", "Médio", "Alto"
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        // Navigation property
        public virtual ICollection<UserInvestment> UserInvestments { get; set; } = new List<UserInvestment>();
    }
}
