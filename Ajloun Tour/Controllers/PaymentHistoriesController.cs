using Ajloun_Tour.DTOs2.PaymentHistoryDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentHistoriesController : ControllerBase
    {
        private readonly IPaymentHistoryRepository _historyRepository;

        public PaymentHistoriesController(IPaymentHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentHistoryDTO>>> GetAllPaymentHistory()
        {
            var history = await _historyRepository.GetAllPaymentHistory();
            return Ok(history);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentHistoryDTO>> GetPaymentHistoryById(int id)
        {
            var history = await _historyRepository.GetPaymentHistoryById(id);
            if (history == null)
                return NotFound();

            return Ok(history);
        }

        [HttpGet("payment/{paymentId}")]
        public async Task<ActionResult<IEnumerable<PaymentHistoryDTO>>> GetPaymentHistoryByPaymentId(int paymentId)
        {
            var history = await _historyRepository.GetPaymentHistoryByPaymentId(paymentId);
            return Ok(history);
        }

        [HttpGet("payment/{paymentId}/latest")]
        public async Task<ActionResult<IEnumerable<PaymentHistoryDTO>>> GetLatestHistoryByPaymentId(int paymentId, [FromQuery] int count = 5)
        {
            var history = await _historyRepository.GetLatestHistoryByPaymentId(paymentId, count);
            return Ok(history);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentHistoryDTO>> CreatePaymentHistory([FromBody] CreatePaymentHistory createHistory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var history = await _historyRepository.AddPaymentHistory(createHistory);
            return CreatedAtAction(nameof(GetPaymentHistoryById), new { id = history.PaymentHistoryId }, history);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentHistory(int id)
        {
            await _historyRepository.DeletePaymentHistory(id);
            return NoContent();
        }
    }
}
