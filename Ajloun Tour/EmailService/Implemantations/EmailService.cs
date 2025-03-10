namespace Ajloun_Tour.EmailService.Implemantations;
using Ajloun_Tour.DTOs2.PaymentDTOs;
using Ajloun_Tour.EmailService.Classes;
using Ajloun_Tour.EmailService.Interfaces;
using Ajloun_Tour.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Drawing;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

public class EmailService : IEmailService
{
    private readonly MyDbContext _context;
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger, MyDbContext context)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
        _context = context;
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

            // Get booking details
            var booking = await _context.Bookings
                .Include(b => b.Package)
                .Include(b => b.Tour)
                .Include(b => b.Offer)
                .FirstOrDefaultAsync(b => b.BookingId == payment.BookingId);

            if (booking == null)
                throw new Exception("Booking not found");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(recipientName, recipientEmail));
            email.Subject = $"Booking Confirmation - Ajloun Tour #{payment.PaymentId}";

            var builder = new BodyBuilder();
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
                        .summary-table {{
                            width: 100%;
                            border-collapse: collapse;
                            margin: 20px 0;
                        }}
                        .summary-table td {{
                            padding: 10px;
                            border-bottom: 1px solid #eee;
                        }}
                        .summary-table .text-right {{
                            text-align: right;
                        }}
                        .summary-table .total {{
                            background-color: #f8f9fa;
                            font-weight: bold;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            background-color: #0088cc;
                            color: white;
                            text-decoration: none;
                            border-radius: 5px;
                            margin-top: 20px;
                        }}
                        .footer {{
                            background-color: #f8f9fa;
                            padding: 20px;
                            text-align: center;
                            border-top: 1px solid #dee2e6;
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
                                <p><strong>Payment Reference:</strong> #{payment.PaymentId}</p>
                                <p><strong>Transaction ID:</strong> {payment.TransactionId}</p>
                                <p><strong>Payment Method:</strong> {payment.PaymentMethod}</p>
                                <p><strong>Date:</strong> {payment.CreatedAt:dd MMM yyyy HH:mm}</p>
                                <p><strong>Total Amount Paid:</strong> ${payment.Amount:F2}</p>
                            </div>

                             <div class='booking-details'>
                                    <h3>Booking Summary</h3>
                                    <table class='summary-table'>
                                        <tbody>
                                            <tr>
                                                <td><strong>Booking Reference:</strong></td>
                                                <td class='text-right'>#{booking.BookingId}</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Booking Date:</strong></td>
                                                <td class='text-right'>{booking.BookingDate:dd MMM yyyy}</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Number of People:</strong></td>
                                                <td class='text-right'>{booking.NumberOfPeople}</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Base Price:</strong></td>
                                                <td class='text-right'>${booking.TotalPrice:F2}</td>
                                            </tr>

                                            <!-- Add Selected Options -->
                                            {await GetSelectedOptionsHtmlAsync(booking.BookingId)}

                                            <!-- Add Tax -->
                                            <tr>
                                                <td><strong>Tax (1%):</strong></td>
                                                <td class='text-right'>${(payment.Amount ?? 0m) * 0.01m:F2}</td>
                                            </tr>
                                            <tr class='total'>
                                                <td><strong>Total Amount:</strong></td>
                                                <td class='text-right'><strong>${payment.Amount:F2}</strong></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                            <p>You can view your booking details and manage your reservation by clicking the button below:</p>
                            
                            <center>
                                <a href='https://yourdomain.com/bookings/{booking.BookingId}' class='button'>
                                    View My Booking
                                </a>
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
















    private async Task<string> GetSelectedOptionsHtmlAsync(int bookingId)
    {
        var selectedOptions = await _context.BookingOptionSelections
            .Include(bos => bos.Option)
            .Where(bos => bos.BookingId == bookingId)
            .ToListAsync();

        if (!selectedOptions.Any())
            return string.Empty;

        var optionsHtml = new StringBuilder();
        optionsHtml.Append(@"<tr><td colspan='2' class='options-header'><strong>Selected Options:</strong></td></tr>");

        foreach (var selection in selectedOptions)
        {
            optionsHtml.Append($@"
            <tr class='option-item'>
                <td style='padding-left: 20px;'>• {selection.Option.OptionName}</td>
                <td class='text-right'>${selection.Option.OptionPrice:F2}</td>
            </tr>
        ");
        }

        var totalOptionsCost = selectedOptions.Sum(s => s.Option.OptionPrice);
        optionsHtml.Append($@"
        <tr class='total-row'>
            <td><strong>Options Total:</strong></td>
            <td class='text-right'>${totalOptionsCost:F2}</td>
        </tr>
    ");

        return optionsHtml.ToString();
    }
}