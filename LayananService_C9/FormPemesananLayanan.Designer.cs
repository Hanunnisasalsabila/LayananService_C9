namespace LayananService_C9
{
    partial class FormPemesananLayanan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbPelanggan = new System.Windows.Forms.ComboBox();
            this.cmbLayanan = new System.Windows.Forms.ComboBox();
            this.dtpTanggal = new System.Windows.Forms.DateTimePicker();
            this.btnSimpan = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnBatal = new System.Windows.Forms.Button();
            this.dgvPemesanan = new System.Windows.Forms.DataGridView();
            this.btnPelanggan = new System.Windows.Forms.Button();
            this.btnLayanan = new System.Windows.Forms.Button();
            this.cmbKategori = new System.Windows.Forms.ComboBox();
            this.txtIDPemesanan = new System.Windows.Forms.TextBox();
            this.labelNamaPelanggan = new System.Windows.Forms.Label();
            this.labelJenisPesanan = new System.Windows.Forms.Label();
            this.labelStatusPembayaran = new System.Windows.Forms.Label();
            this.labelTanggalPemesanan = new System.Windows.Forms.Label();
            this.btnAnalisis = new System.Windows.Forms.Button();
            this.btnLaporan = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPemesanan)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbPelanggan
            // 
            this.cmbPelanggan.Location = new System.Drawing.Point(549, 115);
            this.cmbPelanggan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbPelanggan.Name = "cmbPelanggan";
            this.cmbPelanggan.Size = new System.Drawing.Size(265, 28);
            this.cmbPelanggan.TabIndex = 12;
            // 
            // cmbLayanan
            // 
            this.cmbLayanan.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.cmbLayanan.FormattingEnabled = true;
            this.cmbLayanan.Location = new System.Drawing.Point(549, 170);
            this.cmbLayanan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbLayanan.Name = "cmbLayanan";
            this.cmbLayanan.Size = new System.Drawing.Size(265, 28);
            this.cmbLayanan.TabIndex = 1;
            this.cmbLayanan.Text = "Pilih Layanan";
            // 
            // dtpTanggal
            // 
            this.dtpTanggal.Location = new System.Drawing.Point(549, 286);
            this.dtpTanggal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(265, 26);
            this.dtpTanggal.TabIndex = 2;
            // 
            // btnSimpan
            // 
            this.btnSimpan.Location = new System.Drawing.Point(1047, 108);
            this.btnSimpan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(94, 49);
            this.btnSimpan.TabIndex = 3;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.UseVisualStyleBackColor = true;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(1047, 199);
            this.btnHapus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(94, 49);
            this.btnHapus.TabIndex = 5;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnBatal
            // 
            this.btnBatal.Location = new System.Drawing.Point(1047, 279);
            this.btnBatal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBatal.Name = "btnBatal";
            this.btnBatal.Size = new System.Drawing.Size(94, 49);
            this.btnBatal.TabIndex = 6;
            this.btnBatal.Text = "Batal";
            this.btnBatal.UseVisualStyleBackColor = true;
            this.btnBatal.Click += new System.EventHandler(this.btnBatal_Click);
            // 
            // dgvPemesanan
            // 
            this.dgvPemesanan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPemesanan.Location = new System.Drawing.Point(57, 458);
            this.dgvPemesanan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPemesanan.Name = "dgvPemesanan";
            this.dgvPemesanan.RowHeadersWidth = 51;
            this.dgvPemesanan.RowTemplate.Height = 24;
            this.dgvPemesanan.Size = new System.Drawing.Size(1222, 249);
            this.dgvPemesanan.TabIndex = 7;
            // 
            // btnPelanggan
            // 
            this.btnPelanggan.Location = new System.Drawing.Point(386, 391);
            this.btnPelanggan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPelanggan.Name = "btnPelanggan";
            this.btnPelanggan.Size = new System.Drawing.Size(266, 40);
            this.btnPelanggan.TabIndex = 9;
            this.btnPelanggan.Text = "Pelanggan";
            this.btnPelanggan.UseVisualStyleBackColor = true;
            this.btnPelanggan.Click += new System.EventHandler(this.btnPelanggan_Click);
            // 
            // btnLayanan
            // 
            this.btnLayanan.Location = new System.Drawing.Point(700, 391);
            this.btnLayanan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLayanan.Name = "btnLayanan";
            this.btnLayanan.Size = new System.Drawing.Size(266, 40);
            this.btnLayanan.TabIndex = 10;
            this.btnLayanan.Text = "Layanan";
            this.btnLayanan.UseVisualStyleBackColor = true;
            this.btnLayanan.Click += new System.EventHandler(this.btnLayanan_Click);
            // 
            // cmbKategori
            // 
            this.cmbKategori.FormattingEnabled = true;
            this.cmbKategori.Items.AddRange(new object[] {
            "Lunas",
            "Belum Lunas"});
            this.cmbKategori.Location = new System.Drawing.Point(549, 225);
            this.cmbKategori.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbKategori.Name = "cmbKategori";
            this.cmbKategori.Size = new System.Drawing.Size(265, 28);
            this.cmbKategori.TabIndex = 11;
            // 
            // txtIDPemesanan
            // 
            this.txtIDPemesanan.Location = new System.Drawing.Point(1328, 246);
            this.txtIDPemesanan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtIDPemesanan.Name = "txtIDPemesanan";
            this.txtIDPemesanan.Size = new System.Drawing.Size(100, 26);
            this.txtIDPemesanan.TabIndex = 8;
            this.txtIDPemesanan.Visible = false;
            // 
            // labelNamaPelanggan
            // 
            this.labelNamaPelanggan.AutoSize = true;
            this.labelNamaPelanggan.Location = new System.Drawing.Point(382, 118);
            this.labelNamaPelanggan.Name = "labelNamaPelanggan";
            this.labelNamaPelanggan.Size = new System.Drawing.Size(131, 20);
            this.labelNamaPelanggan.TabIndex = 13;
            this.labelNamaPelanggan.Text = "Nama Pelanggan";
            // 
            // labelJenisPesanan
            // 
            this.labelJenisPesanan.AutoSize = true;
            this.labelJenisPesanan.Location = new System.Drawing.Point(382, 170);
            this.labelJenisPesanan.Name = "labelJenisPesanan";
            this.labelJenisPesanan.Size = new System.Drawing.Size(113, 20);
            this.labelJenisPesanan.TabIndex = 14;
            this.labelJenisPesanan.Text = "Jenis Pesanan";
            this.labelJenisPesanan.Click += new System.EventHandler(this.labelJenisPesanan_Click);
            // 
            // labelStatusPembayaran
            // 
            this.labelStatusPembayaran.AutoSize = true;
            this.labelStatusPembayaran.Location = new System.Drawing.Point(382, 233);
            this.labelStatusPembayaran.Name = "labelStatusPembayaran";
            this.labelStatusPembayaran.Size = new System.Drawing.Size(149, 20);
            this.labelStatusPembayaran.TabIndex = 15;
            this.labelStatusPembayaran.Text = "Status Pembayaran";
            // 
            // labelTanggalPemesanan
            // 
            this.labelTanggalPemesanan.AutoSize = true;
            this.labelTanggalPemesanan.Location = new System.Drawing.Point(382, 291);
            this.labelTanggalPemesanan.Name = "labelTanggalPemesanan";
            this.labelTanggalPemesanan.Size = new System.Drawing.Size(155, 20);
            this.labelTanggalPemesanan.TabIndex = 16;
            this.labelTanggalPemesanan.Text = "Tanggal Pemesanan";
            // 
            // btnAnalisis
            // 
            this.btnAnalisis.Location = new System.Drawing.Point(1047, 350);
            this.btnAnalisis.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAnalisis.Name = "btnAnalisis";
            this.btnAnalisis.Size = new System.Drawing.Size(94, 49);
            this.btnAnalisis.TabIndex = 17;
            this.btnAnalisis.Text = "Analisis";
            this.btnAnalisis.UseVisualStyleBackColor = true;
            this.btnAnalisis.Click += new System.EventHandler(this.btnAnalisis_Click);
            // 
            // btnLaporan
            // 
            this.btnLaporan.Location = new System.Drawing.Point(1047, 31);
            this.btnLaporan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLaporan.Name = "btnLaporan";
            this.btnLaporan.Size = new System.Drawing.Size(94, 49);
            this.btnLaporan.TabIndex = 18;
            this.btnLaporan.Text = "Laporan";
            this.btnLaporan.UseVisualStyleBackColor = true;
            this.btnLaporan.Click += new System.EventHandler(this.btnLaporan_Click);
            // 
            // FormPemesananLayanan
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1340, 721);
            this.Controls.Add(this.btnLaporan);
            this.Controls.Add(this.btnAnalisis);
            this.Controls.Add(this.labelTanggalPemesanan);
            this.Controls.Add(this.labelStatusPembayaran);
            this.Controls.Add(this.labelJenisPesanan);
            this.Controls.Add(this.labelNamaPelanggan);
            this.Controls.Add(this.cmbKategori);
            this.Controls.Add(this.btnLayanan);
            this.Controls.Add(this.btnPelanggan);
            this.Controls.Add(this.txtIDPemesanan);
            this.Controls.Add(this.dgvPemesanan);
            this.Controls.Add(this.btnBatal);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.dtpTanggal);
            this.Controls.Add(this.cmbLayanan);
            this.Controls.Add(this.cmbPelanggan);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormPemesananLayanan";
            this.Text = "IDPemesanan";
            this.Load += new System.EventHandler(this.FormPemesananLayanan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPemesanan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPelanggan;
        private System.Windows.Forms.ComboBox cmbLayanan;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.Button btnSimpan;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnBatal;
        private System.Windows.Forms.DataGridView dgvPemesanan;
        private System.Windows.Forms.Button btnPelanggan;
        private System.Windows.Forms.Button btnLayanan;
        private System.Windows.Forms.ComboBox cmbKategori;
        private System.Windows.Forms.TextBox txtIDPemesanan;
        private System.Windows.Forms.Label labelNamaPelanggan;
        private System.Windows.Forms.Label labelJenisPesanan;
        private System.Windows.Forms.Label labelStatusPembayaran;
        private System.Windows.Forms.Label labelTanggalPemesanan;
        private System.Windows.Forms.Button btnAnalisis;
        private System.Windows.Forms.Button btnLaporan;
    }
}