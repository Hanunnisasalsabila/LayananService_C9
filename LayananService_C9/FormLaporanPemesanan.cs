using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LayananService_C9
{
    public partial class FormLaporanPemesanan : Form
    {
        public FormLaporanPemesanan()
        {
            InitializeComponent();
        }

        private void FormLaporanPemesanan_Load(object sender, EventArgs e)
        {
            try
            {
                // Konfigurasi dan muat data ke ReportViewer
                SetupReportViewer();

                // Refresh report untuk menampilkan data
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat laporan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupReportViewer()
        {
            // 1. Definisikan query untuk mengambil data laporan
            string query = @"
                SELECT 
                    pl.ID_Pemesanan, p.Nama_Pelanggan, p.Poin_Loyalitas,
                    l.Nama_Layanan, pl.Tanggal_Pemesanan, pl.Jumlah_Layanan, 
                    pl.Total_Biaya, pl.Status_Pembayaran
                FROM 
                    PemesananLayanan pl
                JOIN 
                    Pelanggan p ON p.ID_Pelanggan = pl.ID_Pelanggan
                JOIN 
                    Layanan l ON l.ID_Layanan = pl.ID_Layanan";

            DataTable dt = new DataTable();

            // 2. Isi DataTable menggunakan koneksi dan query
            using (SqlConnection conn = Koneksi.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            // 3. Buat ReportDataSource
            //    Nama "DataSetPemesanan" HARUS SAMA dengan nama DataSet di file .rdlc Anda
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);

            // 4. Konfigurasi ReportViewer
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            // PENTING: Ganti path ini dengan path absolut ke file LaporanPemesanan.rdlc di komputer Anda
            reportViewer1.LocalReport.ReportPath = @"E:\PABD\LayananService_C9_UCPDua\LayananService_C9_UCP1\LayananService_C9_AlmostFinished\LayananService_C9\LaporanPemesanan.rdlc";
        }

        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            FormLaporanPemesanan formLaporan = new FormLaporanPemesanan();
            formLaporan.Show();
        }
    }
}
