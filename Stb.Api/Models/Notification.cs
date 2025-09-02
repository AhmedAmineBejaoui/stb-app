namespace Stb.Api.Models;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = null!;
    public DateTime CreationDate { get; set; }
    public bool IsRead { get; set; }

    public int UserId { get; set; }
    public int DemandeId { get; set; }

    public User? User { get; set; }
    public Demande? Demande { get; set; }
}

