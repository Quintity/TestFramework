namespace Quintity.TestFramework.TestEngineer
{
    partial class TestPropertiesEditorDialog2
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Test Properties");
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._testPropertiesTreeView = new System.Windows.Forms.TreeView();
            this._testPropertyPropertyGrid = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(533, 340);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(452, 340);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._testPropertiesTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._testPropertyPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(596, 322);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.TabIndex = 2;
            // 
            // _testPropertiesTreeView
            // 
            this._testPropertiesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._testPropertiesTreeView.Location = new System.Drawing.Point(0, 0);
            this._testPropertiesTreeView.Name = "_testPropertiesTreeView";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Test Properties";
            this._testPropertiesTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this._testPropertiesTreeView.Size = new System.Drawing.Size(198, 322);
            this._testPropertiesTreeView.TabIndex = 0;
            this._testPropertiesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._testPropertiesTreeView_AfterSelect);
            // 
            // _testPropertyPropertyGrid
            // 
            this._testPropertyPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._testPropertyPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this._testPropertyPropertyGrid.Name = "_testPropertyPropertyGrid";
            this._testPropertyPropertyGrid.Size = new System.Drawing.Size(394, 322);
            this._testPropertyPropertyGrid.TabIndex = 0;
            // 
            // TestPropertiesEditorDialog2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 375);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "TestPropertiesEditorDialog2";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView _testPropertiesTreeView;
        private System.Windows.Forms.PropertyGrid _testPropertyPropertyGrid;
    }
}