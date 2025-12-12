using HamatetsuScheduler.Api.Domain.Entity;

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
                PerDays = SchedulePerDayDto.toSchedulePerDayResponseWithoutDetail([.. entity.Schedules])
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
        public PartResponseForSchedule Part { get; set; } = new();
        public CustomerResponseForSchedule Customer { get; set; } = new();
        public int Quantity { get; set; }
        public List<SchedulePerDayResponseWithoutDetail> PerDays { get; set; } = [];
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
}
