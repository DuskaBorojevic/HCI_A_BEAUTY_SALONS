using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Phone
    {
        public string PhoneNumber { get; set; }
        public int BeautySalonId { get; set; }

        public Phone() { }

        public Phone(int beautySalonId, string phoneNumber)
        {
            BeautySalonId = beautySalonId;
            PhoneNumber = phoneNumber;
        }
    }
}
