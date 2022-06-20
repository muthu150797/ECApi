using EcAPI.Entities;

namespace EcAPI.Interfaces
{
	public interface IUserRepository
	{
		public dynamic? GetUser(UserModel userMode);
	}
}
