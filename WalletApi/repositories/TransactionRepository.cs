using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Transactions
            .Include(t => t.SenderWallet)
            .Include(t => t.ReceiverWallet)
            .Where(t => t.SenderWallet.UserId == userId || t.ReceiverWallet.UserId == userId)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(t => t.Timestamp >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(t => t.Timestamp <= endDate.Value);

        return await query.ToListAsync();
    }
}
