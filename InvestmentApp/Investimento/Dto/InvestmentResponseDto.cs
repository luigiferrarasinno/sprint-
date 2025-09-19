namespace InvestmentApp.Investimento.Dto
{
    public class InvestmentResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal BaseValue { get; set; }
        public decimal ExpectedYieldPercent { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
