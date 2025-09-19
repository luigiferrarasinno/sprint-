using InvestmentApp.Common.Models.Dto;

namespace InvestmentApp.Common.Services
{
    public interface IUserInvestmentService
    {
        Task<IEnumerable<UserInvestmentResponseDto>> GetUserInvestmentsAsync(Guid userId);
        Task<UserInvestmentResponseDto?> GetUserInvestmentByIdAsync(Guid id);
        Task<UserInvestmentResponseDto> CreateUserInvestmentAsync(Guid userId, UserInvestmentCreateDto userInvestmentCreateDto);
        Task<UserInvestmentResponseDto> UpdateUserInvestmentAsync(Guid id, UserInvestmentUpdateDto userInvestmentUpdateDto);
        Task<bool> DeleteUserInvestmentAsync(Guid id);
        Task<bool> UserInvestmentExistsAsync(Guid id);
    }
}
