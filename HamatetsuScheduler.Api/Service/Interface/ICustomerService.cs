using HamatetsuScheduler.Api.Domain.DTO;

namespace HamatetsuScheduler.Api.Service.Interface
{
    public interface ICustomerService
    {
        Task<CustomerResponse> AddCustomerAsync(AddCustomerRequest request);
        Task<CustomerResponse> UpdateCustomerAsync(UpdateCustomerRequest request);
        Task Delete(int customer_id);
        Task<IEnumerable<CustomerResponse>> GetAllCustomerAsync();
        Task<CustomerResponse> GetCustomerById(int customer_id);
    }
}
