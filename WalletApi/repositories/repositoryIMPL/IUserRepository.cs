public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user, string password);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string id);
    }
