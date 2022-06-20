using EcAPI.Interfaces;
using EcAPI.Entities;

namespace EcAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<UserDTO> users = new List<UserDTO>();

        public UserRepository()
        {
            users.Add(new UserDTO
            {
                UserName = "joydipkanjilal",
                Role = "manager"
            });
            users.Add(new UserDTO
            {
                UserName = "michaelsanders",
                Role = "developer"
            });
            users.Add(new UserDTO
            {
                UserName = "stephensmith",
                Role = "tester"
            });
            users.Add(new UserDTO
            {
                UserName = "rodpaddock",
                Role = "admin"
            });
            users.Add(new UserDTO
            {
                UserName = "rexwills",
                Role = "admin"
            });
        }
        public dynamic GetUser(UserModel userModel)
        {
            // dynamic userDetails=null;
            // using (var context = new MyDBContext())
            // {
            //     try
            //     {
            //         //checking User for admin login
            //         //userDetails = context.Accounts.Where(user => user.Email == userModel.UserName && user.Password == userModel.Password).First();
            //         return userDetails;
            //     }
            //     catch (Exception ex)
            //     {

            //         return userDetails;
            //     }
            // }
            return null;
            //return users.Where(x => x.UserName == "rodpaddock").First();
        }
    }



}
