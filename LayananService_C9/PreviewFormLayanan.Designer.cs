namespace LayananService_C9
{
    partial class PreviewFormLayanan
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
            this.dgvPreviewLayanan = new System.Windows.Forms.DataGridView();
            this.btnOK2 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreviewLayanan)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPreviewLayanan
            // 
            this.dgvPreviewLayanan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreviewLayanan.Location = new System.Drawing.Point(46, 38);
            this.dgvPreviewLayanan.Name = "dgvPreviewLayanan";
            this.dgvPreviewLayanan.RowHeadersWidth = 51;
            this.dgvPreviewLayanan.RowTemplate.Height = 24;
            this.dgvPreviewLayanan.Size = new System.Drawing.Size(711, 319);
            this.dgvPreviewLayanan.TabIndex = 0;
            // 
            // btnOK2
            // 
            this.btnOK2.Location = new System.Drawing.Point(592, 398);
            this.btnOK2.Name = "btnOK2";
            this.btnOK2.Size = new System.Drawing.Size(76, 40);
            this.btnOK2.TabIndex = 1;
            this.btnOK2.Text = "OK";
            this.btnOK2.UseVisualStyleBackColor = true;
            this.btnOK2.Click += new System.EventHandler(this.btnOK2_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(682, 398);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 40);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PreviewFormLayanan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK2);
            this.Controls.Add(this.dgvPreviewLayanan);
            this.Name = "PreviewFormLayanan";
            this.Text = "PreviewFormLayanan";
            this.Load += new System.EventHandler(this.PreviewFormLayanan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreviewLayanan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPreviewLayanan;
        private System.Windows.Forms.Button btnOK2;
        private System.Windows.Forms.Button btnCancel;
    }
}