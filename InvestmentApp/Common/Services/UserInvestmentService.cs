using AutoMapper;
using InvestmentApp.Common.Models;
using InvestmentApp.Common.Models.Dto;
using InvestmentApp.Common.Repository;
using InvestmentApp.User.Service;
using InvestmentApp.Investimento.Service;

namespace InvestmentApp.Common.Services
{
    public class UserInvestmentService : IUserInvestmentService
    {
        private readonly IUserInvestmentRepository _userInvestmentRepository;
        private readonly IUserService _userService;
        private readonly IInvestmentService _investmentService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserInvestmentService> _logger;

        public UserInvestmentService(
            IUserInvestmentRepository userInvestmentRepository,
            IUserService userService,
            IInvestmentService investmentService,
            IMapper mapper,
            ILogger<UserInvestmentService> logger)
        {
            _userInvestmentRepository = userInvestmentRepository;
            _userService = userService;
            _investmentService = investmentService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserInvestmentResponseDto>> GetUserInvestmentsAsync(Guid userId)
        {
            _logger.LogInformation("Fetching investments for user ID: {UserId}", userId);
            
            // Verify user exists
            if (!await _userService.UserExistsAsync(userId))
            {
                throw new InvalidOperationException("Usuário não encontrado");
            }

            var userInvestments = await _userInvestmentRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserInvestmentResponseDto>>(userInvestments);
        }

        public async Task<UserInvestmentResponseDto?> GetUserInvestmentByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching user investment with ID: {UserInvestmentId}", id);
            var userInvestment = await _userInvestmentRepository.GetByIdAsync(id);
            return userInvestment == null ? null : _mapper.Map<UserInvestmentResponseDto>(userInvestment);
        }

        public async Task<UserInvestmentResponseDto> CreateUserInvestmentAsync(Guid userId, UserInvestmentCreateDto userInvestmentCreateDto)
        {
            _logger.LogInformation("Creating new user investment for user ID: {UserId}", userId);

            // Verify user exists
            if (!await _userService.UserExistsAsync(userId))
            {
                throw new InvalidOperationException("Usuário não encontrado");
            }

            // Verify investment exists
            if (!await _investmentService.InvestmentExistsAsync(userInvestmentCreateDto.InvestmentId))
            {
                throw new InvalidOperationException("Investimento não encontrado");
            }

            var userInvestment = _mapper.Map<UserInvestment>(userInvestmentCreateDto);
            userInvestment.UserId = userId;

            var createdUserInvestment = await _userInvestmentRepository.CreateAsync(userInvestment);
            
            _logger.LogInformation("User investment created successfully with ID: {UserInvestmentId}", createdUserInvestment.Id);
            return _mapper.Map<UserInvestmentResponseDto>(createdUserInvestment);
        }

        public async Task<UserInvestmentResponseDto> UpdateUserInvestmentAsync(Guid id, UserInvestmentUpdateDto userInvestmentUpdateDto)
        {
            _logger.LogInformation("Updating user investment with ID: {UserInvestmentId}", id);

            var existingUserInvestment = await _userInvestmentRepository.GetByIdAsync(id);
            if (existingUserInvestment == null)
            {
                throw new InvalidOperationException("Investimento do usuário não encontrado");
            }

            _mapper.Map(userInvestmentUpdateDto, existingUserInvestment);
            var updatedUserInvestment = await _userInvestmentRepository.UpdateAsync(existingUserInvestment);
            
            _logger.LogInformation("User investment updated successfully with ID: {UserInvestmentId}", updatedUserInvestment.Id);
            return _mapper.Map<UserInvestmentResponseDto>(updatedUserInvestment);
        }

        public async Task<bool> DeleteUserInvestmentAsync(Guid id)
        {
            _logger.LogInformation("Deleting user investment with ID: {UserInvestmentId}", id);
            var result = await _userInvestmentRepository.DeleteAsync(id);
            
            if (result)
            {
                _logger.LogInformation("User investment deleted successfully with ID: {UserInvestmentId}", id);
            }
            else
            {
                _logger.LogWarning("Failed to delete user investment with ID: {UserInvestmentId}", id);
            }
            
            return result;
        }

        public async Task<bool> UserInvestmentExistsAsync(Guid id)
        {
            return await _userInvestmentRepository.ExistsAsync(id);
        }
    }
}
