using HamatetsuScheduler.Api.Domain.DTO;

namespace HamatetsuScheduler.Api.Service.Interface
{
    public interface IPartService
    {
        Task<PartResponse> AddPartAsync(AddPartRequest request);
        Task<PartResponse> UpdatePartAsync(UpdatePartRequest request);
        Task<IEnumerable<PartResponse>> GetByCustomer(int customer_id);
        Task<IEnumerable<PartResponse>> GetByCustomerAndId(int customer_id, int part_id);
        Task Delete(int part_id);
        Task<IEnumerable<PartResponse>> GetAllPartAsync();
        Task<PartResponse> GetPartById(int part_id);
    }
}
