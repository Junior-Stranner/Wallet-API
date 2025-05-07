using Microsoft.EntityFrameworkCore;

public class WalletRepository
{
    private readonly AppDbContext _context;

    public WalletRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet> GetByIdAsync(int id)
    {
        return await _context.Wallets.FindAsync(id);
    }

    public async Task<Wallet> GetByUserIdAsync(int userId)
    {
        return await _context.Wallets.SingleOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task AddAsync(Wallet wallet)
    {
        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Wallet wallet)
    {
        _context.Entry(wallet).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}
