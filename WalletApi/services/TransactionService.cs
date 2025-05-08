using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class TransactionService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionService(AppDbContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _httpContextAccessor = accessor;
    }

    public async Task TransferAsync(int toUserId, decimal amount)
    {
        var fromUserId = GetUserId();
        if (fromUserId == toUserId)
            throw new Exception("Você não pode transferir para si mesmo.");

        var fromWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == fromUserId);
        var toWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == toUserId);

        if (fromWallet == null || toWallet == null)
            throw new Exception("Carteira(s) não encontrada(s)");

        if (fromWallet.Balance < amount)
            throw new Exception("Saldo insuficiente");

        fromWallet.Balance -= amount;
        toWallet.Balance += amount;

        var transaction = new Transaction
        {
            FromUserId = fromUserId,
            ToUserId = toUserId,
            Amount = amount,
            Date = DateTime.UtcNow
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Transaction>> GetTransactionsAsync(DateTime? start, DateTime? end)
    {
        var userId = GetUserId();

        var query = _context.Transactions
            .Where(t => t.FromUserId == userId || t.ToUserId == userId);

        if (start.HasValue)
            query = query.Where(t => t.Date >= start.Value);

        if (end.HasValue)
            query = query.Where(t => t.Date <= end.Value);

        return await query
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    private int GetUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) throw new Exception("Usuário não autenticado");
        return int.Parse(claim.Value);
    }
}
