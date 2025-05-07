public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly WalletRepository _walletRepository; // Adicionado WalletRepository
    private readonly AppDbContext _context;

    public UserService(UserRepository userRepository, WalletRepository walletRepository, AppDbContext context) // Adicionado WalletRepository
    {
        _userRepository = userRepository;
        _walletRepository = walletRepository; // Injetado
        _context = context;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return null;

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email
        };
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
         if (user == null)
            return null;
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email
        };
    }

    public async Task AddUserAsync(UserCreateDto userDto)
    {
        // Validação de e-mail único pode ser feita aqui também, se necessário.

        var user = new User
        {
            Email = userDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
        };

        // Inicia uma transação para garantir a atomicidade da operação
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _userRepository.AddAsync(user); // Adiciona o usuário

            var wallet = new Wallet { Balance = 0, UserId = user.Id }; //cria a carteira
            await _walletRepository.AddAsync(wallet); //adiciona a carteira

            await transaction.CommitAsync(); //commit
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(); //rollback em caso de erro
            throw; // Relança a exceção para ser tratada na camada superior
        }
    }

    public async Task UpdateUserAsync(int id, UserDto userDto)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }

        existingUser.Email = userDto.Email; // Atualiza outros campos se necessário
        await _userRepository.UpdateAsync(existingUser);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }
        await _userRepository.DeleteAsync(user);
    }

     public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Email = user.Email
        }).ToList();
    }
}
