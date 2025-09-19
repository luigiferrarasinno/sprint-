using InvestmentApp.Common.Models;

namespace InvestmentApp.Common.Repository
{
    public interface IUserInvestmentRepository
    {
        Task<IEnumerable<UserInvestment>> GetByUserIdAsync(Guid userId);
        Task<UserInvestment?> GetByIdAsync(Guid id);
        Task<UserInvestment?> GetByUserAndInvestmentAsync(Guid userId, Guid investmentId);
        Task<UserInvestment> CreateAsync(UserInvestment userInvestment);
        Task<UserInvestment> UpdateAsync(UserInvestment userInvestment);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
