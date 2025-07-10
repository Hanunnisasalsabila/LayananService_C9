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
    public partial class PreviewFormLayanan : Form
    {
        private DataGridView dgvPreview;
        private Button btnOK;
        public PreviewFormLayanan(DataTable data)
        {
            InitializeComponent();
            dgvPreviewLayanan.DataSource = data;
        }

        private void PreviewFormLayanan_Load(object sender, EventArgs e)
        {
            dgvPreviewLayanan.AutoResizeColumns();
        }

        private void btnOK2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah Anda yakin ingin mengimpor data ini ke database?",
                "Konfirmasi Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ImportDataToDatabase();
            }
        }

        private bool ValidateRow(DataRow row)
        {
            try
            {
                // Validasi Nama Layanan tidak boleh kosong
                if (string.IsNullOrWhiteSpace(row["Nama_Layanan"].ToString()))
                {
                    MessageBox.Show($"Nama layanan tidak boleh kosong pada baris data.",
                        "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Validasi Harga harus berupa angka dan lebih dari 0
                if (!decimal.TryParse(row["Harga"].ToString(), out decimal harga) || harga <= 0)
                {
                    MessageBox.Show($"Harga harus berupa angka yang valid dan lebih dari 0 pada baris: {row["Nama_Layanan"]}",
                        "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Validasi Kategori tidak boleh kosong
                if (string.IsNullOrWhiteSpace(row["Kategori_Layanan"].ToString()))
                {
                    MessageBox.Show($"Kategori layanan tidak boleh kosong pada baris: {row["Nama_Layanan"]}",
                        "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validasi data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void ImportDataToDatabase()
        {
            try
            {
                DataTable dt = (DataTable)dgvPreviewLayanan.DataSource;
                int successCount = 0;
                int failCount = 0;

                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            // Validasi setiap baris sebelum import
                            if (!ValidateRow(row))
                            {
                                failCount++;
                                continue;
                            }

                            // Gunakan stored procedure untuk insert data
                            using (SqlCommand cmd = new SqlCommand("sp_CreateLayanan", conn, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@Nama_Layanan", row["Nama_Layanan"].ToString().Trim());
                                cmd.Parameters.AddWithValue("@Harga", Convert.ToDecimal(row["Harga"]));
                                cmd.Parameters.AddWithValue("@Kategori_Layanan", row["Kategori_Layanan"].ToString().Trim());

                                cmd.ExecuteNonQuery();
                                successCount++;
                            }
                        }

                        // Commit transaksi jika semua berhasil
                        transaction.Commit();

                        MessageBox.Show($"Data berhasil diimpor!\n\nBerhasil: {successCount} data\nGagal: {failCount} data",
                            "Import Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        // Rollback jika terjadi error
                        transaction.Rollback();
                        MessageBox.Show($"Gagal mengimpor data: {ex.Message}",
                            "Error Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saat mengimpor data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.ShowDialog();
            this.Show();
        }
    }
}
