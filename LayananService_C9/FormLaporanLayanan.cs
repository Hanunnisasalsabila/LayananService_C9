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
    public partial class FormLaporanLayanan : Form
    {
        public FormLaporanLayanan()
        {
            InitializeComponent();
        }

        private void FormLaporanLayanan_Load(object sender, EventArgs e)
        {

            SetupReport();
            this.reportViewer1.RefreshReport();
        }
        private void SetupReport()
        {
            // 1. Definisikan query SQL
            string query = "SELECT ID_Layanan, Nama_Layanan, Harga, Kategori_Layanan FROM Layanan";

            // 2. Buat DataTable
            DataTable dt = new DataTable();

            // 3. Gunakan koneksi terpusat
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

            // Mengambil path dari folder instalasi aplikasi
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            // Menggabungkan path dengan nama file laporan
            string reportPath = Path.Combine(appPath, "LaporanLayanan.rdlc");

            reportViewer1.LocalReport.ReportPath = reportPath;

            // Refresh report
            reportViewer1.RefreshReport();
        }

        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            FormLaporanLayanan formLaporan = new FormLaporanLayanan();
            formLaporan.Show();
        }
    }
}
