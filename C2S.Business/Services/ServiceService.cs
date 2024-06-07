using C2S.Business.Interfaces;
using C2S.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C2S.Business.Services
{
    public class ServiceService : IServiceService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceService> _logger;

        public ServiceService(ApplicationDbContext context, ILogger<ServiceService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            try
            {
                return await _context.Services.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all services.");
                throw;
            }
        }

        public async Task<Service> GetFullDetailedServiceByIdAsync(Guid id)
        {
            if (!await AnyServiceAsync()) return null;

            try
            {
                return await _context.Services.AsNoTracking()
                    .Include(s => s.Provider)
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching service with id {id}.");
                throw;
            }
        }

        public async Task<Service> GetMinDetailedServiceByIdAsync(Guid id)
        {
            try
            {
                return await _context.Services.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching minimal detailed service with id {id}.");
                throw;
            }
        }

        public async Task<Service> CreateServiceAsync(Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            try
            {
                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();
                return service;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating service.");
                throw;
            }
        }

        public async Task<Service> UpdateServiceAsync(Guid id, Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            if (!await AnyServiceAsync()) return null;

            var serviceExists = await ServiceExistsAsync(id);
            if (!serviceExists)
            {
                _logger.LogWarning($"Service with id {id} not found.");
                return null;
            }

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return service;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await ServiceExistsAsync(id))
                {
                    _logger.LogWarning($"Service with id {id} not found during update.");
                    throw new KeyNotFoundException($"Service with id {id} not found.");
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error while updating service.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service.");
                throw;
            }
        }

        public async Task<bool> DeleteServiceAsync(Guid id)
        {
            try
            {
                var service = await GetMinDetailedServiceByIdAsync(id);
                if (service == null)
                {
                    _logger.LogWarning($"Service with id {id} not found for deletion.");
                    return false;
                }

                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting service with id {id}.");
                throw;
            }
        }

        private async Task<bool> ServiceExistsAsync(Guid id)
        {
            if(!await AnyServiceAsync()) return false;
            try
            {
                return await _context.Services.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of service with id {id}.");
                throw;
            }
        }
        public async Task<bool> AnyServiceAsync()
        {
            try
            {
                return await _context.Services.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if there are any services.");
                throw;
            }
        }
    }
}
