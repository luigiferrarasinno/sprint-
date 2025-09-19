using Microsoft.AspNetCore.Mvc;
using InvestmentApp.Common.Services;
using InvestmentApp.Common.Models.Dto;
using InvestmentApp.User.Service;

namespace InvestmentApp.Common.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class InvestimentosController : ControllerBase
    {
        private readonly IUserInvestmentService _userInvestmentService;
        private readonly IUserService _userService;
        private readonly ILogger<InvestimentosController> _logger;

        public InvestimentosController(IUserInvestmentService userInvestmentService, IUserService userService, ILogger<InvestimentosController> logger)
        {
            _userInvestmentService = userInvestmentService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Lista investimentos do usuário
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInvestmentResponseDto>>> GetUserInvestments(Guid userId, [FromHeader] Guid? requestingUserId)
        {
            try
            {
                // Verificar se é admin ou o próprio usuário
                if (!requestingUserId.HasValue || (!await _userService.IsAdminAsync(requestingUserId.Value) && requestingUserId.Value != userId))
                {
                    return Forbid("Você não tem permissão para visualizar os investimentos deste usuário");
                }

                var userInvestments = await _userInvestmentService.GetUserInvestmentsAsync(userId);
                return Ok(userInvestments);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user investments for user {UserId}", userId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cria um investimento para o usuário
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserInvestmentResponseDto>> CreateUserInvestment(Guid userId, [FromBody] UserInvestmentCreateDto userInvestmentCreateDto, [FromHeader] Guid? requestingUserId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se é admin ou o próprio usuário
                if (!requestingUserId.HasValue || (!await _userService.IsAdminAsync(requestingUserId.Value) && requestingUserId.Value != userId))
                {
                    return Forbid("Você não tem permissão para criar investimentos para este usuário");
                }

                var userInvestment = await _userInvestmentService.CreateUserInvestmentAsync(userId, userInvestmentCreateDto);
                return CreatedAtAction(nameof(GetUserInvestment), new { userId, id = userInvestment.Id }, userInvestment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user investment for user {UserId}", userId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obtém um investimento específico do usuário
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInvestmentResponseDto>> GetUserInvestment(Guid userId, Guid id, [FromHeader] Guid? requestingUserId)
        {
            try
            {
                // Verificar se é admin ou o próprio usuário
                if (!requestingUserId.HasValue || (!await _userService.IsAdminAsync(requestingUserId.Value) && requestingUserId.Value != userId))
                {
                    return Forbid("Você não tem permissão para visualizar este investimento");
                }

                var userInvestment = await _userInvestmentService.GetUserInvestmentByIdAsync(id);
                if (userInvestment == null || userInvestment.UserId != userId)
                {
                    return NotFound("Investimento não encontrado");
                }

                return Ok(userInvestment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user investment {UserInvestmentId} for user {UserId}", id, userId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualiza um investimento do usuário
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserInvestmentResponseDto>> UpdateUserInvestment(Guid userId, Guid id, [FromBody] UserInvestmentUpdateDto userInvestmentUpdateDto, [FromHeader] Guid? requestingUserId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se é admin ou o próprio usuário
                if (!requestingUserId.HasValue || (!await _userService.IsAdminAsync(requestingUserId.Value) && requestingUserId.Value != userId))
                {
                    return Forbid("Você não tem permissão para atualizar este investimento");
                }

                // Verificar se o investimento pertence ao usuário
                var existingInvestment = await _userInvestmentService.GetUserInvestmentByIdAsync(id);
                if (existingInvestment == null || existingInvestment.UserId != userId)
                {
                    return NotFound("Investimento não encontrado");
                }

                var userInvestment = await _userInvestmentService.UpdateUserInvestmentAsync(id, userInvestmentUpdateDto);
                return Ok(userInvestment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user investment {UserInvestmentId} for user {UserId}", id, userId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Remove um investimento do usuário
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserInvestment(Guid userId, Guid id, [FromHeader] Guid? requestingUserId)
        {
            try
            {
                // Verificar se é admin ou o próprio usuário
                if (!requestingUserId.HasValue || (!await _userService.IsAdminAsync(requestingUserId.Value) && requestingUserId.Value != userId))
                {
                    return Forbid("Você não tem permissão para remover este investimento");
                }

                // Verificar se o investimento pertence ao usuário
                var existingInvestment = await _userInvestmentService.GetUserInvestmentByIdAsync(id);
                if (existingInvestment == null || existingInvestment.UserId != userId)
                {
                    return NotFound("Investimento não encontrado");
                }

                var result = await _userInvestmentService.DeleteUserInvestmentAsync(id);
                if (!result)
                {
                    return NotFound("Investimento não encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user investment {UserInvestmentId} for user {UserId}", id, userId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
