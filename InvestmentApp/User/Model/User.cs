using InvestmentApp.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace InvestmentApp.User.Model
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string Senha { get; set; } = string.Empty;
        
        [Required]
        [StringLength(11)]
        public string CPF { get; set; } = string.Empty;
        
        [Required]
        public DateTime DataNascimento { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "User"; // "Admin" ou "User"
        
        // Navigation property
        public virtual ICollection<UserInvestment> UserInvestments { get; set; } = new List<UserInvestment>();
    }
}
