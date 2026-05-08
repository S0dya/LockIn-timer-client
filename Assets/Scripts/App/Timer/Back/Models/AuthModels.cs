using System;

namespace App.Timer.Back.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
    }
    
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }

    public enum UserRole
    {
        User,
        Admin,
    }
}
