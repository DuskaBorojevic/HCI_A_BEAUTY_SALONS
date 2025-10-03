using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Location
    {
        public int PostNumber { get; set; }
        public string Name { get; set; }

        public Location() { }

        public Location(int postNumber, string name)
        {
            PostNumber = postNumber;
            Name = name;
        }
    }
}
