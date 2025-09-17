using KosDrive.Data;
using KosDrive.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add-payment-method")]
        public async Task<IActionResult> AddPaymentMethod([FromBody] PaymentDto dto)
        {
            var customerOptions = new CustomerCreateOptions
            {
                Email = dto.Email,
            };

            var customerService = new CustomerService();
            var customer = await customerService.CreateAsync(customerOptions);

            var paymentMethodService = new PaymentMethodService();
            await paymentMethodService.AttachAsync(dto.PaymentMethodId, new PaymentMethodAttachOptions
            {
                Customer = customer.Id,
            });

            var options = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = dto.PaymentMethodId
                }
            };

            await customerService.UpdateAsync(customer.Id, options); 

            return Ok(new { CustomerId = customer.Id });
        }

        [HttpGet("payment-method-breakdown")]
        public async Task<IActionResult> GetPaymentBreakdown()
        {
            var breakdown = await _context.Rides
                .Where(r => r.Status == Models.RideStatus.Completed)
                .GroupBy(r => r.PaymentMethod).Select(g => new
                {
                    Method = g.Key,
                    Count = g.Count(),
                    TotalAmount = g.Sum(r => r.Price)
                }).ToListAsync();

            return Ok(breakdown);
        }
    }
}
