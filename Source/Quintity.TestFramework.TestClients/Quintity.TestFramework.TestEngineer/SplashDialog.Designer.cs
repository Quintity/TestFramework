namespace Quintity.TestFramework.TestEngineer
{
    partial class SplashDialog
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
            this.m_timerSplash = new System.Windows.Forms.Timer(this.components);
            this.m_copyright = new System.Windows.Forms.Label();
            this.m_version = new System.Windows.Forms.Label();
            this.m_pbSplash = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_pbSplash)).BeginInit();
            this.SuspendLayout();
            // 
            // m_timerSplash
            // 
            this.m_timerSplash.Interval = 1000;
            this.m_timerSplash.Tick += new System.EventHandler(this.m_timerSplash_Tick);
            // 
            // m_copyright
            // 
            this.m_copyright.AutoSize = true;
            this.m_copyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_copyright.ForeColor = System.Drawing.Color.RoyalBlue;
            this.m_copyright.Location = new System.Drawing.Point(93, 321);
            this.m_copyright.Name = "m_copyright";
            this.m_copyright.Size = new System.Drawing.Size(86, 13);
            this.m_copyright.TabIndex = 1;
            this.m_copyright.Text = "Copyright Info";
            // 
            // m_version
            // 
            this.m_version.AutoSize = true;
            this.m_version.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_version.ForeColor = System.Drawing.Color.RoyalBlue;
            this.m_version.Location = new System.Drawing.Point(171, 338);
            this.m_version.Name = "m_version";
            this.m_version.Size = new System.Drawing.Size(71, 13);
            this.m_version.TabIndex = 2;
            this.m_version.Text = "VersionInfo";
            // 
            // m_pbSplash
            // 
            this.m_pbSplash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.m_pbSplash.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_pbSplash.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.TestEngineerLogo;
            this.m_pbSplash.Location = new System.Drawing.Point(0, 0);
            this.m_pbSplash.Name = "m_pbSplash";
            this.m_pbSplash.Size = new System.Drawing.Size(457, 364);
            this.m_pbSplash.TabIndex = 0;
            this.m_pbSplash.TabStop = false;
            this.m_pbSplash.Click += new System.EventHandler(this.m_pbSplash_Click);
            // 
            // SplashDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(458, 364);
            this.ControlBox = false;
            this.Controls.Add(this.m_version);
            this.Controls.Add(this.m_copyright);
            this.Controls.Add(this.m_pbSplash);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashDialog";
            this.Text = "SplashDlg";
            ((System.ComponentModel.ISupportInitialize)(this.m_pbSplash)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}