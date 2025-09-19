using Microsoft.EntityFrameworkCore;
using InvestmentApp.Data;
using InvestmentApp.Investimento.Model;

namespace InvestmentApp.Investimento.Repository
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly InvestmentAppDbContext _context;

        public InvestmentRepository(InvestmentAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Investment>> GetAllAsync()
        {
            return await _context.Investments
                .Where(i => i.IsActive)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<Investment?> GetByIdAsync(Guid id)
        {
            return await _context.Investments
                .Where(i => i.Id == id && i.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<Investment> CreateAsync(Investment investment)
        {
            investment.CreatedAt = DateTime.UtcNow;
            investment.UpdatedAt = DateTime.UtcNow;
            
            _context.Investments.Add(investment);
            await _context.SaveChangesAsync();
            return investment;
        }

        public async Task<Investment> UpdateAsync(Investment investment)
        {
            investment.UpdatedAt = DateTime.UtcNow;
            
            _context.Investments.Update(investment);
            await _context.SaveChangesAsync();
            return investment;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var investment = await _context.Investments.FindAsync(id);
            if (investment == null) return false;

            investment.IsActive = false;
            investment.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Investments
                .AnyAsync(i => i.Id == id && i.IsActive);
        }
    }
}
