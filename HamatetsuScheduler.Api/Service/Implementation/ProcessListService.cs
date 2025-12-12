using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Exceptions;
using HamatetsuScheduler.Api.Repository.Implementation;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Service.Implementation
{
    public class ProcessListService : IProcessListService
    {
        private readonly ProcessListRepository _repository;
        private readonly IPartService _partService;

        public ProcessListService(ProcessListRepository repository, IPartService partService    )
        {
            _repository = repository;
            _partService = partService;
        }

        public async Task<ProcessListResponse> AddProcessListAsync(AddProcessListRequest request)
        {
            var partExist = await _repository.DbContext.Parts.FindAsync(request.PartId);
            if (partExist == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Part not found");

            var customerExist = await _repository.DbContext.Customers.FindAsync(request.CustomerId);
            if (customerExist == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Customer not found");

            var part = await _partService.GetByCustomerAndId(request.CustomerId, request.PartId);
            if (part == null) 
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "This customer doesn't have this part");

            var existing = await _repository.Dbset.Where(d => d.CustomerId == request.CustomerId && request.PartId == d.PartId).ToListAsync();
            if (existing.Count > 0)
                throw new ResponseException(System.Net.HttpStatusCode.BadRequest, "Process list already exist");

            var entity = new ProcessList
            {
                Order = request.Order,
                PartId = request.PartId,
                ProcessId = request.ProcessId,
                CustomerId = request.CustomerId,
            };

            var result = await _repository.SaveAsync(entity);

            return ProcessListDto.toProcessListResponse(result);
        }

        public async Task<ProcessListByWorkResponse> AddProcessListByWork(List<AddProcessListRequest> requests)
        {
            if (requests.Count == 0)
                return new ProcessListByWorkResponse();

            var first = requests.FirstOrDefault();

            var partExist = await _repository.DbContext.Parts.FindAsync(first!.PartId);
            if (partExist == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Part not found");

            var customerExist = await _repository.DbContext.Customers.FindAsync(first!.CustomerId);
            if (customerExist == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Customer not found");

            var part = await _partService.GetByCustomerAndId(first!.CustomerId, first!.PartId);
            if (part.Count() == 0)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "This customer doesn't have this part");

            var existing = await _repository.Dbset.Where(d => d.CustomerId == first.CustomerId && first.PartId == d.PartId).ToListAsync();
            if (existing.Count > 0)
                throw new ResponseException(System.Net.HttpStatusCode.BadRequest, "Process list already exist");

            var entities = requests.Select(d => new ProcessList
            {
                Order = d.Order,
                PartId = d.PartId,
                ProcessId = d.ProcessId,
                CustomerId = d.CustomerId,
            }).ToList();

            await _repository.SaveRangeAsync(entities);

            var result = await GetByWork(first.CustomerId, first.PartId);

            return result;
        }

        public Task Delete(int process_list_id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProcessListByWorkResponse>> GetAllProcessAsync()
        {
            var result = new List<ProcessListByWorkResponse>();

            var processList = await _repository.Dbset
                .Include(d => d.Customer)
                .Include(d => d.Part)
                .Include(d => d.Process)
                .ToListAsync();

            var grouped = processList.GroupBy(d => d.PartId);
            foreach (var group in grouped)
            {
                var data = group.ToList();
                var dto = ProcessListDto.toProcessListResponse(data);
                result.Add(dto);
            }

            return result;
        }

        public async Task<ProcessListByWorkResponse> GetByWork(int customer_id, int part_id)
        {
            var processLists = await _repository
                .Dbset
                .Where(d => d.CustomerId == customer_id && d.PartId == part_id)
                .Include(d => d.Customer)
                .Include(d => d.Part)
                .Include(d => d.Process)
                .ToListAsync();

            if (processLists.Count == 0)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Process list not found");

            return ProcessListDto.toProcessListResponse(processLists);
        }

        public async Task<ProcessListByWorkResponse> UpdateorDeleteProcessList(UpdateorDeleteProcessListRequest request)
        {
            var existing = await _repository
                .Dbset
                .Include(d => d.Customer)
                .Include(d => d.Part)
                .Include(d => d.Process)
                .Where(d => d.CustomerId == request.CustomerId && d.PartId == request.PartId)
                .ToListAsync();

            if (existing.Count == 0)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "process not found");

            var incoming = request.ProcessLists;
            var incomingIds = incoming.Select(d => d.Id).ToHashSet();

            var toDelete = existing
                .Where(d => !incomingIds.Contains(d.Id))
                .ToList();

            if (toDelete.Count != 0)
                await _repository.DeleteRange(toDelete);

            foreach (var entity in existing)
            {
                var updated = incoming.FirstOrDefault(d => d.Id == entity.Id);
                if (updated == null)
                    continue;

                entity.Order = updated.Order;
                entity.ProcessId = updated.ProcessId;
            }

            await _repository.DbContext.SaveChangesAsync();

            return ProcessListDto.toProcessListResponse(existing);
        }

        public async Task Delete(int customer_id, int part_id)
        {
            var existing = await _repository.Dbset.Where(d => d.CustomerId == customer_id && d.PartId == part_id).ToListAsync();
            if (existing.Count == 0)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "process not found");

            await _repository.DeleteRange(existing);
        }
    }
}
