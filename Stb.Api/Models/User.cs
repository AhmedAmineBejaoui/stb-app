namespace Stb.Api.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Userfirstname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Mobile { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = "User";

    public ICollection<Demande> Demandes { get; set; } = new List<Demande>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

