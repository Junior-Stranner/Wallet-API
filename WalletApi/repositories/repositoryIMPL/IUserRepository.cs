using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByIdAsync(int id);
    Task AddAsync(User user);
}
