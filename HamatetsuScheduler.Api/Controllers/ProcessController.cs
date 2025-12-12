using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HamatetsuScheduler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessService _service;

        public ProcessController(IProcessService service)
        {   
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAllProcessAsync();

            var response = new
            {
                status = "Ok",
                data = result
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProcessRequest request)
        {
            var result = await _service.AddProcessAsync(request);

            var response = new
            {
                status = "Ok",
                data = result
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{process_id:int}")]
        public async Task<IActionResult> Delete(int process_id)
        {
            await _service.Delete(process_id);

            var response = new
            {
                status = "Ok",
            };

            return Ok(response);
        }

        [HttpPatch]
        [Route("{process_id:int}")]
        public async Task<IActionResult> Update(int process_id, UpdateProcessData request)
        {
            var dto = new UpdateProcessRequest
            {
                Id = process_id,
                Name = request.Name,
                Type = request.Type,
            };

            var result = await _service.UpdateProcessAsync(dto);

            var response = new
            {
                status = "Ok",
                data = result
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{process_id:int}")]
        public async Task<IActionResult> GetById(int process_id)
        {
            var result = await _service.GetProcessById(process_id);

            var response = new
            {
                status = "Ok",
                data = result
            };

            return Ok(response);
        }
    }
}
