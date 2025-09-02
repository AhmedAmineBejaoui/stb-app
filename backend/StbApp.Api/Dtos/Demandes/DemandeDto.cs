namespace StbApp.Api.Dtos.Demandes
{
    public class DemandeDto
    {
        public int Id { get; set; }
        public string? RequestType { get; set; }
        public string? RequestSubType { get; set; }
        public string? Description { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Status { get; set; } = "En attente";
        public int UserId { get; set; }
    }
}

