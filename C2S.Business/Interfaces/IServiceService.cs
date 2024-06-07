using C2S.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2S.Business.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetAllServicesAsync();
        Task<Service> GetFullDetailedServiceByIdAsync(Guid id);
        Task<Service> GetMinDetailedServiceByIdAsync(Guid id);
        Task<Service> CreateServiceAsync(Service service);
        Task<Service> UpdateServiceAsync(Guid id, Service service);
        Task<bool> DeleteServiceAsync(Guid id);
    }
}
