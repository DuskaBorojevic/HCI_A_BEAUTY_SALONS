using HCI_A.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class OrderItem
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Price * Quantity;
        public string ProductName { get; set; }
        public string ProductDescription => ProductDao.GetProductById(ProductId).Description;

        public OrderItem() { }

        public OrderItem(int itemId, int productId, int quantity, decimal price, string productName)
        {
            ItemId = itemId;
            OrderId = 0;
            CartId = 0;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
            ProductName = productName;
        }

    }
}
