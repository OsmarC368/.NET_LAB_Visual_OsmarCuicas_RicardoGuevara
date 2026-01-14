using Core.Entities;

namespace Core.Interfaces.Repositories
{
	public interface IUserRepository : IBaseRepository<User>
	{
		ValueTask<User> Login(string email, string password);
		ValueTask<User> GetByEmail(string email);
	}
}
