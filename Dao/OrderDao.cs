using HCI_A.Models;
using HCI_A.Models.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace HCI_A.Dao
{
    public static class OrderDao
    {
      
        public static int CheckoutCart(int cartId, string address, string contactPhone)
        {
            try
            {
                using (var conn = DBUtil.GetConnection())
                {
                    using (var cmd = new MySqlCommand("kreiraj_narudzbu_iz_korpe", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("pIdKorpe", cartId);
                        cmd.Parameters.AddWithValue("pAdresa", string.IsNullOrWhiteSpace(address) ? "" : address);
                        cmd.Parameters.AddWithValue("pKontakt", string.IsNullOrWhiteSpace(contactPhone) ? "" : contactPhone);

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Convert.ToInt32(reader["IdNarudzbe"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CheckoutCart error: " + ex.Message);
            }
            return -1;
        }

        public static List<Order> GetOrdersForClient(int clientId)
        {
            var orders = new List<Order>();
            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("dohvati_narudzbe_klijenta", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pIdKlijenta", clientId);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ord = new Order
                            {
                                OrderId = reader.GetInt32("IdNarudzbe"),
                                Date = reader.GetDateTime("Datum"),
                                Status = (OrderStatus)reader.GetInt32("Status"),
                                ClientId = clientId,
                                Total = reader.GetDecimal("UkupnaVrijednost"),
                                DeliveryAddress = reader.IsDBNull("Adresa") ? "" : reader.GetString("Adresa"),
                                ContactPhone = reader.IsDBNull("KontaktTelefon") ? "" : reader.GetString("KontaktTelefon")
                            };
                            ord.BeautySalonId = reader.GetInt32("KOZMETICKI_SALON_IdKozmetickogSalona");
                            orders.Add(ord);
                        }
                    }
                }

                foreach (var order in orders)
                {
                    order.Items = GetOrderItems(order.OrderId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetOrdersForClient error: " + ex.Message);
            }
            return orders;
        }

        public static List<Order> GetOrdersForSalon(int salonId)
        {
            var orders = new List<Order>();
            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("dohvati_narudzbe_salona", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pIdSalona", salonId);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ord = new Order
                            {
                                OrderId = reader.GetInt32("IdNarudzbe"),
                                Date = reader.GetDateTime("Datum"),
                                Status = (OrderStatus)reader.GetInt32("Status"),
                                Total = reader.GetDecimal("UkupnaVrijednost"),
                                SalonId = salonId,
                                DeliveryAddress = reader.IsDBNull("Adresa") ? "" : reader.GetString("Adresa"),
                                ContactPhone = reader.IsDBNull("KontaktTelefon") ? "" : reader.GetString("KontaktTelefon"),
                                ClientId = reader.IsDBNull("KLIJENT_KORISNIK_IdKorisnika") ? 0 : reader.GetInt32("KLIJENT_KORISNIK_IdKorisnika")
                            };
                            orders.Add(ord);
                        }
                    }
                }

                foreach (var order in orders)
                {
                    order.Items = GetOrderItems(order.OrderId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetOrdersForSalon error: " + ex.Message);
            }
            return orders;
        }

        public static List<OrderItem> GetOrderItems(int orderId)
        {
            var items = new List<OrderItem>();
            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("dohvati_stavke_narudzbe", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pIdNarudzbe", orderId);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var it = new OrderItem
                            {
                                ProductId = reader.GetInt32("PROIZVOD_IdProizvoda"),
                                Quantity = reader.GetInt32("Kolicina"),
                                Price = reader.GetDecimal("Cijena"),
                                ProductName = reader.GetString("NazivProizvoda")
                            };
                            items.Add(it);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetOrderItems error: " + ex.Message);
            }
            return items;
        }

        public static bool UpdateOrderStatus(int orderId, OrderStatus status)
        {
            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("azuriraj_status_narudzbe", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pIdNarudzbe", orderId);
                    cmd.Parameters.AddWithValue("@pStatus", (int)status);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateOrderStatus error: " + ex.Message);
                return false;
            }
        }

        public static bool CancelOrder(int orderId)
        {
            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("obrisi_narudzbu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pIdNarudzbe", orderId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CancelOrder error: " + ex.Message);
                return false;
            }
        }
    }
}
