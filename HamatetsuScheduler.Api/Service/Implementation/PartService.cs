using HamatetsuScheduler.Api.Domain.DTO;
using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Exceptions;
using HamatetsuScheduler.Api.Repository.Implementation;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Service.Implementation
{
    public class PartService : IPartService
    {
        private readonly PartRepository _repository;

        public PartService(PartRepository repository)
        {
            _repository = repository;
        }

        public async Task<PartResponse> AddPartAsync(AddPartRequest request)
        {
            var entity = new Part
            {
                Name = request.Name,
                Type = request.Type,
                CustomerId = request.CustomerId,
            };

            var result = await _repository.SaveAsync(entity);

            return PartDto.toPartResponse(result);
        }

        public async Task Delete(int part_id)
        {
            var part = await _repository
                .Dbset
                .FirstOrDefaultAsync(d => d.Id == part_id);

            if (part == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Part not found");

            await _repository.Delete(part);
        }

        public async Task<IEnumerable<PartResponse>> GetAllPartAsync()
        {
            var parts = await _repository
                .Dbset
                .Include(d => d.Customer)
                .ToListAsync();

            var result = parts.Select(PartDto.toPartResponse).ToList();
            return result;
        }

        public async Task<IEnumerable<PartResponse>> GetByCustomer(int customer_id)
        {
            var part = await _repository
                .Dbset
                .Include(d => d.Customer)
                .Where(d => d.CustomerId == customer_id)
                .ToListAsync();

            var result = part.Select(PartDto.toPartResponse).ToList();

            return result;
        }

        public async Task<IEnumerable<PartResponse>> GetByCustomerAndId(int customer_id, int part_id)
        {
            var customer = _repository.DbContext.Customers.FindAsync(customer_id);

            var part = await _repository
               .Dbset
               .Include(d => d.Customer)
               .Where(d => d.CustomerId == customer_id && d.Id == part_id)
               .ToListAsync();

            var result = part.Select(PartDto.toPartResponse).ToList();

            return result;
        }

        public async Task<PartResponse> GetPartById(int part_id)
        {
            var result = await _repository
                .Dbset
                .Include(d => d.Customer)
                .FirstOrDefaultAsync(d => d.Id == part_id);

            if (result == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Part not found");

            return PartDto.toPartResponse(result);
        }

        public async Task<PartResponse> UpdatePartAsync(UpdatePartRequest request)
        {
            var part = await _repository
                .Dbset
                .FirstOrDefaultAsync(d => d.Id == request.Id);

            if (part == null)
                throw new ResponseException(System.Net.HttpStatusCode.NotFound, "Part not found");

            if (!string.IsNullOrEmpty(request.Name))
                part.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Type))
                part.Type = request.Type;

            if (request.CustomerId.HasValue && request.CustomerId != 0)
                part.CustomerId = request.CustomerId.Value;

            await _repository.DbContext.SaveChangesAsync();

            await _repository.DbContext.Entry(part)
                .Reference(p => p.Customer)
                .LoadAsync();

            return PartDto.toPartResponse(part);
        }
    }
}
