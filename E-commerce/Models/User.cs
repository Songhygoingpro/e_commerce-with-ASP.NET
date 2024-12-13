namespace E_commerce.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; } // Plain text for demonstration
        public bool IsAdmin { get; set; }
    }
}
