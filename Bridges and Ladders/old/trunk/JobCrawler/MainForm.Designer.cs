namespace JobCrawler
{
    partial class MainForm
    {
        private System.Windows.Forms.WebBrowser crawlerBrowser;
        //private AxSHDocVw.AxWebBrowser crawlerBrowser;
        
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            //this.crawlerBrowser = new AxSHDocVw.AxWebBrowser();
            this.crawlerBrowser = new System.Windows.Forms.WebBrowser();
            //((System.ComponentModel.ISupportInitialize)(this.crawlerBrowser)).BeginInit();
            this.SuspendLayout();
            // 
            // crawlerBrowser
            // 
            this.crawlerBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.crawlerBrowser.Enabled = true;
            this.crawlerBrowser.Location = new System.Drawing.Point(0, 0);
         //   this.crawlerBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("crawlerBrowser.OcxState")));
            this.crawlerBrowser.Size = new System.Drawing.Size(576, 488);
            this.crawlerBrowser.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 488);
            this.Controls.Add(this.crawlerBrowser);
            this.Name = "MainForm";
            this.Text = "Bridges and Ladders - Job Crawler";
            this.Load += new System.EventHandler(this.FrmMain_Load);
           // ((System.ComponentModel.ISupportInitialize)(this.crawlerBrowser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}

