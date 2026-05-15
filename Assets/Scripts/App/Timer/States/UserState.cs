using App.Timer.Back.Models;

namespace App.Timer.States
{
    public class UserState
    {
        public string Username { get; set; } = null;
        public UserRole UserRole { get; set; }

        public bool Equals(UserState other)
        {
            if (other == null) return false;

            return Username == other.Username && UserRole == other.UserRole;
        }
    }
}