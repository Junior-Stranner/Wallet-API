public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public int SenderWalletId { get; set; }
    public Wallet SenderWallet { get; set; }
    public int ReceiverWalletId { get; set; }
    public Wallet ReceiverWallet { get; set; }
}
