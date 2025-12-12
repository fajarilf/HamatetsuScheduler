using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Exceptions;
using HamatetsuScheduler.Api.Repository.Implementation;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Service.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _repository;

        public CustomerService(CustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomerResponse> AddCustomerAsync(AddCustomerRequest request)
        {
            var customer = await _repository.Dbset.FirstOrDefaultAsync(d => d.Name.ToLower() == request.Name.ToLower());
            if (customer != null)
                throw new ResponseException(System.Net.HttpStatusCode.BadRequest, "Customer already exist");

            var entity = new Customer
            {
                Name = request.Name,
            };

            var result = await _repository.SaveAsync(entity);

            return CustomerDto.toCustomerResponse(result);
        }

        public async Task Delete(int customer_id)
        {
            var customer = await _repository.Dbset.FirstOrDefaultAsync(d => d.Id == customer_id);
            if (customer == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound,"Customer not found");

            await _repository.Delete(customer);
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllCustomerAsync()
        {
            var customers = await _repository.Dbset.ToListAsync();

            var result = customers
                .Select(CustomerDto.toCustomerResponse)
                .ToList();

            return result;
        }

        public async Task<CustomerResponse> GetCustomerById(int customer_id)
        {
            var result = await _repository.Dbset.FirstOrDefaultAsync(d => d.Id == customer_id);
            if (result == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Customer not found");

            return CustomerDto.toCustomerResponse(result);
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(UpdateCustomerRequest request)
        {
            var customer = await _repository
                .Dbset
                .FirstOrDefaultAsync(d => d.Id == request.Id);

            if (customer == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Customer not exts");

            if (!string.IsNullOrEmpty(request.Name))
                customer.Name = request.Name;

            await _repository.Dbcontex.SaveChangesAsync();

            return CustomerDto.toCustomerResponse(customer);
        }
    }
}
