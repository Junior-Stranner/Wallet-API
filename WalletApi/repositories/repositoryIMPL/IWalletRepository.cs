using System.Threading.Tasks;

public interface IWalletRepository
{
    Task<Wallet> GetByUserIdAsync(int userId);
    Task UpdateAsync(Wallet wallet);
}
