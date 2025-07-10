using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
            // 1. Definisikan query SQL
            string query = "SELECT ID_Pelanggan, Nama_Pelanggan, Email, No_Telp, Poin_Loyalitas FROM Pelanggan";

            // 2. Buat DataTable untuk menampung data
            DataTable dt = new DataTable();

            // 3. Gunakan koneksi dari kelas Koneksi untuk mengisi DataTable
            using (SqlConnection conn = Koneksi.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            // 4. Buat ReportDataSource
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);

            // 5. Konfigurasi ReportViewer dengan path dinamis
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            // --- BAGIAN YANG DIUBAH ---
            // Mengambil path folder tempat .exe berjalan
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            // Menggabungkan path folder dengan nama file .rdlc
            string reportPath = Path.Combine(appPath, "LaporanPelanggan.rdlc");

            reportViewer1.LocalReport.ReportPath = reportPath;
            // -------------------------

            // Refresh report viewer
            reportViewer1.RefreshReport();
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
