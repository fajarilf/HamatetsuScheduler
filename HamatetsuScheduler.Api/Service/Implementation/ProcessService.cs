using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Exceptions;
using HamatetsuScheduler.Api.Repository.Implementation;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Service.Implementation
{
    public class ProcessService : IProcessService
    {
        private readonly ProcessRepository _repository;

        public ProcessService(ProcessRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProcessResponse> AddProcessAsync(AddProcessRequest request)
        {
            var entity = new Process
            {
                Name = request.Name,
                Type = request.Type,
            };

            var result = await _repository.SaveAsync(entity);

            return ProcessDto.toProcessResponse(result);
        }

        public async Task Delete(int process_id)
        {
            var process = await _repository.Dbset.FirstOrDefaultAsync(d => d.Id ==  process_id);
            if (process == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Process not found");

            await _repository.Delete(process);
        }

        public async Task<IEnumerable<ProcessResponse>> GetAllProcessAsync()
        {
            var process = await _repository
                .Dbset
                .ToListAsync();

            var result = process.Select(ProcessDto.toProcessResponse);

            return result;
        }

        public async Task<ProcessResponse> GetProcessById(int process_id)
        {
            var result = await _repository.Dbset.FirstOrDefaultAsync(d => d.Id == process_id);
            if (result == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Process not found");

            return ProcessDto.toProcessResponse(result);
        }

        public async Task<ProcessResponse> UpdateProcessAsync(UpdateProcessRequest request)
        {
            var process = await _repository
                .Dbset
                .FirstOrDefaultAsync(d => d.Id == request.Id);

            if (process == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Process not found");

            if (!string.IsNullOrEmpty(request.Name))
                process.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Type))
                process.Type = request.Type;

            await _repository.DbContext.SaveChangesAsync();

            return ProcessDto.toProcessResponse(process);
        }
    }
}
