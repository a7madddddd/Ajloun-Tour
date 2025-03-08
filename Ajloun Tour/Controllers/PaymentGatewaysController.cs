using Ajloun_Tour.DTOs2.PaymentGatewayDTOs;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ajloun_Tour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewaysController : ControllerBase
    {
        private readonly IPaymentGatewayRepository _gatewayRepository;

        public PaymentGatewaysController(IPaymentGatewayRepository gatewayRepository)
        {
            _gatewayRepository = gatewayRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentGatewayDTO>>> GetAllGateways()
        {
            var gateways = await _gatewayRepository.GetAllGateways();
            return Ok(gateways);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<PaymentGatewayDTO>>> GetActiveGateways()
        {
            var gateways = await _gatewayRepository.GetActiveGateways();
            return Ok(gateways);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentGatewayDTO>> GetGatewayById(int id)
        {
            var gateway = await _gatewayRepository.GetGatewayById(id);
            if (gateway == null)
                return NotFound();

            return Ok(gateway);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentGatewayDTO>> CreateGateway([FromBody] CreatePaymentGateway createGateway)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var gateway = await _gatewayRepository.AddGateway(createGateway);
            return CreatedAtAction(nameof(GetGatewayById), new { id = gateway.GatewayId }, gateway);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentGatewayDTO>> UpdateGateway(int id, [FromBody] CreatePaymentGateway createGateway)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var gateway = await _gatewayRepository.UpdateGateway(id, createGateway);
            if (gateway == null)
                return NotFound();

            return Ok(gateway);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGateway(int id)
        {
            await _gatewayRepository.DeleteGateway(id);
            return NoContent();
        }
    }
}
