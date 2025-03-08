using Ajloun_Tour.DTOs2.PaymentHistoryDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class PaymentHistoryRepository : IPaymentHistoryRepository
    {
        private readonly MyDbContext _context;

        public PaymentHistoryRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentHistoryDTO>> GetAllPaymentHistory()
        {
            return await _context.PaymentHistories
                .OrderByDescending(ph => ph.CreatedAt)
                .Select(ph => new PaymentHistoryDTO
                {
                    PaymentHistoryId = ph.PaymentHistoryId,
                    PaymentId = ph.PaymentId,
                    Status = ph.Status,
                    Message = ph.Message,
                    CreatedAt = ph.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<PaymentHistoryDTO> GetPaymentHistoryById(int id)
        {
            var history = await _context.PaymentHistories
                .FirstOrDefaultAsync(ph => ph.PaymentHistoryId == id);

            if (history == null)
                return null;

            return new PaymentHistoryDTO
            {
                PaymentHistoryId = history.PaymentHistoryId,
                PaymentId = history.PaymentId,
                Status = history.Status,
                Message = history.Message,
                CreatedAt = history.CreatedAt
            };
        }

        public async Task<IEnumerable<PaymentHistoryDTO>> GetPaymentHistoryByPaymentId(int paymentId)
        {
            return await _context.PaymentHistories
                .Where(ph => ph.PaymentId == paymentId)
                .OrderByDescending(ph => ph.CreatedAt)
                .Select(ph => new PaymentHistoryDTO
                {
                    PaymentHistoryId = ph.PaymentHistoryId,
                    PaymentId = ph.PaymentId,
                    Status = ph.Status,
                    Message = ph.Message,
                    CreatedAt = ph.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PaymentHistoryDTO>> GetLatestHistoryByPaymentId(int paymentId, int count = 5)
        {
            return await _context.PaymentHistories
                .Where(ph => ph.PaymentId == paymentId)
                .OrderByDescending(ph => ph.CreatedAt)
                .Take(count)
                .Select(ph => new PaymentHistoryDTO
                {
                    PaymentHistoryId = ph.PaymentHistoryId,
                    PaymentId = ph.PaymentId,
                    Status = ph.Status,
                    Message = ph.Message,
                    CreatedAt = ph.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<PaymentHistoryDTO> AddPaymentHistory(CreatePaymentHistory createHistory)
        {
            var history = new PaymentHistory
            {
                PaymentId = createHistory.PaymentID,
                Status = createHistory.Status,
                Message = createHistory.Message,
                CreatedAt = DateTime.UtcNow
            };

            _context.PaymentHistories.Add(history);
            await _context.SaveChangesAsync();

            return new PaymentHistoryDTO
            {
                PaymentHistoryId = history.PaymentHistoryId,
                PaymentId = history.PaymentId,
                Status = history.Status,
                Message = history.Message,
                CreatedAt = history.CreatedAt
            };
        }

        public async Task DeletePaymentHistory(int id)
        {
            var history = await _context.PaymentHistories.FindAsync(id);
            if (history != null)
            {
                _context.PaymentHistories.Remove(history);
                await _context.SaveChangesAsync();
            }
        }
    }
}
