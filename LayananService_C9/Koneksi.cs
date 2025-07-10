using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace LayananService_C9
{
    internal class Koneksi
    {
        public static SqlConnection GetConnection()
        {
            string localIP = GetLocalIPAddress();
            string connectionStr = $"Data Source={localIP};Initial Catalog=SistemManajemenOtomotif;Integrated Security=True;";
            return new SqlConnection(connectionStr);
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Tidak ada alamat IP lokal yang ditemukan.");
        }
    }
}
