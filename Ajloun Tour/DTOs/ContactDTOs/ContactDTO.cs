namespace Ajloun_Tour.DTOs.ContactDTOs
{
    public class ContactDTO
    {
        public int MessageId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Subject { get; set; }
        public string Message { get; set; } = null!;
        public DateTime? SubmittedAt { get; set; }
        public bool? IsRead { get; set; }
    }
}
