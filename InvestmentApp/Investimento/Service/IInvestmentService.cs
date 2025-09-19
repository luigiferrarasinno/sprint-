using InvestmentApp.Investimento.Dto;

namespace InvestmentApp.Investimento.Service
{
    public interface IInvestmentService
    {
        Task<IEnumerable<InvestmentResponseDto>> GetAllInvestmentsAsync();
        Task<InvestmentResponseDto?> GetInvestmentByIdAsync(Guid id);
        Task<InvestmentResponseDto> CreateInvestmentAsync(InvestmentCreateDto investmentCreateDto);
        Task<InvestmentResponseDto> UpdateInvestmentAsync(Guid id, InvestmentUpdateDto investmentUpdateDto);
        Task<bool> DeleteInvestmentAsync(Guid id);
        Task<bool> InvestmentExistsAsync(Guid id);
    }
}
