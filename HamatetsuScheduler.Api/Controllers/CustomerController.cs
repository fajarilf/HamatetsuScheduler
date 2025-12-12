using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HamatetsuScheduler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllCustomerAsync();

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCustomerRequest request)
        {
            var result = await _service.AddCustomerAsync(request);

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{customer_id:int}")]
        public async Task<IActionResult> Delete(int customer_id)
        {
            await _service.Delete(customer_id);

            var response = new
            {
                status = "OK",
            };

            return Ok(response);
        }

        [HttpPatch]
        [Route("{customer_id:int}")]
        public async Task<IActionResult> Update(int customer_id, UpdateBody request)
        {
            var dto = new UpdateCustomerRequest
            {
                Id = customer_id,
                Name = request.Name
            };

            var result = await _service.UpdateCustomerAsync(dto);

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{customer_id:int}")]
        public async Task<IActionResult> GetById(int customer_id)
        {
            var result = await _service.GetCustomerById(customer_id);

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }
    }
}
