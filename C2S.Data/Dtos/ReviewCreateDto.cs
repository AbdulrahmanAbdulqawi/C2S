using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2S.Data.Dtos
{
    public class ReviewCreateDto
    {
        public Guid ServiceId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; } // Typically on a scale of 1 to 5
        public string Comment { get; set; }
    }
}
