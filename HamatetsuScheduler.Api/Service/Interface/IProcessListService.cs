using HamatetsuScheduler.Api.Domain.DTO;

namespace HamatetsuScheduler.Api.Service.Interface
{
    public interface IProcessListService
    {
        Task<ProcessListResponse> AddProcessListAsync(AddProcessListRequest request);
        Task<IEnumerable<ProcessListByWorkResponse>> GetAllProcessAsync();
        Task<ProcessListByWorkResponse> GetByWork(int customer_id, int part_id);
        Task Delete(int customer_id, int part_id);
        Task<ProcessListByWorkResponse> UpdateorDeleteProcessList(UpdateorDeleteProcessListRequest request);
        Task<ProcessListByWorkResponse> AddProcessListByWork(List<AddProcessListRequest> requests);
    }
}
