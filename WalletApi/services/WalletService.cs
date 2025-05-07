using System.Security.Claims;

public class WalletService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WalletService(AppDbContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _httpContextAccessor = accessor;
    }

    public decimal GetBalance()
    {
        var userId = GetUserId();
        var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == userId)
                     ?? throw new Exception("Carteira não encontrada");

        return wallet.Balance;
    }

    public decimal Deposit(decimal amount)
    {
        var userId = GetUserId();
        var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == userId)
                     ?? throw new Exception("Carteira não encontrada");

        wallet.Balance += amount;
        _context.SaveChanges();

        return wallet.Balance;
    }

    private int GetUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) throw new Exception("Usuário não autenticado");
        return int.Parse(claim.Value);
    }
}
