using System.Security.Cryptography;
using System.Text;


public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public User Register(string email, string password)
    {
        if (_context.Users.Any(u => u.Email == email))
            throw new Exception("Usuário já existe.");

        var passwordHash = HashPassword(password);
        var user = new User { Email = email, PasswordHash = passwordHash };

        _context.Users.Add(user);
        _context.SaveChanges();

        // Cria carteira vazia
        var wallet = new Wallet { Balance = 0, UserId = user.Id };
        _context.Wallets.Add(wallet);
        _context.SaveChanges();

        return user;
    }

    public string Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
            throw new Exception("Credenciais inválidas");

        return GenerateJwtToken(user);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }


}
