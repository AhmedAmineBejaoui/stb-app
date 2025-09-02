namespace StbApp.Api.Dtos.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? CreationDate { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public int DemandeId { get; set; }
    }
}

