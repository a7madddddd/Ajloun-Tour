using Ajloun_Tour.DTOs2.PaymentDetailDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailsController : ControllerBase
    {
        private readonly IPaymentDetailRepository _detailRepository;

        public PaymentDetailsController(IPaymentDetailRepository detailRepository)
        {
            _detailRepository = detailRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetailDTO>>> GetAllPaymentDetails()
        {
            var details = await _detailRepository.GetAllPaymentDetails();
            return Ok(details);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetailDTO>> GetPaymentDetailById(int id)
        {
            var detail = await _detailRepository.GetPaymentDetailById(id);
            if (detail == null)
                return NotFound();

            return Ok(detail);
        }

        [HttpGet("payment/{paymentId}")]
        public async Task<ActionResult<PaymentDetailDTO>> GetPaymentDetailByPaymentId(int paymentId)
        {
            var detail = await _detailRepository.GetPaymentDetailByPaymentId(paymentId);
            if (detail == null)
                return NotFound();

            return Ok(detail);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDetailDTO>> CreatePaymentDetail([FromBody] CreatePaymentDetail createDetail)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var detail = await _detailRepository.AddPaymentDetail(createDetail);
            return CreatedAtAction(nameof(GetPaymentDetailById), new { id = detail.PaymentDetailId }, detail);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentDetailDTO>> UpdatePaymentDetail(int id, [FromBody] CreatePaymentDetail createDetail)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var detail = await _detailRepository.UpdatePaymentDetail(id, createDetail);
            if (detail == null)
                return NotFound();

            return Ok(detail);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentDetail(int id)
        {
            await _detailRepository.DeletePaymentDetail(id);
            return NoContent();
        }
    }
}
