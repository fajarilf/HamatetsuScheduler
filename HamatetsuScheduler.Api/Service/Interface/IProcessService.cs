using HamatetsuScheduler.Api.Domain.DTO;

namespace HamatetsuScheduler.Api.Service.Interface
{
    public interface IProcessService
    {
        Task<ProcessResponse> AddProcessAsync(AddProcessRequest request);
        Task<ProcessResponse> UpdateProcessAsync(UpdateProcessRequest request);
        Task Delete(int process_list_id);
        Task<IEnumerable<ProcessResponse>> GetAllProcessAsync();
        Task<ProcessResponse> GetProcessById(int process_id);
    }
}
