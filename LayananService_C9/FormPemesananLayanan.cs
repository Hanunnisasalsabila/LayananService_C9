using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Runtime.Caching; // Diperlukan untuk Caching

namespace LayananService_C9
{
    public partial class FormPemesananLayanan : Form
    {
        // Inisialisasi cache dan kebijakan kadaluarsa
        private readonly MemoryCache cache = MemoryCache.Default;
        private readonly CacheItemPolicy policy = new CacheItemPolicy
        {
            // Data di cache akan kadaluarsa setelah 5 menit
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };

        // Kunci unik untuk cache
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
            EnsureIndexes(); // Pastikan index ada sebelum memuat data
            TampilkanPelanggan();
            TampilkanLayanan();
            TampilkanDataPemesanan();
        }

        private void EnsureIndexes()
        {
            // Script untuk membuat index jika belum ada
            var indexScript = @"
            -- Index untuk foreign key di PemesananLayanan untuk mempercepat JOIN
            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PemesananLayanan_IDPelanggan')
                CREATE NONCLUSTERED INDEX IX_PemesananLayanan_IDPelanggan ON dbo.PemesananLayanan(ID_Pelanggan);
            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PemesananLayanan_IDLayanan')
                CREATE NONCLUSTERED INDEX IX_PemesananLayanan_IDLayanan ON dbo.PemesananLayanan(ID_Layanan);
            -- Index pada tabel Pelanggan
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
                // Tampilkan pesan jika gagal membuat index, namun aplikasi tetap berjalan
                MessageBox.Show("Gagal memastikan indeks database: " + ex.Message, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void SetupUI()
        {
            // Kode UI Anda tetap sama
            PictureBox background = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = Image.FromFile("C:\\Users\\Acer\\OneDrive\\Pictures\\860.jpeg"),
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
                // Ambil data dari cache jika ada
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
                    // Simpan data ke cache
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
                // Ambil data dari cache
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
                    // Simpan data ke cache
                    cache.Add(CacheKeyLayanan, dt, policy);
                }
            }
            cmbLayanan.DisplayMember = "Nama_Layanan";
            cmbLayanan.ValueMember = "ID_Layanan";
            cmbLayanan.SelectedIndex = -1;
        }

        void TampilkanDataPemesanan()
        {
            // Selalu ambil data terbaru untuk grid pemesanan, tapi bisa juga di-cache jika diinginkan
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
            // Hapus cache yang relevan setelah ada perubahan data
            cache.Remove(CacheKeyPemesanan);
            // Anda mungkin juga ingin merefresh cache pelanggan jika poin loyalitas berubah
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

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (cmbPelanggan.SelectedValue == null || cmbLayanan.SelectedValue == null || cmbKategori.SelectedItem == null)
            {
                MessageBox.Show("Harap lengkapi semua data (pelanggan, layanan, dan status).", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idPelanggan = Convert.ToInt32(cmbPelanggan.SelectedValue);
            int idLayanan = Convert.ToInt32(cmbLayanan.SelectedValue);
            decimal total = HitungTotalBiaya(idLayanan, 1);

            using (SqlConnection conn = Koneksi.GetConnection())
            {
                conn.Open();
                // Deklarasi transaksi
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Panggil Stored Procedure untuk membuat pemesanan
                    SqlCommand cmd = new SqlCommand("dbo.sp_CreatePemesananLayanan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Pelanggan", idPelanggan);
                    cmd.Parameters.AddWithValue("@ID_Layanan", idLayanan);
                    cmd.Parameters.AddWithValue("@Jumlah_Layanan", 1);
                    cmd.Parameters.AddWithValue("@Total_Biaya", total);
                    cmd.Parameters.AddWithValue("@Status_Pembayaran", cmbKategori.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Tanggal_Pemesanan", dtpTanggal.Value);
                    cmd.ExecuteNonQuery();

                    // 2. Logika untuk menambah poin loyalitas
                    int poinTambahan = (int)(total / 100000);
                    if (poinTambahan > 0)
                    {
                        SqlCommand cmdPoin = new SqlCommand("UPDATE Pelanggan SET Poin_Loyalitas = Poin_Loyalitas + @Poin WHERE ID_Pelanggan = @ID", conn, transaction);
                        cmdPoin.Parameters.AddWithValue("@Poin", poinTambahan);
                        cmdPoin.Parameters.AddWithValue("@ID", idPelanggan);
                        cmdPoin.ExecuteNonQuery();
                    }

                    // Jika semua perintah berhasil, simpan perubahan secara permanen
                    transaction.Commit();

                    MessageBox.Show("Data pemesanan berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InvalidatePemesananCache();
                    TampilkanDataPemesanan();
                    TampilkanPelanggan(); // Refresh data pelanggan untuk poin baru
                    BersihkanForm();
                }
                catch (Exception ex)
                {
                    // Jika terjadi error, batalkan semua perubahan dalam transaksi
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

                        // Commit transaksi jika berhasil
                        transaction.Commit();

                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InvalidatePemesananCache(); // Hapus cache
                        TampilkanDataPemesanan();
                        BersihkanForm();
                    }
                    catch (Exception ex)
                    {
                        // Rollback jika terjadi error
                        transaction.Rollback();
                        MessageBox.Show("Gagal menghapus data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void AnalyzeQuery(string sqlQuery)
        {
            // Gunakan StringBuilder untuk mengumpulkan semua pesan
            var statsMessages = new StringBuilder();

            using (var conn = Koneksi.GetConnection())
            {
                // Tambahkan event handler untuk mengumpulkan pesan ke StringBuilder
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
                        // Loop yang lebih andal untuk memastikan semua hasil dan pesan diproses
                        do
                        {
                            while (reader.Read()) { /* Cukup baca semua baris */ }
                        } while (reader.NextResult());
                    }
                }
            }

            // Tampilkan semua pesan yang terkumpul dalam satu MessageBox
            if (statsMessages.Length > 0)
            {
                MessageBox.Show(statsMessages.ToString(), "Informasi Statistik Query");
            }
            else
            {
                MessageBox.Show("Tidak ada informasi statistik yang dihasilkan. Query mungkin tidak mengembalikan baris data.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            // Query berat yang menggabungkan beberapa tabel untuk dianalisis
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
                               WHERE p.Nama_Pelanggan LIKE 'A%';"; // Contoh filter untuk analisis

            AnalyzeQuery(heavyQuery);
        }

        private void dgvPemesanan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPemesanan.Rows[e.RowIndex].Cells["ID_Pemesanan"].Value != null)
            {
                DataGridViewRow row = dgvPemesanan.Rows[e.RowIndex];
                txtIDPemesanan.Text = row.Cells["ID_Pemesanan"].Value.ToString();

                // Cari ID_Pelanggan & ID_Layanan yang sesuai untuk mengisi ComboBox
                string namaPelanggan = row.Cells["Nama_Pelanggan"].Value.ToString();
                string namaLayanan = row.Cells["Nama_Layanan"].Value.ToString();

                // Set ComboBox berdasarkan nama yang ditampilkan (kurang ideal, lebih baik pakai ID jika ada di grid)
                cmbPelanggan.Text = namaPelanggan;
                cmbLayanan.Text = namaLayanan;

                cmbKategori.SelectedItem = row.Cells["Status_Pembayaran"].Value.ToString();
                dtpTanggal.Value = Convert.ToDateTime(row.Cells["Tanggal_Pemesanan"].Value);
            }
        }

        // ... (Tambahkan method event handler lain seperti btnBatal_Click, dll.)

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