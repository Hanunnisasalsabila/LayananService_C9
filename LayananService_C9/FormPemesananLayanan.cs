using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Runtime.Caching;

namespace LayananService_C9
{
    public partial class FormPemesananLayanan : Form
    {
        private readonly MemoryCache cache = MemoryCache.Default;
        private readonly CacheItemPolicy policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };

        private const string CacheKeyPelanggan = "PelangganData";
        private const string CacheKeyLayanan = "LayananData";
        private const string CacheKeyPemesanan = "PemesananData";

        public FormPemesananLayanan()
        {
            InitializeComponent();
            SetupUI();
        }

        private void FormPemesananLayanan_Load(object sender, EventArgs e)
        {
            EnsureIndexes();
            TampilkanPelanggan();
            TampilkanLayanan();
            TampilkanDataPemesanan();
        }

        private void EnsureIndexes()
        {
            var indexScript = @"
            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PemesananLayanan_IDPelanggan')
                CREATE NONCLUSTERED INDEX IX_PemesananLayanan_IDPelanggan ON dbo.PemesananLayanan(ID_Pelanggan);
            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PemesananLayanan_IDLayanan')
                CREATE NONCLUSTERED INDEX IX_PemesananLayanan_IDLayanan ON dbo.PemesananLayanan(ID_Layanan);
            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Pelanggan_Nama')
                CREATE NONCLUSTERED INDEX IX_Pelanggan_Nama ON dbo.Pelanggan(Nama_Pelanggan);
            ";
            try
            {
                using (var conn = Koneksi.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(indexScript, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memastikan indeks database: " + ex.Message, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupUI()
        {
            PictureBox background = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = Properties.Resources.otomotif,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            this.Controls.Add(background);

            Label lblTitle = new Label
            {
                Text = "Form Pemesanan Layanan",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            background.Controls.Add(lblTitle);
        }

        void TampilkanPelanggan()
        {
            if (cache.Contains(CacheKeyPelanggan))
            {
                cmbPelanggan.DataSource = cache.Get(CacheKeyPelanggan) as DataTable;
            }
            else
            {
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT ID_Pelanggan, Nama_Pelanggan FROM Pelanggan", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbPelanggan.DataSource = dt;
                    cache.Add(CacheKeyPelanggan, dt, policy);
                }
            }
            cmbPelanggan.DisplayMember = "Nama_Pelanggan";
            cmbPelanggan.ValueMember = "ID_Pelanggan";
            cmbPelanggan.SelectedIndex = -1;
        }

        void TampilkanLayanan()
        {
            if (cache.Contains(CacheKeyLayanan))
            {
                cmbLayanan.DataSource = cache.Get(CacheKeyLayanan) as DataTable;
            }
            else
            {
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT ID_Layanan, Nama_Layanan FROM Layanan", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbLayanan.DataSource = dt;
                    cache.Add(CacheKeyLayanan, dt, policy);
                }
            }
            cmbLayanan.DisplayMember = "Nama_Layanan";
            cmbLayanan.ValueMember = "ID_Layanan";
            cmbLayanan.SelectedIndex = -1;
        }

        void TampilkanDataPemesanan()
        {
            string query = @"SELECT pl.ID_Pemesanan, 
                                    p.Nama_Pelanggan, 
                                    p.Poin_Loyalitas,
                                    l.Nama_Layanan, 
                                    pl.Tanggal_Pemesanan, 
                                    pl.Jumlah_Layanan, 
                                    pl.Total_Biaya, 
                                    pl.Status_Pembayaran
                               FROM PemesananLayanan pl
                               JOIN Pelanggan p ON p.ID_Pelanggan = pl.ID_Pelanggan
                               JOIN Layanan l ON l.ID_Layanan = pl.ID_Layanan";

            using (SqlConnection conn = Koneksi.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPemesanan.DataSource = dt;
            }
        }

        void InvalidatePemesananCache()
        {
            cache.Remove(CacheKeyPemesanan);
            cache.Remove(CacheKeyPelanggan);
        }

        void BersihkanForm()
        {
            txtIDPemesanan.Clear();
            cmbPelanggan.SelectedIndex = -1;
            cmbLayanan.SelectedIndex = -1;
            cmbKategori.SelectedIndex = -1;
            dtpTanggal.Value = DateTime.Now;
            dgvPemesanan.ClearSelection();
        }

        decimal HitungTotalBiaya(int idLayanan, int jumlah)
        {
            using (SqlConnection conn = Koneksi.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Harga FROM Layanan WHERE ID_Layanan = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", idLayanan);
                object result = cmd.ExecuteScalar();
                decimal harga = result != null ? Convert.ToDecimal(result) : 0;
                return harga * jumlah;
            }
        }

        // MODIFIKASI: Metode terpusat untuk menangani eror SQL
        private void HandleSqlException(SqlException sqlEx)
        {
            // Eror 547: Pelanggaran CHECK atau FOREIGN KEY constraint
            if (sqlEx.Number == 547)
            {
                if (sqlEx.Message.Contains("FK_Pemesanan_To_Pelanggan"))
                {
                    MessageBox.Show("Gagal. Pelanggan yang dipilih tidak valid atau tidak ditemukan.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (sqlEx.Message.Contains("FK_Pemesanan_To_Layanan"))
                {
                    MessageBox.Show("Gagal. Layanan yang dipilih tidak valid atau tidak ditemukan.", "Data Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (sqlEx.Message.Contains("Status_Pembayaran"))
                {
                    MessageBox.Show("Status pembayaran tidak valid. Harap pilih 'Lunas' atau 'Belum Lunas'.", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Data yang Anda masukkan tidak sesuai aturan yang ada di database.", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            // Untuk eror SQL lainnya
            else
            {
                MessageBox.Show("Terjadi kesalahan pada database: " + sqlEx.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // MODIFIKASI: Validasi input di sisi aplikasi
            if (cmbPelanggan.SelectedValue == null || cmbLayanan.SelectedValue == null || cmbKategori.SelectedItem == null)
            {
                MessageBox.Show("Harap pilih semua kolom dengan benar!.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idPelanggan = Convert.ToInt32(cmbPelanggan.SelectedValue);
            int idLayanan = Convert.ToInt32(cmbLayanan.SelectedValue);
            decimal total = HitungTotalBiaya(idLayanan, 1);

            using (SqlConnection conn = Koneksi.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.sp_CreatePemesananLayanan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Pelanggan", idPelanggan);
                    cmd.Parameters.AddWithValue("@ID_Layanan", idLayanan);
                    cmd.Parameters.AddWithValue("@Jumlah_Layanan", 1);
                    cmd.Parameters.AddWithValue("@Total_Biaya", total);
                    cmd.Parameters.AddWithValue("@Status_Pembayaran", cmbKategori.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Tanggal_Pemesanan", dtpTanggal.Value);
                    cmd.ExecuteNonQuery();

                    int poinTambahan = (int)(total / 100000);
                    if (poinTambahan > 0)
                    {
                        SqlCommand cmdPoin = new SqlCommand("UPDATE Pelanggan SET Poin_Loyalitas = Poin_Loyalitas + @Poin WHERE ID_Pelanggan = @ID", conn, transaction);
                        cmdPoin.Parameters.AddWithValue("@Poin", poinTambahan);
                        cmdPoin.Parameters.AddWithValue("@ID", idPelanggan);
                        cmdPoin.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    MessageBox.Show("Data pemesanan berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InvalidatePemesananCache();
                    TampilkanDataPemesanan();
                    TampilkanPelanggan();
                    BersihkanForm();
                }
                catch (SqlException sqlEx)
                {
                    transaction.Rollback();
                    HandleSqlException(sqlEx); // Panggil metode penanganan eror
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Gagal menyimpan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvPemesanan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                int idPemesanan = Convert.ToInt32(dgvPemesanan.SelectedRows[0].Cells["ID_Pemesanan"].Value);

                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand("dbo.sp_DeletePemesananLayanan", conn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_Pemesanan", idPemesanan);
                        cmd.ExecuteNonQuery();
                        transaction.Commit();

                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InvalidatePemesananCache();
                        TampilkanDataPemesanan();
                        BersihkanForm();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Gagal menghapus data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ... (Sisa kode seperti Analisis, CellClick, dan navigasi form tetap sama) ...
        private void AnalyzeQuery(string sqlQuery)
        {
            var statsMessages = new StringBuilder();

            using (var conn = Koneksi.GetConnection())
            {
                conn.InfoMessage += (s, e) => statsMessages.AppendLine(e.Message);
                conn.Open();
                var wrappedQuery = $@"
                SET STATISTICS IO ON;
                SET STATISTICS TIME ON;
                {sqlQuery}
                SET STATISTICS IO OFF;
                SET STATISTICS TIME OFF;";

                using (var cmd = new SqlCommand(wrappedQuery, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        do
                        {
                            while (reader.Read()) { /* Cukup baca semua baris */ }
                        } while (reader.NextResult());
                    }
                }
            }

            if (statsMessages.Length > 0)
            {
                MessageBox.Show(statsMessages.ToString(), "Informasi Statistik Query");
            }
            else
            {
                MessageBox.Show("Tidak ada informasi statistik yang dihasilkan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            string heavyQuery = @"SELECT pl.ID_Pemesanan, 
                                    p.Nama_Pelanggan, 
                                    p.Poin_Loyalitas,
                                    l.Nama_Layanan, 
                                    pl.Tanggal_Pemesanan, 
                                    pl.Jumlah_Layanan, 
                                    pl.Total_Biaya, 
                                    pl.Status_Pembayaran
                               FROM PemesananLayanan pl
                               JOIN Pelanggan p ON p.ID_Pelanggan = pl.ID_Pelanggan
                               JOIN Layanan l ON l.ID_Layanan = pl.ID_Layanan
                               WHERE p.Nama_Pelanggan LIKE 'A%';";

            AnalyzeQuery(heavyQuery);
        }

        private void dgvPemesanan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPemesanan.Rows[e.RowIndex].Cells["ID_Pemesanan"].Value != null)
            {
                DataGridViewRow row = dgvPemesanan.Rows[e.RowIndex];
                txtIDPemesanan.Text = row.Cells["ID_Pemesanan"].Value.ToString();
                string namaPelanggan = row.Cells["Nama_Pelanggan"].Value.ToString();
                string namaLayanan = row.Cells["Nama_Layanan"].Value.ToString();
                cmbPelanggan.Text = namaPelanggan;
                cmbLayanan.Text = namaLayanan;
                cmbKategori.SelectedItem = row.Cells["Status_Pembayaran"].Value.ToString();
                dtpTanggal.Value = Convert.ToDateTime(row.Cells["Tanggal_Pemesanan"].Value);
            }
        }

        private void labelJenisPesanan_Click(object sender, EventArgs e)
        {

        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            BersihkanForm();
        }

        private void btnPelanggan_Click(object sender, EventArgs e)
        {
            Form1 formPelanggan = new Form1();
            this.Hide();
            formPelanggan.ShowDialog();
        }

        private void btnLayanan_Click(object sender, EventArgs e)
        {
            Form2 formLayanan = new Form2();
            this.Hide();
            formLayanan.ShowDialog();
        }

        private void btnLaporan_Click(object sender, EventArgs e)
        {
            FormLaporanPemesanan formLaporanPemesanan = new FormLaporanPemesanan();
            this.Hide();
            formLaporanPemesanan.ShowDialog();
        }
    }
}