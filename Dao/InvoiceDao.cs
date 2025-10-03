using HCI_A.Models;
using HCI_A.Models.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Dao
{
    public class InvoiceDao
    {
        public static List<Invoice> GetInvoicesForClient(int clientId)
        {
            var invoices = new List<Invoice>();
            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("dohvati_racune_klijenta", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pIdKorisnika", clientId);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            invoices.Add(new Invoice
                            {
                                OrderId = reader.GetInt32("IdRacuna"),
                                Amount = reader.GetDecimal("Iznos"),
                                Date = reader.GetDateTime("Datum"),
                                Status = (InvoiceStatus)reader.GetInt32("Status"),
                                OrderDate = reader.GetDateTime("DatumNarudzbe")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetInvoicesForClient error: " + ex.Message);
            }
            return invoices;
        }

        public static List<Invoice> GetInvoicesForSalon(int salonId)
        {
            var invoices = new List<Invoice>();
            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("dohvati_racune_salona", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pIdSalona", salonId);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            invoices.Add(new Invoice
                            {
                                OrderId = reader.GetInt32("IdRacuna"),
                                Amount = reader.GetDecimal("Iznos"),
                                Date = reader.GetDateTime("Datum"),
                                Status = (InvoiceStatus)reader.GetInt32("Status"),
                                OrderDate = reader.GetDateTime("DatumNarudzbe"),
                                ClientFirstName = reader.GetString("KlijentIme"),
                                ClientLastName = reader.GetString("KlijentPrezime")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetInvoicesForSalon error: " + ex.Message);
            }
            return invoices;
        }

        public static bool UpdateInvoiceStatus(int invoiceId, InvoiceStatus status)
        {
            try
            {
                using (var conn = DBUtil.GetConnection())
                using (var cmd = new MySqlCommand("UPDATE racun SET Status = @status WHERE IdRacuna = @invoiceId", conn))
                {
                    cmd.Parameters.AddWithValue("@status", (int)status);
                    cmd.Parameters.AddWithValue("@invoiceId", invoiceId);
                    conn.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateInvoiceStatus error: " + ex.Message);
                return false;
            }
        }





























        //public static List<InvoiceView> GetAllClientInvoices(int clientId)
        //{
        //    List<InvoiceView> invoices = new List<InvoiceView>();
        //    string query = "SELECT * FROM racun_view WHERE IdKlijenta = @clientId";

        //    using (MySqlConnection connection = DBUtil.GetConnection())
        //    using (MySqlCommand command = new MySqlCommand(query, connection))
        //    {
        //        command.Parameters.AddWithValue("@clientId", clientId);
        //        connection.Open();

        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                //int beauticianId = Convert.ToInt32(reader["IdKozmeticara"]);
        //                //int queryId = Convert.ToInt32(reader["IdUpita"]);
        //                //double price = Convert.ToDouble(reader["Iznos"]);
        //                //DateTime date = Convert.ToDateTime(reader["Datum"]);
        //                //TimeSpan time = ((TimeSpan)reader["Vrijeme"]);
        //                //string service = reader["Usluga"].ToString();
        //                //string salon = reader["KozmetickiSalon"].ToString();
        //                int orderId = Convert.ToInt32(reader["NARUDZBA_IdNarudzbe"]);
        //                //int promotionId = Convert.ToInt32(reader["PROMOCIJA_IdPromocije"]);

        //                //TODO !!!!!!!!!!!!!!

        //                //invoices.Add(new InvoiceView(beauticianId, queryId, price, date, time, service, salon));
        //                invoices.Add(new InvoiceView());
        //            }
        //        }
        //    }
        //    return invoices;
        //}

        //public static List<InvoiceView> GetAllInvoicesForBeautySalon(int beautySalonId)
        //{
        //    List<InvoiceView> invoices = new List<InvoiceView>();
        //    string query = "SELECT * FROM racun_view WHERE KozmetickiSalonId = @beautySalonId";

        //    using (MySqlConnection connection = DBUtil.GetConnection())
        //    using (MySqlCommand command = new MySqlCommand(query, connection))
        //    {
        //        command.Parameters.AddWithValue("@beautySalonId", beautySalonId);
        //        connection.Open();

        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                int beauticianId = Convert.ToInt32(reader["IdKozmeticara"]);
        //                int queryId = Convert.ToInt32(reader["IdUpita"]);
        //                double price = Convert.ToDouble(reader["Iznos"]);
        //                DateTime date = Convert.ToDateTime(reader["Datum"]);
        //                TimeSpan time = ((TimeSpan)reader["Vrijeme"]);
        //                string service = reader["Usluga"].ToString();
        //                string salon = reader["KozmetickiSalon"].ToString();

        //                //TODO !!!
        //                //invoices.Add(new InvoiceView(beauticianId, queryId, price, date, time, service, salon));
        //                invoices.Add(new InvoiceView());
        //            }
        //        }
        //    }
        //    return invoices;
        //}
    }
}
