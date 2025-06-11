namespace LayananService_C9
{
    partial class FormLaporanPemesanan
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.btnCetakLaporan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Location = new System.Drawing.Point(22, 26);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(766, 364);
            this.reportViewer1.TabIndex = 0;
            // 
            // btnCetakLaporan
            // 
            this.btnCetakLaporan.Location = new System.Drawing.Point(454, 397);
            this.btnCetakLaporan.Name = "btnCetakLaporan";
            this.btnCetakLaporan.Size = new System.Drawing.Size(166, 41);
            this.btnCetakLaporan.TabIndex = 1;
            this.btnCetakLaporan.Text = "Cetak Laporan";
            this.btnCetakLaporan.UseVisualStyleBackColor = true;
            this.btnCetakLaporan.Click += new System.EventHandler(this.btnCetakLaporan_Click);
            // 
            // FormLaporanPemesanan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCetakLaporan);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FormLaporanPemesanan";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.FormLaporanPemesanan_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Button btnCetakLaporan;
    }
}