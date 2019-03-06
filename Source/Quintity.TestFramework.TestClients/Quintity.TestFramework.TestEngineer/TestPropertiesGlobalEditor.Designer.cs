namespace Quintity.TestFramework.TestEngineer
{
    partial class TestPropertiesGlobalEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestPropertiesGlobalEditor));
            this.m_toolStrip = new System.Windows.Forms.ToolStrip();
            this.m_addToolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.m_removeStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_moveUpStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_moveDownStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_testPropertiesDataGridView = new System.Windows.Forms.DataGridView();
            this.m_overriddenColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_activeColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.m_nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_typeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_cancelButton = new System.Windows.Forms.Button();
            this.m_saveButton = new System.Windows.Forms.Button();
            this.m_saveAsButton = new System.Windows.Forms.Button();
            this.m_overrideLabel = new System.Windows.Forms.Label();
            this.m_newButton = new System.Windows.Forms.Button();
            this.m_openButton = new System.Windows.Forms.Button();
            this.m_toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_testPropertiesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // m_toolStrip
            // 
            this.m_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_addToolStripButton1,
            this.m_removeStripButton,
            this.toolStripSeparator2,
            this.m_moveUpStripButton,
            this.m_moveDownStripButton});
            this.m_toolStrip.Location = new System.Drawing.Point(0, 0);
            this.m_toolStrip.Name = "m_toolStrip";
            this.m_toolStrip.Size = new System.Drawing.Size(1064, 28);
            this.m_toolStrip.TabIndex = 1;
            this.m_toolStrip.Text = "toolStrip1";
            // 
            // m_addToolStripButton1
            // 
            this.m_addToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_addToolStripButton1.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_addToolStripButton1.Name = "m_addToolStripButton1";
            this.m_addToolStripButton1.Size = new System.Drawing.Size(26, 25);
            this.m_addToolStripButton1.Text = "+";
            this.m_addToolStripButton1.ToolTipText = "Add a test property";
            this.m_addToolStripButton1.Click += new System.EventHandler(this.m_addToolStripButton1_Click);
            // 
            // m_removeStripButton
            // 
            this.m_removeStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_removeStripButton.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_removeStripButton.Name = "m_removeStripButton";
            this.m_removeStripButton.Size = new System.Drawing.Size(23, 25);
            this.m_removeStripButton.Text = "-";
            this.m_removeStripButton.ToolTipText = "Remove selected test property.";
            this.m_removeStripButton.Click += new System.EventHandler(this.m_removeStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // m_moveUpStripButton
            // 
            this.m_moveUpStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_moveUpStripButton.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_moveUpStripButton.Name = "m_moveUpStripButton";
            this.m_moveUpStripButton.Size = new System.Drawing.Size(23, 25);
            this.m_moveUpStripButton.Text = "↑";
            this.m_moveUpStripButton.ToolTipText = "Move selected test property up.";
            this.m_moveUpStripButton.Click += new System.EventHandler(this.m_moveUpStripButton_Click);
            // 
            // m_moveDownStripButton
            // 
            this.m_moveDownStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_moveDownStripButton.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_moveDownStripButton.Name = "m_moveDownStripButton";
            this.m_moveDownStripButton.Size = new System.Drawing.Size(23, 25);
            this.m_moveDownStripButton.Text = "↓";
            this.m_moveDownStripButton.ToolTipText = "Move selected test property down.";
            this.m_moveDownStripButton.Click += new System.EventHandler(this.m_moveDownStripButton_Click);
            // 
            // m_testPropertiesDataGridView
            // 
            this.m_testPropertiesDataGridView.AllowUserToAddRows = false;
            this.m_testPropertiesDataGridView.AllowUserToDeleteRows = false;
            this.m_testPropertiesDataGridView.AllowUserToResizeRows = false;
            this.m_testPropertiesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_testPropertiesDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m_testPropertiesDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.m_testPropertiesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_testPropertiesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_overriddenColumn,
            this.m_activeColumn,
            this.m_nameColumn,
            this.m_typeColumn,
            this.m_valueColumn,
            this.m_descriptionColumn});
            this.m_testPropertiesDataGridView.Location = new System.Drawing.Point(0, 31);
            this.m_testPropertiesDataGridView.MultiSelect = false;
            this.m_testPropertiesDataGridView.Name = "m_testPropertiesDataGridView";
            this.m_testPropertiesDataGridView.Size = new System.Drawing.Size(1064, 392);
            this.m_testPropertiesDataGridView.TabIndex = 0;
            this.m_testPropertiesDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_testPropertiesDataGridView_CellValueChanged);
            this.m_testPropertiesDataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_testPropertiesDataGridView_RowEnter);
            this.m_testPropertiesDataGridView.SelectionChanged += new System.EventHandler(this.m_testPropertiesDataGridView_SelectionChanged);
            // 
            // m_overriddenColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_overriddenColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.m_overriddenColumn.HeaderText = "";
            this.m_overriddenColumn.Name = "m_overriddenColumn";
            this.m_overriddenColumn.ReadOnly = true;
            this.m_overriddenColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.m_overriddenColumn.Width = 16;
            // 
            // m_activeColumn
            // 
            this.m_activeColumn.HeaderText = "Active";
            this.m_activeColumn.Name = "m_activeColumn";
            this.m_activeColumn.Width = 50;
            // 
            // m_nameColumn
            // 
            this.m_nameColumn.HeaderText = "Name";
            this.m_nameColumn.Name = "m_nameColumn";
            this.m_nameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.m_nameColumn.Width = 150;
            // 
            // m_typeColumn
            // 
            this.m_typeColumn.HeaderText = "Type";
            this.m_typeColumn.Name = "m_typeColumn";
            this.m_typeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.m_typeColumn.Width = 75;
            // 
            // m_valueColumn
            // 
            this.m_valueColumn.HeaderText = "Value";
            this.m_valueColumn.Name = "m_valueColumn";
            this.m_valueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.m_valueColumn.Width = 200;
            // 
            // m_descriptionColumn
            // 
            this.m_descriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_descriptionColumn.HeaderText = "Description";
            this.m_descriptionColumn.Name = "m_descriptionColumn";
            this.m_descriptionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_cancelButton
            // 
            this.m_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cancelButton.Location = new System.Drawing.Point(977, 437);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.m_cancelButton.TabIndex = 1;
            this.m_cancelButton.Text = "Cancel";
            this.m_cancelButton.UseVisualStyleBackColor = true;
            this.m_cancelButton.Click += new System.EventHandler(this.m_cancelButton_Click);
            // 
            // m_saveButton
            // 
            this.m_saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_saveButton.Enabled = false;
            this.m_saveButton.Location = new System.Drawing.Point(885, 437);
            this.m_saveButton.Name = "m_saveButton";
            this.m_saveButton.Size = new System.Drawing.Size(75, 23);
            this.m_saveButton.TabIndex = 2;
            this.m_saveButton.Text = "Save";
            this.m_saveButton.UseVisualStyleBackColor = true;
            this.m_saveButton.Click += new System.EventHandler(this.m_saveButton_Click);
            // 
            // m_saveAsButton
            // 
            this.m_saveAsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_saveAsButton.Location = new System.Drawing.Point(793, 437);
            this.m_saveAsButton.Name = "m_saveAsButton";
            this.m_saveAsButton.Size = new System.Drawing.Size(75, 23);
            this.m_saveAsButton.TabIndex = 3;
            this.m_saveAsButton.Text = "Save As...";
            this.m_saveAsButton.UseVisualStyleBackColor = true;
            this.m_saveAsButton.Click += new System.EventHandler(this.m_saveAsButton_Click);
            // 
            // m_overrideLabel
            // 
            this.m_overrideLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_overrideLabel.AutoSize = true;
            this.m_overrideLabel.Location = new System.Drawing.Point(3, 427);
            this.m_overrideLabel.Name = "m_overrideLabel";
            this.m_overrideLabel.Size = new System.Drawing.Size(157, 13);
            this.m_overrideLabel.TabIndex = 4;
            this.m_overrideLabel.Text = "*Test property override indicator";
            // 
            // m_newButton
            // 
            this.m_newButton.Location = new System.Drawing.Point(606, 437);
            this.m_newButton.Name = "m_newButton";
            this.m_newButton.Size = new System.Drawing.Size(75, 23);
            this.m_newButton.TabIndex = 5;
            this.m_newButton.Text = "New";
            this.m_newButton.UseVisualStyleBackColor = true;
            this.m_newButton.Click += new System.EventHandler(this.m_newButton_Click);
            // 
            // m_openButton
            // 
            this.m_openButton.Location = new System.Drawing.Point(687, 437);
            this.m_openButton.Name = "m_openButton";
            this.m_openButton.Size = new System.Drawing.Size(75, 23);
            this.m_openButton.TabIndex = 6;
            this.m_openButton.Text = "Open";
            this.m_openButton.UseVisualStyleBackColor = true;
            this.m_openButton.Click += new System.EventHandler(this.m_openButton_Click);
            // 
            // TestPropertiesGlobalEditor
            // 
            this.AcceptButton = this.m_saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_cancelButton;
            this.ClientSize = new System.Drawing.Size(1064, 472);
            this.Controls.Add(this.m_openButton);
            this.Controls.Add(this.m_newButton);
            this.Controls.Add(this.m_overrideLabel);
            this.Controls.Add(this.m_saveAsButton);
            this.Controls.Add(this.m_toolStrip);
            this.Controls.Add(this.m_saveButton);
            this.Controls.Add(this.m_testPropertiesDataGridView);
            this.Controls.Add(this.m_cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestPropertiesGlobalEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Test Properties Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestPropertiesGlobalEditor_FormClosing);
            this.Load += new System.EventHandler(this.TestPropertiesGlobalEditor_Load);
            this.m_toolStrip.ResumeLayout(false);
            this.m_toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_testPropertiesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView m_testPropertiesDataGridView;
        private System.Windows.Forms.Button m_cancelButton;
        private System.Windows.Forms.Button m_saveButton;
        private System.Windows.Forms.ToolStrip m_toolStrip;
        private System.Windows.Forms.ToolStripButton m_addToolStripButton1;
        private System.Windows.Forms.ToolStripButton m_removeStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton m_moveUpStripButton;
        private System.Windows.Forms.ToolStripButton m_moveDownStripButton;
        private System.Windows.Forms.Button m_saveAsButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_overriddenColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn m_activeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_typeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_valueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_descriptionColumn;
        private System.Windows.Forms.Label m_overrideLabel;
        private System.Windows.Forms.Button m_newButton;
        private System.Windows.Forms.Button m_openButton;
    }
}