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
                Image = Image.FromFile("C:\\Users\\Acer\\OneDrive\\Pictures\\860.jpeg"),
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

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNamaLayanan.Text) || numHarga.Value <= 0)
            {
                MessageBox.Show("Nama layanan dan harga harus diisi dengan benar.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = Koneksi.GetConnection())
            {
                // BARU: Deklarasi transaksi
                SqlTransaction transaction = null;

                try
                {
                    conn.Open();
                    // BARU: Memulai transaksi
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("sp_CreateLayanan", conn, transaction); // BARU: Mengaitkan command dengan transaksi
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nama_Layanan", txtNamaLayanan.Text);
                    cmd.Parameters.AddWithValue("@Harga", numHarga.Value);
                    cmd.Parameters.AddWithValue("@Kategori_Layanan", cmbKategori.SelectedItem.ToString());

                    cmd.ExecuteNonQuery();

                    // BARU: Jika berhasil, commit perubahan
                    transaction.Commit();

                    MessageBox.Show("Data layanan berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _cache.Remove(CacheKeyLayanan);
                    TampilkanDataLayanan();
                    BersihkanFormLayanan();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menambahkan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    try
                    {
                        // BARU: Jika gagal, batalkan semua perubahan
                        transaction?.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        MessageBox.Show("Gagal melakukan rollback: " + exRollback.Message, "Error Kritis", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

            using (SqlConnection conn = Koneksi.GetConnection())
            {
                // BARU: Deklarasi transaksi
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    // BARU: Memulai transaksi
                    transaction = conn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("sp_UpdateLayanan", conn, transaction); // BARU: Mengaitkan command dengan transaksi
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_Layanan", Convert.ToInt32(txtIDLayanan.Text));
                    cmd.Parameters.AddWithValue("@Nama_Layanan", txtNamaLayanan.Text);
                    cmd.Parameters.AddWithValue("@Harga", numHarga.Value);
                    cmd.Parameters.AddWithValue("@Kategori_Layanan", cmbKategori.SelectedItem.ToString());

                    cmd.ExecuteNonQuery();

                    // BARU: Jika berhasil, commit perubahan
                    transaction.Commit();

                    MessageBox.Show("Data layanan berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _cache.Remove(CacheKeyLayanan);
                    TampilkanDataLayanan();
                    BersihkanFormLayanan();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengedit data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    try
                    {
                        // BARU: Jika gagal, batalkan semua perubahan
                        transaction?.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        MessageBox.Show("Gagal melakukan rollback: " + exRollback.Message, "Error Kritis", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                    // BARU: Deklarasi transaksi
                    SqlTransaction transaction = null;
                    try
                    {
                        conn.Open();
                        // BARU: Memulai transaksi
                        transaction = conn.BeginTransaction();

                        SqlCommand cmd = new SqlCommand("sp_DeleteLayanan", conn, transaction); // BARU: Mengaitkan command dengan transaksi
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ID_Layanan", Convert.ToInt32(txtIDLayanan.Text));
                        cmd.ExecuteNonQuery();

                        // BARU: Jika berhasil, commit perubahan
                        transaction.Commit();

                        MessageBox.Show("Data layanan berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _cache.Remove(CacheKeyLayanan);
                        TampilkanDataLayanan();
                        BersihkanFormLayanan();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menghapus data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        try
                        {
                            // BARU: Jika gagal, batalkan semua perubahan
                            transaction?.Rollback();
                        }
                        catch (Exception exRollback)
                        {
                            MessageBox.Show("Gagal melakukan rollback: " + exRollback.Message, "Error Kritis", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            BersihkanFormLayanan();
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xlsm";
            openFileDialog.Title = "Pilih File Excel untuk Import Data Layanan";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                PreviewData(filePath);
            }
        }

        private void PreviewData(string filePath)
        {
            try
            {
                DataTable dt = new DataTable();

                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheetAt(0);

                    // Baca header row (baris pertama)
                    IRow headerRow = sheet.GetRow(0);
                    if (headerRow != null)
                    {
                        // Buat kolom berdasarkan header
                        foreach (var cell in headerRow.Cells)
                        {
                            dt.Columns.Add(cell.ToString());
                        }
                    }
                    else
                    {
                        // Jika tidak ada header, buat kolom default untuk layanan
                        dt.Columns.Add("Nama_Layanan");
                        dt.Columns.Add("Harga");
                        dt.Columns.Add("Kategori_Layanan");
                    }

                    // Baca data dari baris kedua hingga terakhir
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow dataRow = sheet.GetRow(i);
                        if (dataRow != null)
                        {
                            DataRow newRow = dt.NewRow();

                            for (int cellIndex = 0; cellIndex < dt.Columns.Count && cellIndex < dataRow.Cells.Count; cellIndex++)
                            {
                                var cell = dataRow.GetCell(cellIndex);
                                if (cell != null)
                                {
                                    // Handle different cell types
                                    switch (cell.CellType)
                                    {
                                        case CellType.String:
                                            newRow[cellIndex] = cell.StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            newRow[cellIndex] = cell.NumericCellValue.ToString();
                                            break;
                                        case CellType.Boolean:
                                            newRow[cellIndex] = cell.BooleanCellValue.ToString();
                                            break;
                                        case CellType.Formula:
                                            try
                                            {
                                                newRow[cellIndex] = cell.NumericCellValue.ToString();
                                            }
                                            catch
                                            {
                                                newRow[cellIndex] = cell.StringCellValue;
                                            }
                                            break;
                                        default:
                                            newRow[cellIndex] = "";
                                            break;
                                    }
                                }
                                else
                                {
                                    newRow[cellIndex] = "";
                                }
                            }

                            dt.Rows.Add(newRow);
                        }
                    }
                }

                // Tampilkan preview form
                PreviewFormLayanan previewForm = new PreviewFormLayanan(dt);
                if (previewForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh data setelah import berhasil
                    _cache.Remove(CacheKeyLayanan);
                    TampilkanDataLayanan();
                    MessageBox.Show("Import data selesai. Data layanan telah diperbarui.",
                        "Import Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saat membaca file Excel: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}