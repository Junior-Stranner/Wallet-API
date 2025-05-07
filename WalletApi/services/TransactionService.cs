public class TransactionService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionService(ApplicationDbContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _httpContextAccessor = accessor;
    }

    public void Transfer(int toUserId, decimal amount)
    {
        var fromUserId = GetUserId();
        if (fromUserId == toUserId)
            throw new Exception("Você não pode transferir para si mesmo.");

        var fromWallet = _context.Wallets.FirstOrDefault(w => w.UserId == fromUserId);
        var toWallet = _context.Wallets.FirstOrDefault(w => w.UserId == toUserId);

        if (fromWallet == null || toWallet == null)
            throw new Exception("Carteira(s) não encontrada(s)");

        if (fromWallet.Balance < amount)
            throw new Exception("Saldo insuficiente");

        // Efetua a transferência
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
        _context.SaveChanges();
    }

    public List<Transaction> GetTransactions(DateTime? start, DateTime? end)
    {
        var userId = GetUserId();

        var query = _context.Transactions
            .Where(t => t.FromUserId == userId || t.ToUserId == userId);

        if (start.HasValue)
            query = query.Where(t => t.Date >= start.Value);

        if (end.HasValue)
            query = query.Where(t => t.Date <= end.Value);

        return query
            .OrderByDescending(t => t.Date)
            .ToList();
    }

    private int GetUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) throw new Exception("Usuário não autenticado");
        return int.Parse(claim.Value);
    }
}
