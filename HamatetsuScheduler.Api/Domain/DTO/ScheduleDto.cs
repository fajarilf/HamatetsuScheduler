using HamatetsuScheduler.Api.Domain.Entity;
using System.Globalization;

namespace HamatetsuScheduler.Api.Domain.DTO
{
    public static class ScheduleDto
    {
        public static ScheduleResponse ToScheduleResponse(Schedule entity)
        {
            return new ScheduleResponse
            {
                Id = entity.Id,
                PartId = entity.PartId,
                CustomerId = entity.CustomerId,
                Month = CultureInfo.InvariantCulture
                                .DateTimeFormat
                                .GetMonthName(entity.Month),
                Year = entity.Year,
                Part = new PartResponseForSchedule
                {
                    Name = entity.Part.Name,
                    Type = entity.Part.Type,
                },
                Customer = new CustomerResponseForSchedule
                {
                    Name = entity.Customer.Name
                },
                Quantity = entity.Quantity,
            };
        }

        public static ScheduleWithProcessResponse toScheduleWithProcessResponse(Schedule entity)
        {
            var schedules = entity.Schedules.FirstOrDefault();
            if (schedules == null)
                return new ScheduleWithProcessResponse
                {
                    ScheduleId = entity.Id,
                    Month = CultureInfo.InvariantCulture
                                .DateTimeFormat
                                .GetMonthName(entity.Month),
                    Year = entity.Year,
                    PartName = entity.Part.Name,
                    CustomerName = entity.Customer.Name,
                    ProcessLists = []
                };

            return new ScheduleWithProcessResponse
            {
                ScheduleId = entity.Id,
                Month = CultureInfo.InvariantCulture
                                .DateTimeFormat
                                .GetMonthName(entity.Month),
                Year = entity.Year,
                PartName = entity.Part.Name,
                CustomerName = entity.Customer.Name,
                ProcessLists = [.. schedules.Details.Select(d => new ProcessListResponse
                {
                    Id = d.Process.Id,
                    Order = d.Order,
                    ProcessName = d.Process.Name,
                    ProcessType = d.Process.Type,
                })]
            };
        }
    }

    public class AddScheduleRequest 
    {
        public int PartId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateScheduleRequest
    {
        public required int Id { get; set; }
        public int? PartId { get; set; }
        public int? CustomerId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }

    public class ScheduleResponse
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public int CustomerId { get; set; }
        public string Month { get; set; } = string.Empty;
        public int Year { get; set; }
        public PartResponseForSchedule Part { get; set; } = new();
        public CustomerResponseForSchedule Customer { get; set; } = new();
        public int Quantity { get; set; }
    }

    public class PartResponseForSchedule
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    public class CustomerResponseForSchedule
    {
        public string Name { get; set; } = string.Empty;
    }

    public class ProcessDetail
    {
        public string PartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string PONumber { get; set; } = string.Empty;
    }

    public class ScheduleByProcess
    {
        public DateTime Date { get; set; }
        public List<ProcessDetail> ProcessDetail { get; set; } = new();
    }

    public class ScheduleProcessList
    {
        public string ProcessName { get; set; } = string.Empty;
        public List<ScheduleByProcess> DataList { get; set; } = [];
    }

    public class ScheduleWithProcessResponse
    {
        public int ScheduleId { get; set; }
        public string Month { get ; set; } = string.Empty;
        public int Year { get ; set; }
        public string PartName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public List<ProcessListResponse> ProcessLists { get; set; } = [];
    }
}
