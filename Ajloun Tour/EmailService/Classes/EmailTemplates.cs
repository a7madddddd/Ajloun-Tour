using Ajloun_Tour.DTOs2.PaymentDTOs;

namespace Ajloun_Tour.EmailService.Classes
{
    // Services/EmailTemplates.cs
    public static class EmailTemplates
    {
        public static string GetPaymentConfirmationTemplate(string recipientName, PaymentDTO payment)
        {
            return $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h1 style='color: #2c3e50;'>Payment Confirmation</h1>
                    <p>Dear {recipientName},</p>
                    <p>Thank you for your payment. Here are your payment details:</p>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p><strong>Payment ID:</strong> {payment.PaymentId}</p>
                        <p><strong>Amount Paid:</strong> ${payment.Amount:F2}</p>
                        <p><strong>Payment Method:</strong> {payment.PaymentMethod}</p>
                        <p><strong>Transaction ID:</strong> {payment.TransactionId}</p>
                        <p><strong>Date:</strong> {payment.CreatedAt:g}</p>
                    </div>

                    <p>If you have any questions about your payment, please don't hesitate to contact us.</p>
                    
                    <p style='margin-top: 30px;'>
                        Best regards,<br>
                        The Ajloun Tour Team
                    </p>
                </div>
            </body>
            </html>";
        }
    }
}
