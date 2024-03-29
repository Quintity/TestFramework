﻿namespace Quintity.TestFramework.TestEngineer
{
    partial class MainForm
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
            Quintity.TestFramework.TestEngineer.CheckedBoxProperties checkedBoxProperties2 = new Quintity.TestFramework.TestEngineer.CheckedBoxProperties();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.m_mainMenu = new System.Windows.Forms.MenuStrip();
            this.m_fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.m_suiteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.m_toolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_testPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testListenersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.m_testPropertiesStatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_totalAvailableStatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_inprocessStatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_passedStatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_failedStatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_erroredStatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_inconclusiveStatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_didnotexecuteStatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_elapsedTimeStatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.m_testPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.m_filterToolStripButton = new System.Windows.Forms.Button();
            this.m_tagSelectorLabel = new System.Windows.Forms.Label();
            this.m_samplingRateLabel = new System.Windows.Forms.Label();
            this.m_samplingNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.m_treeViewToolStrip = new System.Windows.Forms.ToolStrip();
            this.m_viewersTabControl = new System.Windows.Forms.TabControl();
            this.m_resultsViewerTabPage = new System.Windows.Forms.TabPage();
            this.m_traceViewerTabPage = new System.Windows.Forms.TabPage();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_executionTimer = new System.Windows.Forms.Timer(this.components);
            this.m_tagComboBox = new Quintity.TestFramework.TestEngineer.CheckedComboBox();
            this.m_testTreeView = new Quintity.TestFramework.TestEngineer.TestTreeView();
            this.m_resultsViewer = new Quintity.TestFramework.TestEngineer.ResultsViewer();
            this.m_traceViewer = new Quintity.TestFramework.TestEngineer.TraceViewer();
            this.m_toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.m_collapseAllToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_testCasesOnlyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_expandAllToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_executeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_stopToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_resetToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.m_stepOverButton = new System.Windows.Forms.ToolStripButton();
            this.m_fileExplorerButton = new System.Windows.Forms.ToolStripButton();
            this.m_fileNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_fileOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miFileExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.m_fileSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_fileSaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_fileExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_editUndoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_editRedoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_editCutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_editCopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_editPasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_editDeleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_suiteExecuteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_suiteStopExecuteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_suiteResetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_suiteStepOverMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_suiteToggleBreakpointMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_suiteDeleteAllBreakpointsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_suiteDisableAllBreakpointsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_testHomeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_testSuitesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_testConfigsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_testOutputMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_testResultsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_testDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mainMenu.SuspendLayout();
            this.m_mainToolStrip.SuspendLayout();
            this.m_mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_splitContainer1)).BeginInit();
            this.m_splitContainer1.Panel1.SuspendLayout();
            this.m_splitContainer1.Panel2.SuspendLayout();
            this.m_splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_samplingNumericUpDown)).BeginInit();
            this.m_treeViewToolStrip.SuspendLayout();
            this.m_viewersTabControl.SuspendLayout();
            this.m_resultsViewerTabPage.SuspendLayout();
            this.m_traceViewerTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_mainMenu
            // 
            this.m_mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_fileMenuItem,
            this.m_editMenuItem,
            this.m_suiteMenuItem,
            this.m_toolsMenuItem,
            this.m_helpMenuItem});
            this.m_mainMenu.Location = new System.Drawing.Point(0, 0);
            this.m_mainMenu.Name = "m_mainMenu";
            this.m_mainMenu.Size = new System.Drawing.Size(1234, 24);
            this.m_mainMenu.TabIndex = 0;
            this.m_mainMenu.Text = "menuStrip1";
            this.m_mainMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.m_mainMenu_ItemClicked);
            // 
            // m_fileMenuItem
            // 
            this.m_fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_fileNewMenuItem,
            this.m_fileOpenMenuItem,
            this.m_toolStripSeparator1,
            this.m_fileSaveMenuItem,
            this.m_fileSaveAsMenuItem,
            this.m_toolStripSeparator2,
            this.m_miFileExplorer,
            this.m_toolStripSeparator4,
            this.m_fileExitMenuItem});
            this.m_fileMenuItem.Name = "m_fileMenuItem";
            this.m_fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.m_fileMenuItem.Text = "&File";
            // 
            // m_toolStripSeparator1
            // 
            this.m_toolStripSeparator1.Name = "m_toolStripSeparator1";
            this.m_toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // m_toolStripSeparator2
            // 
            this.m_toolStripSeparator2.Name = "m_toolStripSeparator2";
            this.m_toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // m_editMenuItem
            // 
            this.m_editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_editUndoMenuItem,
            this.m_editRedoMenuItem,
            this.m_toolStripSeparator3,
            this.m_editCutMenuItem,
            this.m_editCopyMenuItem,
            this.m_editPasteMenuItem,
            this.m_editDeleteMenuItem});
            this.m_editMenuItem.Name = "m_editMenuItem";
            this.m_editMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.m_editMenuItem.Size = new System.Drawing.Size(39, 20);
            this.m_editMenuItem.Text = "&Edit";
            this.m_editMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_editMenuItem_MouseDown);
            // 
            // m_toolStripSeparator3
            // 
            this.m_toolStripSeparator3.Name = "m_toolStripSeparator3";
            this.m_toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // m_suiteMenuItem
            // 
            this.m_suiteMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_suiteExecuteMenuItem,
            this.m_suiteStopExecuteMenuItem,
            this.m_suiteResetMenuItem,
            this.m_toolStripSeparator5,
            this.m_suiteStepOverMenuItem,
            this.toolStripSeparator5,
            this.m_suiteToggleBreakpointMenuItem,
            this.m_suiteDeleteAllBreakpointsMenuItem,
            this.m_suiteDisableAllBreakpointsMenuItem});
            this.m_suiteMenuItem.Name = "m_suiteMenuItem";
            this.m_suiteMenuItem.Size = new System.Drawing.Size(68, 20);
            this.m_suiteMenuItem.Text = "Test &Suite";
            this.m_suiteMenuItem.DropDownOpening += new System.EventHandler(this.m_suiteMenuItem_DropDownOpening);
            // 
            // m_toolStripSeparator5
            // 
            this.m_toolStripSeparator5.Name = "m_toolStripSeparator5";
            this.m_toolStripSeparator5.Size = new System.Drawing.Size(264, 6);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(264, 6);
            // 
            // m_toolsMenuItem
            // 
            this.m_toolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_testPropertiesToolStripMenuItem,
            this.testListenersToolStripMenuItem});
            this.m_toolsMenuItem.Name = "m_toolsMenuItem";
            this.m_toolsMenuItem.Size = new System.Drawing.Size(46, 20);
            this.m_toolsMenuItem.Text = "&Tools";
            this.m_toolsMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // m_testPropertiesToolStripMenuItem
            // 
            this.m_testPropertiesToolStripMenuItem.Name = "m_testPropertiesToolStripMenuItem";
            this.m_testPropertiesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.m_testPropertiesToolStripMenuItem.Text = "Test &Properties Editor...";
            this.m_testPropertiesToolStripMenuItem.Click += new System.EventHandler(this.m_testPropertiesToolStripMenuItem_Click);
            // 
            // testListenersToolStripMenuItem
            // 
            this.testListenersToolStripMenuItem.Name = "testListenersToolStripMenuItem";
            this.testListenersToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.testListenersToolStripMenuItem.Text = "Test &Listeners Editor...";
            this.testListenersToolStripMenuItem.Click += new System.EventHandler(this.testListenersToolStripMenuItem_Click);
            // 
            // m_helpMenuItem
            // 
            this.m_helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.m_helpMenuItem.Name = "m_helpMenuItem";
            this.m_helpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.m_helpMenuItem.Text = "&Help";
            // 
            // m_mainToolStrip
            // 
            this.m_mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_newToolStripButton,
            this.m_openToolStripButton,
            this.m_saveToolStripButton,
            this.toolStripSeparator2,
            this.m_executeToolStripButton,
            this.m_stopToolStripButton,
            this.m_resetToolStripButton,
            this.toolStripSeparator4,
            this.m_stepOverButton,
            this.toolStripSeparator1,
            this.m_fileExplorerButton});
            this.m_mainToolStrip.Location = new System.Drawing.Point(0, 24);
            this.m_mainToolStrip.Name = "m_mainToolStrip";
            this.m_mainToolStrip.Size = new System.Drawing.Size(1234, 25);
            this.m_mainToolStrip.TabIndex = 1;
            this.m_mainToolStrip.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // m_mainStatusStrip
            // 
            this.m_mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_testPropertiesStatusBarLabel,
            this.m_totalAvailableStatusBarLabel,
            this.m_inprocessStatusBarLabel,
            this.m_passedStatusBarLabel,
            this.m_failedStatusBarLabel,
            this.m_erroredStatusBarLabel,
            this.m_inconclusiveStatusBarLabel,
            this.m_didnotexecuteStatusBarLabel,
            this.m_elapsedTimeStatusBarLabel});
            this.m_mainStatusStrip.Location = new System.Drawing.Point(0, 738);
            this.m_mainStatusStrip.Name = "m_mainStatusStrip";
            this.m_mainStatusStrip.ShowItemToolTips = true;
            this.m_mainStatusStrip.Size = new System.Drawing.Size(1234, 24);
            this.m_mainStatusStrip.TabIndex = 2;
            this.m_mainStatusStrip.Text = "statusStrip1";
            // 
            // m_testPropertiesStatusBarLabel
            // 
            this.m_testPropertiesStatusBarLabel.AutoSize = false;
            this.m_testPropertiesStatusBarLabel.Name = "m_testPropertiesStatusBarLabel";
            this.m_testPropertiesStatusBarLabel.Size = new System.Drawing.Size(901, 19);
            this.m_testPropertiesStatusBarLabel.Spring = true;
            this.m_testPropertiesStatusBarLabel.Text = "Properties file";
            this.m_testPropertiesStatusBarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_testPropertiesStatusBarLabel.ToolTipText = "Currently loaded test properties";
            // 
            // m_totalAvailableStatusBarLabel
            // 
            this.m_totalAvailableStatusBarLabel.AutoSize = false;
            this.m_totalAvailableStatusBarLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_totalAvailableStatusBarLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.m_totalAvailableStatusBarLabel.Name = "m_totalAvailableStatusBarLabel";
            this.m_totalAvailableStatusBarLabel.Size = new System.Drawing.Size(25, 19);
            this.m_totalAvailableStatusBarLabel.Text = "0";
            this.m_totalAvailableStatusBarLabel.ToolTipText = "Total test cases available for execution";
            // 
            // m_inprocessStatusBarLabel
            // 
            this.m_inprocessStatusBarLabel.AutoSize = false;
            this.m_inprocessStatusBarLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_inprocessStatusBarLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.m_inprocessStatusBarLabel.Name = "m_inprocessStatusBarLabel";
            this.m_inprocessStatusBarLabel.Size = new System.Drawing.Size(40, 19);
            this.m_inprocessStatusBarLabel.Text = "0";
            this.m_inprocessStatusBarLabel.ToolTipText = "Total test cases currently in progress";
            // 
            // m_passedStatusBarLabel
            // 
            this.m_passedStatusBarLabel.AutoSize = false;
            this.m_passedStatusBarLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_passedStatusBarLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.m_passedStatusBarLabel.ForeColor = System.Drawing.Color.ForestGreen;
            this.m_passedStatusBarLabel.Name = "m_passedStatusBarLabel";
            this.m_passedStatusBarLabel.Size = new System.Drawing.Size(40, 19);
            this.m_passedStatusBarLabel.Text = "0";
            this.m_passedStatusBarLabel.ToolTipText = "Test cases that passed";
            // 
            // m_failedStatusBarLabel
            // 
            this.m_failedStatusBarLabel.AutoSize = false;
            this.m_failedStatusBarLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_failedStatusBarLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.m_failedStatusBarLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.m_failedStatusBarLabel.Name = "m_failedStatusBarLabel";
            this.m_failedStatusBarLabel.Size = new System.Drawing.Size(40, 19);
            this.m_failedStatusBarLabel.Text = "0";
            this.m_failedStatusBarLabel.ToolTipText = "Test cases that failed";
            // 
            // m_erroredStatusBarLabel
            // 
            this.m_erroredStatusBarLabel.AutoSize = false;
            this.m_erroredStatusBarLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_erroredStatusBarLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.m_erroredStatusBarLabel.Name = "m_erroredStatusBarLabel";
            this.m_erroredStatusBarLabel.Size = new System.Drawing.Size(40, 19);
            this.m_erroredStatusBarLabel.Text = "0";
            this.m_erroredStatusBarLabel.ToolTipText = "Test cases that errored in execution";
            // 
            // m_inconclusiveStatusBarLabel
            // 
            this.m_inconclusiveStatusBarLabel.AutoSize = false;
            this.m_inconclusiveStatusBarLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_inconclusiveStatusBarLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.m_inconclusiveStatusBarLabel.Name = "m_inconclusiveStatusBarLabel";
            this.m_inconclusiveStatusBarLabel.Size = new System.Drawing.Size(40, 19);
            this.m_inconclusiveStatusBarLabel.Text = "0";
            this.m_inconclusiveStatusBarLabel.ToolTipText = "Test cases with inconclusive results";
            // 
            // m_didnotexecuteStatusBarLabel
            // 
            this.m_didnotexecuteStatusBarLabel.AutoSize = false;
            this.m_didnotexecuteStatusBarLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_didnotexecuteStatusBarLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.m_didnotexecuteStatusBarLabel.Name = "m_didnotexecuteStatusBarLabel";
            this.m_didnotexecuteStatusBarLabel.Size = new System.Drawing.Size(40, 19);
            this.m_didnotexecuteStatusBarLabel.Text = "0";
            this.m_didnotexecuteStatusBarLabel.ToolTipText = "Test cases not execute";
            // 
            // m_elapsedTimeStatusBarLabel
            // 
            this.m_elapsedTimeStatusBarLabel.AutoSize = false;
            this.m_elapsedTimeStatusBarLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_elapsedTimeStatusBarLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.m_elapsedTimeStatusBarLabel.Name = "m_elapsedTimeStatusBarLabel";
            this.m_elapsedTimeStatusBarLabel.Size = new System.Drawing.Size(53, 19);
            this.m_elapsedTimeStatusBarLabel.Text = "00:00:00";
            this.m_elapsedTimeStatusBarLabel.ToolTipText = "Elapsed execution time";
            // 
            // m_splitContainer1
            // 
            this.m_splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.m_splitContainer1.Name = "m_splitContainer1";
            // 
            // m_splitContainer1.Panel1
            // 
            this.m_splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.m_splitContainer1.Panel1.Controls.Add(this.m_testPropertyGrid);
            // 
            // m_splitContainer1.Panel2
            // 
            this.m_splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.m_splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.m_splitContainer1.Size = new System.Drawing.Size(1234, 689);
            this.m_splitContainer1.SplitterDistance = 287;
            this.m_splitContainer1.TabIndex = 3;
            // 
            // m_testPropertyGrid
            // 
            this.m_testPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_testPropertyGrid.Enabled = false;
            this.m_testPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.m_testPropertyGrid.Name = "m_testPropertyGrid";
            this.m_testPropertyGrid.Size = new System.Drawing.Size(287, 689);
            this.m_testPropertyGrid.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Panel1.Controls.Add(this.m_filterToolStripButton);
            this.splitContainer2.Panel1.Controls.Add(this.m_tagSelectorLabel);
            this.splitContainer2.Panel1.Controls.Add(this.m_tagComboBox);
            this.splitContainer2.Panel1.Controls.Add(this.m_samplingRateLabel);
            this.splitContainer2.Panel1.Controls.Add(this.m_samplingNumericUpDown);
            this.splitContainer2.Panel1.Controls.Add(this.m_testTreeView);
            this.splitContainer2.Panel1.Controls.Add(this.m_treeViewToolStrip);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Panel2.Controls.Add(this.m_viewersTabControl);
            this.splitContainer2.Size = new System.Drawing.Size(943, 689);
            this.splitContainer2.SplitterDistance = 434;
            this.splitContainer2.TabIndex = 0;
            // 
            // m_filterToolStripButton
            // 
            this.m_filterToolStripButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_filterToolStripButton.Enabled = false;
            this.m_filterToolStripButton.FlatAppearance.BorderSize = 0;
            this.m_filterToolStripButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_filterToolStripButton.Location = new System.Drawing.Point(738, 3);
            this.m_filterToolStripButton.Name = "m_filterToolStripButton";
            this.m_filterToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.m_filterToolStripButton.TabIndex = 6;
            this.m_filterToolStripButton.Text = "F";
            this.m_filterToolStripButton.UseVisualStyleBackColor = true;
            this.m_filterToolStripButton.Visible = false;
            this.m_filterToolStripButton.Click += new System.EventHandler(this.m_filterToolStripButton_Click);
            // 
            // m_tagSelectorLabel
            // 
            this.m_tagSelectorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tagSelectorLabel.AutoSize = true;
            this.m_tagSelectorLabel.Enabled = false;
            this.m_tagSelectorLabel.Location = new System.Drawing.Point(496, 6);
            this.m_tagSelectorLabel.Name = "m_tagSelectorLabel";
            this.m_tagSelectorLabel.Size = new System.Drawing.Size(71, 13);
            this.m_tagSelectorLabel.TabIndex = 5;
            this.m_tagSelectorLabel.Text = "Tag Selector:";
            this.m_tagSelectorLabel.Visible = false;
            // 
            // m_samplingRateLabel
            // 
            this.m_samplingRateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_samplingRateLabel.AutoSize = true;
            this.m_samplingRateLabel.Location = new System.Drawing.Point(778, 6);
            this.m_samplingRateLabel.Name = "m_samplingRateLabel";
            this.m_samplingRateLabel.Size = new System.Drawing.Size(96, 13);
            this.m_samplingRateLabel.TabIndex = 3;
            this.m_samplingRateLabel.Text = "Sampling Rate (%):";
            this.m_samplingRateLabel.Visible = false;
            // 
            // m_samplingNumericUpDown
            // 
            this.m_samplingNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_samplingNumericUpDown.Enabled = false;
            this.m_samplingNumericUpDown.Location = new System.Drawing.Point(882, 3);
            this.m_samplingNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_samplingNumericUpDown.Name = "m_samplingNumericUpDown";
            this.m_samplingNumericUpDown.Size = new System.Drawing.Size(48, 20);
            this.m_samplingNumericUpDown.TabIndex = 2;
            this.m_samplingNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.m_samplingNumericUpDown.Visible = false;
            this.m_samplingNumericUpDown.ValueChanged += new System.EventHandler(this.m_samplingNumericUpDown_ValueChanged);
            // 
            // m_treeViewToolStrip
            // 
            this.m_treeViewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_collapseAllToolStripButton,
            this.m_testCasesOnlyToolStripButton,
            this.m_expandAllToolStripButton});
            this.m_treeViewToolStrip.Location = new System.Drawing.Point(0, 0);
            this.m_treeViewToolStrip.Name = "m_treeViewToolStrip";
            this.m_treeViewToolStrip.Size = new System.Drawing.Size(943, 25);
            this.m_treeViewToolStrip.TabIndex = 0;
            this.m_treeViewToolStrip.Text = "toolStrip2";
            // 
            // m_viewersTabControl
            // 
            this.m_viewersTabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.m_viewersTabControl.Controls.Add(this.m_resultsViewerTabPage);
            this.m_viewersTabControl.Controls.Add(this.m_traceViewerTabPage);
            this.m_viewersTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_viewersTabControl.Location = new System.Drawing.Point(0, 0);
            this.m_viewersTabControl.Name = "m_viewersTabControl";
            this.m_viewersTabControl.SelectedIndex = 0;
            this.m_viewersTabControl.Size = new System.Drawing.Size(943, 251);
            this.m_viewersTabControl.TabIndex = 0;
            // 
            // m_resultsViewerTabPage
            // 
            this.m_resultsViewerTabPage.Controls.Add(this.m_resultsViewer);
            this.m_resultsViewerTabPage.Location = new System.Drawing.Point(4, 4);
            this.m_resultsViewerTabPage.Name = "m_resultsViewerTabPage";
            this.m_resultsViewerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.m_resultsViewerTabPage.Size = new System.Drawing.Size(935, 225);
            this.m_resultsViewerTabPage.TabIndex = 0;
            this.m_resultsViewerTabPage.Text = "Results Viewer";
            this.m_resultsViewerTabPage.UseVisualStyleBackColor = true;
            // 
            // m_traceViewerTabPage
            // 
            this.m_traceViewerTabPage.Controls.Add(this.m_traceViewer);
            this.m_traceViewerTabPage.Location = new System.Drawing.Point(4, 4);
            this.m_traceViewerTabPage.Name = "m_traceViewerTabPage";
            this.m_traceViewerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.m_traceViewerTabPage.Size = new System.Drawing.Size(935, 225);
            this.m_traceViewerTabPage.TabIndex = 1;
            this.m_traceViewerTabPage.Text = "Trace Output";
            this.m_traceViewerTabPage.UseVisualStyleBackColor = true;
            // 
            // m_executionTimer
            // 
            this.m_executionTimer.Interval = 1000;
            // 
            // m_tagComboBox
            // 
            this.m_tagComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            checkedBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_tagComboBox.CheckBoxProperties = checkedBoxProperties2;
            this.m_tagComboBox.DisplayMemberSingleItem = "";
            this.m_tagComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_tagComboBox.Enabled = false;
            this.m_tagComboBox.FormattingEnabled = true;
            this.m_tagComboBox.Location = new System.Drawing.Point(573, 2);
            this.m_tagComboBox.MaxDropDownItems = 20;
            this.m_tagComboBox.Name = "m_tagComboBox";
            this.m_tagComboBox.Size = new System.Drawing.Size(160, 21);
            this.m_tagComboBox.TabIndex = 4;
            this.m_tagComboBox.Visible = false;
            this.m_tagComboBox.CheckBoxCheckedChanged += new System.EventHandler(this.m_tagComboBox_CheckBoxCheckedChanged);
            // 
            // m_testTreeView
            // 
            this.m_testTreeView.AllowDrop = true;
            this.m_testTreeView.CachedTestAssemblies = null;
            this.m_testTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_testTreeView.FullRowSelect = true;
            this.m_testTreeView.HideSelection = false;
            this.m_testTreeView.ImageIndex = 0;
            this.m_testTreeView.Location = new System.Drawing.Point(0, 25);
            this.m_testTreeView.Name = "m_testTreeView";
            this.m_testTreeView.SelectedImageIndex = 0;
            this.m_testTreeView.SelectedNode = null;
            this.m_testTreeView.ShowNodeToolTips = true;
            this.m_testTreeView.Size = new System.Drawing.Size(943, 409);
            this.m_testTreeView.SuppressExecution = false;
            this.m_testTreeView.TabIndex = 1;
            this.m_testTreeView.TestListeners = null;
            this.m_testTreeView.TestProfile = null;
            this.m_testTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_testTreeView_AfterSelect);
            // 
            // m_resultsViewer
            // 
            this.m_resultsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_resultsViewer.Location = new System.Drawing.Point(3, 3);
            this.m_resultsViewer.Name = "m_resultsViewer";
            this.m_resultsViewer.ReadOnly = true;
            this.m_resultsViewer.Size = new System.Drawing.Size(929, 219);
            this.m_resultsViewer.TabIndex = 0;
            this.m_resultsViewer.Text = "";
            this.m_resultsViewer.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.m_resultsViewer_LinkClicked);
            // 
            // m_traceViewer
            // 
            this.m_traceViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_traceViewer.Location = new System.Drawing.Point(3, 3);
            this.m_traceViewer.Name = "m_traceViewer";
            this.m_traceViewer.ReadOnly = true;
            this.m_traceViewer.Size = new System.Drawing.Size(929, 219);
            this.m_traceViewer.TabIndex = 0;
            this.m_traceViewer.Text = "";
            this.m_traceViewer.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.m_traceViewer_LinkClicked);
            // 
            // m_toolStripSeparator4
            // 
            this.m_toolStripSeparator4.Name = "m_toolStripSeparator4";
            this.m_toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // m_collapseAllToolStripButton
            // 
            this.m_collapseAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_collapseAllToolStripButton.Enabled = false;
            this.m_collapseAllToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("m_collapseAllToolStripButton.Image")));
            this.m_collapseAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_collapseAllToolStripButton.Name = "m_collapseAllToolStripButton";
            this.m_collapseAllToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.m_collapseAllToolStripButton.Text = "1";
            this.m_collapseAllToolStripButton.ToolTipText = "Collapse all to root test suite ";
            this.m_collapseAllToolStripButton.Click += new System.EventHandler(this.m_collapseAllToolStripButton_Click);
            // 
            // m_testCasesOnlyToolStripButton
            // 
            this.m_testCasesOnlyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_testCasesOnlyToolStripButton.Enabled = false;
            this.m_testCasesOnlyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("m_testCasesOnlyToolStripButton.Image")));
            this.m_testCasesOnlyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_testCasesOnlyToolStripButton.Name = "m_testCasesOnlyToolStripButton";
            this.m_testCasesOnlyToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.m_testCasesOnlyToolStripButton.Text = "2";
            this.m_testCasesOnlyToolStripButton.ToolTipText = "Expand test suites only";
            this.m_testCasesOnlyToolStripButton.Click += new System.EventHandler(this.m_testCasesOnlyToolStripButton_Click);
            // 
            // m_expandAllToolStripButton
            // 
            this.m_expandAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_expandAllToolStripButton.Enabled = false;
            this.m_expandAllToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("m_expandAllToolStripButton.Image")));
            this.m_expandAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_expandAllToolStripButton.Name = "m_expandAllToolStripButton";
            this.m_expandAllToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.m_expandAllToolStripButton.Text = "A";
            this.m_expandAllToolStripButton.ToolTipText = "Expand all to root test suite";
            this.m_expandAllToolStripButton.Click += new System.EventHandler(this.m_expandAllToolStripButton_Click);
            // 
            // m_newToolStripButton
            // 
            this.m_newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("m_newToolStripButton.Image")));
            this.m_newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_newToolStripButton.Name = "m_newToolStripButton";
            this.m_newToolStripButton.Size = new System.Drawing.Size(51, 22);
            this.m_newToolStripButton.Text = "New";
            this.m_newToolStripButton.Click += new System.EventHandler(this.m_newToolStripButton_Click);
            // 
            // m_openToolStripButton
            // 
            this.m_openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("m_openToolStripButton.Image")));
            this.m_openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_openToolStripButton.Name = "m_openToolStripButton";
            this.m_openToolStripButton.Size = new System.Drawing.Size(56, 22);
            this.m_openToolStripButton.Text = "Open";
            this.m_openToolStripButton.Click += new System.EventHandler(this.m_openToolStripButton_Click);
            // 
            // m_saveToolStripButton
            // 
            this.m_saveToolStripButton.Enabled = false;
            this.m_saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("m_saveToolStripButton.Image")));
            this.m_saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_saveToolStripButton.Name = "m_saveToolStripButton";
            this.m_saveToolStripButton.Size = new System.Drawing.Size(51, 22);
            this.m_saveToolStripButton.Text = "Save";
            this.m_saveToolStripButton.ToolTipText = "m_savToolStripButton";
            this.m_saveToolStripButton.Click += new System.EventHandler(this.m_saveToolStripButton_Click);
            // 
            // m_executeToolStripButton
            // 
            this.m_executeToolStripButton.Enabled = false;
            this.m_executeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("m_executeToolStripButton.Image")));
            this.m_executeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_executeToolStripButton.Name = "m_executeToolStripButton";
            this.m_executeToolStripButton.Size = new System.Drawing.Size(68, 22);
            this.m_executeToolStripButton.Text = "Execute";
            this.m_executeToolStripButton.ToolTipText = "Begins loaded test suite execution";
            this.m_executeToolStripButton.Click += new System.EventHandler(this.m_executeToolStripButton_Click);
            // 
            // m_stopToolStripButton
            // 
            this.m_stopToolStripButton.Enabled = false;
            this.m_stopToolStripButton.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.StopExecution;
            this.m_stopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_stopToolStripButton.Name = "m_stopToolStripButton";
            this.m_stopToolStripButton.Size = new System.Drawing.Size(51, 22);
            this.m_stopToolStripButton.Text = "Stop";
            this.m_stopToolStripButton.ToolTipText = "Stops current test execution";
            this.m_stopToolStripButton.Click += new System.EventHandler(this.m_stopToolStripButton_Click);
            // 
            // m_resetToolStripButton
            // 
            this.m_resetToolStripButton.Enabled = false;
            this.m_resetToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("m_resetToolStripButton.Image")));
            this.m_resetToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_resetToolStripButton.Name = "m_resetToolStripButton";
            this.m_resetToolStripButton.Size = new System.Drawing.Size(55, 22);
            this.m_resetToolStripButton.Text = "Reset";
            this.m_resetToolStripButton.ToolTipText = "Reset all test results";
            this.m_resetToolStripButton.Click += new System.EventHandler(this.m_resetToolStripButton_Click);
            // 
            // m_stepOverButton
            // 
            this.m_stepOverButton.Enabled = false;
            this.m_stepOverButton.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.StepOver;
            this.m_stepOverButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_stepOverButton.Name = "m_stepOverButton";
            this.m_stepOverButton.Size = new System.Drawing.Size(78, 22);
            this.m_stepOverButton.Text = "Step Over";
            this.m_stepOverButton.Click += new System.EventHandler(this.m_stepOverButton_Click);
            // 
            // m_fileExplorerButton
            // 
            this.m_fileExplorerButton.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Open;
            this.m_fileExplorerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_fileExplorerButton.Name = "m_fileExplorerButton";
            this.m_fileExplorerButton.Size = new System.Drawing.Size(83, 22);
            this.m_fileExplorerButton.Text = "Test Home";
            this.m_fileExplorerButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.m_fileExplorerButton.ToolTipText = "Opens TestHome in File Explorer";
            this.m_fileExplorerButton.Click += new System.EventHandler(this.m_fileExplorerButton_Click);
            // 
            // m_fileNewMenuItem
            // 
            this.m_fileNewMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.FileNew;
            this.m_fileNewMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_fileNewMenuItem.Name = "m_fileNewMenuItem";
            this.m_fileNewMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.m_fileNewMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_fileNewMenuItem.Text = "New";
            this.m_fileNewMenuItem.Click += new System.EventHandler(this.m_fileNewMenuItem_Click);
            // 
            // m_fileOpenMenuItem
            // 
            this.m_fileOpenMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.FileOpen;
            this.m_fileOpenMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_fileOpenMenuItem.Name = "m_fileOpenMenuItem";
            this.m_fileOpenMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.m_fileOpenMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_fileOpenMenuItem.Text = "Open...";
            this.m_fileOpenMenuItem.Click += new System.EventHandler(this.m_fileOpenMenuItem_Click);
            // 
            // m_miFileExplorer
            // 
            this.m_miFileExplorer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_testHomeMenuItem,
            this.m_testConfigsMenuItem,
            this.m_testSuitesMenuItem,
            this.m_testDataMenuItem,
            this.m_testOutputMenuItem,
            this.m_testResultsMenuItem});
            this.m_miFileExplorer.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Open;
            this.m_miFileExplorer.Name = "m_miFileExplorer";
            this.m_miFileExplorer.Size = new System.Drawing.Size(180, 22);
            this.m_miFileExplorer.Text = "Open File Explorer";
            // 
            // m_fileSaveMenuItem
            // 
            this.m_fileSaveMenuItem.Enabled = false;
            this.m_fileSaveMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.FileSave;
            this.m_fileSaveMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_fileSaveMenuItem.Name = "m_fileSaveMenuItem";
            this.m_fileSaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.m_fileSaveMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_fileSaveMenuItem.Text = "&Save";
            this.m_fileSaveMenuItem.Click += new System.EventHandler(this.m_fileSaveMenuItem_Click);
            // 
            // m_fileSaveAsMenuItem
            // 
            this.m_fileSaveAsMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.SaveAs;
            this.m_fileSaveAsMenuItem.Name = "m_fileSaveAsMenuItem";
            this.m_fileSaveAsMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_fileSaveAsMenuItem.Text = "Save &As";
            this.m_fileSaveAsMenuItem.Click += new System.EventHandler(this.m_fileSaveAsMenuItem_Click);
            // 
            // m_fileExitMenuItem
            // 
            this.m_fileExitMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Exit;
            this.m_fileExitMenuItem.Name = "m_fileExitMenuItem";
            this.m_fileExitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.m_fileExitMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_fileExitMenuItem.Text = "E&xit";
            this.m_fileExitMenuItem.Click += new System.EventHandler(this.m_fileExitMenuItem_Click);
            // 
            // m_editUndoMenuItem
            // 
            this.m_editUndoMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Undo;
            this.m_editUndoMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_editUndoMenuItem.Name = "m_editUndoMenuItem";
            this.m_editUndoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.m_editUndoMenuItem.Size = new System.Drawing.Size(144, 22);
            this.m_editUndoMenuItem.Text = "Undo";
            this.m_editUndoMenuItem.Click += new System.EventHandler(this.m_editUndoMenuItem_Click);
            // 
            // m_editRedoMenuItem
            // 
            this.m_editRedoMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Redo;
            this.m_editRedoMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_editRedoMenuItem.Name = "m_editRedoMenuItem";
            this.m_editRedoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.m_editRedoMenuItem.Size = new System.Drawing.Size(144, 22);
            this.m_editRedoMenuItem.Text = "Redo";
            this.m_editRedoMenuItem.Click += new System.EventHandler(this.m_editRedoMenuItem_Click);
            // 
            // m_editCutMenuItem
            // 
            this.m_editCutMenuItem.Enabled = false;
            this.m_editCutMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Cut;
            this.m_editCutMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_editCutMenuItem.Name = "m_editCutMenuItem";
            this.m_editCutMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.m_editCutMenuItem.Size = new System.Drawing.Size(144, 22);
            this.m_editCutMenuItem.Text = "Cut";
            this.m_editCutMenuItem.Click += new System.EventHandler(this.m_editCutMenuItem_Click);
            // 
            // m_editCopyMenuItem
            // 
            this.m_editCopyMenuItem.Enabled = false;
            this.m_editCopyMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Copy;
            this.m_editCopyMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_editCopyMenuItem.Name = "m_editCopyMenuItem";
            this.m_editCopyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.m_editCopyMenuItem.Size = new System.Drawing.Size(144, 22);
            this.m_editCopyMenuItem.Text = "Copy";
            this.m_editCopyMenuItem.Click += new System.EventHandler(this.m_editCopyMenuItem_Click);
            // 
            // m_editPasteMenuItem
            // 
            this.m_editPasteMenuItem.Enabled = false;
            this.m_editPasteMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Paste;
            this.m_editPasteMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_editPasteMenuItem.Name = "m_editPasteMenuItem";
            this.m_editPasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.m_editPasteMenuItem.Size = new System.Drawing.Size(144, 22);
            this.m_editPasteMenuItem.Text = "Paste";
            this.m_editPasteMenuItem.Click += new System.EventHandler(this.m_editPasteMenuItem_Click);
            // 
            // m_editDeleteMenuItem
            // 
            this.m_editDeleteMenuItem.Enabled = false;
            this.m_editDeleteMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Delete;
            this.m_editDeleteMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_editDeleteMenuItem.Name = "m_editDeleteMenuItem";
            this.m_editDeleteMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.m_editDeleteMenuItem.Size = new System.Drawing.Size(144, 22);
            this.m_editDeleteMenuItem.Text = "Delete";
            this.m_editDeleteMenuItem.Click += new System.EventHandler(this.m_editDeleteMenuItem_Click);
            // 
            // m_suiteExecuteMenuItem
            // 
            this.m_suiteExecuteMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.StartExecution;
            this.m_suiteExecuteMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_suiteExecuteMenuItem.Name = "m_suiteExecuteMenuItem";
            this.m_suiteExecuteMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.m_suiteExecuteMenuItem.Size = new System.Drawing.Size(267, 22);
            this.m_suiteExecuteMenuItem.Text = "Execute";
            this.m_suiteExecuteMenuItem.ToolTipText = "Begins loaded test suite execution";
            this.m_suiteExecuteMenuItem.Click += new System.EventHandler(this.m_suiteExecuteMenuItem_Click);
            // 
            // m_suiteStopExecuteMenuItem
            // 
            this.m_suiteStopExecuteMenuItem.Enabled = false;
            this.m_suiteStopExecuteMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.StopExecution;
            this.m_suiteStopExecuteMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_suiteStopExecuteMenuItem.Name = "m_suiteStopExecuteMenuItem";
            this.m_suiteStopExecuteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.m_suiteStopExecuteMenuItem.Size = new System.Drawing.Size(267, 22);
            this.m_suiteStopExecuteMenuItem.Text = "&Stop Execution";
            this.m_suiteStopExecuteMenuItem.ToolTipText = "Stops current test execution";
            this.m_suiteStopExecuteMenuItem.Click += new System.EventHandler(this.m_suiteStopExecuteMenuItem_Click);
            // 
            // m_suiteResetMenuItem
            // 
            this.m_suiteResetMenuItem.Enabled = false;
            this.m_suiteResetMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("m_suiteResetMenuItem.Image")));
            this.m_suiteResetMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_suiteResetMenuItem.Name = "m_suiteResetMenuItem";
            this.m_suiteResetMenuItem.Size = new System.Drawing.Size(267, 22);
            this.m_suiteResetMenuItem.Text = "Reset Results";
            this.m_suiteResetMenuItem.ToolTipText = "Reset all test results";
            this.m_suiteResetMenuItem.Click += new System.EventHandler(this.m_suiteResetMenuItem_Click);
            // 
            // m_suiteStepOverMenuItem
            // 
            this.m_suiteStepOverMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.StepOver;
            this.m_suiteStepOverMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_suiteStepOverMenuItem.Name = "m_suiteStepOverMenuItem";
            this.m_suiteStepOverMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.m_suiteStepOverMenuItem.Size = new System.Drawing.Size(267, 22);
            this.m_suiteStepOverMenuItem.Text = "Step Over";
            this.m_suiteStepOverMenuItem.Click += new System.EventHandler(this.m_suiteStepOverMenuItem_Click);
            // 
            // m_suiteToggleBreakpointMenuItem
            // 
            this.m_suiteToggleBreakpointMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.ToggleAllBreakpoints;
            this.m_suiteToggleBreakpointMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_suiteToggleBreakpointMenuItem.Name = "m_suiteToggleBreakpointMenuItem";
            this.m_suiteToggleBreakpointMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.m_suiteToggleBreakpointMenuItem.Size = new System.Drawing.Size(267, 22);
            this.m_suiteToggleBreakpointMenuItem.Text = "Toggle Breakpoint";
            this.m_suiteToggleBreakpointMenuItem.Click += new System.EventHandler(this.m_suiteToggleBreakpointMenuItem_Click);
            // 
            // m_suiteDeleteAllBreakpointsMenuItem
            // 
            this.m_suiteDeleteAllBreakpointsMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.ClearBreakpointGroup;
            this.m_suiteDeleteAllBreakpointsMenuItem.Name = "m_suiteDeleteAllBreakpointsMenuItem";
            this.m_suiteDeleteAllBreakpointsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F9)));
            this.m_suiteDeleteAllBreakpointsMenuItem.Size = new System.Drawing.Size(267, 22);
            this.m_suiteDeleteAllBreakpointsMenuItem.Text = "Delete All Breakpoints";
            this.m_suiteDeleteAllBreakpointsMenuItem.Click += new System.EventHandler(this.m_suiteDeleteAllBreakpointsMenuItem_Click);
            // 
            // m_suiteDisableAllBreakpointsMenuItem
            // 
            this.m_suiteDisableAllBreakpointsMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.DisableAllBreakpoints;
            this.m_suiteDisableAllBreakpointsMenuItem.Name = "m_suiteDisableAllBreakpointsMenuItem";
            this.m_suiteDisableAllBreakpointsMenuItem.Size = new System.Drawing.Size(267, 22);
            this.m_suiteDisableAllBreakpointsMenuItem.Text = "Disable All Breakpoints";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.AboutBox;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // m_testHomeMenuItem
            // 
            this.m_testHomeMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_testHomeMenuItem.Name = "m_testHomeMenuItem";
            this.m_testHomeMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_testHomeMenuItem.Text = "TestHome";
            this.m_testHomeMenuItem.ToolTipText = "Opens TestHome folder in File Explorer";
            this.m_testHomeMenuItem.Click += new System.EventHandler(this.m_testHomeMenuItem_Click);
            // 
            // m_testSuitesMenuItem
            // 
            this.m_testSuitesMenuItem.Name = "m_testSuitesMenuItem";
            this.m_testSuitesMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_testSuitesMenuItem.Text = "TestSuites";
            this.m_testSuitesMenuItem.ToolTipText = "Opens TestSuites folder in File Explorer";
            this.m_testSuitesMenuItem.Click += new System.EventHandler(this.m_testSuitesMenuItem_Click);
            // 
            // m_testConfigsMenuItem
            // 
            this.m_testConfigsMenuItem.Name = "m_testConfigsMenuItem";
            this.m_testConfigsMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_testConfigsMenuItem.Text = "TestConfigs";
            this.m_testConfigsMenuItem.ToolTipText = "Opens TestConfigs folder in File Explorer";
            this.m_testConfigsMenuItem.Click += new System.EventHandler(this.m_testConfigsMenuItem_Click);
            // 
            // m_testOutputMenuItem
            // 
            this.m_testOutputMenuItem.Name = "m_testOutputMenuItem";
            this.m_testOutputMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_testOutputMenuItem.Text = "TestOutput";
            this.m_testOutputMenuItem.ToolTipText = "Opens TestOutput folder in File Explorer";
            this.m_testOutputMenuItem.Click += new System.EventHandler(this.m_testOutputMenuItem_Click);
            // 
            // m_testResultsMenuItem
            // 
            this.m_testResultsMenuItem.Name = "m_testResultsMenuItem";
            this.m_testResultsMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_testResultsMenuItem.Text = "TestResults";
            this.m_testResultsMenuItem.ToolTipText = "Opens TestResults folder in File Explorer";
            this.m_testResultsMenuItem.Click += new System.EventHandler(this.m_testResultsMenuItem_Click);
            // 
            // m_testDataMenuItem
            // 
            this.m_testDataMenuItem.Name = "m_testDataMenuItem";
            this.m_testDataMenuItem.Size = new System.Drawing.Size(180, 22);
            this.m_testDataMenuItem.Text = "TestData";
            this.m_testDataMenuItem.ToolTipText = "Opens TestData folder in File Explorer";
            this.m_testDataMenuItem.Click += new System.EventHandler(this.m_testDataMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 762);
            this.Controls.Add(this.m_splitContainer1);
            this.Controls.Add(this.m_mainStatusStrip);
            this.Controls.Add(this.m_mainToolStrip);
            this.Controls.Add(this.m_mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.m_mainMenu;
            this.Name = "MainForm";
            this.Text = "Quintity TestEngineer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.TestEngineer_Shown);
            this.m_mainMenu.ResumeLayout(false);
            this.m_mainMenu.PerformLayout();
            this.m_mainToolStrip.ResumeLayout(false);
            this.m_mainToolStrip.PerformLayout();
            this.m_mainStatusStrip.ResumeLayout(false);
            this.m_mainStatusStrip.PerformLayout();
            this.m_splitContainer1.Panel1.ResumeLayout(false);
            this.m_splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_splitContainer1)).EndInit();
            this.m_splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_samplingNumericUpDown)).EndInit();
            this.m_treeViewToolStrip.ResumeLayout(false);
            this.m_treeViewToolStrip.PerformLayout();
            this.m_viewersTabControl.ResumeLayout(false);
            this.m_resultsViewerTabPage.ResumeLayout(false);
            this.m_traceViewerTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip m_mainMenu;
        private System.Windows.Forms.ToolStripMenuItem m_fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_suiteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_helpMenuItem;
        private System.Windows.Forms.ToolStrip m_mainToolStrip;
        private System.Windows.Forms.ToolStripButton m_newToolStripButton;
        private System.Windows.Forms.StatusStrip m_mainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel m_testPropertiesStatusBarLabel;
        private System.Windows.Forms.SplitContainer m_splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid m_testPropertyGrid;
        private System.Windows.Forms.ToolStrip m_treeViewToolStrip;
        private System.Windows.Forms.TabControl m_viewersTabControl;
        private System.Windows.Forms.TabPage m_resultsViewerTabPage;
        private System.Windows.Forms.TabPage m_traceViewerTabPage;
        private TestTreeView m_testTreeView;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.SaveFileDialog m_saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem m_fileNewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_fileOpenMenuItem;
        private System.Windows.Forms.ToolStripSeparator m_toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem m_fileSaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_fileSaveAsMenuItem;
        private System.Windows.Forms.ToolStripSeparator m_toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem m_fileExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_editUndoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_editRedoMenuItem;
        private System.Windows.Forms.ToolStripSeparator m_toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem m_editCutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_editCopyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_editPasteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_editDeleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_suiteExecuteMenuItem;
        private System.Windows.Forms.ToolStripSeparator m_toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem m_suiteResetMenuItem;
        private Quintity.TestFramework.TestEngineer.ResultsViewer m_resultsViewer;
        private Quintity.TestFramework.TestEngineer.TraceViewer m_traceViewer;
        private System.Windows.Forms.ToolStripButton m_openToolStripButton;
        private System.Windows.Forms.ToolStripButton m_saveToolStripButton;
        private System.Windows.Forms.ToolStripButton m_executeToolStripButton;
        private System.Windows.Forms.ToolStripButton m_resetToolStripButton;
        private System.Windows.Forms.ToolStripStatusLabel m_elapsedTimeStatusBarLabel;
        private System.Windows.Forms.Timer m_executionTimer;
        private System.Windows.Forms.ToolStripStatusLabel m_passedStatusBarLabel;
        private System.Windows.Forms.ToolStripStatusLabel m_failedStatusBarLabel;
        private System.Windows.Forms.ToolStripStatusLabel m_didnotexecuteStatusBarLabel;
        private System.Windows.Forms.ToolStripStatusLabel m_erroredStatusBarLabel;
        private System.Windows.Forms.ToolStripStatusLabel m_inconclusiveStatusBarLabel;
        private System.Windows.Forms.ToolStripStatusLabel m_totalAvailableStatusBarLabel;
        private System.Windows.Forms.NumericUpDown m_samplingNumericUpDown;
        private System.Windows.Forms.ToolStripButton m_collapseAllToolStripButton;
        private System.Windows.Forms.ToolStripButton m_testCasesOnlyToolStripButton;
        private System.Windows.Forms.ToolStripButton m_expandAllToolStripButton;
        private System.Windows.Forms.Label m_tagSelectorLabel;
        private Quintity.TestFramework.TestEngineer.CheckedComboBox  m_tagComboBox;
        private System.Windows.Forms.Label m_samplingRateLabel;
        private System.Windows.Forms.Button m_filterToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem m_toolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_testPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testListenersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripStatusLabel m_inprocessStatusBarLabel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton m_stepOverButton;
        private System.Windows.Forms.ToolStripButton m_stopToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem m_suiteStopExecuteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_suiteStepOverMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem m_suiteDeleteAllBreakpointsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_suiteDisableAllBreakpointsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_suiteToggleBreakpointMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton m_fileExplorerButton;
        private System.Windows.Forms.ToolStripMenuItem m_miFileExplorer;
        private System.Windows.Forms.ToolStripSeparator m_toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem m_testHomeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_testSuitesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_testConfigsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_testOutputMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_testResultsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_testDataMenuItem;
    }
}

