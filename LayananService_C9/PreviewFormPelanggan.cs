using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LayananService_C9
{
    public partial class PreviewFormPelanggan : Form
    {
        // DataTable untuk menampung data yang akan di-preview
        private readonly DataTable dataToImport;

        // --- KONTROL UI ---
        // Dideklarasikan di sini agar bisa diakses di seluruh kelas
        private DataGridView dgvPreviewPelanggan;
        private Button btnImport;
        private Button btnBatal;
        private Label lblInfo;


        public PreviewFormPelanggan(DataTable data)
        {
            // Membuat semua komponen visual (DataGridView, Button, dll)
            InitializeComponent();

            // Menyimpan data yang diterima untuk digunakan nanti
            this.dataToImport = data;
        }

        private void PreviewFormPelanggan_Load(object sender, EventArgs e)
        {
            // Menetapkan sumber data setelah form dan kontrolnya selesai dibuat
            dgvPreview.DataSource = dataToImport;

            // Menyesuaikan lebar kolom secara otomatis
            dgvPreview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ImportDataToDatabase()
        {
            DataTable dt = (DataTable)dgvPreview.DataSource;
            int successCount = 0;
            int failCount = 0;

            // Menggunakan koneksi dari class Koneksi yang sudah ada
            using (SqlConnection conn = Koneksi.GetConnection())
            {
                conn.Open();
                // Memulai transaksi untuk memastikan semua data valid diimpor, atau tidak sama sekali.
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        // Validasi setiap baris sebelum diimpor
                        if (!ValidateRow(row))
                        {
                            failCount++;
                            continue; // Lanjut ke baris berikutnya jika validasi gagal
                        }

                        // Gunakan stored procedure 'sp_CreatePelanggan'
                        using (SqlCommand cmd = new SqlCommand("sp_CreatePelanggan", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Clear();
                            // Sesuaikan parameter dengan yang ada di sp_CreatePelanggan
                            cmd.Parameters.AddWithValue("@Nama_Pelanggan", row["Nama_Pelanggan"].ToString().Trim());
                            cmd.Parameters.AddWithValue("@Email", row["Email"].ToString().Trim());
                            cmd.Parameters.AddWithValue("@No_Telp", row["No_Telp"].ToString().Trim());

                            cmd.ExecuteNonQuery();
                            successCount++;
                        }
                    }

                    // Commit transaksi jika semua baris yang valid berhasil diproses
                    transaction.Commit();

                    MessageBox.Show($"Data pelanggan berhasil diimpor!\n\nBerhasil: {successCount} data\nGagal: {failCount} data",
                        "Import Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK; // Tandai bahwa proses berhasil
                    this.Close();
                }
                catch (Exception ex)
                {
                    // Rollback (batalkan) semua perubahan jika terjadi error di tengah jalan
                    transaction.Rollback();
                    MessageBox.Show($"Gagal mengimpor data: {ex.Message}",
                        "Error Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateRow(DataRow row)
        {
            try
            {
                // Validasi Nama Pelanggan tidak boleh kosong
                if (string.IsNullOrWhiteSpace(row["Nama_Pelanggan"].ToString()))
                {
                    MessageBox.Show("Nama pelanggan tidak boleh kosong.",
                        "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Validasi Email tidak boleh kosong
                if (string.IsNullOrWhiteSpace(row["Email"].ToString()))
                {
                    MessageBox.Show($"Email tidak boleh kosong untuk pelanggan: {row["Nama_Pelanggan"]}",
                        "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Validasi No. Telepon tidak boleh kosong
                if (string.IsNullOrWhiteSpace(row["No_Telp"].ToString()))
                {
                    MessageBox.Show($"No. Telepon tidak boleh kosong untuk pelanggan: {row["Nama_Pelanggan"]}",
                        "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true; // Semua validasi untuk baris ini berhasil
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validasi data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah Anda yakin ingin mengimpor data pelanggan ini ke database?",
                "Konfirmasi Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ImportDataToDatabase();
            }
        }

        // Bagian ini menggantikan file .Designer.cs untuk kemudahan

    }
}