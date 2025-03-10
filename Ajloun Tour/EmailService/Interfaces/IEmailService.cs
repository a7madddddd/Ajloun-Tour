using Ajloun_Tour.DTOs2.PaymentDTOs;
using Ajloun_Tour.EmailService.Classes;

namespace Ajloun_Tour.EmailService.Interfaces
{
    public interface IEmailService
    {
        Task SendPaymentConfirmationEmailAsync(string recipientEmail, string recipientName, PaymentDTO payment);

    }
}
