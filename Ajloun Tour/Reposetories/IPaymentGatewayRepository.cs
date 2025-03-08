using Ajloun_Tour.DTOs2.PaymentGatewayDTOs;

namespace Ajloun_Tour.Reposetories
{
    public interface IPaymentGatewayRepository
    {
        Task<IEnumerable<PaymentGatewayDTO>> GetAllGateways();
        Task<PaymentGatewayDTO> GetGatewayById(int id);
        Task<PaymentGatewayDTO> AddGateway(CreatePaymentGateway createGateway);
        Task<PaymentGatewayDTO> UpdateGateway(int id, CreatePaymentGateway createGateway);
        Task DeleteGateway(int id);
        Task<IEnumerable<PaymentGatewayDTO>> GetActiveGateways();
    }
}
