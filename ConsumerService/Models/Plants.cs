using System;
using System.Collections.Generic;

namespace ConsumerService.Models
{
    public partial class Plants
    {
        public int PlantId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int TimeZone { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public long? PhoneNumber { get; set; }
        public string Director { get; set; }
        public bool? Active { get; set; }
    }
}
