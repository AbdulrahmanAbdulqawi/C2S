using C2S.Business.Interfaces;
using C2S.Business.Models;
using C2S.Data.Dtos;
using C2S.Data.Enumrations;
using C2S.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace C2S.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceService.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var service = await _serviceService.GetFullDetailedServiceByIdAsync(id);
            if (service == null)
                return NotFound();
            return Ok(service);
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServiceCreateDto request)
        {
            var service = new Service
            {
                Availability = request.Availability,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Category = request.Category,
                Description = request.Description,
                Id = request.Id,
                Price = request.Price,
                Title = request.Title,
                ProviderId = request.ProviderId,
            };

            var result = await _serviceService.CreateServiceAsync(service);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(Guid id, [FromBody] ServiceUpdateDto request)
        {
            var service = new Service
            {
                Availability = request.Availability,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Category = request.Category,
                Description = request.Description,
                Id = request.Id,
                Price = request.Price,
                Title = request.Title,
            };
            var result = await _serviceService.UpdateServiceAsync(id, service);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var result = await _serviceService.DeleteServiceAsync(id);
            if (!result)
                return BadRequest();
            return Ok(result);
        }
    }
}
