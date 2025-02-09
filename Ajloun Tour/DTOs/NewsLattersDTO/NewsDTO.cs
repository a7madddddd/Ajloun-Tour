namespace Ajloun_Tour.DTOs.NewsLattersDTO
{
    public class NewsDTO
    {
        public int SubscriberId { get; set; }
        public string Email { get; set; } = null!;
        public DateTime? SubscribedAt { get; set; }
    }
}
