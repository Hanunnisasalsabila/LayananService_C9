using System.Data.SqlClient;

namespace LayananService_C9
{
    internal class Koneksi
    {
        private static string connectionString = "Data Source=LAPTOP-DKIM0LL5\\HANUNNISA;Initial Catalog=SistemManajemenPelanggandanLayananOtomotifff;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
