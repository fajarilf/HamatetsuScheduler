using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Domain.Entity;

namespace HamatetsuScheduler.Api.Service.Interface
{
    public interface IScheduleService
    {
        Task<ScheduleResponse> AddScheduleAsync(AddScheduleRequest schedule);
        Task<ScheduleResponse> UpdateScheduleAsync(UpdateScheduleRequest schedule);
        Task DeleteScheduleAsync(int id);
        Task<IEnumerable<ScheduleResponse>> GetAllSchedulesAsync();
        Task<ScheduleWithProcessResponse> GetScheduleDetail(int schedule_id);
        Task<List<ListOfSchedulePerDayResponse>> GetScheduleDetail();
        Task<ScheduleProcessList> GetScheduleByProcess(int process_id);
        Task<List<ScheduleProcessList>> GetScheduleByProcessAll();
    }
}
