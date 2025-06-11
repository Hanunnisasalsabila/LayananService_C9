using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayananService_C9
{
    public partial class FormLaporanPelanggan : Form
    {
        public FormLaporanPelanggan()
        {
            InitializeComponent();
        }

        private void FormLaporanPelanggan_Load(object sender, EventArgs e)
        {

            SetupReport();
            this.reportViewer1.RefreshReport();
        }

        private void SetupReport()
        {
            string connectionString = "Data Source=LAPTOP-DKIM0LL5\\HANUNNISA;Initial Catalog=SistemManajemenPelanggandanLayananOtomotifff;Integrated Security=True";

            // 2. Definisikan query SQL untuk mengambil data yang diperlukan
            string query = "SELECT ID_Pelanggan, Nama_Pelanggan, Email, No_Telp, Poin_Loyalitas FROM Pelanggan";

            // 3. Buat DataTable untuk menampung data dari database
            DataTable dt = new DataTable();

            // 4. Gunakan SqlDataAdapter untuk mengisi DataTable
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            // 5. Buat ReportDataSource
            // Pastikan nama "DataSetPelanggan" SAMA PERSIS dengan nama DataSet di file .rdlc Anda.
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);

            // 6. Konfigurasi ReportViewer
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.LocalReport.ReportPath = @"E:\PABD\LayananService_C9_UCPDua\LayananService_C9_UCP1\LayananService_C9_AlmostFinished\LayananService_C9\LaporanPelanggan.rdlc";
        }

        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            FormLaporanPelanggan formLaporan = new FormLaporanPelanggan();
            formLaporan.Show();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
