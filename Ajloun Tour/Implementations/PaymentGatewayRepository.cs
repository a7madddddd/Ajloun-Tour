using Ajloun_Tour.DTOs2.PaymentGatewayDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class PaymentGatewayRepository : IPaymentGatewayRepository
    {
        private readonly MyDbContext _context;

        public PaymentGatewayRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentGatewayDTO>> GetAllGateways()
        {
            return await _context.PaymentGateways
                .Select(g => new PaymentGatewayDTO
                {
                    GatewayId = g.GatewayId,
                    GatewayName = g.GatewayName,
                    IsActive = g.IsActive,
                    ApiKey = g.ApiKey,
                    SecretKey = g.SecretKey,
                    WebhookUrl = g.WebhookUrl,
                    Environment = g.Environment,
                    CreatedAt = g.CreatedAt,
                    UpdatedAt = g.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<PaymentGatewayDTO> GetGatewayById(int id)
        {
            var gateway = await _context.PaymentGateways
                .FirstOrDefaultAsync(g => g.GatewayId == id);

            if (gateway == null)
                return null;

            return new PaymentGatewayDTO
            {
                GatewayId = gateway.GatewayId,
                GatewayName = gateway.GatewayName,
                IsActive = gateway.IsActive,
                ApiKey = gateway.ApiKey,
                SecretKey = gateway.SecretKey,
                WebhookUrl = gateway.WebhookUrl,
                Environment = gateway.Environment,
                CreatedAt = gateway.CreatedAt,
                UpdatedAt = gateway.UpdatedAt
            };
        }

        public async Task<IEnumerable<PaymentGatewayDTO>> GetActiveGateways()
        {
            return await _context.PaymentGateways
                .Where(g => g.IsActive)
                .Select(g => new PaymentGatewayDTO
                {
                    GatewayId = g.GatewayId,
                    GatewayName = g.GatewayName,
                    IsActive = g.IsActive,
                    ApiKey = g.ApiKey,
                    SecretKey = g.SecretKey,
                    WebhookUrl = g.WebhookUrl,
                    Environment = g.Environment,
                    CreatedAt = g.CreatedAt,
                    UpdatedAt = g.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<PaymentGatewayDTO> AddGateway(CreatePaymentGateway createGateway)
        {
            var gateway = new PaymentGateway
            {
                GatewayName = createGateway.GatewayName,
                IsActive = true,
                ApiKey = createGateway.ApiKey,
                SecretKey = createGateway.SecretKey,
                WebhookUrl = createGateway.WebhookUrl,
                Environment = createGateway.Environment,
                CreatedAt = DateTime.UtcNow
            };

            _context.PaymentGateways.Add(gateway);
            await _context.SaveChangesAsync();

            return new PaymentGatewayDTO
            {
                GatewayId = gateway.GatewayId,
                GatewayName = gateway.GatewayName,
                IsActive = gateway.IsActive,
                ApiKey = gateway.ApiKey,
                SecretKey = gateway.SecretKey,
                WebhookUrl = gateway.WebhookUrl,
                Environment = gateway.Environment,
                CreatedAt = gateway.CreatedAt,
                UpdatedAt = gateway.UpdatedAt
            };
        }

        public async Task<PaymentGatewayDTO> UpdateGateway(int id, CreatePaymentGateway createGateway)
        {
            var gateway = await _context.PaymentGateways.FindAsync(id);
            if (gateway == null)
                return null;

            gateway.GatewayName = createGateway.GatewayName;
            gateway.ApiKey = createGateway.ApiKey;
            gateway.SecretKey = createGateway.SecretKey;
            gateway.WebhookUrl = createGateway.WebhookUrl;
            gateway.Environment = createGateway.Environment;
            gateway.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new PaymentGatewayDTO
            {
                GatewayId = gateway.GatewayId,
                GatewayName = gateway.GatewayName,
                IsActive = gateway.IsActive,
                ApiKey = gateway.ApiKey,
                SecretKey = gateway.SecretKey,
                WebhookUrl = gateway.WebhookUrl,
                Environment = gateway.Environment,
                CreatedAt = gateway.CreatedAt,
                UpdatedAt = gateway.UpdatedAt
            };
        }

        public async Task DeleteGateway(int id)
        {
            var gateway = await _context.PaymentGateways.FindAsync(id);
            if (gateway != null)
            {
                _context.PaymentGateways.Remove(gateway);
                await _context.SaveChangesAsync();
            }
        }
    }
}
