using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using HCI_A.Models;

namespace HCI_A.Dao
{
    public static class CartDao
    {
        public static List<Cart> GetCartsByClient(int clientId)
        {
            List<Cart> carts = new List<Cart>();

            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("dohvati_korpe", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pIdKlijenta", clientId);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            carts.Add(new Cart
                            {
                                CartId = reader.GetInt32("IdKorpe"),
                                ClientId = reader.GetInt32("KLIJENT_KORISNIK_IdKorisnika"),
                                SalonId = reader.GetInt32("KOZMETICKI_SALON_IdKozmetickogSalona"),
                                SalonName = reader.GetString("NazivSalona"),
                                Items = new List<OrderItem>()
                            });
                        }
                    }
                }

                foreach (var cart in carts)
                {
                    cart.Items = GetCartItems(cart.CartId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetCartsByClient error: " + ex.Message);
            }

            return carts;
        }

        public static bool AddToCart(int clientId, int productId, int quantity)
        {
            try
            {
                using (var conn = DBUtil.GetConnection())
                {
                    using (var cmd = new MySqlCommand("dodaj_u_korpu", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("pIdKlijenta", clientId);
                        cmd.Parameters.AddWithValue("pIdProizvoda", productId);
                        cmd.Parameters.AddWithValue("pKolicina", quantity);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AddToCart error: " + ex.Message);
                return false;
            }
        }

        public static List<OrderItem> GetCartItems(int cartId)
        {
            List<OrderItem> items = new List<OrderItem>();

            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("dohvati_stavke_u_korpi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pIdKorpe", cartId);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderItem item= new OrderItem {
                                ItemId = reader.GetInt32("IdStavke"),
                                ProductId = reader.GetInt32("PROIZVOD_IdProizvoda"),
                                Quantity = reader.GetInt32("Kolicina"),
                                Price = reader.GetDecimal("Cijena"),
                                ProductName = reader.GetString("Naziv")
                            };
                            item.CartId = reader.GetInt32("KORPA_IdKorpe");
                            Product product = ProductDao.GetProductById(item.ProductId);
                            if (product.Availability && !product.Deleted)
                            {
                                items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetCartItems error: " + ex.Message);
            }

            return items;
        }

        public static bool RemoveFromCart(int itemId)
        {
            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("obrisi_stavku_korpe", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pIdStavke", itemId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("RemoveFromCart error: " + ex.Message);
                return false;
            }
        }

        public static bool UpdateCartItemQuantity(int itemId, int newQuantity)
        {
            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("azuriraj_kolicinu_stavke", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pIdStavke", itemId);
                    cmd.Parameters.AddWithValue("pNovaKolicina", newQuantity);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateCartItemQuantity error: " + ex.Message);
                return false;
            }
        }

        public static List<OrderItem> GetCartItemsForClient(int clientId)
        {
            var items = new List<OrderItem>();
            try
            {
                using (var conn = DBUtil.GetConnection())
                {
                    using (var cmd = new MySqlCommand("dohvati_korpu_za_korisnika", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("pIdKlijenta", clientId);

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var it = new OrderItem
                                {
                                    ItemId = reader.GetInt32("IdStavke"),
                                    ProductId = reader.GetInt32("IdProizvoda"),
                                    Quantity = reader.GetInt32("Kolicina"),
                                    Price = reader.GetDecimal("Cijena"),
                                    ProductName = reader.GetString("NazivProizvoda")
                                };
                                it.CartId = reader.GetInt32("Korpa_IdKorpe");
                                items.Add(it);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetCartItemsForClient error: " + ex.Message);
            }
            return items;
        }
    }
}
