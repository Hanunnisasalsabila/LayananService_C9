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
    public partial class Form1 : Form
    {
        // Kontrol untuk panel login
        Panel panelLogin;
        TextBox txtUsername;
        TextBox txtPassword;
        Button btnLogin;

        // Inisialisasi cache
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKeyPelanggan = "PelangganData";

        public Form1()
        {
            InitializeComponent();
            SetupUI();
            SetupLoginPanel();
            panelLogin.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Dibiarkan kosong, data dimuat setelah login
        }

        #region Bagian Login
        private void SetupLoginPanel()
        {
            panelLogin = new Panel
            {
                Size = new Size(400, 200),
                Location = new Point((this.ClientSize.Width - 400) / 2, (this.ClientSize.Height - 200) / 2),
                BackColor = Color.White,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Anchor = AnchorStyles.None
            };

            Label lblUser = new Label { Text = "Username:", Location = new Point(30, 30), AutoSize = true };
            txtUsername = new TextBox { Location = new Point(120, 30), Size = new Size(200, 20) };
            Label lblPass = new Label { Text = "Password:", Location = new Point(30, 70), AutoSize = true };
            txtPassword = new TextBox { Location = new Point(120, 70), PasswordChar = '*', Size = new Size(200, 20) };
            btnLogin = new Button { Text = "Login", Location = new Point(120, 110) };
            btnLogin.Click += BtnLogin_Click;

            panelLogin.Controls.AddRange(new Control[] { lblUser, txtUsername, lblPass, txtPassword, btnLogin });
            this.Controls.Add(panelLogin);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (SqlConnection conn = Koneksi.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Admin WHERE Username = @Username AND Password = @Password";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Login berhasil!");
                        panelLogin.Visible = false;
                        EnsureIndexes();
                        TampilkanDataPelanggan();
                    }
                    else
                    {
                        MessageBox.Show("Username atau password salah!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat login: " + ex.Message);
                }
            }
        }
        #endregion

        #region Modul Optimasi Query
        private void EnsureIndexes()
        {
            var indexScript = @"
            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Pelanggan_Nama')
            BEGIN
                CREATE NONCLUSTERED INDEX IX_Pelanggan_Nama ON dbo.Pelanggan(Nama_Pelanggan);
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

        void TampilkanDataPelanggan()
        {
            if (_cache.Contains(CacheKeyPelanggan))
            {
                dgvPelanggan.DataSource = _cache.Get(CacheKeyPelanggan) as DataTable;
            }
            else
            {
                try
                {
                    using (SqlConnection conn = Koneksi.GetConnection())
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Pelanggan ORDER BY ID_Pelanggan DESC", conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvPelanggan.DataSource = dt;
                        _cache.Add(CacheKeyPelanggan, dt, _policy);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menampilkan data: " + ex.Message);
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
                        do { while (reader.Read()) { } } while (reader.NextResult());
                    }
                }
            }
            if (statsMessages.Length > 0)
            {
                MessageBox.Show(statsMessages.ToString(), "Informasi Statistik Query");
            }
        }
        #endregion

        #region Modul Stored Procedure, Transaksi, dan Error Handling
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtNoTelp.Text))
            {
                MessageBox.Show("Semua kolom harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    SqlCommand cmd = new SqlCommand("sp_CreatePelanggan", conn, transaction); // BARU: Mengaitkan command dengan transaksi
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nama_Pelanggan", txtNama.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@No_Telp", txtNoTelp.Text);

                    cmd.ExecuteNonQuery();

                    // BARU: Jika berhasil, commit perubahan
                    transaction.Commit();

                    MessageBox.Show("Data pelanggan berhasil ditambahkan!");
                    _cache.Remove(CacheKeyPelanggan);
                    TampilkanDataPelanggan();
                    BersihkanForm();
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

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvPelanggan.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvPelanggan.SelectedRows[0].Cells["ID_Pelanggan"].Value);
                    using (SqlConnection conn = Koneksi.GetConnection())
                    {
                        // BARU: Deklarasi transaksi
                        SqlTransaction transaction = null;
                        try
                        {
                            conn.Open();
                            // BARU: Memulai transaksi
                            transaction = conn.BeginTransaction();

                            SqlCommand cmd = new SqlCommand("sp_DeletePelanggan", conn, transaction); // BARU: Mengaitkan command dengan transaksi
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID_Pelanggan", id);
                            cmd.ExecuteNonQuery();

                            // BARU: Jika berhasil, commit perubahan
                            transaction.Commit();

                            MessageBox.Show("Data berhasil dihapus!");
                            _cache.Remove(CacheKeyPelanggan);
                            TampilkanDataPelanggan();
                            BersihkanForm();
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
            else
            {
                MessageBox.Show("Pilih baris data yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPelanggan.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvPelanggan.SelectedRows[0].Cells["ID_Pelanggan"].Value);
                using (SqlConnection conn = Koneksi.GetConnection())
                {
                    // BARU: Deklarasi transaksi
                    SqlTransaction transaction = null;
                    try
                    {
                        conn.Open();
                        // BARU: Memulai transaksi
                        transaction = conn.BeginTransaction();

                        SqlCommand cmd = new SqlCommand("sp_UpdatePelanggan", conn, transaction); // BARU: Mengaitkan command dengan transaksi
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ID_Pelanggan", id);
                        cmd.Parameters.AddWithValue("@Nama_Pelanggan", txtNama.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@No_Telp", txtNoTelp.Text);

                        cmd.ExecuteNonQuery();

                        // BARU: Jika berhasil, commit perubahan
                        transaction.Commit();

                        MessageBox.Show("Data pelanggan berhasil diperbarui!");
                        _cache.Remove(CacheKeyPelanggan);
                        TampilkanDataPelanggan();
                        BersihkanForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal memperbarui data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            else
            {
                MessageBox.Show("Pilih data yang ingin diedit.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region UI dan Helper Methods
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
                Text = "Manajemen Pelanggan",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            background.Controls.Add(lblTitle);
        }

        void BersihkanForm()
        {
            txtNama.Text = "";
            txtEmail.Text = "";
            txtNoTelp.Text = "";
            dgvPelanggan.ClearSelection();
        }

        private void dgvPelanggan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPelanggan.Rows[e.RowIndex];
                txtNama.Text = row.Cells["Nama_Pelanggan"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtNoTelp.Text = row.Cells["No_Telp"].Value.ToString();
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            BersihkanForm();
        }

        private void btnLayanan_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.ShowDialog();
            this.Show();
        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            string queryToAnalyze = "SELECT * FROM Pelanggan";
            AnalyzeQuery(queryToAnalyze);
        }
        #endregion

        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            FormLaporanPelanggan formLaporan = new FormLaporanPelanggan();
            formLaporan.Show();
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

                using (PreviewFormPelanggan previewForm = new PreviewFormPelanggan(dt))
                {
                    var result = previewForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        MessageBox.Show("Data di form utama akan diperbarui.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _cache.Remove(CacheKeyPelanggan);
                        TampilkanDataPelanggan();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat membaca file Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}