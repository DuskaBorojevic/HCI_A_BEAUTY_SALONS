using HCI_A.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HCI_A.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public Enums.OrderStatus Status { get; set; }
        public int ClientId { get; set; }
        public int SalonId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal Total { get; set; }
        public string ContactPhone { get; set; }                      
        public string DeliveryAddress { get; set; } 
        public int BeautySalonId { get; set; } 

        public Order() { }

        public Order(int orderId, DateTime date, OrderStatus status, int clientId, int salonId, List<OrderItem> items, decimal total)
        {
            OrderId = orderId;
            Date = date;
            Status = status;
            ClientId = clientId;
            SalonId = salonId;
            Items = items ?? new List<OrderItem>();
            Total = total;
        }
    }
}
