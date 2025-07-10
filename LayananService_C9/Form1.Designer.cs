namespace LayananService_C9
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblNama = new System.Windows.Forms.Label();
            this.txtNama = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblNoTelp = new System.Windows.Forms.Label();
            this.txtNoTelp = new System.Windows.Forms.TextBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnSimpan = new System.Windows.Forms.Button();
            this.btnBatal = new System.Windows.Forms.Button();
            this.dgvPelanggan = new System.Windows.Forms.DataGridView();
            this.btnLayanan = new System.Windows.Forms.Button();
            this.btnCetakLaporan = new System.Windows.Forms.Button();
            this.btnAnalisis = new System.Windows.Forms.Button();
            this.btnImportData = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPelanggan)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNama
            // 
            this.lblNama.AutoSize = true;
            this.lblNama.Location = new System.Drawing.Point(84, 104);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(131, 20);
            this.lblNama.TabIndex = 0;
            this.lblNama.Text = "Nama Pelanggan";
            // 
            // txtNama
            // 
            this.txtNama.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtNama.Location = new System.Drawing.Point(291, 100);
            this.txtNama.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(211, 26);
            this.txtNama.TabIndex = 1;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(84, 168);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(48, 20);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(291, 164);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(211, 26);
            this.txtEmail.TabIndex = 3;
            // 
            // lblNoTelp
            // 
            this.lblNoTelp.AutoSize = true;
            this.lblNoTelp.Location = new System.Drawing.Point(84, 226);
            this.lblNoTelp.Name = "lblNoTelp";
            this.lblNoTelp.Size = new System.Drawing.Size(94, 20);
            this.lblNoTelp.TabIndex = 4;
            this.lblNoTelp.Text = "No. Telepon";
            // 
            // txtNoTelp
            // 
            this.txtNoTelp.Location = new System.Drawing.Point(291, 222);
            this.txtNoTelp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNoTelp.Name = "txtNoTelp";
            this.txtNoTelp.Size = new System.Drawing.Size(211, 26);
            this.txtNoTelp.TabIndex = 5;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(208, 312);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(99, 60);
            this.btnEdit.TabIndex = 7;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(344, 312);
            this.btnHapus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(99, 60);
            this.btnHapus.TabIndex = 8;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnSimpan
            // 
            this.btnSimpan.Location = new System.Drawing.Point(78, 312);
            this.btnSimpan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(99, 60);
            this.btnSimpan.TabIndex = 9;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.UseVisualStyleBackColor = true;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // btnBatal
            // 
            this.btnBatal.Location = new System.Drawing.Point(494, 312);
            this.btnBatal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBatal.Name = "btnBatal";
            this.btnBatal.Size = new System.Drawing.Size(99, 58);
            this.btnBatal.TabIndex = 10;
            this.btnBatal.Text = "Batal";
            this.btnBatal.UseVisualStyleBackColor = true;
            this.btnBatal.Click += new System.EventHandler(this.btnBatal_Click);
            // 
            // dgvPelanggan
            // 
            this.dgvPelanggan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPelanggan.Location = new System.Drawing.Point(638, 82);
            this.dgvPelanggan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvPelanggan.Name = "dgvPelanggan";
            this.dgvPelanggan.RowHeadersWidth = 51;
            this.dgvPelanggan.RowTemplate.Height = 24;
            this.dgvPelanggan.Size = new System.Drawing.Size(677, 509);
            this.dgvPelanggan.TabIndex = 11;
            this.dgvPelanggan.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPelanggan_CellClick);
            // 
            // btnLayanan
            // 
            this.btnLayanan.Location = new System.Drawing.Point(78, 438);
            this.btnLayanan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLayanan.Name = "btnLayanan";
            this.btnLayanan.Size = new System.Drawing.Size(515, 42);
            this.btnLayanan.TabIndex = 12;
            this.btnLayanan.Text = "Layanan";
            this.btnLayanan.UseVisualStyleBackColor = true;
            this.btnLayanan.Click += new System.EventHandler(this.btnLayanan_Click);
            // 
            // btnCetakLaporan
            // 
            this.btnCetakLaporan.Location = new System.Drawing.Point(78, 544);
            this.btnCetakLaporan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCetakLaporan.Name = "btnCetakLaporan";
            this.btnCetakLaporan.Size = new System.Drawing.Size(515, 48);
            this.btnCetakLaporan.TabIndex = 13;
            this.btnCetakLaporan.Text = "Cetak Laporan";
            this.btnCetakLaporan.UseVisualStyleBackColor = true;
            this.btnCetakLaporan.Click += new System.EventHandler(this.btnCetakLaporan_Click);
            // 
            // btnAnalisis
            // 
            this.btnAnalisis.Location = new System.Drawing.Point(78, 599);
            this.btnAnalisis.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAnalisis.Name = "btnAnalisis";
            this.btnAnalisis.Size = new System.Drawing.Size(515, 48);
            this.btnAnalisis.TabIndex = 14;
            this.btnAnalisis.Text = "Analisis";
            this.btnAnalisis.UseVisualStyleBackColor = true;
            this.btnAnalisis.Click += new System.EventHandler(this.btnAnalisis_Click);
            // 
            // btnImportData
            // 
            this.btnImportData.Location = new System.Drawing.Point(78, 488);
            this.btnImportData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(515, 49);
            this.btnImportData.TabIndex = 15;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(1046, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 33);
            this.button1.TabIndex = 17;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(729, 32);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(286, 26);
            this.txtSearch.TabIndex = 18;
            this.txtSearch.Text = "Search";
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1348, 696);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnImportData);
            this.Controls.Add(this.btnAnalisis);
            this.Controls.Add(this.btnCetakLaporan);
            this.Controls.Add(this.btnLayanan);
            this.Controls.Add(this.dgvPelanggan);
            this.Controls.Add(this.btnBatal);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.txtNoTelp);
            this.Controls.Add(this.lblNoTelp);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.lblNama);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Layanan Service C9";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPelanggan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNama;
        private System.Windows.Forms.TextBox txtNama;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblNoTelp;
        private System.Windows.Forms.TextBox txtNoTelp;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnSimpan;
        private System.Windows.Forms.Button btnBatal;
        private System.Windows.Forms.DataGridView dgvPelanggan;
        private System.Windows.Forms.Button btnLayanan;
        private System.Windows.Forms.Button btnCetakLaporan;
        private System.Windows.Forms.Button btnAnalisis;
        private System.Windows.Forms.Button btnImportData;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSearch;
    }
}

