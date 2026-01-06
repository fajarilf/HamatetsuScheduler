using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Exceptions;
using HamatetsuScheduler.Api.Repository.Implementation;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Service.Implementation
{
    public class ScheduleService : IScheduleService
    {
        private readonly ScheduleRepository _repository;
        private readonly ScheduleDetailRepository _detailRepository;
        private readonly SchedulePerDayRespository _perdayRepository;
        private readonly IProcessListService _processListService;
        private readonly IPartService _partService;

        public ScheduleService(ScheduleRepository repository, ScheduleDetailRepository detailRepository, SchedulePerDayRespository perdayRepository, IProcessListService processList, IPartService partService)
        {
            _repository = repository; 
            _detailRepository = detailRepository;
            _perdayRepository = perdayRepository;
            _processListService = processList;
            _partService = partService;
        }

        private static (int totalDays, List<DateTime> Dates) CalculateWorkDays(int year, int month)
        {
            var date = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var dates = new List<DateTime>();
            var workDays = 0;

            while (date.Month == month)
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    workDays++;
                    dates.Add(date);
                }

                date = date.AddDays(-1);
            }

            return (workDays, dates);
        }

        public async Task<ScheduleResponse> AddScheduleAsync(AddScheduleRequest request)
        {
            var part = await _partService.GetPartById(request.PartId);
            var processes = await _processListService.GetByWork(part.Customer.Id, part.Id);

            var entity = new Schedule()
            {
                CustomerId = part.Customer.Id,
                PartId = request.PartId,
                Quantity = request.Quantity,
                Month = request.Month,
                Year = request.Year,
            };

            var result = await _repository.SaveAsync(entity);
            var (totalDays, Dates) = CalculateWorkDays(request.Year, request.Month);

            await ProduceWorkPerDay(totalDays, Dates, request.Quantity, processes.ProcessLists, result.Id);

            return ScheduleDto.ToScheduleResponse(result);
        }

        private async Task<List<SchedulePerDay>> ProduceWorkPerDay(int workdays, List<DateTime> dates, int qty, List<ProcessListResponse> process, int schedule_id)
        {
            var workPerDays = new List<SchedulePerDay>(capacity: workdays);
            var qtyPerDays = qty / workdays;
            var totalProcessDays = process.Count * 2;

            foreach (var date in dates)
            {
                var startDate = date.AddDays(-totalProcessDays);

                var schedule = new SchedulePerDay
                {
                    ScheduleId = schedule_id,
                    Date = date,
                    Quantity = qtyPerDays,
                    StartDate = startDate,
                };

                workPerDays.Add(schedule);
                var result = await _perdayRepository.SaveAsync(schedule);

                //Console.WriteLine($"{schedule.Date}");

                await BuildBackwardSchedule(process, result);
            }

            //var result = await _perdayRepository.SaveRangeAsync(workPerDays);

            return [.. workPerDays];
        }

        private static DateTime SubtractWorkingDays(DateTime date, int workDays = 1)
        {
            DateTime d = date;

            if (d.DayOfWeek == DayOfWeek.Sunday || d.DayOfWeek == DayOfWeek.Saturday)
            {
                while (workDays > 0)
                {
                    if (d.DayOfWeek != DayOfWeek.Sunday && d.DayOfWeek != DayOfWeek.Saturday)
                    {
                        workDays--;
                    }

                    d = d.AddDays(-1);
                }
            }
            else
            {
                while (workDays > 0)
                {
                    d = d.AddDays(-1);

                    if (d.DayOfWeek != DayOfWeek.Sunday && d.DayOfWeek != DayOfWeek.Saturday)
                    {
                        workDays--;
                    }
                }
            }

            return d;
        }

        private static DateTime DetermineFinishDate(int iteration, int total, DateTime targetDate)
        {
            // first
            if (iteration == total)
                return targetDate;

            //if (iteration == total - 1)
            //    return SubtractWorkingDays(targetDate, 1);

            //// the rest
            //return targetDate; // Temporary; updated later through nextFinish
            return SubtractWorkingDays(targetDate, 1);
        }

        private static DateTime DetermineStartDate(int iteration, int total, DateTime finish, int WorkingDay)
        {
            //if (iteration == total)
            //    return finish;

            return SubtractWorkingDays(finish, WorkingDay);
        }

        private async Task BuildBackwardSchedule(List<ProcessListResponse> processes, SchedulePerDay schedule)
        {
            var details = new List<ScheduleDetail>(processes.Count);
            var nextTarget = schedule.Date;
            var total = processes.Count - 1;

            for (int i = total; i >= 0; i--)
            {
                var proc = processes[i];

                //var workingDays = i == total - 1 ? 2 : 1;
                var finish = DetermineFinishDate(i, total, nextTarget);
                var start = DetermineStartDate(i, total, finish, 1);
                //var quantity = i == total ? schedule.Quantity : schedule.Quantity ;

                details.Add(new ScheduleDetail
                {
                    SchedulePerDayId = schedule.Id,
                    Order = proc.Order,
                    ProcessId = proc.Id,
                    StartTime = start,
                    FinishTime = finish,
                    TargetQuantityPerDay = schedule.Quantity / 2,
                    TargetQuantityTotal = schedule.Quantity
                });

                //Console.WriteLine($"Process: {proc.ProcessName}, Start: {start.ToShortDateString()}, Finish: {finish.ToShortDateString()}");

                //// ignore first 2 process
                //if (i >= processes.Count - 2)
                //    continue;

                nextTarget = start;
            }

            details.Reverse();

            await _detailRepository.SaveRangeAsync(details);
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await _repository.Dbset.FirstOrDefaultAsync(d => d.Id ==  id);
            if (schedule == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Item not found");

            await _repository.Delete(schedule);

            var perDays = await _perdayRepository.Dbset.Where(d => d.ScheduleId == id).ToListAsync();
            await _perdayRepository.DeleteRange(perDays);

            var first_id = perDays.FirstOrDefault()?.Id;
            var details = await _detailRepository.Dbset.Where(d => d.SchedulePerDayId == first_id).ToListAsync();
            await _detailRepository.DeleteRange(details);
        }

        public async Task<IEnumerable<ScheduleResponse>> GetAllSchedulesAsync()
        {
            var entities = await _repository.Dbset
                .Include(d => d.Part)
                .Include(d => d.Customer)
                .ToListAsync();

            var result = entities
                .Select(ScheduleDto.ToScheduleResponse)
                .ToList();

            return result;
        }

        public Task<ScheduleResponse> UpdateScheduleAsync(UpdateScheduleRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ScheduleWithProcessResponse> GetScheduleDetail(int schedule_id)
        {
            var result = await _repository
                .Dbset
                .Include(d => d.Part)
                .Include(d => d.Customer)
                .Include(d => d.Schedules)
                    .ThenInclude(d => d.Details)
                        .ThenInclude(d => d.Process)
                .FirstOrDefaultAsync(d => d.Id == schedule_id);

            if (result == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Schedule not found");

            var response = ScheduleDto.toScheduleWithProcessResponse(result);

            return response;
        }

        public async Task<List<ListOfSchedulePerDayResponse>> GetScheduleDetail()
        {
            var response = new List<ListOfSchedulePerDayResponse>();

            var result = await _perdayRepository.Dbset
               .Include(d => d.Details)
               .ThenInclude(d => d.Process)
               .ToListAsync();

            var grouped = result.GroupBy(d => d.ScheduleId);

            foreach (var group in grouped)
            {
                var data = group.ToList();

                var schedule = data
                    .Select(SchedulePerDayDto.toSchedulePerDayResponse)
                    .ToList();

                var value = new ListOfSchedulePerDayResponse
                {
                    scheduleId = data[0].ScheduleId,
                    Schedules = schedule
                };

                response.Add(value);
            }

            return response;
        }

        public async Task<ScheduleProcessList> GetScheduleByProcess(int processId)
        {
            var result = await _detailRepository.Dbset
                .Where(d => d.ProcessId == processId)
                .Include(d => d.SchedulePerDay)
                    .ThenInclude(spd => spd.Schedule)
                        .ThenInclude(s => s.Part)
                .Include(d => d.Process)
                .ToListAsync();

            // If no records, return an empty structure
            if (result.Count == 0)
            {
                return new ScheduleProcessList
                {
                    ProcessName = string.Empty,
                    DataList = []
                };
            }

            var processName = result[0].Process.Name;

            var datalist = result
                .SelectMany(item => ScheduleDetailDto.MakeDateRange(item.StartTime, item.FinishTime)
                    .Select(date => new { date, item }))
                .GroupBy(d => d.date)
                .OrderBy(g => g.Key)
                .Select(group => new ScheduleByProcess
                {
                    Date = group.Key,
                    ProcessDetail = [.. group
                        .Select(d => new ProcessDetail
                        {
                            PartName = d.item.SchedulePerDay.Schedule.Part.Name,
                            Quantity = d.item.TargetQuantityPerDay,
                            PONumber = $"PO-{d.item.SchedulePerDay.Date}"
                        })]
                })
                .ToList();

            return new ScheduleProcessList
            {
                ProcessName = processName,
                DataList = datalist
            };
        }

        public async Task<List<ScheduleProcessList>> GetScheduleByProcessAll()
        {
            var result = await _detailRepository.Dbset
                .Include(d => d.SchedulePerDay)
                    .ThenInclude(spd => spd.Schedule)
                        .ThenInclude(s => s.Part)
                .Include(d => d.Process)
                .ToListAsync();

            var response = result
                .GroupBy(d => new { d.ProcessId, ProcessName = d.Process.Name })
                .Select(processGroup => new ScheduleProcessList
                {
                    ProcessName = processGroup.Key.ProcessName,
                    DataList = [.. processGroup
                        .SelectMany(item => ScheduleDetailDto.MakeDateRange(item.StartTime, item.FinishTime)
                            .Select(date => new { date, item }))
                        .GroupBy(d => d.date)
                        .OrderBy(g => g.Key)
                        .Select(timeGroup => new ScheduleByProcess
                        {
                            Date = timeGroup.Key,
                            ProcessDetail = [.. timeGroup
                                .Select(d => new ProcessDetail
                                {
                                    PartName = d.item.SchedulePerDay.Schedule.Part.Name,
                                    Quantity = d.item.TargetQuantityPerDay,
                                    PONumber = $"PO-{d.item.SchedulePerDay.Date}"
                                })]
                        })]
                })
                .OrderBy(p => p.ProcessName)
                .ToList();

            return response;
        }
    }
}
