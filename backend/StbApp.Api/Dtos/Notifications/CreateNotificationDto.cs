namespace StbApp.Api.Dtos.Notifications
{
    public class CreateNotificationDto
    {
        public string Message { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int DemandeId { get; set; }
    }
}

