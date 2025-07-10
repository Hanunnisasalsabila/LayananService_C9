using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Caching;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LayananService_C9
{
    public partial class Form2 : Form
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };

        private const string CacheKeyLayanan = "LayananData";

        public Form2()
        {
            InitializeComponent();
            SetupUI();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            EnsureIndexes();
            TampilkanDataLayanan();

            if (cmbKategori != null && cmbKategori.Items.Count > 0)
            {
                cmbKategori.SelectedIndex = 0;
            }
        }

        private void EnsureIndexes()
        {
            var indexScript = @"
            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Layanan_Nama')
            BEGIN
                CREATE NONCLUSTERED INDEX IX_Layanan_Nama ON dbo.Layanan(Nama_Layanan);
            END;";
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
                Text = "Form Layanan",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            background.Controls.Add(lblTitle);
        }

        void TampilkanDataLayanan()
        {
            if (_cache.Contains(CacheKeyLayanan))
            {
                dgvLayanan.DataSource = _cache.Get(CacheKeyLayanan) as DataTable;
            }
            else
            {
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    try
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Layanan", conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvLayanan.DataSource = dt;
                        _cache.Add(CacheKeyLayanan, dt, _policy);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menampilkan data: " + ex.Message);
                    }
                }
            }
        }

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
                            while (reader.Read()) { /* Cukup baca semua baris data */ }
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

        void BersihkanFormLayanan()
        {
            txtIDLayanan.Text = "";
            txtNamaLayanan.Text = "";
            numHarga.Value = 0;
            cmbKategori.SelectedIndex = 0;
            dgvLayanan.ClearSelection();
        }

        private void HandleSqlException(SqlException sqlEx)
        {
            // Eror 2627: Pelanggaran UNIQUE KEY (data duplikat)
            if (sqlEx.Number == 2627)
            {
                if (sqlEx.Message.Contains("Nama_Layanan"))
                {
                    MessageBox.Show("Gagal. Nama layanan ini sudah ada. Silakan gunakan nama lain.", "Data Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Gagal menyimpan karena ada data yang sama di database.", "Data Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            // Eror 547: Pelanggaran CHECK constraint (format data salah)
            else if (sqlEx.Number == 547)
            {
                if (sqlEx.Message.Contains("Nama_Layanan"))
                {
                    MessageBox.Show("Nama layanan hanya boleh berisi huruf dan spasi.", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (sqlEx.Message.Contains("Harga"))
                {
                    MessageBox.Show("Harga layanan tidak boleh negatif.", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // BARU: Menangani eror untuk Kategori Layanan
                else if (sqlEx.Message.Contains("Kategori_Layanan"))
                {
                    MessageBox.Show("Kategori layanan tidak valid. Harap pilih 'Standar' atau 'Premium'.", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Data yang Anda masukkan tidak sesuai format yang ditentukan.", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (string.IsNullOrWhiteSpace(txtNamaLayanan.Text))
            {
                MessageBox.Show("Nama layanan harus diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (numHarga.Value < 0)
            {
                MessageBox.Show("Harga tidak boleh negatif.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbKategori.SelectedItem == null)
            {
                MessageBox.Show("Kategori layanan tidak valid. Harap pilih 'Standar' atau 'Premium'.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = Koneksi.GetConnection())
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("sp_CreateLayanan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nama_Layanan", txtNamaLayanan.Text);
                    cmd.Parameters.AddWithValue("@Harga", numHarga.Value);
                    cmd.Parameters.AddWithValue("@Kategori_Layanan", cmbKategori.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    transaction.Commit();

                    MessageBox.Show("Data layanan berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _cache.Remove(CacheKeyLayanan);
                    TampilkanDataLayanan();
                    BersihkanFormLayanan();
                }
                catch (SqlException sqlEx)
                {
                    transaction?.Rollback();
                    HandleSqlException(sqlEx); // Panggil metode penanganan eror
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show("Gagal menambahkan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIDLayanan.Text))
            {
                MessageBox.Show("Pilih data yang ingin diedit.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNamaLayanan.Text))
            {
                MessageBox.Show("Nama layanan harus diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (numHarga.Value < 0)
            {
                MessageBox.Show("Harga tidak boleh negatif.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbKategori.SelectedItem == null)
            {
                MessageBox.Show("Kategori layanan tidak valid. Harap pilih 'Standar' atau 'Premium'.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = Koneksi.GetConnection())
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("sp_UpdateLayanan", conn, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Layanan", Convert.ToInt32(txtIDLayanan.Text));
                    cmd.Parameters.AddWithValue("@Nama_Layanan", txtNamaLayanan.Text);
                    cmd.Parameters.AddWithValue("@Harga", numHarga.Value);
                    cmd.Parameters.AddWithValue("@Kategori_Layanan", cmbKategori.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    transaction.Commit();

                    MessageBox.Show("Data layanan berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _cache.Remove(CacheKeyLayanan);
                    TampilkanDataLayanan();
                    BersihkanFormLayanan();
                }
                catch (SqlException sqlEx)
                {
                    transaction?.Rollback();
                    HandleSqlException(sqlEx); // Panggil metode penanganan eror
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show("Gagal mengedit data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIDLayanan.Text))
            {
                MessageBox.Show("Pilih data yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Yakin ingin menghapus layanan ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    SqlTransaction transaction = null;
                    try
                    {
                        conn.Open();
                        transaction = conn.BeginTransaction();
                        SqlCommand cmd = new SqlCommand("sp_DeleteLayanan", conn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_Layanan", Convert.ToInt32(txtIDLayanan.Text));
                        cmd.ExecuteNonQuery();
                        transaction.Commit();

                        MessageBox.Show("Data layanan berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _cache.Remove(CacheKeyLayanan);
                        TampilkanDataLayanan();
                        BersihkanFormLayanan();
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show("Gagal menghapus data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            BersihkanFormLayanan();
            txtSearchLayanan.Text = "";
            TampilkanDataLayanan();
        }

        private void dgvLayanan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtIDLayanan.Text = dgvLayanan.Rows[e.RowIndex].Cells["ID_Layanan"].Value.ToString();
                txtNamaLayanan.Text = dgvLayanan.Rows[e.RowIndex].Cells["Nama_Layanan"].Value.ToString();
                numHarga.Value = Convert.ToDecimal(dgvLayanan.Rows[e.RowIndex].Cells["Harga"].Value);
                cmbKategori.SelectedItem = dgvLayanan.Rows[e.RowIndex].Cells["Kategori_Layanan"].Value.ToString();
            }
        }

        private void btnPelanggan_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPemesanan_Click(object sender, EventArgs e)
        {
            FormPemesananLayanan formPemesanan = new FormPemesananLayanan();
            this.Hide();
            formPemesanan.ShowDialog();
            this.Show();
        }

        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            FormLaporanLayanan formLaporan = new FormLaporanLayanan();
            formLaporan.Show();
        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            string queryToAnalyze = "SELECT * FROM Layanan WHERE Kategori_Layanan = 'Premium'";
            AnalyzeQuery(queryToAnalyze);
        }

        private void btnImportData_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Pilih File Excel untuk Impor Data Pelanggan"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                PreviewDataFromExcel(filePath);
            }
        }

        private void PreviewDataFromExcel(string filePath)
        {
            try
            {
                DataTable dt = new DataTable();
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);

                    IRow headerRow = sheet.GetRow(0);
                    foreach (ICell headerCell in headerRow.Cells)
                    {
                        dt.Columns.Add(headerCell.ToString());
                    }

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow dataRowExcel = sheet.GetRow(i);
                        if (dataRowExcel == null) continue;

                        DataRow newDataRow = dt.NewRow();
                        for (int j = 0; j < headerRow.LastCellNum; j++)
                        {
                            ICell cell = dataRowExcel.GetCell(j);
                            newDataRow[j] = (cell == null) ? string.Empty : cell.ToString();
                        }
                        dt.Rows.Add(newDataRow);
                    }
                }

                using (PreviewFormLayanan previewForm = new PreviewFormLayanan(dt))
                {
                    var result = previewForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        MessageBox.Show("Data di form utama akan diperbarui.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _cache.Remove(CacheKeyLayanan);
                        TampilkanDataLayanan();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat membaca file Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchLayanan(string searchTerm)
        {
            try
            {
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_SearchLayanan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvLayanan.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal melakukan pencarian: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearchLayanan_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearchLayanan.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                TampilkanDataLayanan();
            }
            else
            {
                SearchLayanan(searchTerm);
            }
        }

        private void txtSearchLayanan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearchLayanan_Click(this, new EventArgs());
                e.SuppressKeyPress = true;
            }
        }
    }
}