using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HamatetsuScheduler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly IPartService _service;

        public PartController(IPartService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int customer_id = 0)
        {
            IEnumerable<PartResponse> result = [];

            if (customer_id == 0)
                result = await _service.GetAllPartAsync();
            else
                result = await _service.GetByCustomer(customer_id);

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddPartRequest request)
        {
            var result = await _service.AddPartAsync(request);

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{part_id:int}")]
        public async Task<IActionResult> Delete(int part_id)
        {
            await _service.Delete(part_id);

            var response = new
            {
                status = "OK",
            };

            return Ok(response);
        }

        [HttpPatch]
        [Route("{part_id:int}")]
        public async Task<IActionResult> Update(int part_id, UpdatePartData request)
        {
            var dto = new UpdatePartRequest
            {
                Id = part_id,
                Name = request.Name,
                Type = request.Type,
                CustomerId = request.CustomerId,
            };

            var result = await _service.UpdatePartAsync(dto);

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{part_id:int}")]
        public async Task<IActionResult> GetById(int part_id)
        {
            var result = await _service.GetPartById(part_id);

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }
    }
}
