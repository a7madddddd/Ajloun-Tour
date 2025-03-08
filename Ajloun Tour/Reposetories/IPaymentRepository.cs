using Ajloun_Tour.DTOs2.PaymentDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<PaymentDTO>> GetAllPayments();
        Task<PaymentDTO> GetPaymentById(int id);
        Task<IEnumerable<PaymentDTO>> GetPaymentsByUserId(int userId);
        Task<PaymentDTO> AddPayment(CreatePayment createPayment);
        Task<PaymentDTO> UpdatePayment(int id, CreatePayment createPayment);
        Task DeletePayment(int id);
    }
}
