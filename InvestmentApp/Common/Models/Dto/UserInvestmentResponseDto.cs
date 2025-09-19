using InvestmentApp.Investimento.Dto;
using InvestmentApp.User.Dto;

namespace InvestmentApp.Common.Models.Dto
{
    public class UserInvestmentResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid InvestmentId { get; set; }
        public decimal AmountInvested { get; set; }
        public decimal Units { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal CurrentValue { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties DTOs
        public UserResponseDto? User { get; set; }
        public InvestmentResponseDto? Investment { get; set; }
    }
}
