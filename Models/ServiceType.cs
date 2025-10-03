using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class ServiceType
    {
        public int ServiceTypeId { get; set; }
        public string Name { get; set; }

        public ServiceType() { }

        public ServiceType(int serviceTypeId, string name)
        {
            ServiceTypeId = serviceTypeId;
            Name = name;
        }
    }
}
