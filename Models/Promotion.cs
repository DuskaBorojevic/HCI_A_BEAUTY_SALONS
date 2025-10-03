using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Promotion
    {
        public int PromotionId { get; set; }
        public decimal Discount { get; set; }
        public DateTime Date { get; set; }
        public int? ServiceId { get; set; }
        public int? ProductId { get; set; }

        public Promotion() { }
    }
}
