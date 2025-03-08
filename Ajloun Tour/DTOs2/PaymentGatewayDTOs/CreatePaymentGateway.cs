using System.ComponentModel.DataAnnotations;

namespace Ajloun_Tour.DTOs2.PaymentGatewayDTOs
{
    public class CreatePaymentGateway
    {
        [Required]
        public string? GatewayName { get; set; }
        [Required]
        public string? ApiKey { get; set; }
        [Required]
        public string? SecretKey { get; set; }
        public string? WebhookUrl { get; set; }
        [Required]
        public string? Environment { get; set; }
    }
}
