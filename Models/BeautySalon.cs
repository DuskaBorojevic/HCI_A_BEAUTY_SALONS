using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace HCI_A.Models
{
    public partial class BeautySalon
    {
        public int BeautySalonId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string WorkTime { get; set; }
        public Location Location { get; set; }
        public string PhoneNumber { get; set; }
        public PriceList PriceList { get; set; }
        public List<Employee> Employees { get; set; }


        public BeautySalon() { }

        public BeautySalon(int beautySalonId, string name, string address, string workTime, Location location, PriceList priceList, string phoneNumber, List<Employee> employees)
        {
            BeautySalonId = beautySalonId;
            Name = name;
            Address = address;
            WorkTime = workTime;
            Location = location;
            PriceList = priceList;
            PhoneNumber = phoneNumber;
            Employees = employees;
        }
    }
}
