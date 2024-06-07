using C2S.Data.Enumrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2S.Business.Models
{
    public class RegisterationRequest: BaseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; } // "Customer" or "Provider"
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
