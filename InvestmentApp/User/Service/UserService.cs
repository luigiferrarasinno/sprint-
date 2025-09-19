using AutoMapper;
using InvestmentApp.User.Dto;
using InvestmentApp.User.Repository;

namespace InvestmentApp.User.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users");
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching user with ID: {UserId}", id);
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation("Fetching user with email: {Email}", email);
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null ? null : _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> CreateUserAsync(UserCreateDto userCreateDto)
        {
            _logger.LogInformation("Creating new user with email: {Email}", userCreateDto.Email);

            // Check if email already exists
            if (await EmailExistsAsync(userCreateDto.Email))
            {
                throw new InvalidOperationException("Email já está em uso");
            }

            // Check if CPF already exists
            if (await CpfExistsAsync(userCreateDto.CPF))
            {
                throw new InvalidOperationException("CPF já está em uso");
            }

            var user = _mapper.Map<Model.User>(userCreateDto);
            var createdUser = await _userRepository.CreateAsync(user);
            
            _logger.LogInformation("User created successfully with ID: {UserId}", createdUser.Id);
            return _mapper.Map<UserResponseDto>(createdUser);
        }

        public async Task<UserResponseDto> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto)
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);

            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new InvalidOperationException("Usuário não encontrado");
            }

            // Check if email is being changed and if it already exists
            if (existingUser.Email != userUpdateDto.Email && await EmailExistsAsync(userUpdateDto.Email))
            {
                throw new InvalidOperationException("Email já está em uso");
            }

            _mapper.Map(userUpdateDto, existingUser);
            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            
            _logger.LogInformation("User updated successfully with ID: {UserId}", updatedUser.Id);
            return _mapper.Map<UserResponseDto>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);
            var result = await _userRepository.DeleteAsync(id);
            
            if (result)
            {
                _logger.LogInformation("User deleted successfully with ID: {UserId}", id);
            }
            else
            {
                _logger.LogWarning("Failed to delete user with ID: {UserId}", id);
            }
            
            return result;
        }

        public async Task<bool> UserExistsAsync(Guid id)
        {
            return await _userRepository.ExistsAsync(id);
        }

        public async Task<bool> IsAdminAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.Role == "Admin";
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> CpfExistsAsync(string cpf)
        {
            var user = await _userRepository.GetByCpfAsync(cpf);
            return user != null;
        }
    }
}
