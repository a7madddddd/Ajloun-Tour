using Ajloun_Tour.DTOs2.PaymentHistoryDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IPaymentHistoryRepository
    {
        Task<IEnumerable<PaymentHistoryDTO>> GetAllPaymentHistory();
        Task<PaymentHistoryDTO> GetPaymentHistoryById(int id);
        Task<IEnumerable<PaymentHistoryDTO>> GetPaymentHistoryByPaymentId(int paymentId);
        Task<PaymentHistoryDTO> AddPaymentHistory(CreatePaymentHistory createHistory);
        Task DeletePaymentHistory(int id);
        Task<IEnumerable<PaymentHistoryDTO>> GetLatestHistoryByPaymentId(int paymentId, int count = 5);
    }
}
