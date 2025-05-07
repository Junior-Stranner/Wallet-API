   public interface IWalletRepository
    {
        Task<Wallet> GetByUserIdAsync(string userId);
        Task<decimal> GetBalanceAsync(string userId);
        Task<Wallet> AddFundsAsync(string userId, decimal amount);
        Task<(Wallet FromWallet, Wallet ToWallet)> TransferFundsAsync(string fromUserId, string toUserId, decimal amount);
    }