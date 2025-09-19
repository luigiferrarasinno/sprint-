using Microsoft.EntityFrameworkCore;
using InvestmentApp.Data;
using InvestmentApp.User.Model;

namespace InvestmentApp.User.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly InvestmentAppDbContext _context;

        public UserRepository(InvestmentAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Model.User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }

        public async Task<Model.User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Where(u => u.Id == id && u.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<Model.User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Where(u => u.Email.ToLower() == email.ToLower() && u.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<Model.User?> GetByCpfAsync(string cpf)
        {
            return await _context.Users
                .Where(u => u.CPF == cpf && u.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<Model.User> CreateAsync(Model.User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Model.User> UpdateAsync(Model.User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Users
                .AnyAsync(u => u.Id == id && u.IsActive);
        }
    }
}
