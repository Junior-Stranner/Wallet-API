 public interface ITransactionRepository
    {
        Task<Transaction> GetByIdAsync(int id);
        Task<IEnumerable<Transaction>> GetTransactionsByUserAsync(string userId, DateTime? startDate = null, DateTime? endDate = null, bool asSender = true, bool asReceiver = true);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<IDictionary<string, object>> GetTransactionStatsAsync(string userId);
    }