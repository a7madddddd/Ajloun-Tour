﻿using Ajloun_Tour.DTOs2.PaymentDTOs;
using Ajloun_Tour.EmailService.Interfaces;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MyDbContext _context;
        private readonly IEmailService _emailService;
        public PaymentRepository(MyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IEnumerable<PaymentDTO>> GetAllPayments()
        {
            return await _context.Payments
                .Select(p => new PaymentDTO
                {
                    PaymentId = p.PaymentId,
                    BookingId = p.BookingId,
                    UserId = p.UserId,
                    GatewayId = p.GatewayId,
                    CartId = p.CartId,
                    Amount = p.Amount,
                    PaymentStatus = p.PaymentStatus,
                    PaymentMethod = p.PaymentMethod,
                    TransactionId = p.TransactionId,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<PaymentDTO> GetPaymentById(int id)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment == null)
                return null;

            return new PaymentDTO
            {
                PaymentId = payment.PaymentId,
                BookingId = payment.BookingId,
                UserId = payment.UserId,
                GatewayId = payment.GatewayId,
                CartId = payment.CartId,
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus,
                PaymentMethod = payment.PaymentMethod,
                TransactionId = payment.TransactionId,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt
            };
        }

        public async Task<IEnumerable<PaymentDTO>> GetPaymentsByUserId(int userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .Select(p => new PaymentDTO
                {
                    PaymentId = p.PaymentId,
                    BookingId = p.BookingId,
                    UserId = p.UserId,
                    GatewayId = p.GatewayId,
                    CartId = p.CartId,
                    Amount = p.Amount,
                    PaymentStatus = p.PaymentStatus,
                    PaymentMethod = p.PaymentMethod,
                    TransactionId = p.TransactionId,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<PaymentDTO> AddPayment(CreatePayment createPayment)
        {
            // Check if gateway exists
            var gateway = await _context.PaymentGateways.FindAsync(createPayment.GatewayID);
            if (gateway == null)
            {
                throw new Exception($"Payment gateway with ID {createPayment.GatewayID} not found");
            }

            var payment = new Payment
            {
                BookingId = createPayment.BookingID,
                UserId = createPayment.UserID,
                GatewayId = createPayment.GatewayID,
                CartId = createPayment.CartId,
                Amount = createPayment.Amount,
                PaymentMethod = createPayment.PaymentMethod,
                PaymentStatus = createPayment.PaymentStatus ?? "Pending",
                TransactionId = createPayment.TransactionId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var paymentDTO = new PaymentDTO
            {
                PaymentId = payment.PaymentId,
                BookingId = payment.BookingId,
                UserId = payment.UserId,
                GatewayId = payment.GatewayId,
                CartId = payment.CartId,
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus,
                PaymentMethod = payment.PaymentMethod,
                TransactionId = payment.TransactionId,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt
            };

            // Send confirmation email
            try
            {
                var user = await _context.Users.FindAsync(payment.UserId);
                if (user != null)
                {
                    await _emailService.SendPaymentConfirmationEmailAsync(
                        user.Email,
                        user.FullName,
                        paymentDTO
                    );
                }
            }
            catch (Exception emailEx)
            {
                // Log the email error but don't throw it
                // This allows the payment process to continue even if email fails
                Console.WriteLine($"Email sending failed: {emailEx.Message}");
            }

            return paymentDTO;
        }



        public async Task<PaymentDTO> UpdatePayment(int id, CreatePayment createPayment)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return null;

            payment.BookingId = createPayment.BookingID;
            payment.UserId = createPayment.UserID;
            payment.GatewayId = createPayment.GatewayID;
            payment.CartId = createPayment.CartId;
            payment.Amount = createPayment.Amount;
            payment.PaymentMethod = createPayment.PaymentMethod;
            payment.PaymentStatus = createPayment.PaymentStatus;
            payment.TransactionId = createPayment.TransactionId; // Add this line
            payment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new PaymentDTO
            {
                PaymentId = payment.PaymentId,
                BookingId = payment.BookingId,
                UserId = payment.UserId,
                GatewayId = payment.GatewayId,
                CartId = payment.CartId,
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus,
                PaymentMethod = payment.PaymentMethod,
                TransactionId = payment.TransactionId, // Include in response
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt
            };
        }
        public async Task DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
