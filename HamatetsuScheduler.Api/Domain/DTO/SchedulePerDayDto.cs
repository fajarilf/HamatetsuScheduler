using HamatetsuScheduler.Api.Domain.Entity;

namespace HamatetsuScheduler.Api.Domain.DTO
{
    public static class SchedulePerDayDto
    {
        public static SchedulePerDayResponse toSchedulePerDayResponse(SchedulePerDay entity)
        {
            return new SchedulePerDayResponse
            {
                Id = entity.Id,
                Date = entity.Date,
                Quantity = entity.Quantity,
                StartDate = entity.StartDate,
                Details = ScheduleDetailDto.toScheduleDetailResponse([..entity.Details])
            };
        }

        public static List<SchedulePerDayResponseWithoutDetail> toSchedulePerDayResponseWithoutDetail(List<SchedulePerDay> enities)
        {
            if (enities.Count == 0)
                return [];

            var result = enities.Select(d => new SchedulePerDayResponseWithoutDetail
            {
                Id = d.Id,
                Date = d.Date,
                Quantity = d.Quantity,
                StartDate = d.StartDate,
            });

            return [.. result];
        }
    }

    public class SchedulePerDayResponseWithoutDetail
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class ListOfSchedulePerDayResponse
    {
        public int scheduleId { get; set; }
        public List<SchedulePerDayResponse> Schedules { get; set; } = [];
    }

    public class SchedulePerDayResponse 
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public List<ScheduleDetailResponse> Details { get; set; } = [];
    }
}
