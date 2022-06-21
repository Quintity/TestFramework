namespace Quintity.TestFramework.Core
{
    partial class TestScriptObjectEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestScriptObjectEditorDialog));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Test Libraries");
            this.m_cancelBtn = new System.Windows.Forms.Button();
            this.m_okBtn = new System.Windows.Forms.Button();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_addTestAssemblyContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_miAddTestAssembly = new System.Windows.Forms.ToolStripMenuItem();
            this.m_automationDefinitionTab = new System.Windows.Forms.TabPage();
            this.m_splitContainer = new System.Windows.Forms.SplitContainer();
            this.m_toolStrip = new System.Windows.Forms.ToolStrip();
            this.m_tsAddAssemblyButton = new System.Windows.Forms.ToolStripButton();
            this.m_tsRefreshButton = new System.Windows.Forms.ToolStripButton();
            this.m_testAssemblytreeView = new System.Windows.Forms.TreeView();
            this.m_testParameterGrid = new System.Windows.Forms.DataGridView();
            this.m_parameterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_parameterDatatype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_parameterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_generalTab = new System.Windows.Forms.TabPage();
            this.m_activatecheckBox = new System.Windows.Forms.CheckBox();
            this.m_expectedText = new System.Windows.Forms.RichTextBox();
            this.m_descText = new System.Windows.Forms.RichTextBox();
            this.m_titleText = new System.Windows.Forms.TextBox();
            this.m_expectedLabel = new System.Windows.Forms.Label();
            this.m_automatecheckBox = new System.Windows.Forms.CheckBox();
            this.m_descLabel = new System.Windows.Forms.Label();
            this.m_titleLabel = new System.Windows.Forms.Label();
            this.m_editorTabs = new System.Windows.Forms.TabControl();
            this.m_treeViewPopupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_addTestAssemblyContextMenuStrip.SuspendLayout();
            this.m_automationDefinitionTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_splitContainer)).BeginInit();
            this.m_splitContainer.Panel1.SuspendLayout();
            this.m_splitContainer.Panel2.SuspendLayout();
            this.m_splitContainer.SuspendLayout();
            this.m_toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_testParameterGrid)).BeginInit();
            this.m_generalTab.SuspendLayout();
            this.m_editorTabs.SuspendLayout();
            this.m_treeViewPopupMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_cancelBtn
            // 
            this.m_cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cancelBtn.Location = new System.Drawing.Point(725, 447);
            this.m_cancelBtn.Name = "m_cancelBtn";
            this.m_cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.m_cancelBtn.TabIndex = 8;
            this.m_cancelBtn.Text = "Cancel";
            this.m_cancelBtn.UseVisualStyleBackColor = true;
            this.m_cancelBtn.Click += new System.EventHandler(this.m_cancelBtn_Click);
            // 
            // m_okBtn
            // 
            this.m_okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_okBtn.Location = new System.Drawing.Point(644, 447);
            this.m_okBtn.Name = "m_okBtn";
            this.m_okBtn.Size = new System.Drawing.Size(75, 23);
            this.m_okBtn.TabIndex = 7;
            this.m_okBtn.Text = "OK";
            this.m_okBtn.UseVisualStyleBackColor = true;
            this.m_okBtn.Click += new System.EventHandler(this.m_okBtn_Click);
            // 
            // m_addTestAssemblyContextMenuStrip
            // 
            this.m_addTestAssemblyContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_miAddTestAssembly});
            this.m_addTestAssemblyContextMenuStrip.Name = "m_addTestAssemblyContextMenuStrip";
            this.m_addTestAssemblyContextMenuStrip.Size = new System.Drawing.Size(174, 26);
            // 
            // m_miAddTestAssembly
            // 
            this.m_miAddTestAssembly.Name = "m_miAddTestAssembly";
            this.m_miAddTestAssembly.Size = new System.Drawing.Size(173, 22);
            this.m_miAddTestAssembly.Text = "Add Test Assembly";
            this.m_miAddTestAssembly.Click += new System.EventHandler(this.m_miAddTestAssembly_Click);
            this.m_miAddTestAssembly.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_miAddTestAssembly_MouseUp);
            // 
            // m_automationDefinitionTab
            // 
            this.m_automationDefinitionTab.Controls.Add(this.m_splitContainer);
            this.m_automationDefinitionTab.Location = new System.Drawing.Point(4, 22);
            this.m_automationDefinitionTab.Name = "m_automationDefinitionTab";
            this.m_automationDefinitionTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_automationDefinitionTab.Size = new System.Drawing.Size(788, 404);
            this.m_automationDefinitionTab.TabIndex = 1;
            this.m_automationDefinitionTab.Text = "Automation Definition";
            this.m_automationDefinitionTab.UseVisualStyleBackColor = true;
            // 
            // m_splitContainer
            // 
            this.m_splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_splitContainer.Location = new System.Drawing.Point(3, 3);
            this.m_splitContainer.Name = "m_splitContainer";
            // 
            // m_splitContainer.Panel1
            // 
            this.m_splitContainer.Panel1.Controls.Add(this.m_toolStrip);
            this.m_splitContainer.Panel1.Controls.Add(this.m_testAssemblytreeView);
            // 
            // m_splitContainer.Panel2
            // 
            this.m_splitContainer.Panel2.Controls.Add(this.m_testParameterGrid);
            this.m_splitContainer.Size = new System.Drawing.Size(782, 398);
            this.m_splitContainer.SplitterDistance = 300;
            this.m_splitContainer.TabIndex = 0;
            // 
            // m_toolStrip
            // 
            this.m_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_tsAddAssemblyButton,
            this.m_tsRefreshButton});
            this.m_toolStrip.Location = new System.Drawing.Point(0, 0);
            this.m_toolStrip.Name = "m_toolStrip";
            this.m_toolStrip.Size = new System.Drawing.Size(300, 25);
            this.m_toolStrip.TabIndex = 1;
            this.m_toolStrip.Text = "toolStrip1";
            // 
            // m_tsAddAssemblyButton
            // 
            this.m_tsAddAssemblyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_tsAddAssemblyButton.Image = ((System.Drawing.Image)(resources.GetObject("m_tsAddAssemblyButton.Image")));
            this.m_tsAddAssemblyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_tsAddAssemblyButton.Name = "m_tsAddAssemblyButton";
            this.m_tsAddAssemblyButton.Size = new System.Drawing.Size(23, 22);
            this.m_tsAddAssemblyButton.Text = "toolStripButton1";
            this.m_tsAddAssemblyButton.ToolTipText = "Add test assembly";
            this.m_tsAddAssemblyButton.Click += new System.EventHandler(this.m_tsAddAssemblyButton_Click);
            // 
            // m_tsRefreshButton
            // 
            this.m_tsRefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_tsRefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("m_tsRefreshButton.Image")));
            this.m_tsRefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_tsRefreshButton.Name = "m_tsRefreshButton";
            this.m_tsRefreshButton.Size = new System.Drawing.Size(23, 22);
            this.m_tsRefreshButton.Text = "toolStripButton2";
            this.m_tsRefreshButton.ToolTipText = "Refresh assemblies";
            // 
            // m_testAssemblytreeView
            // 
            this.m_testAssemblytreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_testAssemblytreeView.HideSelection = false;
            this.m_testAssemblytreeView.Location = new System.Drawing.Point(0, 25);
            this.m_testAssemblytreeView.Name = "m_testAssemblytreeView";
            treeNode1.Name = "RootNode";
            treeNode1.Text = "Test Libraries";
            this.m_testAssemblytreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.m_testAssemblytreeView.ShowNodeToolTips = true;
            this.m_testAssemblytreeView.Size = new System.Drawing.Size(300, 373);
            this.m_testAssemblytreeView.TabIndex = 0;
            this.m_testAssemblytreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_testAssemblytreeView_AfterSelect);
            this.m_testAssemblytreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.m_testAssemblytreeView_MouseClick);
            this.m_testAssemblytreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_testAssemblytreeView_MouseUp);
            // 
            // m_testParameterGrid
            // 
            this.m_testParameterGrid.AllowUserToAddRows = false;
            this.m_testParameterGrid.AllowUserToDeleteRows = false;
            this.m_testParameterGrid.AllowUserToResizeRows = false;
            this.m_testParameterGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.m_testParameterGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.m_testParameterGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_testParameterGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_parameterName,
            this.m_parameterDatatype,
            this.m_parameterValue});
            this.m_testParameterGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_testParameterGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.m_testParameterGrid.Location = new System.Drawing.Point(0, 0);
            this.m_testParameterGrid.MultiSelect = false;
            this.m_testParameterGrid.Name = "m_testParameterGrid";
            this.m_testParameterGrid.RowHeadersVisible = false;
            this.m_testParameterGrid.Size = new System.Drawing.Size(478, 398);
            this.m_testParameterGrid.TabIndex = 10;
            this.m_testParameterGrid.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.m_testParameterGrid_RowValidating);
            // 
            // m_parameterName
            // 
            this.m_parameterName.HeaderText = "Parameter";
            this.m_parameterName.Name = "m_parameterName";
            this.m_parameterName.ReadOnly = true;
            this.m_parameterName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.m_parameterName.Width = 61;
            // 
            // m_parameterDatatype
            // 
            this.m_parameterDatatype.HeaderText = "Data Type";
            this.m_parameterDatatype.Name = "m_parameterDatatype";
            this.m_parameterDatatype.ReadOnly = true;
            this.m_parameterDatatype.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.m_parameterDatatype.Width = 63;
            // 
            // m_parameterValue
            // 
            this.m_parameterValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_parameterValue.HeaderText = "Value";
            this.m_parameterValue.Name = "m_parameterValue";
            this.m_parameterValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_generalTab
            // 
            this.m_generalTab.Controls.Add(this.m_activatecheckBox);
            this.m_generalTab.Controls.Add(this.m_expectedText);
            this.m_generalTab.Controls.Add(this.m_descText);
            this.m_generalTab.Controls.Add(this.m_titleText);
            this.m_generalTab.Controls.Add(this.m_expectedLabel);
            this.m_generalTab.Controls.Add(this.m_automatecheckBox);
            this.m_generalTab.Controls.Add(this.m_descLabel);
            this.m_generalTab.Controls.Add(this.m_titleLabel);
            this.m_generalTab.Location = new System.Drawing.Point(4, 22);
            this.m_generalTab.Name = "m_generalTab";
            this.m_generalTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_generalTab.Size = new System.Drawing.Size(788, 404);
            this.m_generalTab.TabIndex = 0;
            this.m_generalTab.Text = "General";
            this.m_generalTab.UseVisualStyleBackColor = true;
            // 
            // m_activatecheckBox
            // 
            this.m_activatecheckBox.AutoSize = true;
            this.m_activatecheckBox.Location = new System.Drawing.Point(6, 381);
            this.m_activatecheckBox.Name = "m_activatecheckBox";
            this.m_activatecheckBox.Size = new System.Drawing.Size(71, 17);
            this.m_activatecheckBox.TabIndex = 22;
            this.m_activatecheckBox.Text = "&Activated";
            this.m_activatecheckBox.UseVisualStyleBackColor = true;
            this.m_activatecheckBox.CheckedChanged += new System.EventHandler(this.m_activatecheckBox_CheckedChanged);
            // 
            // m_expectedText
            // 
            this.m_expectedText.AcceptsTab = true;
            this.m_expectedText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_expectedText.BulletIndent = 4;
            this.m_expectedText.Location = new System.Drawing.Point(6, 180);
            this.m_expectedText.Name = "m_expectedText";
            this.m_expectedText.ShowSelectionMargin = true;
            this.m_expectedText.Size = new System.Drawing.Size(775, 195);
            this.m_expectedText.TabIndex = 3;
            this.m_expectedText.Text = "";
            this.m_expectedText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.m_expectedText_LinkClicked);
            // 
            // m_descText
            // 
            this.m_descText.AcceptsTab = true;
            this.m_descText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_descText.BulletIndent = 4;
            this.m_descText.Location = new System.Drawing.Point(6, 66);
            this.m_descText.Name = "m_descText";
            this.m_descText.ShowSelectionMargin = true;
            this.m_descText.Size = new System.Drawing.Size(776, 90);
            this.m_descText.TabIndex = 2;
            this.m_descText.Text = "";
            this.m_descText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.m_descText_LinkClicked);
            this.m_descText.TextChanged += new System.EventHandler(this.m_descText_TextChanged);
            // 
            // m_titleText
            // 
            this.m_titleText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_titleText.Location = new System.Drawing.Point(6, 26);
            this.m_titleText.Name = "m_titleText";
            this.m_titleText.Size = new System.Drawing.Size(775, 20);
            this.m_titleText.TabIndex = 1;
            this.m_titleText.TextChanged += new System.EventHandler(this.m_titleText_TextChanged);
            // 
            // m_expectedLabel
            // 
            this.m_expectedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_expectedLabel.AutoSize = true;
            this.m_expectedLabel.Location = new System.Drawing.Point(6, 162);
            this.m_expectedLabel.Name = "m_expectedLabel";
            this.m_expectedLabel.Size = new System.Drawing.Size(100, 13);
            this.m_expectedLabel.TabIndex = 21;
            this.m_expectedLabel.Text = "&Expected Behavior:";
            // 
            // m_automatecheckBox
            // 
            this.m_automatecheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_automatecheckBox.AutoSize = true;
            this.m_automatecheckBox.Location = new System.Drawing.Point(77, 381);
            this.m_automatecheckBox.Name = "m_automatecheckBox";
            this.m_automatecheckBox.Size = new System.Drawing.Size(71, 17);
            this.m_automatecheckBox.TabIndex = 4;
            this.m_automatecheckBox.Text = "Au&tomate";
            this.m_automatecheckBox.UseVisualStyleBackColor = true;
            this.m_automatecheckBox.CheckedChanged += new System.EventHandler(this.m_automatedChk_CheckedChanged);
            // 
            // m_descLabel
            // 
            this.m_descLabel.AutoSize = true;
            this.m_descLabel.Location = new System.Drawing.Point(6, 49);
            this.m_descLabel.Name = "m_descLabel";
            this.m_descLabel.Size = new System.Drawing.Size(63, 13);
            this.m_descLabel.TabIndex = 19;
            this.m_descLabel.Text = "&Description:";
            // 
            // m_titleLabel
            // 
            this.m_titleLabel.AutoSize = true;
            this.m_titleLabel.Location = new System.Drawing.Point(6, 10);
            this.m_titleLabel.Name = "m_titleLabel";
            this.m_titleLabel.Size = new System.Drawing.Size(30, 13);
            this.m_titleLabel.TabIndex = 18;
            this.m_titleLabel.Text = "&Title:";
            // 
            // m_editorTabs
            // 
            this.m_editorTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_editorTabs.Controls.Add(this.m_generalTab);
            this.m_editorTabs.Controls.Add(this.m_automationDefinitionTab);
            this.m_editorTabs.Location = new System.Drawing.Point(4, 8);
            this.m_editorTabs.Name = "m_editorTabs";
            this.m_editorTabs.SelectedIndex = 0;
            this.m_editorTabs.Size = new System.Drawing.Size(796, 430);
            this.m_editorTabs.TabIndex = 20;
            // 
            // m_treeViewPopupMenu
            // 
            this.m_treeViewPopupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_executeToolStripMenuItem,
            this.toolStripSeparator1,
            this.m_resetToolStripMenuItem});
            this.m_treeViewPopupMenu.Name = "m_treeViewPopupMenu";
            this.m_treeViewPopupMenu.Size = new System.Drawing.Size(116, 54);
            // 
            // m_executeToolStripMenuItem
            // 
            this.m_executeToolStripMenuItem.Enabled = false;
            this.m_executeToolStripMenuItem.Name = "m_executeToolStripMenuItem";
            this.m_executeToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.m_executeToolStripMenuItem.Text = "Execute";
            this.m_executeToolStripMenuItem.ToolTipText = "Executes the selected test method with seleceted test parameters.";
            this.m_executeToolStripMenuItem.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(112, 6);
            this.toolStripSeparator1.Visible = false;
            // 
            // m_resetToolStripMenuItem
            // 
            this.m_resetToolStripMenuItem.Name = "m_resetToolStripMenuItem";
            this.m_resetToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.m_resetToolStripMenuItem.Text = "Reset";
            this.m_resetToolStripMenuItem.ToolTipText = "Resets test parameters to original values.";
            // 
            // TestScriptObjectEditorDialog
            // 
            this.AcceptButton = this.m_okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_cancelBtn;
            this.ClientSize = new System.Drawing.Size(809, 482);
            this.Controls.Add(this.m_editorTabs);
            this.Controls.Add(this.m_okBtn);
            this.Controls.Add(this.m_cancelBtn);
            this.MinimumSize = new System.Drawing.Size(825, 520);
            this.Name = "TestScriptObjectEditorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AutoTestMethodDlg";
            this.Activated += new System.EventHandler(this.AutoTestMethodDlg_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TestScriptObjectEditorDialog_FormClosed);
            this.Load += new System.EventHandler(this.TestScriptObjectEditDialog_Load);
            this.m_addTestAssemblyContextMenuStrip.ResumeLayout(false);
            this.m_automationDefinitionTab.ResumeLayout(false);
            this.m_splitContainer.Panel1.ResumeLayout(false);
            this.m_splitContainer.Panel1.PerformLayout();
            this.m_splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_splitContainer)).EndInit();
            this.m_splitContainer.ResumeLayout(false);
            this.m_toolStrip.ResumeLayout(false);
            this.m_toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_testParameterGrid)).EndInit();
            this.m_generalTab.ResumeLayout(false);
            this.m_generalTab.PerformLayout();
            this.m_editorTabs.ResumeLayout(false);
            this.m_treeViewPopupMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_cancelBtn;
        private System.Windows.Forms.Button m_okBtn;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.ContextMenuStrip m_addTestAssemblyContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem m_miAddTestAssembly;
        private System.Windows.Forms.TabPage m_automationDefinitionTab;
        private System.Windows.Forms.SplitContainer m_splitContainer;
        private System.Windows.Forms.TreeView m_testAssemblytreeView;
        private System.Windows.Forms.DataGridView m_testParameterGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_parameterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_parameterDatatype;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_parameterValue;
        private System.Windows.Forms.TabPage m_generalTab;
        private System.Windows.Forms.RichTextBox m_expectedText;
        private System.Windows.Forms.RichTextBox m_descText;
        private System.Windows.Forms.TextBox m_titleText;
        private System.Windows.Forms.Label m_expectedLabel;
        private System.Windows.Forms.CheckBox m_automatecheckBox;
        private System.Windows.Forms.Label m_descLabel;
        private System.Windows.Forms.Label m_titleLabel;
        private System.Windows.Forms.TabControl m_editorTabs;
        private System.Windows.Forms.ToolStrip m_toolStrip;
        private System.Windows.Forms.ToolStripButton m_tsAddAssemblyButton;
        private System.Windows.Forms.ToolStripButton m_tsRefreshButton;
        private System.Windows.Forms.CheckBox m_activatecheckBox;
        private System.Windows.Forms.ContextMenuStrip m_treeViewPopupMenu;
        private System.Windows.Forms.ToolStripMenuItem m_resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_executeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}