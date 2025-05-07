using Microsoft.EntityFrameworkCore;

public class TransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Transactions.Where(t => t.SenderWallet.UserId == userId || t.ReceiverWallet.UserId == userId);

        if (startDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate >= startDate.Value.ToUniversalTime());
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.TransactionDate <= endDate.Value.ToUniversalTime());
        }

        return await query.OrderByDescending(t => t.TransactionDate).ToListAsync();
    }
}