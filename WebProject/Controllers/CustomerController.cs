using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // POST: api/customers
        [HttpGet("{customerId}")]
        public async Task<ActionResult> GetFreeBookDatesForRoom(string customerId)
        {
            var result = JsonConvert.SerializeObject(await _customerService.GetByIdAsync(customerId));
            return Content(result);
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] CustomerDto value)
        {
            var result = await _customerService.UpdateAsync(value);

            return Content(JsonConvert.SerializeObject(result));
        }
    }
}
