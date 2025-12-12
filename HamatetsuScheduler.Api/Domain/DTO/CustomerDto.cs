using HamatetsuScheduler.Api.Domain.Entity;

namespace HamatetsuScheduler.Api.Domain.DTO
{
    public static class CustomerDto
    {
        public static CustomerResponse toCustomerResponse(Customer entity)
        {
            return new CustomerResponse
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }

    public class AddCustomerRequest
    {
        public required string Name { get; set; }
    }

    public class UpdateCustomerRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class UpdateBody
    {
        public string? Name { get; set; }
    }

    public class CustomerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
