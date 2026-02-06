using AppBlazor.Data.Models.Core.Responses;

namespace AppBlazor.Data.Models.Core.Interfaces.Services
{
	public interface IUserService : IBaseService<User>
	{
		Task<Response<ResponseLogin>> Login(string Email, string Password );
		Task<Response<RegisterResponse>> Register(RegisterResponse registerResponse);
	}
}
