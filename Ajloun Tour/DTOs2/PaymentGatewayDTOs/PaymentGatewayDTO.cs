namespace Ajloun_Tour.DTOs2.PaymentGatewayDTOs
{
    public class PaymentGatewayDTO
    {
        public int GatewayId { get; set; }
        public string? GatewayName { get; set; }
        public bool? IsActive { get; set; }
        public string? ApiKey { get; set; }
        public string? SecretKey { get; set; }
        public string? WebhookUrl { get; set; }
        public string? Environment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
