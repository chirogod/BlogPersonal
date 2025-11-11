using BlogPersonal.Models;

namespace BlogPersonal.Services
{ 
    public class UserService : IUserService
    {
        private List<User> users = new List<User>
        {
            new User { Id = 1, FullName = "Administrador", Username = "admin", Password = "1411" },
        };

        public bool IsUser(string username, string password)
        {
            bool isUser = false;
            foreach (var user in users)
            {
                if(user.Username == username && user.Password == password)
                {
                    isUser = true;
                    break;
                }
            }
            return isUser;
        }
    }
}
