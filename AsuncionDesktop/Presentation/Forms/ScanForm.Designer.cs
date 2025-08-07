namespace AsuncionDesktop.Presentation.Forms
{
    partial class ScanForm
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
            this.components = new System.ComponentModel.Container();
            this.picActa = new System.Windows.Forms.PictureBox();
            this.lstImages = new System.Windows.Forms.ListView();
            this.lstActas = new System.Windows.Forms.ListView();
            this.Codigo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmdScan = new System.Windows.Forms.Button();
            this.cmdPrepare = new System.Windows.Forms.Button();
            this.cmdProcess = new System.Windows.Forms.Button();
            this.lstPages = new System.Windows.Forms.ListView();
            this.bgwPrincipal = new System.ComponentModel.BackgroundWorker();
            this.pbProgreso = new System.Windows.Forms.ProgressBar();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblTotalActasT = new System.Windows.Forms.Label();
            this.lblTotalActas = new System.Windows.Forms.Label();
            this.lblTotalImagenes = new System.Windows.Forms.Label();
            this.lblTotalImagenesT = new System.Windows.Forms.Label();
            this.lblTotalPaginas = new System.Windows.Forms.Label();
            this.lblTotalPaginasT = new System.Windows.Forms.Label();
            this.picSegmento = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picActa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSegmento)).BeginInit();
            this.SuspendLayout();
            // 
            // picActa
            // 
            this.picActa.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.picActa.Location = new System.Drawing.Point(819, 31);
            this.picActa.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.picActa.Name = "picActa";
            this.picActa.Size = new System.Drawing.Size(447, 575);
            this.picActa.TabIndex = 0;
            this.picActa.TabStop = false;
            // 
            // lstImages
            // 
            this.lstImages.HideSelection = false;
            this.lstImages.Location = new System.Drawing.Point(18, 53);
            this.lstImages.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lstImages.Name = "lstImages";
            this.lstImages.Size = new System.Drawing.Size(212, 360);
            this.lstImages.TabIndex = 1;
            this.lstImages.UseCompatibleStateImageBehavior = false;
            this.lstImages.View = System.Windows.Forms.View.Details;
            this.lstImages.SelectedIndexChanged += new System.EventHandler(this.lstImages_SelectedIndexChanged);
            // 
            // lstActas
            // 
            this.lstActas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Codigo,
            this.columnHeader1,
            this.columnHeader2});
            this.lstActas.HideSelection = false;
            this.lstActas.Location = new System.Drawing.Point(17, 448);
            this.lstActas.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lstActas.Name = "lstActas";
            this.lstActas.Size = new System.Drawing.Size(799, 158);
            this.lstActas.TabIndex = 2;
            this.lstActas.UseCompatibleStateImageBehavior = false;
            this.lstActas.View = System.Windows.Forms.View.Details;
            // 
            // Codigo
            // 
            this.Codigo.Text = "";
            this.Codigo.Width = 40;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Codigo";
            this.columnHeader1.Width = 72;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Seguridad";
            this.columnHeader2.Width = 95;
            // 
            // cmdScan
            // 
            this.cmdScan.Location = new System.Drawing.Point(508, 84);
            this.cmdScan.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmdScan.Name = "cmdScan";
            this.cmdScan.Size = new System.Drawing.Size(137, 47);
            this.cmdScan.TabIndex = 4;
            this.cmdScan.Text = "Digitalizar";
            this.cmdScan.UseVisualStyleBackColor = true;
            this.cmdScan.Click += new System.EventHandler(this.cmdScan_Click);
            // 
            // cmdPrepare
            // 
            this.cmdPrepare.Location = new System.Drawing.Point(508, 145);
            this.cmdPrepare.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmdPrepare.Name = "cmdPrepare";
            this.cmdPrepare.Size = new System.Drawing.Size(136, 45);
            this.cmdPrepare.TabIndex = 5;
            this.cmdPrepare.Text = "Preparar";
            this.cmdPrepare.UseVisualStyleBackColor = true;
            this.cmdPrepare.Click += new System.EventHandler(this.cmdPrepare_Click);
            // 
            // cmdProcess
            // 
            this.cmdProcess.Location = new System.Drawing.Point(508, 207);
            this.cmdProcess.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmdProcess.Name = "cmdProcess";
            this.cmdProcess.Size = new System.Drawing.Size(136, 47);
            this.cmdProcess.TabIndex = 6;
            this.cmdProcess.Text = "Procesar";
            this.cmdProcess.UseVisualStyleBackColor = true;
            this.cmdProcess.Click += new System.EventHandler(this.cmdProcess_Click);
            // 
            // lstPages
            // 
            this.lstPages.HideSelection = false;
            this.lstPages.Location = new System.Drawing.Point(247, 53);
            this.lstPages.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lstPages.Name = "lstPages";
            this.lstPages.Size = new System.Drawing.Size(235, 360);
            this.lstPages.TabIndex = 7;
            this.lstPages.UseCompatibleStateImageBehavior = false;
            this.lstPages.View = System.Windows.Forms.View.Details;
            this.lstPages.SelectedIndexChanged += new System.EventHandler(this.lstPages_SelectedIndexChanged);
            // 
            // bgwPrincipal
            // 
            this.bgwPrincipal.WorkerReportsProgress = true;
            this.bgwPrincipal.WorkerSupportsCancellation = true;
            this.bgwPrincipal.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwPrincipal_DoWork_1);
            // 
            // pbProgreso
            // 
            this.pbProgreso.Location = new System.Drawing.Point(819, 11);
            this.pbProgreso.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbProgreso.Name = "pbProgreso";
            this.pbProgreso.Size = new System.Drawing.Size(447, 16);
            this.pbProgreso.TabIndex = 8;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lblTotalActasT
            // 
            this.lblTotalActasT.AutoSize = true;
            this.lblTotalActasT.Location = new System.Drawing.Point(15, 428);
            this.lblTotalActasT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalActasT.Name = "lblTotalActasT";
            this.lblTotalActasT.Size = new System.Drawing.Size(63, 13);
            this.lblTotalActasT.TabIndex = 9;
            this.lblTotalActasT.Text = "Total actas:";
            // 
            // lblTotalActas
            // 
            this.lblTotalActas.AutoSize = true;
            this.lblTotalActas.Location = new System.Drawing.Point(85, 428);
            this.lblTotalActas.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalActas.Name = "lblTotalActas";
            this.lblTotalActas.Size = new System.Drawing.Size(25, 13);
            this.lblTotalActas.TabIndex = 10;
            this.lblTotalActas.Text = "000";
            // 
            // lblTotalImagenes
            // 
            this.lblTotalImagenes.AutoSize = true;
            this.lblTotalImagenes.Location = new System.Drawing.Point(100, 31);
            this.lblTotalImagenes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalImagenes.Name = "lblTotalImagenes";
            this.lblTotalImagenes.Size = new System.Drawing.Size(25, 13);
            this.lblTotalImagenes.TabIndex = 12;
            this.lblTotalImagenes.Text = "000";
            // 
            // lblTotalImagenesT
            // 
            this.lblTotalImagenesT.AutoSize = true;
            this.lblTotalImagenesT.Location = new System.Drawing.Point(15, 31);
            this.lblTotalImagenesT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalImagenesT.Name = "lblTotalImagenesT";
            this.lblTotalImagenesT.Size = new System.Drawing.Size(82, 13);
            this.lblTotalImagenesT.TabIndex = 11;
            this.lblTotalImagenesT.Text = "Total imágenes:";
            // 
            // lblTotalPaginas
            // 
            this.lblTotalPaginas.AutoSize = true;
            this.lblTotalPaginas.Location = new System.Drawing.Point(321, 31);
            this.lblTotalPaginas.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalPaginas.Name = "lblTotalPaginas";
            this.lblTotalPaginas.Size = new System.Drawing.Size(25, 13);
            this.lblTotalPaginas.TabIndex = 14;
            this.lblTotalPaginas.Text = "000";
            // 
            // lblTotalPaginasT
            // 
            this.lblTotalPaginasT.AutoSize = true;
            this.lblTotalPaginasT.Location = new System.Drawing.Point(245, 31);
            this.lblTotalPaginasT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalPaginasT.Name = "lblTotalPaginasT";
            this.lblTotalPaginasT.Size = new System.Drawing.Size(74, 13);
            this.lblTotalPaginasT.TabIndex = 13;
            this.lblTotalPaginasT.Text = "Total páginas:";
            // 
            // picSegmento
            // 
            this.picSegmento.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.picSegmento.Location = new System.Drawing.Point(485, 289);
            this.picSegmento.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.picSegmento.Name = "picSegmento";
            this.picSegmento.Size = new System.Drawing.Size(330, 122);
            this.picSegmento.TabIndex = 15;
            this.picSegmento.TabStop = false;
            // 
            // ScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 633);
            this.Controls.Add(this.picSegmento);
            this.Controls.Add(this.lblTotalPaginas);
            this.Controls.Add(this.lblTotalPaginasT);
            this.Controls.Add(this.lblTotalImagenes);
            this.Controls.Add(this.lblTotalImagenesT);
            this.Controls.Add(this.lblTotalActas);
            this.Controls.Add(this.lblTotalActasT);
            this.Controls.Add(this.pbProgreso);
            this.Controls.Add(this.lstPages);
            this.Controls.Add(this.cmdProcess);
            this.Controls.Add(this.cmdPrepare);
            this.Controls.Add(this.cmdScan);
            this.Controls.Add(this.lstActas);
            this.Controls.Add(this.lstImages);
            this.Controls.Add(this.picActa);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ScanForm";
            this.Text = "ScanForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ScanForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picActa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSegmento)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picActa;
        private System.Windows.Forms.ListView lstImages;
        private System.Windows.Forms.ListView lstActas;
        private System.Windows.Forms.Button cmdScan;
        private System.Windows.Forms.Button cmdPrepare;
        private System.Windows.Forms.Button cmdProcess;
        private System.Windows.Forms.ListView lstPages;
        private System.Windows.Forms.ColumnHeader Codigo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.ComponentModel.BackgroundWorker bgwPrincipal;
        private System.Windows.Forms.ProgressBar pbProgreso;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lblTotalActasT;
        private System.Windows.Forms.Label lblTotalActas;
        private System.Windows.Forms.Label lblTotalImagenes;
        private System.Windows.Forms.Label lblTotalImagenesT;
        private System.Windows.Forms.Label lblTotalPaginas;
        private System.Windows.Forms.Label lblTotalPaginasT;
        private System.Windows.Forms.PictureBox picSegmento;
    }
}