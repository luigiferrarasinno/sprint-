using AutoMapper;
using InvestmentApp.Investimento.Dto;
using InvestmentApp.Investimento.Repository;
using InvestmentApp.Investimento.Model;

namespace InvestmentApp.Investimento.Service
{
    public class InvestmentService : IInvestmentService
    {
        private readonly IInvestmentRepository _investmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestmentService> _logger;

        public InvestmentService(IInvestmentRepository investmentRepository, IMapper mapper, ILogger<InvestmentService> logger)
        {
            _investmentRepository = investmentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<InvestmentResponseDto>> GetAllInvestmentsAsync()
        {
            _logger.LogInformation("Fetching all investments");
            var investments = await _investmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InvestmentResponseDto>>(investments);
        }

        public async Task<InvestmentResponseDto?> GetInvestmentByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching investment with ID: {InvestmentId}", id);
            var investment = await _investmentRepository.GetByIdAsync(id);
            return investment == null ? null : _mapper.Map<InvestmentResponseDto>(investment);
        }

        public async Task<InvestmentResponseDto> CreateInvestmentAsync(InvestmentCreateDto investmentCreateDto)
        {
            _logger.LogInformation("Creating new investment with name: {InvestmentName}", investmentCreateDto.Name);

            var investment = _mapper.Map<Investment>(investmentCreateDto);
            var createdInvestment = await _investmentRepository.CreateAsync(investment);
            
            _logger.LogInformation("Investment created successfully with ID: {InvestmentId}", createdInvestment.Id);
            return _mapper.Map<InvestmentResponseDto>(createdInvestment);
        }

        public async Task<InvestmentResponseDto> UpdateInvestmentAsync(Guid id, InvestmentUpdateDto investmentUpdateDto)
        {
            _logger.LogInformation("Updating investment with ID: {InvestmentId}", id);

            var existingInvestment = await _investmentRepository.GetByIdAsync(id);
            if (existingInvestment == null)
            {
                throw new InvalidOperationException("Investimento não encontrado");
            }

            _mapper.Map(investmentUpdateDto, existingInvestment);
            var updatedInvestment = await _investmentRepository.UpdateAsync(existingInvestment);
            
            _logger.LogInformation("Investment updated successfully with ID: {InvestmentId}", updatedInvestment.Id);
            return _mapper.Map<InvestmentResponseDto>(updatedInvestment);
        }

        public async Task<bool> DeleteInvestmentAsync(Guid id)
        {
            _logger.LogInformation("Deleting investment with ID: {InvestmentId}", id);
            var result = await _investmentRepository.DeleteAsync(id);
            
            if (result)
            {
                _logger.LogInformation("Investment deleted successfully with ID: {InvestmentId}", id);
            }
            else
            {
                _logger.LogWarning("Failed to delete investment with ID: {InvestmentId}", id);
            }
            
            return result;
        }

        public async Task<bool> InvestmentExistsAsync(Guid id)
        {
            return await _investmentRepository.ExistsAsync(id);
        }
    }
}
