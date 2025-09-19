using InvestmentApp.Investimento.Model;

namespace InvestmentApp.Investimento.Repository
{
    public interface IInvestmentRepository
    {
        Task<IEnumerable<Investment>> GetAllAsync();
        Task<Investment?> GetByIdAsync(Guid id);
        Task<Investment> CreateAsync(Investment investment);
        Task<Investment> UpdateAsync(Investment investment);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
