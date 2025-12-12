using HamatetsuScheduler.Api.Domain.Entity;

namespace HamatetsuScheduler.Api.Domain.DTO
{
    public static class ProcessDto
    {
        public static ProcessResponse toProcessResponse (Process entity)
        {
            return new ProcessResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type,
            };
        }
    }

    public class AddProcessRequest
    {
        public required string Name { get; set; }
        public required string Type { get; set; }
    }

    public class UpdateProcessRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
    }

    public class UpdateProcessData
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
    }

    public class ProcessResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
