using Microsoft.EntityFrameworkCore;
using InvestmentApp.Data;
using InvestmentApp.Common.Models;

namespace InvestmentApp.Common.Repository
{
    public class UserInvestmentRepository : IUserInvestmentRepository
    {
        private readonly InvestmentAppDbContext _context;

        public UserInvestmentRepository(InvestmentAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserInvestment>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserInvestments
                .Include(ui => ui.Investment)
                .Include(ui => ui.User)
                .Where(ui => ui.UserId == userId && ui.IsActive)
                .OrderByDescending(ui => ui.PurchaseDate)
                .ToListAsync();
        }

        public async Task<UserInvestment?> GetByIdAsync(Guid id)
        {
            return await _context.UserInvestments
                .Include(ui => ui.Investment)
                .Include(ui => ui.User)
                .Where(ui => ui.Id == id && ui.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<UserInvestment?> GetByUserAndInvestmentAsync(Guid userId, Guid investmentId)
        {
            return await _context.UserInvestments
                .Include(ui => ui.Investment)
                .Include(ui => ui.User)
                .Where(ui => ui.UserId == userId && ui.InvestmentId == investmentId && ui.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<UserInvestment> CreateAsync(UserInvestment userInvestment)
        {
            userInvestment.CreatedAt = DateTime.UtcNow;
            userInvestment.UpdatedAt = DateTime.UtcNow;
            
            _context.UserInvestments.Add(userInvestment);
            await _context.SaveChangesAsync();
            
            // Reload with includes
            return await GetByIdAsync(userInvestment.Id) ?? userInvestment;
        }

        public async Task<UserInvestment> UpdateAsync(UserInvestment userInvestment)
        {
            userInvestment.UpdatedAt = DateTime.UtcNow;
            
            _context.UserInvestments.Update(userInvestment);
            await _context.SaveChangesAsync();
            
            // Reload with includes
            return await GetByIdAsync(userInvestment.Id) ?? userInvestment;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var userInvestment = await _context.UserInvestments.FindAsync(id);
            if (userInvestment == null) return false;

            userInvestment.IsActive = false;
            userInvestment.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.UserInvestments
                .AnyAsync(ui => ui.Id == id && ui.IsActive);
        }
    }
}
