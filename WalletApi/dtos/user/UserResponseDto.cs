public class UserResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public WalletDto? Wallet { get; set; }
}