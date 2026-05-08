using App.Timer.Back.Models;

namespace App.Timer.States
{
    public class UserState
    {
        public string Username { get; set; } = null;
        public UserRole UserRole { get; set; }
    }
}