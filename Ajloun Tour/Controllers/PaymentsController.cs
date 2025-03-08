using Ajloun_Tour.DTOs2.PaymentDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentsController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetAllPayments()
        {
            var payments = await _paymentRepository.GetAllPayments();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDTO>> GetPaymentById(int id)
        {
            var payment = await _paymentRepository.GetPaymentById(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPaymentsByUserId(int userId)
        {
            var payments = await _paymentRepository.GetPaymentsByUserId(userId);
            return Ok(payments);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> CreatePayment([FromBody] CreatePayment createPayment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var payment = await _paymentRepository.AddPayment(createPayment);
            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentId }, payment);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentDTO>> UpdatePayment(int id, [FromBody] CreatePayment createPayment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var payment = await _paymentRepository.UpdatePayment(id, createPayment);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            await _paymentRepository.DeletePayment(id);
            return NoContent();
        }
    }
}
