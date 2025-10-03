using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public TimeSpan DurationTime { get; set; }
        public string Description { get; set; }
        public int PriceListId { get; set; }
        public ServiceType ServiceType { get; set; }

        public Service() { }

        public Service(int serviceId, string name, double price, TimeSpan durationTime, string description, int priceListId, ServiceType serviceType)
        {
            ServiceId = serviceId;
            Name = name;
            Price = price;
            DurationTime = durationTime;
            Description = description;
            PriceListId = priceListId;
            ServiceType = serviceType;
        }
    }
}
