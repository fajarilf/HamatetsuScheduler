using HamatetsuScheduler.Api.Domain.Entity;

namespace HamatetsuScheduler.Api.Domain.DTO
{
    public static class ProcessListDto
    {
        public static ProcessListResponse toProcessListResponse(ProcessList entity)
        {
            return new ProcessListResponse
            {
                Id = entity.Process.Id,
                Order = entity.Order,
                ProcessName = entity.Process.Name,
                ProcessType = entity.Process.Type
            };
        }

        public static ProcessListByWorkResponse toProcessListResponse(List<ProcessList> entity)
        {
            var first = entity.FirstOrDefault();

            return new ProcessListByWorkResponse
            {
                PartId = first?.PartId ?? 0,
                CustomerId = first?.CustomerId ?? 0,
                CustomerName = first?.Customer?.Name ?? string.Empty,
                PartName = first?.Part?.Name ?? string.Empty,
                PartType = first?.Part?.Type ?? string.Empty,
                ProcessLists = [.. entity.Select(toProcessListResponse)]
            };
        }
    }

    public class AddProcessListRequest
    {
        public int CustomerId { get; set; }
        public int PartId { get; set; }
        public int Order { get; set; }
        public int ProcessId { get; set; }
    }

    public class UpdateorDeleteProcessListRequest
    {
        public required int CustomerId { get; set; }
        public required int PartId { get; set; }
        public required List<UpdateorDeleteData> ProcessLists { get; set; }
    }

    public class UpdateorDeleteData
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int ProcessId { get; set; }
    }

    public class ProcessListResponse
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public string ProcessType { get; set; } = string.Empty;
    }

    public class ProcessListByWorkResponse
    {
        public int PartId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string PartType {  get; set; } = string.Empty;
        public List<ProcessListResponse> ProcessLists { get; set; } = [];
    }

    public class DeleteRequest
    {
        public required int CustomerId { get; set; }
        public required int PartId { get; set; }
    }
}
