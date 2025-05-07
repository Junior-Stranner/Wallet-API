using System.Threading.Tasks;


public class WalletRepository : IWalletRepository
{
    private readonly ApplicationDbContext _context;

    public WalletRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet> GetByUserIdAsync(int userId)
    {
        return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task UpdateAsync(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync();
    }
}
