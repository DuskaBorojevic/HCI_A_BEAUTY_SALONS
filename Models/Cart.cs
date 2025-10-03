using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int ClientId { get; set; }
        public List<OrderItem> Items { get; set; }
        public int SalonId { get; set; }
        public string SalonName { get; set; }

        public Cart() { }

        public Cart(int cartId, int clientId, List<OrderItem> items, int salonId, string salonName)
        {
            CartId = cartId;
            ClientId = clientId;
            Items = items ?? new List<OrderItem>();
            SalonId = salonId;
            SalonName = salonName;
        }
    }
}
