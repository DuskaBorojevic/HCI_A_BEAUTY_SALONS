using HCI_A.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Invoice
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public InvoiceStatus Status { get; set;  }
        public DateTime Date { get; set; }
        public DateTime OrderDate { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public Invoice() { }

    }
}
