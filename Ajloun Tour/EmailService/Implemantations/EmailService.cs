namespace Ajloun_Tour.EmailService.Implemantations;
using Ajloun_Tour.DTOs2.PaymentDTOs;
using Ajloun_Tour.EmailService.Classes;
using Ajloun_Tour.EmailService.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendPaymentConfirmationEmailAsync(string recipientEmail, string recipientName, PaymentDTO payment)
    {
        try
        {
            if (string.IsNullOrEmpty(recipientEmail))
                throw new ArgumentException("Recipient email is required");
            if (string.IsNullOrEmpty(recipientName))
                throw new ArgumentException("Recipient name is required");
            if (payment == null)
                throw new ArgumentException("Payment details are required");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(recipientName, recipientEmail));
            email.Subject = $"Booking Confirmation - Ajloun Tour #{payment.PaymentId}";

            var builder = new BodyBuilder();

            // Create HTML body with styling matching your website
            builder.HtmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{
                        font-family: 'Helvetica', Arial, sans-serif;
                        line-height: 1.6;
                        margin: 0;
                        padding: 0;
                        background-color: #f4f4f4;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background: white;
                        border-radius: 8px;
                        overflow: hidden;
                        box-shadow: 0 0 10px rgba(0,0,0,0.1);
                    }}
                    .header {{
                        background-color: #0088cc;
                        color: white;
                        padding: 20px;
                        text-align: center;
                    }}
                    .header h1 {{
                        margin: 0;
                        font-size: 24px;
                    }}
                    .content {{
                        padding: 20px;
                    }}
                    .booking-details {{
                        background-color: #f8f9fa;
                        border: 1px solid #dee2e6;
                        border-radius: 5px;
                        padding: 15px;
                        margin: 20px 0;
                    }}
                    .booking-details p {{
                        margin: 10px 0;
                        border-bottom: 1px solid #eee;
                        padding-bottom: 10px;
                    }}
                    .price {{
                        color: #ff6b6b;
                        font-size: 20px;
                        font-weight: bold;
                    }}
                    .footer {{
                        background-color: #f8f9fa;
                        padding: 20px;
                        text-align: center;
                        border-top: 1px solid #dee2e6;
                    }}
                    .button {{
                        display: inline-block;
                        padding: 10px 20px;
                        background-color: #ff6b6b;
                        color: white;
                        text-decoration: none;
                        border-radius: 5px;
                        margin-top: 20px;
                    }}
                    .social-links {{
                        margin-top: 20px;
                    }}
                    .social-links a {{
                        margin: 0 10px;
                        color: #0088cc;
                        text-decoration: none;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Booking Confirmation</h1>
                    </div>
                    <div class='content'>
                        <h2>Thank you for booking with Ajloun Tour!</h2>
                        <p>Dear {recipientName},</p>
                        <p>Your booking has been confirmed and your payment has been processed successfully.</p>
                        
                        <div class='booking-details'>
                            <h3>Payment Details</h3>
                            <p><strong>Booking Reference:</strong> #{payment.PaymentId}</p>
                            <p><strong>Transaction ID:</strong> {payment.TransactionId}</p>
                            <p><strong>Payment Method:</strong> {payment.PaymentMethod}</p>
                            <p><strong>Date:</strong> {payment.CreatedAt:dd MMM yyyy HH:mm}</p>
                            <p class='price'><strong>Total Amount Paid:</strong> ${payment.Amount:F2}</p>
                        </div>

                        <div class='booking-details'>
                            <h3>Package Details</h3>
                            <p><strong>Package:</strong> Castle Pack</p>
                            <p><strong>Duration:</strong> 2D/2N</p>
                            <p><strong>Location:</strong> Ajloun</p>
                        </div>

                        <p>You can view your booking details and manage your reservation by clicking the button below:</p>
                        
                        <center>
                            <a href='' class='button'>View My Booking</a>
                        </center>
                    </div>
                    
                    <div class='footer'>
                        <p>Need help? Contact our support team</p>
                        <p>Email: support@ajlountour.com</p>
                        <p>Phone: +962 XXX XXX XXX</p>
                        
                        <div class='social-links'>
                            <a href='#'>Facebook</a>
                            <a href='#'>Instagram</a>
                            <a href='#'>Twitter</a>
                        </div>
                        
                        <p style='font-size: 12px; color: #666; margin-top: 20px;'>
                            This is an automated message, please do not reply to this email.
                        </p>
                    </div>
                </div>
            </body>
            </html>";

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation($"Payment confirmation email sent to {recipientEmail}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending payment confirmation email: {ex.Message}");
            throw;
        }
    }
}
