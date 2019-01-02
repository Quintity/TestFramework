namespace Quintity.TestFramework.Core
{
    partial class TestListenersEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestListenersEditorDialog));
            this.m_testListenerPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.m_testListenersTreeView = new System.Windows.Forms.TreeView();
            this.m_testListenerToolStrip = new System.Windows.Forms.ToolStrip();
            this.m_addToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_removeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.m_saveBtn = new System.Windows.Forms.Button();
            this.m_cancelButton = new System.Windows.Forms.Button();
            this.m_newBtn = new System.Windows.Forms.Button();
            this.m_openBtn = new System.Windows.Forms.Button();
            this.m_saveAsBtn = new System.Windows.Forms.Button();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_testListenerToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_testListenerPropertyGrid
            // 
            this.m_testListenerPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_testListenerPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.m_testListenerPropertyGrid.Name = "m_testListenerPropertyGrid";
            this.m_testListenerPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.m_testListenerPropertyGrid.Size = new System.Drawing.Size(566, 361);
            this.m_testListenerPropertyGrid.TabIndex = 4;
            this.m_testListenerPropertyGrid.ToolbarVisible = false;
            this.m_testListenerPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.m_testListenerPropertyGrid_PropertyValueChanged);
            this.m_testListenerPropertyGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.m_testListenerPropertyGrid_SelectedGridItemChanged);
            this.m_testListenerPropertyGrid.SelectedObjectsChanged += new System.EventHandler(this.m_testListenerPropertyGrid_SelectedObjectsChanged);
            this.m_testListenerPropertyGrid.Leave += new System.EventHandler(this.m_testListenerPropertyGrid_Leave);
            // 
            // m_testListenersTreeView
            // 
            this.m_testListenersTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_testListenersTreeView.HideSelection = false;
            this.m_testListenersTreeView.Location = new System.Drawing.Point(0, 0);
            this.m_testListenersTreeView.Name = "m_testListenersTreeView";
            this.m_testListenersTreeView.ShowNodeToolTips = true;
            this.m_testListenersTreeView.Size = new System.Drawing.Size(285, 361);
            this.m_testListenersTreeView.TabIndex = 3;
            this.m_testListenersTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_testListenersTreeView_AfterSelect);
            // 
            // m_testListenerToolStrip
            // 
            this.m_testListenerToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_addToolStripButton,
            this.m_removeToolStripButton,
            this.toolStripSeparator1});
            this.m_testListenerToolStrip.Location = new System.Drawing.Point(0, 0);
            this.m_testListenerToolStrip.Name = "m_testListenerToolStrip";
            this.m_testListenerToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.m_testListenerToolStrip.Size = new System.Drawing.Size(855, 28);
            this.m_testListenerToolStrip.TabIndex = 3;
            this.m_testListenerToolStrip.Text = "m_testListenerToolStrip";
            // 
            // m_addToolStripButton
            // 
            this.m_addToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_addToolStripButton.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold);
            this.m_addToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("m_addToolStripButton.Image")));
            this.m_addToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_addToolStripButton.Name = "m_addToolStripButton";
            this.m_addToolStripButton.Size = new System.Drawing.Size(26, 25);
            this.m_addToolStripButton.Text = "+";
            this.m_addToolStripButton.ToolTipText = "Adds a new Test Listener.\r\n";
            this.m_addToolStripButton.Click += new System.EventHandler(this.m_addToolStripButton_Click_1);
            // 
            // m_removeToolStripButton
            // 
            this.m_removeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_removeToolStripButton.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold);
            this.m_removeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("m_removeToolStripButton.Image")));
            this.m_removeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_removeToolStripButton.Name = "m_removeToolStripButton";
            this.m_removeToolStripButton.Size = new System.Drawing.Size(23, 25);
            this.m_removeToolStripButton.Text = "-";
            this.m_removeToolStripButton.ToolTipText = "Removes the selected Test Listener.";
            this.m_removeToolStripButton.Click += new System.EventHandler(this.m_removeToolStripButton_Click_1);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.m_testListenersTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.m_testListenerPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(855, 361);
            this.splitContainer1.SplitterDistance = 285;
            this.splitContainer1.TabIndex = 4;
            this.splitContainer1.TabStop = false;
            // 
            // m_saveBtn
            // 
            this.m_saveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_saveBtn.Location = new System.Drawing.Point(687, 408);
            this.m_saveBtn.Name = "m_saveBtn";
            this.m_saveBtn.Size = new System.Drawing.Size(75, 23);
            this.m_saveBtn.TabIndex = 0;
            this.m_saveBtn.Text = "Save";
            this.m_saveBtn.UseVisualStyleBackColor = true;
            this.m_saveBtn.Click += new System.EventHandler(this.m_saveBtn_Click);
            // 
            // m_cancelButton
            // 
            this.m_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cancelButton.Location = new System.Drawing.Point(768, 408);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.m_cancelButton.TabIndex = 1;
            this.m_cancelButton.Text = "Cancel";
            this.m_cancelButton.UseVisualStyleBackColor = true;
            this.m_cancelButton.Click += new System.EventHandler(this.m_cancelButton_Click);
            // 
            // m_newBtn
            // 
            this.m_newBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_newBtn.Location = new System.Drawing.Point(12, 408);
            this.m_newBtn.Name = "m_newBtn";
            this.m_newBtn.Size = new System.Drawing.Size(75, 23);
            this.m_newBtn.TabIndex = 5;
            this.m_newBtn.Text = "New";
            this.m_newBtn.UseVisualStyleBackColor = true;
            this.m_newBtn.Click += new System.EventHandler(this.m_newBtn_Click);
            // 
            // m_openBtn
            // 
            this.m_openBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_openBtn.Location = new System.Drawing.Point(93, 408);
            this.m_openBtn.Name = "m_openBtn";
            this.m_openBtn.Size = new System.Drawing.Size(75, 23);
            this.m_openBtn.TabIndex = 6;
            this.m_openBtn.Text = "Open...";
            this.m_openBtn.UseVisualStyleBackColor = true;
            this.m_openBtn.Click += new System.EventHandler(this.m_openBtn_Click);
            // 
            // m_saveAsBtn
            // 
            this.m_saveAsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_saveAsBtn.Location = new System.Drawing.Point(174, 408);
            this.m_saveAsBtn.Name = "m_saveAsBtn";
            this.m_saveAsBtn.Size = new System.Drawing.Size(75, 23);
            this.m_saveAsBtn.TabIndex = 7;
            this.m_saveAsBtn.Text = "Save As...";
            this.m_saveAsBtn.UseVisualStyleBackColor = true;
            this.m_saveAsBtn.Click += new System.EventHandler(this.m_saveAsBtn_Click);
            // 
            // m_openFileDialog
            // 
            this.m_openFileDialog.Filter = "Test listener files|*.config|All files|*.*";
            // 
            // m_saveFileDialog
            // 
            this.m_saveFileDialog.DefaultExt = "config";
            this.m_saveFileDialog.Filter = "Test listener files|*.config|All files|*.*";
            // 
            // TestListenersEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 443);
            this.Controls.Add(this.m_saveAsBtn);
            this.Controls.Add(this.m_openBtn);
            this.Controls.Add(this.m_newBtn);
            this.Controls.Add(this.m_cancelButton);
            this.Controls.Add(this.m_saveBtn);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.m_testListenerToolStrip);
            this.Name = "TestListenersEditorDialog";
            this.Text = "Test Listeners Editor";
            this.Load += new System.EventHandler(this.TestListenersEditorDialog_Load);
            this.Shown += new System.EventHandler(this.TestListenersEditorDialog_Shown);
            this.m_testListenerToolStrip.ResumeLayout(false);
            this.m_testListenerToolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid m_testListenerPropertyGrid;
        private System.Windows.Forms.TreeView m_testListenersTreeView;
        private System.Windows.Forms.ToolStrip m_testListenerToolStrip;
        private System.Windows.Forms.ToolStripButton m_addToolStripButton;
        private System.Windows.Forms.ToolStripButton m_removeToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button m_saveBtn;
        private System.Windows.Forms.Button m_cancelButton;
        private System.Windows.Forms.Button m_newBtn;
        private System.Windows.Forms.Button m_openBtn;
        private System.Windows.Forms.Button m_saveAsBtn;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.SaveFileDialog m_saveFileDialog;
    }
}