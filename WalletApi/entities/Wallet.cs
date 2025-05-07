public class Wallet
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<Transaction> SentTransactions { get; set; } = new List<Transaction>();
    public List<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
}
