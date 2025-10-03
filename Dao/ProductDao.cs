using HCI_A.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Serialization;

namespace HCI_A.Dao
{
    public class ProductDao
    {
        public static List<Product> GetProductsByPriceListId(int priceListId)
        {
            List<Product> products = new List<Product>();
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("dohvati_proizvode", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pIdCj", priceListId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bool deleted = reader.GetBoolean("Obrisan");
                        Product product = new Product(
                            reader.GetInt32("IdProizvoda"),
                            reader.GetString("Naziv"),
                            reader.GetString("Opis"),
                            reader.GetDouble("Cijena"),
                            reader.GetBoolean("Dostupnost"),
                            priceListId);
                        product.Deleted = deleted;
                        if (!deleted)
                        {
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public static bool AddProduct(string name, string description, double price, bool availability, int priceListId)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("dodaj_proizvod", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pNaziv", name);
                command.Parameters.AddWithValue("@pOpis", description);
                command.Parameters.AddWithValue("@pCijena", price);
                command.Parameters.AddWithValue("@pDostupnost", availability);
                command.Parameters.AddWithValue("@pIdCjenovnika", priceListId);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public static Product GetProductById(int productId)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM proizvod WHERE IdProizvoda=@productId", connection))
            {
                command.Parameters.AddWithValue("@productId", productId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool deleted = reader.GetBoolean("Obrisan");
                        Product product= new Product(
                            reader.GetInt32("IdProizvoda"),
                            reader.GetString("Naziv"),
                            reader.GetString("Opis"),
                            reader.GetDouble("Cijena"),
                            reader.GetBoolean("Dostupnost"),
                            reader.GetInt32("CJENOVNIK_IdCjenovnika"));

                        product.Deleted = deleted;
                        return product;
                    }
                }
            }
            return null;
        }

        public static bool UpdateProduct(int id, string name, string description, double price, bool availability, int priceListId)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("azuriraj_proizvod", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", id);
                command.Parameters.AddWithValue("@pNaziv", name);
                command.Parameters.AddWithValue("@pOpis", description);
                command.Parameters.AddWithValue("@pCijena", price);
                command.Parameters.AddWithValue("@pDostupnost", availability);
                command.Parameters.AddWithValue("@pIdCjenovnika", priceListId);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public static bool DeleteProduct(int id)
        {
            using (MySqlConnection connection = DBUtil.GetConnection())
            using (MySqlCommand command = new MySqlCommand("obrisi_proizvod", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pId", id);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }
    }
}
