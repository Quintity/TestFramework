namespace Quintity.TestFramework.TestEngineer
{
    partial class TestsuiteRelationshipDialog
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
            this.m_sublingBtn = new System.Windows.Forms.Button();
            this.m_childBtn = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.m_promptlabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_sublingBtn
            // 
            this.m_sublingBtn.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.m_sublingBtn.Location = new System.Drawing.Point(56, 80);
            this.m_sublingBtn.Name = "m_sublingBtn";
            this.m_sublingBtn.Size = new System.Drawing.Size(75, 23);
            this.m_sublingBtn.TabIndex = 2;
            this.m_sublingBtn.Text = "&Sibling";
            this.m_sublingBtn.UseVisualStyleBackColor = true;
            // 
            // m_childBtn
            // 
            this.m_childBtn.DialogResult = System.Windows.Forms.DialogResult.No;
            this.m_childBtn.Location = new System.Drawing.Point(158, 80);
            this.m_childBtn.Name = "m_childBtn";
            this.m_childBtn.Size = new System.Drawing.Size(75, 23);
            this.m_childBtn.TabIndex = 1;
            this.m_childBtn.Text = "&Child";
            this.m_childBtn.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(262, 80);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 0;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // m_promptlabel
            // 
            this.m_promptlabel.AutoSize = true;
            this.m_promptlabel.Location = new System.Drawing.Point(22, 37);
            this.m_promptlabel.Name = "m_promptlabel";
            this.m_promptlabel.Size = new System.Drawing.Size(342, 13);
            this.m_promptlabel.TabIndex = 3;
            this.m_promptlabel.Text = "Do you want the new suite to be a child or sibling of the selected suite?\r\n";
            // 
            // TestsuiteRelationshipDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 116);
            this.Controls.Add(this.m_promptlabel);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.m_childBtn);
            this.Controls.Add(this.m_sublingBtn);
            this.MaximumSize = new System.Drawing.Size(400, 155);
            this.MinimumSize = new System.Drawing.Size(400, 155);
            this.Name = "TestsuiteRelationshipDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quintity TestFramework";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_sublingBtn;
        private System.Windows.Forms.Button m_childBtn;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label m_promptlabel;
    }
}