using InvestmentApp.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentApp.Common.Models
{
    public class UserInvestment : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid InvestmentId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountInvested { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal Units { get; set; }
        
        [Required]
        public DateTime PurchaseDate { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentValue { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Ativo"; // "Ativo", "Resgatado"
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.Model.User User { get; set; } = null!;
        
        [ForeignKey("InvestmentId")]
        public virtual Investimento.Model.Investment Investment { get; set; } = null!;
    }
}
