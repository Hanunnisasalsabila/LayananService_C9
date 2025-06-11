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
            string connectionString = "Data Source=LAPTOP-DKIM0LL5\\HANUNNISA;Initial Catalog=SistemManajemenPelanggandanLayananOtomotifff;Integrated Security=True";
            string query = "SELECT ID_Layanan, Nama_Layanan, Harga, Kategori_Layanan FROM Layanan";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.LocalReport.ReportPath = @"E:\PABD\LayananService_C9_UCPDua\LayananService_C9_UCP1\LayananService_C9_AlmostFinished\LayananService_C9\LaporanLayanan.rdlc";
            reportViewer1.RefreshReport();
        }

        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            FormLaporanLayanan formLaporan = new FormLaporanLayanan();
            formLaporan.Show();
        }
    }
}
