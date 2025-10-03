using HCI_A.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Dao
{
    public class PriceListDao
    {
        public static PriceList GetPriceListById(int priceListId)
        {
            PriceList pl = null;
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM cjenovnik WHERE IdCjenovnika=@Id", connection))
            {
                command.Parameters.AddWithValue("@Id", priceListId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int plId = reader.GetInt32(reader.GetOrdinal("IdCjenovnika"));
                        string date = (reader.GetDateTime(reader.GetOrdinal("DatumObjavljivanja"))).ToString();
                        List<Service> services = ServiceDao.GetServicesByPriceListId(plId);
                        List<Product> products = ProductDao.GetProductsByPriceListId(plId);
                        pl = new PriceList(plId, date, services, products);
                    }
                }
            }
            return pl;
        }
    }
}
