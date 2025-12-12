using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HamatetsuScheduler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _service;

        public ScheduleController(IScheduleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllSchedulesAsync();

            var response = new
            {
                status = "OK",
                data = result
            };
             
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddScheduleRequest request)
        {
            var result = await _service.AddScheduleAsync(request);

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{schedule_id:int}")]
        public async Task<IActionResult> Delete(int schedule_id)
        {
            await _service.DeleteScheduleAsync(schedule_id);

            var response = new
            {
               status = "OK", 
            };

            return Ok(response);    
        }

        [HttpGet]
        [Route("{schedule_id:int}")]
        public async Task<IActionResult> GetScheduleDetails(int schedule_id)
        {
            var result = await _service.GetScheduleDetail(schedule_id);

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetScheduleDetails()
        {
            var result = await _service.GetScheduleDetail();

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("ByProcess/{process_id:int}")]
        public async Task<IActionResult> GetScheduleByProcessId (int process_id)
        {
            var result = await _service.GetScheduleByProcess(process_id);

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("ByProcess")]
        public async Task<IActionResult> GetScheduleByProcess()
        {
            var result = await _service.GetScheduleByProcessAll();

            var response = new
            {
                status = "OK",
                data = result
            };

            return Ok(response);
        }
    }
}
