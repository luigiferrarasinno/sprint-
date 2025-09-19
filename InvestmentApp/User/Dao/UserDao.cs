using InvestmentApp.User.Repository;
using InvestmentApp.User.Model;

namespace InvestmentApp.User.Dao
{
    public class UserDao : IUserDao
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserDao> _logger;

        public UserDao(IUserRepository userRepository, ILogger<UserDao> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            _logger.LogInformation("Attempting to change password for user {UserId}", userId);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found for password change", userId);
                return false;
            }

            // Simple password validation (no hashing for this demo)
            if (user.Senha != currentPassword)
            {
                _logger.LogWarning("Invalid current password for user {UserId}", userId);
                return false;
            }

            user.Senha = newPassword;
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Password changed successfully for user {UserId}", userId);
            return true;
        }

        public async Task<Model.User?> ValidateUserCredentialsAsync(string email, string password)
        {
            _logger.LogInformation("Validating credentials for email {Email}", email);

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found", email);
                return null;
            }

            // Simple password validation (no hashing for this demo)
            if (user.Senha != password)
            {
                _logger.LogWarning("Invalid password for user {Email}", email);
                return null;
            }

            _logger.LogInformation("Credentials validated successfully for user {Email}", email);
            return user;
        }
    }
}
