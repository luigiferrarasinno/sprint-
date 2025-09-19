using System.ComponentModel.DataAnnotations;

namespace InvestmentApp.Investimento.Dto
{
    public class InvestmentCreateDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo é obrigatório")]
        [StringLength(50, ErrorMessage = "Tipo deve ter no máximo 50 caracteres")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Valor base é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor base deve ser maior que zero")]
        public decimal BaseValue { get; set; }

        [Required(ErrorMessage = "Percentual de rendimento esperado é obrigatório")]
        [Range(0, 100, ErrorMessage = "Percentual deve estar entre 0 e 100")]
        public decimal ExpectedYieldPercent { get; set; }

        [Required(ErrorMessage = "Nível de risco é obrigatório")]
        [StringLength(20, ErrorMessage = "Nível de risco deve ter no máximo 20 caracteres")]
        public string RiskLevel { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string Description { get; set; } = string.Empty;
    }
}
