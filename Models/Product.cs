using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool Availability { get; set; }
        public int PriceListId { get; set; }
        public bool Deleted { get; set; }

        public Product(int id, string name, string description, double price, bool availabiity, int priceListId)
        {
            ProductId = id;
            Name = name;
            Description = description;
            Price = price;
            Availability = availabiity;
            PriceListId = priceListId;
        }
    }
}
