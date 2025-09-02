namespace StbApp.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Userfirstname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User";

        public ICollection<Demande> Demandes { get; set; } = new List<Demande>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}

