using InvestmentApp.User.Model;

namespace InvestmentApp.User.Dao
{
    public interface IUserDao
    {
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<Model.User?> ValidateUserCredentialsAsync(string email, string password);
    }
}
