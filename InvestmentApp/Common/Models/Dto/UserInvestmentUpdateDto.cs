using System.ComponentModel.DataAnnotations;

namespace InvestmentApp.Common.Models.Dto
{
    public class UserInvestmentUpdateDto
    {
        [Required(ErrorMessage = "Valor investido é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor investido deve ser maior que zero")]
        public decimal AmountInvested { get; set; }

        [Required(ErrorMessage = "Quantidade de cotas é obrigatória")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Quantidade de cotas deve ser maior que zero")]
        public decimal Units { get; set; }

        [Required(ErrorMessage = "Data de compra é obrigatória")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Valor atual é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor atual deve ser maior ou igual a zero")]
        public decimal CurrentValue { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        [StringLength(20, ErrorMessage = "Status deve ter no máximo 20 caracteres")]
        public string Status { get; set; } = "Ativo";

        public bool IsActive { get; set; } = true;
    }
}
