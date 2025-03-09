using Ajloun_Tour.DTOs2.PaymentDTOs;

namespace Ajloun_Tour.EmailService.Interfaces
{
    public interface IEmailService
    {
        Task SendPaymentConfirmationEmailAsync(string recipientEmail, string recipientName, PaymentDTO payment);

    }
}
