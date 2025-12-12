using HamatetsuScheduler.Api.Domain.Entity;

namespace HamatetsuScheduler.Api.Domain.DTO
{
    public static class PartDto
    {
        public static PartResponse toPartResponse(Part entity)
        {
            return new PartResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type,
                Customer = CustomerDto.toCustomerResponse(entity.Customer)
            };
        }
    }

    public class AddPartRequest
    {
        public required string Name { get; set; }
        public required string Type { get; set; }
        public int CustomerId { get; set; }
    }

    public class UpdatePartRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? CustomerId { get; set; }
    }

    public class UpdatePartData
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? CustomerId { get; set; }
    }

    public class PartResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public CustomerResponse Customer { get; set; } = new();
    }
}
