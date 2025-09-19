using InvestmentApp.User.Model;

namespace InvestmentApp.User.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<Model.User>> GetAllAsync();
        Task<Model.User?> GetByIdAsync(Guid id);
        Task<Model.User?> GetByEmailAsync(string email);
        Task<Model.User?> GetByCpfAsync(string cpf);
        Task<Model.User> CreateAsync(Model.User user);
        Task<Model.User> UpdateAsync(Model.User user);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
