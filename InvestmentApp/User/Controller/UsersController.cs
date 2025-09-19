using Microsoft.AspNetCore.Mvc;
using InvestmentApp.User.Service;
using InvestmentApp.User.Dto;

namespace InvestmentApp.User.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os usuários (apenas Admin)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers([FromHeader] Guid? userId)
        {
            try
            {
                // Verificar se é admin
                if (!userId.HasValue || !await _userService.IsAdminAsync(userId.Value))
                {
                    return Forbid("Apenas administradores podem listar todos os usuários");
                }

                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obtém um usuário por ID (Admin ou próprio usuário)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(Guid id, [FromHeader] Guid? userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("Usuário não encontrado");
                }

                // Verificar se é admin ou o próprio usuário
                if (userId.HasValue && (await _userService.IsAdminAsync(userId.Value) || userId.Value == id))
                {
                    return Ok(user);
                }

                return Forbid("Você não tem permissão para visualizar este usuário");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {UserId}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userService.CreateUserAsync(userCreateDto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualiza um usuário
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(Guid id, [FromBody] UserUpdateDto userUpdateDto, [FromHeader] Guid? userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se é admin ou o próprio usuário
                if (!userId.HasValue || (!await _userService.IsAdminAsync(userId.Value) && userId.Value != id))
                {
                    return Forbid("Você não tem permissão para atualizar este usuário");
                }

                var user = await _userService.UpdateUserAsync(id, userUpdateDto);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Remove um usuário (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id, [FromHeader] Guid? userId)
        {
            try
            {
                // Verificar se é admin
                if (!userId.HasValue || !await _userService.IsAdminAsync(userId.Value))
                {
                    return Forbid("Apenas administradores podem remover usuários");
                }

                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                {
                    return NotFound("Usuário não encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
