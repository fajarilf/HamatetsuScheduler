using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HamatetsuScheduler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessListController : ControllerBase
    {
        private readonly IProcessListService _service;

        public ProcessListController(IProcessListService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProcessListRequest request)
        {
            var result = await _service.AddProcessListAsync(request);

            var response = new
            {
                status = "Ok",
                data = result
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("multiple")]
        public async Task<IActionResult> CreateMultiple(List<AddProcessListRequest> request)
        {
            var result = await _service.AddProcessListByWork(request);

            var response = new
            {
                status = "Ok",
                data = result
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int customer_id, int part_id)
        {
            var result = await _service.GetByWork(customer_id, part_id);

            var response = new
            {
                status = "Ok",
                data = result
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult>GetAll()
        {
            var result = await _service.GetAllProcessAsync();

            var response = new
            {
                status = "Ok",
                data = result
            };

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult>Update(UpdateorDeleteProcessListRequest request)
        {
            var result = await _service.UpdateorDeleteProcessList(request);

            var response = new
            {
                status = "Ok",
                data = result
            };

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult>Delete(DeleteRequest request)
        {
            await _service.Delete(request.CustomerId, request.PartId);

            var response = new
            {
                status = "Ok",
            };

            return Ok(response);
        }
    }
}
