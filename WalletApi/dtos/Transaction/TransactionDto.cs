public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public int SenderWalletId { get; set; }
    public int ReceiverWalletId { get; set; }
}