using HamatetsuScheduler.Api.Domain.Entity;

namespace HamatetsuScheduler.Api.Domain.DTO
{
    public static class ScheduleDetailDto
    {
        public static List<ScheduleDetailResponse> toScheduleDetailResponse(List<ScheduleDetail> entity)
        {
            if (entity.Count <= 0)
                return [];

            var data = entity.Select(d => new ScheduleDetailResponse
            {
                Order = d.Order,
                ProcessName = d.Process.Name,
                TargetQtyPerDay = d.TargetQuantityPerDay,
                TargetQtyTotal = d.TargetQuantityTotal,
                Dates = MakeDateRange(d.StartTime, d.FinishTime)
            }).ToList();

            return data;
        }

        public static IEnumerable<DateTime> MakeDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                yield break;

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                yield return date;
        }
    }

    public class ScheduleDetailResponse
    {
        public int Order { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public int TargetQtyPerDay { get; set; }
        public int TargetQtyTotal { get; set; }
        public IEnumerable<DateTime> Dates { get; set; } = [];
    }
}
