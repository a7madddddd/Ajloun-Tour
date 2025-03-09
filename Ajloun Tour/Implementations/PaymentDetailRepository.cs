using Ajloun_Tour.DTOs2.PaymentDetailDTOs;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Tour.Implementations
{
    public class PaymentDetailRepository : IPaymentDetailRepository
    {
        private readonly MyDbContext _context;

        public PaymentDetailRepository(MyDbContext context)
        {
            _context = context;
        }

        private static string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber)) return "";
            return "XXXX-XXXX-XXXX-" + cardNumber.Substring(cardNumber.Length - 4);
        }


        public async Task<IEnumerable<PaymentDetailDTO>> GetAllPaymentDetails()
        {
            var paymentDetails = await _context.PaymentDetails
                .ToListAsync();  

            return paymentDetails.Select(pd => new PaymentDetailDTO
            {
                PaymentDetailId = pd.PaymentDetailId,
                PaymentId = pd.PaymentId,
                CardHolderName = pd.CardHolderName,
                CardNumber = MaskCardNumber(pd.CardNumber),  
                ExpiryDate = pd.ExpiryDate,
                BillingAddress = pd.BillingAddress,
                BillingCity = pd.BillingCity,
                BillingCountry = pd.BillingCountry,
                BillingZipCode = pd.BillingZipCode,
                AdditionalNotes = pd.AdditionalNotes,
                Cvv = pd.Cvv,
            }).ToList();
        }

        public async Task<PaymentDetailDTO> GetPaymentDetailById(int id)
        {
            var detail = await _context.PaymentDetails
                .FirstOrDefaultAsync(pd => pd.PaymentDetailId == id);

            if (detail == null)
                return null;

            return new PaymentDetailDTO
            {
                PaymentDetailId = detail.PaymentDetailId,
                PaymentId = detail.PaymentId,
                CardHolderName = detail.CardHolderName,
                CardNumber = MaskCardNumber(detail.CardNumber),
                ExpiryDate = detail.ExpiryDate,
                BillingAddress = detail.BillingAddress,
                BillingCity = detail.BillingCity,
                BillingCountry = detail.BillingCountry,
                BillingZipCode = detail.BillingZipCode,
                AdditionalNotes = detail.AdditionalNotes,
                Cvv = detail.Cvv,
            };
        }

        public async Task<PaymentDetailDTO> GetPaymentDetailByPaymentId(int paymentId)
        {
            var detail = await _context.PaymentDetails
                .FirstOrDefaultAsync(pd => pd.PaymentId == paymentId);

            if (detail == null)
                return null;

            return new PaymentDetailDTO
            {
                PaymentDetailId = detail.PaymentDetailId,
                PaymentId = detail.PaymentId,
                CardHolderName = detail.CardHolderName,
                CardNumber = MaskCardNumber(detail.CardNumber),
                ExpiryDate = detail.ExpiryDate,
                BillingAddress = detail.BillingAddress,
                BillingCity = detail.BillingCity,
                BillingCountry = detail.BillingCountry,
                BillingZipCode = detail.BillingZipCode,
                AdditionalNotes = detail.AdditionalNotes,
                Cvv = detail.Cvv,
            };
        }

        public async Task<PaymentDetailDTO> AddPaymentDetail(CreatePaymentDetail createDetail)
        {
            var detail = new PaymentDetail
            {
                PaymentId = createDetail.PaymentID,
                CardHolderName = createDetail.CardHolderName,
                CardNumber = createDetail.CardNumber, // Should be encrypted
                ExpiryDate = createDetail.ExpiryDate,
                Cvv = createDetail.CVV, // Should be encrypted
                BillingAddress = createDetail.BillingAddress,
                BillingCity = createDetail.BillingCity,
                BillingCountry = createDetail.BillingCountry,
                BillingZipCode = createDetail.BillingZipCode,
                AdditionalNotes = createDetail.AdditionalNotes,
            };

            _context.PaymentDetails.Add(detail);
            await _context.SaveChangesAsync();

            return new PaymentDetailDTO
            {
                PaymentDetailId = detail.PaymentDetailId,
                PaymentId = detail.PaymentId,
                CardHolderName = detail.CardHolderName,
                CardNumber = MaskCardNumber(detail.CardNumber),
                ExpiryDate = detail.ExpiryDate,
                BillingAddress = detail.BillingAddress,
                BillingCity = detail.BillingCity,
                BillingCountry = detail.BillingCountry,
                BillingZipCode = detail.BillingZipCode,
                AdditionalNotes = detail.AdditionalNotes,
            };
        }

        public async Task<PaymentDetailDTO> UpdatePaymentDetail(int id, CreatePaymentDetail createDetail)
        {
            var detail = await _context.PaymentDetails.FindAsync(id);
            if (detail == null)
                return null;

            detail.CardHolderName = createDetail.CardHolderName;
            detail.CardNumber = createDetail.CardNumber; // Should be encrypted
            detail.ExpiryDate = createDetail.ExpiryDate;
            detail.Cvv = createDetail.CVV; // Should be encrypted
            detail.BillingAddress = createDetail.BillingAddress;
            detail.BillingCity = createDetail.BillingCity;
            detail.BillingCountry = createDetail.BillingCountry;
            detail.BillingZipCode = createDetail.BillingZipCode;
            detail.AdditionalNotes = createDetail.AdditionalNotes;

            await _context.SaveChangesAsync();

            return new PaymentDetailDTO
            {
                PaymentDetailId = detail.PaymentDetailId,
                PaymentId = detail.PaymentId,
                CardHolderName = detail.CardHolderName,
                CardNumber = MaskCardNumber(detail.CardNumber),
                ExpiryDate = detail.ExpiryDate,
                BillingAddress = detail.BillingAddress,
                BillingCity = detail.BillingCity,
                BillingCountry = detail.BillingCountry,
                BillingZipCode = detail.BillingZipCode,
                AdditionalNotes = detail.AdditionalNotes,
            };
        }

        public async Task DeletePaymentDetail(int id)
        {
            var detail = await _context.PaymentDetails.FindAsync(id);
            if (detail != null)
            {
                _context.PaymentDetails.Remove(detail);
                await _context.SaveChangesAsync();
            }
        }
    }
}
