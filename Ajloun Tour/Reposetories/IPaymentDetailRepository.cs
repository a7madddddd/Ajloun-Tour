using Ajloun_Tour.DTOs2.PaymentDetailDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IPaymentDetailRepository
    {
        Task<IEnumerable<PaymentDetailDTO>> GetAllPaymentDetails();
        Task<PaymentDetailDTO> GetPaymentDetailById(int id);
        Task<PaymentDetailDTO> GetPaymentDetailByPaymentId(int paymentId);
        Task<PaymentDetailDTO> AddPaymentDetail(CreatePaymentDetail createDetail);
        Task<PaymentDetailDTO> UpdatePaymentDetail(int id, CreatePaymentDetail createDetail);
        Task DeletePaymentDetail(int id);
    }
}
