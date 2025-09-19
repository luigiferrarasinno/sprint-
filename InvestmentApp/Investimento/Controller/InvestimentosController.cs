using Microsoft.AspNetCore.Mvc;
using InvestmentApp.Investimento.Service;
using InvestmentApp.Investimento.Dto;
using InvestmentApp.User.Service;

namespace InvestmentApp.Investimento.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestimentosController : ControllerBase
    {
        private readonly IInvestmentService _investmentService;
        private readonly IUserService _userService;
        private readonly ILogger<InvestimentosController> _logger;

        public InvestimentosController(IInvestmentService investmentService, IUserService userService, ILogger<InvestimentosController> logger)
        {
            _investmentService = investmentService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os investimentos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvestmentResponseDto>>> GetAllInvestments()
        {
            try
            {
                var investments = await _investmentService.GetAllInvestmentsAsync();
                return Ok(investments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all investments");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obtém um investimento por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<InvestmentResponseDto>> GetInvestment(Guid id)
        {
            try
            {
                var investment = await _investmentService.GetInvestmentByIdAsync(id);
                if (investment == null)
                {
                    return NotFound("Investimento não encontrado");
                }

                return Ok(investment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting investment {InvestmentId}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cria um novo investimento (apenas Admin)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<InvestmentResponseDto>> CreateInvestment([FromBody] InvestmentCreateDto investmentCreateDto, [FromHeader] Guid? userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se é admin
                if (!userId.HasValue || !await _userService.IsAdminAsync(userId.Value))
                {
                    return Forbid("Apenas administradores podem criar investimentos");
                }

                var investment = await _investmentService.CreateInvestmentAsync(investmentCreateDto);
                return CreatedAtAction(nameof(GetInvestment), new { id = investment.Id }, investment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating investment");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualiza um investimento (apenas Admin)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<InvestmentResponseDto>> UpdateInvestment(Guid id, [FromBody] InvestmentUpdateDto investmentUpdateDto, [FromHeader] Guid? userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se é admin
                if (!userId.HasValue || !await _userService.IsAdminAsync(userId.Value))
                {
                    return Forbid("Apenas administradores podem atualizar investimentos");
                }

                var investment = await _investmentService.UpdateInvestmentAsync(id, investmentUpdateDto);
                return Ok(investment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating investment {InvestmentId}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Remove um investimento (apenas Admin)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvestment(Guid id, [FromHeader] Guid? userId)
        {
            try
            {
                // Verificar se é admin
                if (!userId.HasValue || !await _userService.IsAdminAsync(userId.Value))
                {
                    return Forbid("Apenas administradores podem remover investimentos");
                }

                var result = await _investmentService.DeleteInvestmentAsync(id);
                if (!result)
                {
                    return NotFound("Investimento não encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting investment {InvestmentId}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
