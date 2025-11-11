using BlogPersonal.Models;

namespace BlogPersonal.Services
{
    // Asegúrate de que UserService implemente la interfaz IUserService
    public class UserService : IUserService
    {
        private List<User> users = new List<User>
        {
            new User { Id = 1, FullName = "Augusto Rolandelli", Username = "admin", Password = "1411" },
            new User { Id = 2, FullName = "Rafaela Arce", Username = "rafita", Password = "1556" }
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
