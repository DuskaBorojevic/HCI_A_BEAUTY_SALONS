using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class PriceList
    {
        public int PriceListId { get; set; }
        public string PublicationDate { get; set; }
        public List<Service> Services { get; set; }
        public List<Product> Products { get; set; }

        public PriceList() { }

        public PriceList(int priceListId, string publicationDate, List<Service> services, List<Product> products)
        {
            PriceListId = priceListId;
            PublicationDate = publicationDate;
            Services = services;
            Products = products;
        }
    }
}
