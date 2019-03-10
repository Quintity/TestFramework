using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Windows.Forms;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace Quintity.TestFramework.TestEngineer
{
    public partial class MainForm : Form
    {
        #region Data members

        private static readonly log4net.ILog LogEvent =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Tag combo box default entries
        const string m_noSelectionString = "No Selection";
        const string m_untaggedOnlyString = "Untagged Only";

        const string m_samplingRateString = "Sampling Rate (%):";
        const string m_samplingRateChangedString = "Sampling Rate (%):*";
        const string m_tagSelectorString = "Tag Selector:";
        const string m_tagSelectorChangedString = "Tag Selector:*";

        // Command line strings.
        private Uri m_testPropertiesUri;
        private Uri m_testListenersUri;
        private Uri m_testProfileUri;
        private Uri m_testSuiteUri;

        private bool m_updateFilterLabels;

        // Test properties members
        TestPropertiesGlobalEditor m_testPropertiesGlobalEditor = null;

        // Test listeners members
        TestListenersEditorDialog m_listenersEditorDlg;
        private bool _promptListenerReload = false;     // Flag to be used when loaded listeners have been updated.

        private bool _isExecuting = false;

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            m_tagSelectorLabel.Text = m_tagSelectorString;
            m_samplingRateLabel.Text = m_samplingRateString;

            m_passedStatusBarLabel.Tag = 0;
            m_didnotexecuteStatusBarLabel.Tag = 0;
            m_failedStatusBarLabel.Tag = 0;
            m_inconclusiveStatusBarLabel.Tag = 0;
            m_erroredStatusBarLabel.Tag = 0;
            m_totalAvailableStatusBarLabel.Tag = 0;
            m_inprocessStatusBarLabel.Tag = 0;

            m_executionTimer.Tick += m_executionTimer_Tick;

            // Some event handlers
            TestProperties.OnTestPropertiesFileChangedHandler += TestProperties_OnTestPropertiesFileChangedHandler;
            TestScriptObject.OnTestPropertyChanged += TestScriptObject_OnTestPropertyChanged;
            TestListenersEditorDialog.OnTestListenerFileChanged += TestListenersEditorDialog_OnTestListenerFileChanged;
            m_testTreeView.OnTestTreeNodeAdded += m_testTreeView_OnTestTreeNodeAdded;
            m_testTreeView.OnTestTreeNodeRemoved += m_testTreeView_OnTestTreeNodeRemoved;

            registerRuntimeEvents();

            m_testTreeView.CachedTestAssemblies = Properties.Settings.Default.TestAssemblies;
        }

        private void TestProperties_OnTestPropertiesFileChangedHandler(string newFileName)
        {
            if (!string.IsNullOrEmpty(newFileName))
            {
                m_testPropertiesStatusBarLabel.Text = Path.GetFileName(newFileName);
                m_testPropertiesStatusBarLabel.ToolTipText = newFileName;
            }
            else
            {
                m_testPropertiesStatusBarLabel.Text = "Unsaved test properties";
                m_testPropertiesStatusBarLabel.ToolTipText = "Default test properties";
            }
        }

        void m_listenersEditorDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_promptListenerReload)
            {
                _promptListenerReload = false;

                var result = MessageBox.Show(this,
                    string.Format("The currently loaded test listeners file has changed:\r\n\r\n\"{0}\".\r\n\r\nDo you wish to reload it?",
                    m_testListenersUri.LocalPath),
                    "Quintity TestEngineer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    TestListeners.ReadFromFile(m_testListenersUri.LocalPath);
                }
            }
        }

        void TestListenersEditorDialog_OnTestListenerFileChanged(TestListenersEditorDialog testListenersEditorDialog, TestListenerFileChangedEventArgs args)
        {
            var changed = new Uri(args.FilePath);

            if (m_testListenersUri != null && m_testListenersUri.Equals(changed))
            {
                _promptListenerReload = true;
            }
        }

        private void m_executionTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = m_stopWatch.Elapsed;

            m_elapsedTimeStatusBarLabel.Text = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);
        }

        #endregion

        #region Main form event handlers

        private void TestEngineer_Shown(object sender, EventArgs e)
        {
            ShowSplash(true);
            initializeTestProperties();
            initializeTestListeners();
            initializeTestSuite();
            initializeTestProfile();
            m_testTreeView.SuppressExecution = Program.SuppressExecution;

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Get/set debug file.
            var executablePath = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            TestBreakpoints.SerializationFile = executablePath.AbsolutePath.Replace(".exe", ".sts");

            if (File.Exists(TestBreakpoints.SerializationFile))
            {
                var breakpoints = TestBreakpoints.ReadFromFile();
            }
            /*
             *          In case current implementation of listener update monitors is not adequate.
             *          
                        FileSystemWatcher watcher = new FileSystemWatcher();
                        watcher.Path = @"C:\TestProjects\Quintity\Quintity.TestFramework\Quintity.TestFramework - Trunk\Quintity.TestFramework.Scratch\TestListeners";
                        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                            | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime;
                        watcher.Filter = "bub.config";
                        watcher.EnableRaisingEvents = true;
                        watcher.Changed += watcher_Changed;
                        watcher.Created += watcher_Created;
                        watcher.Deleted += watcher_Deleted;
             */
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool @continue = true;

            if (m_testTreeView.IsTestTreeNode)
            {
                if (m_testTreeView.HasChanged)
                {
                    @continue = promptToSave(m_testSuiteUri != null ? m_testSuiteUri.LocalPath : null);
                }

                if (m_testTreeView.CachedTestAssemblies.Count > 0)
                {
                    Properties.Settings.Default.TestAssemblies = m_testTreeView.CachedTestAssemblies;
                    Properties.Settings.Default.Save();
                }
            }

            if (!@continue)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Main menu event handlers

        private void m_suiteExecuteMenuItem_Click(object sender, EventArgs e)
        {
            executeTestSuite();
        }

        private void m_suiteResetMenuItem_Click(object sender, EventArgs e)
        {
            resetResults();
            resetViewerAndStatusBar();
        }

        private void m_fileNewMenuItem_Click(object sender, EventArgs e)
        {
            newTestSuite();
        }

        private void m_fileOpenMenuItem_Click(object sender, EventArgs e)
        {
            openTestSuite();
        }

        private void m_fileSaveMenuItem_Click(object sender, EventArgs e)
        {
            saveTestSuite();
        }

        private void m_fileSaveAsMenuItem_Click(object sender, EventArgs e)
        {
            saveAsTestSuite();
        }

        private void m_fileExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void m_editMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            m_editUndoMenuItem.Enabled = m_testTreeView.UndoAvailable();
            m_editRedoMenuItem.Enabled = m_testTreeView.RedoAvailable();
        }

        private void m_editUndoMenuItem_Click(object sender, EventArgs e)
        {
            m_testTreeView.Undo();
        }

        private void m_editRedoMenuItem_Click(object sender, EventArgs e)
        {
            m_testTreeView.Redo();
        }

        private void m_editCutMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void m_editCopyMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void m_editPasteMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void m_editDeleteMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Main toolbar event handlers

        private void m_executeToolStripButton_Click(object sender, EventArgs e)
        {
            if (!_isExecuting)
            {
                executeTestSuite();
            }
            else
            {
                m_testTreeView.ContinueExecution();
            }
        }

        private void m_newToolStripButton_Click(object sender, EventArgs e)
        {
            newTestSuite();
        }

        private void m_openToolStripButton_Click(object sender, EventArgs e)
        {
            openTestSuite();
        }

        private void m_saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveTestSuite();
        }

        private void m_resetToolStripButton_Click(object sender, EventArgs e)
        {
            resetResults();
        }

        private void resetResults()
        {
            this.m_testTreeView.ResetResults();
            resetViewerAndStatusBar();
            m_resetToolStripButton.Enabled = false;
        }

        #endregion

        #region Main status bar event handlers

        private void m_loadTestPropertiesMenuItem_Click(object sender, EventArgs e)
        {
            m_openFileDialog.Title = "Load Test Properties";
            m_openFileDialog.InitialDirectory = TestProperties.ExpandString("[TestHome]\\TestProperties");
            m_openFileDialog.RestoreDirectory = true;
            m_openFileDialog.Filter = "Test properties (*.props)|*.props|All files (*.*)|*.*";
            m_openFileDialog.FilterIndex = 1;
            m_openFileDialog.FileName = null;

            if (DialogResult.OK == m_openFileDialog.ShowDialog())
            {
                Uri testProperties = loadTestProperties(m_openFileDialog.FileName);

                if (testProperties != null)
                {
                    m_testPropertiesUri = testProperties;
                    m_testPropertiesStatusBarLabel.Text = Path.GetFileName(m_testPropertiesUri.LocalPath);
                }
            }
        }

        private void m_reloadTestPropertiesMenuItem_Click(object sender, EventArgs e)
        {
            if (m_testPropertiesUri != null)
            {
                Uri testPropertiesUri = loadTestProperties(m_testPropertiesUri.LocalPath);
            }
        }

        #endregion

        #region Other event handlers

        private void m_testTreeView_OnTestTreeNodeRemoved(TestTreeView testTreeView, TestTreeNode nodeRemoved)
        {
            m_saveToolStripButton.Enabled = true;
            m_fileSaveMenuItem.Enabled = true;
        }

        private void m_testTreeView_OnTestTreeNodeAdded(TestTreeView testTreeView, TestTreeNode nodeAdded)
        {
            m_saveToolStripButton.Enabled = true;
            m_fileSaveMenuItem.Enabled = true;
        }

        private void m_testTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is TestTreeNode)
            {
                TestTreeNode node = TestTreeNode.Convert(e.Node);

                this.m_testPropertyGrid.SelectedObject = node.TestScriptObject;

                // If suite is status is unavailable don't allow editing.
                m_testPropertyGrid.Enabled = node.TestScriptObject.Status != Status.Unavailable ? true : false;

                if (node.TestScriptResult != null)
                {
                    m_resultsViewer.Text = node.TestScriptResult.ToString();
                }
            }
        }

        private void TestScriptObject_OnTestPropertyChanged(TestScriptObject testScriptObject, TestPropertyChangedEventArgs args)
        {
            m_testPropertyGrid.Refresh();

            if (m_testTreeView.RootNode.HasChanged)
            {
                m_fileSaveAsMenuItem.Enabled = true;
                m_fileSaveMenuItem.Enabled = true;
                m_saveToolStripButton.Enabled = true;
            }

            if (args.Property.Equals("Tags"))
            {
                // TODO - Renable once tagging enabled.
                // resetTagComboBox(false);
            }
        }

        private void m_collapseAllToolStripButton_Click(object sender, EventArgs e)
        {
            m_testTreeView.CollapseAll();
        }

        private void m_testCasesOnlyToolStripButton_Click(object sender, EventArgs e)
        {
            m_testTreeView.ShowAllTestCases();
        }

        private void m_expandAllToolStripButton_Click(object sender, EventArgs e)
        {
            m_testTreeView.ExpandAll();
        }

        private void m_traceViewer_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void m_resultsViewer_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void m_unloadTestPropertiesMenuItem_Click(object sender, EventArgs e)
        {
            TestProperties.Initialize();
            m_testPropertiesStatusBarLabel.Text = "Default test properties";
        }

        private void m_tagComboBox_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            Type type = sender.GetType();
            var item = sender as CheckedComboBoxItem;

            List<string> checkedItems = TestUtils.ToList(m_tagComboBox.CheckBoxItems[0].Text);

            if (item.Tag != null && item.Tag.Equals(m_noSelectionString) && item.Checked)
            {
                // Uncheck all others
                uncheckAllTags();
            }
            else
            {
                m_tagComboBox.CheckBoxItems[1].CheckState = getCheckedTags().Count > 0 ? CheckState.Unchecked : CheckState.Checked;
            }

            if (m_updateFilterLabels)
            {
                m_tagSelectorLabel.Text = m_tagSelectorChangedString;
                m_updateFilterLabels = true;
                m_filterToolStripButton.Enabled = true;
            }
        }

        private void m_samplingNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("m_samplingNumericUpDown_ValueChanged");

            m_samplingRateLabel.Text = m_samplingRateChangedString;
            m_filterToolStripButton.Enabled = true;
        }

        private void m_filterToolStripButton_Click(object sender, EventArgs e)
        {
            updateFilteredTreeView();
        }

        private void updateFilteredTreeView()
        {
            TestFilter filter = new TestFilter()
            {
                Tags = getCheckedTags(),
                SamplingRate = (int)m_samplingNumericUpDown.Value
            };

            m_testTreeView.Filter(filter);

            // Reset filter controls text labels.
            m_samplingRateLabel.Text = m_samplingRateString;
            m_tagSelectorLabel.Text = m_tagSelectorString;

            // Disable filter button until subsequent filter content changes.
            m_filterToolStripButton.Enabled = false;
        }

        #endregion

        #region Public methods

        public void SelectGridItem(GridItem baseItem, string description)
        {
            if (baseItem != null)
            {
                if (baseItem.PropertyDescriptor != null &&
                    baseItem.PropertyDescriptor.Description.Equals(description))
                {
                    baseItem.Select();
                    return;
                }
                else
                {
                    foreach (GridItem currentGridItem in baseItem.GridItems)
                    {
                        SelectGridItem(currentGridItem, description);
                    }
                }
            }
        }

        #endregion

        #region Private methods

        private void initializeTestListeners()
        {
            try
            {
                if (!string.IsNullOrEmpty(Program.TestListenersFile))
                {
                    m_testListenersUri = new Uri(TestProperties.ExpandString(Program.TestListenersFile));
                    TestListeners.ReadFromFile(m_testListenersUri.LocalPath);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(
                    string.Format("Unable to locate test listeners file \"{0}\".\r\n\r\nPlease verify the file path.", Program.TestListenersFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (SerializationException)
            {
                MessageBox.Show(
                    string.Format("Unable to read test listeners \"{0}\".\r\n\r\nPlease verify the file is a valid test properties file.", Program.TestListenersFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (UriFormatException)
            {
                MessageBox.Show(
                    string.Format("Unable to read test listeners \"{0}\".\r\n\r\nPlease verify the file is a valid test properties file.", Program.TestListenersFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void initializeTestProfile()
        {
            try
            {
                if (!string.IsNullOrEmpty(Program.TestPerformanceFile))
                {
                    m_testProfileUri = new Uri(TestProperties.ExpandString(Program.TestPerformanceFile));
                    m_testTreeView.TestProfile = TestProfile.ReadFromFile(m_testProfileUri.LocalPath);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(
                    string.Format("Unable to locate test performance file \"{0}\".\r\n\r\nPlease verify the file path.", Program.TestPerformanceFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (SerializationException)
            {
                MessageBox.Show(
                    string.Format("Unable to read test performance \"{0}\".\r\n\r\nPlease verify the file is a valid test performance file.", Program.TestPerformanceFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (UriFormatException)
            {
                MessageBox.Show(
                    string.Format("Unable to read test performance \"{0}\".\r\n\r\nPlease verify the file is a valid test performance file.", Program.TestPerformanceFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private List<TestPropertyOverride> _testPropertyOverrides = null;

        private void initializeTestProperties()
        {
            if (!string.IsNullOrEmpty(Program.TestPropertiesFile))
            {
                Uri testPropertiesUri = loadTestProperties(Program.TestPropertiesFile);

                if (testPropertiesUri != null)
                {
                    m_testPropertiesUri = testPropertiesUri;
                    m_testPropertiesStatusBarLabel.Text = Path.GetFileName(m_testPropertiesUri.LocalPath);
                }
                else
                {
                    TestProperties.Initialize();
                    m_testPropertiesStatusBarLabel.Text = "Default test properties";
                }
            }
            else
            {
                TestProperties.Initialize();
                m_testPropertiesStatusBarLabel.Text = "Default test properties";
            }

            if (!string.IsNullOrEmpty(Program.TestEnvironments))
            {
                _testPropertyOverrides = TestProperties.GetTestProperityOverrides(Program.TestEnvironments);

                if (_testPropertyOverrides != null)
                {
                    var unused = TestProperties.ApplyTestPropertyOverrides(_testPropertyOverrides);

                    if (unused.Count > 0)
                    {
                        displayUnusedOverridesMessage(unused);
                    }
                }
                else
                {
                    MessageBox.Show(this, $"The test environment \"{Program.TestEnvironments }\" has been specified, " +
                        "however it was not located in the application configuration file.",
                        "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void displayUnusedOverridesMessage(List<TestPropertyOverride> unusedOverrides)
        {
            var sb = new StringBuilder();
            sb.AppendLine();

            foreach(var unusedOverride in unusedOverrides)
            {
                sb.AppendLine($"{unusedOverride.Name} ({unusedOverride.Environment})");
            }

            sb.AppendLine();

            MessageBox.Show(this, 
                $"The following test property overrides were not applied to the test properties collection:{Environment.NewLine}{sb.ToString()} " +
                $"In order to be applied the property must already be a member of the current collection.",
                "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private Uri loadTestProperties(string testPropertiesFile)
        {
            Uri testPropertiesUri = null;

            try
            {
                testPropertiesUri = new Uri(testPropertiesFile);
                TestProperties.Initialize(testPropertiesUri.LocalPath);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(
                    string.Format("Unable to locate test properties file \"{0}\".\r\n\r\nPlease verify the file path.", testPropertiesFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                testPropertiesUri = null;
            }
            catch (SerializationException)
            {
                MessageBox.Show(
                    string.Format("Unable to deserialize test properties \"{0}\".\r\n\r\nPlease verify the file is a valid test properties file.", testPropertiesFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                testPropertiesUri = null;
            }
            catch (UriFormatException)
            {
                MessageBox.Show(
                    string.Format("Unable to read test properties \"{0}\".\r\n\r\nPlease verify the file is a valid test properties file.", testPropertiesFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                testPropertiesUri = null;
            }

            return testPropertiesUri;
        }

        private void initializeTestSuite()
        {
            if (!string.IsNullOrEmpty(Program.TestSuiteFile))
            {
                if (null == loadTestSuite(TestProperties.ExpandString(Program.TestSuiteFile)))
                {
                    openTestSuite();
                }
            }
            else
            {
                newTestSuite();
            }
        }

        private TestSuite loadTestSuite(string testSuiteFile)
        {
            TestSuite testSuite = null;

            try
            {
                if (!string.IsNullOrEmpty(testSuiteFile))
                {
                    m_testSuiteUri = new Uri(testSuiteFile);
                    testSuite = TestSuite.ReadFromFile(m_testSuiteUri.LocalPath);

                    this.m_testTreeView.SetTestSuite(testSuite);

                    setLoadedSuiteUI(true);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(
                    string.Format("Unable to locate test suite file \"{0}\".\r\n\r\nPlease verify the file path.", testSuiteFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                m_testSuiteUri = null;
            }
            catch (SerializationException)
            {
                MessageBox.Show(
                    string.Format("Unable to read test suite \"{0}\".\r\n\r\nPlease verify the file is a valid test suite file.", testSuiteFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                m_testSuiteUri = null;
            }
            catch (UriFormatException)
            {
                MessageBox.Show(
                    string.Format("Unable to read test suite \"{0}\".\r\n\r\nPlease verify the file is a valid test suite file.", testSuiteFile),
                    "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                m_testSuiteUri = null;
            }

            return testSuite;
        }

        private void setLoadedSuiteUI(bool loaded)
        {
            setCaption();

            // TODO - Renable once tagging enabled.
            //resetTagComboBox(!loaded);

            m_editMenuItem.Enabled = loaded;
            m_suiteMenuItem.Enabled = loaded;
            m_executeToolStripButton.Enabled = loaded;
            m_testPropertyGrid.Enabled = loaded;
            m_collapseAllToolStripButton.Enabled = loaded;
            m_testCasesOnlyToolStripButton.Enabled = loaded;
            m_expandAllToolStripButton.Enabled = loaded;
            m_fileSaveAsMenuItem.Enabled = loaded;
        }

        private void newTestSuite()
        {
            bool @continue = true;

            // If root node is testtreenode and its contents have change, prompt to save.
            if (m_testTreeView.IsTestTreeNode && m_testTreeView.HasChanged)
            {
                @continue = promptToSave(m_testSuiteUri != null ? m_testSuiteUri.LocalPath : null);
            }

            // If prompt to save was not cancelled, continue to add new suite.
            if (@continue)
            {
                if (null != m_testTreeView.NewRootTestSuite())
                {
                    m_testTreeView.ExpandAll();
                    m_testSuiteUri = null;
                    setCaption();

                    // TODO - Renable once tagging enabled.
                    //resetTagComboBox(false);
                    m_testPropertyGrid.Enabled = true;
                    m_collapseAllToolStripButton.Enabled = true;
                    m_testCasesOnlyToolStripButton.Enabled = true;
                    m_expandAllToolStripButton.Enabled = true;
                }
            }
        }

        private void openTestSuite()
        {
            bool @continue = true;

            if (m_testTreeView.IsTestTreeNode && m_testTreeView.HasChanged)
            {
                @continue = promptToSave(m_testSuiteUri != null ? m_testSuiteUri.LocalPath : null);
            }

            if (@continue)
            {
                if (null != m_testTreeView.OpenExistingTestSuite(null, false))
                {
                    setLoadedSuiteUI(true);
                }
            }
        }

        private bool saveTestSuite()
        {
            bool success = true;

            var testSuite = m_testTreeView.GetTestSuite();

            if (string.IsNullOrEmpty(testSuite.FileName))
            {
                saveAsTestSuite();
            }
            else
            {
                success = saveTestSuite(testSuite);
            }

            return success;
        }

        private bool saveTestSuite(TestSuite testSuite)
        {
            bool @continue = true;

            DialogResult result;

            try
            {
                testSuite.FileSave();
                m_testTreeView.ResetHasChangedFlags();

                //m_fileSaveAsMenuItem.Enabled = false;
                m_fileSaveMenuItem.Enabled = false;
                m_saveToolStripButton.Enabled = false;

                setCaption();
                m_testPropertyGrid.SelectedObject = m_testTreeView.RootNode.TestScriptObject;
            }
            catch (System.UnauthorizedAccessException)
            {
                result = MessageBox.Show(
                    string.Format("Unable to save to file \"{0}\".  Verify the file is not marked readonly.\r\n\r\n" +
                    "Do you wish to save the file under a different name?",
                    testSuite.FilePath),
                    "Quintity TestEngineer", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (result == DialogResult.Yes)
                {
                    saveAsTestSuite();
                }

                @continue = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                @continue = false;
            }

            return @continue;
        }

        public void saveAsTestSuite()
        {
            try
            {
                var testSuite = m_testTreeView.GetTestSuite();

                m_saveFileDialog.Title = "Save Test Suite As";
                m_saveFileDialog.FileName = string.IsNullOrEmpty(testSuite.FileName) ? string.Empty : $"Copy of {testSuite.Title}";
                m_saveFileDialog.InitialDirectory = TestProperties.TestSuites;
                m_saveFileDialog.RestoreDirectory = true;
                m_saveFileDialog.Filter = "Test suites (*.ste)|*.ste";
                m_saveFileDialog.FilterIndex = 1;

                DialogResult result = m_saveFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    testSuite.FileSaveAs(m_saveFileDialog.FileName);

                    //saveTestSuite(testSuite);
                    m_testTreeView.RootNode.TestScriptObject = testSuite;
                    m_testTreeView.RootNode.TestScriptResult = null;

                    // Clean up UI
                    m_testSuiteUri = new Uri(TestProperties.ExpandString(testSuite.FilePath));
                    m_testTreeView.ResetHasChangedFlags();

                    m_fileSaveMenuItem.Enabled = false;
                    //m_fileSaveAsMenuItem.Enabled = false;
                    m_saveToolStripButton.Enabled = false;

                    m_testPropertyGrid.SelectedObject = m_testTreeView.RootNode.TestScriptObject;

                    setCaption();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool promptToSave(string testSuiteFile)
        {
            bool @continue = true;

            DialogResult dlgResult = MessageBox.Show(
                string.Format("The test suite \"{0}\" has unsaved changes, do you wish to save them?",
                        m_testTreeView.RootNode.TestScriptObjectAsTestSuite().Title),
                "Quintity TestEngineer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (dlgResult == DialogResult.Yes)
            {
                //@continue = saveTestSuite(testSuiteFile);
                @continue = saveTestSuite();
            }
            else if (dlgResult == DialogResult.Cancel)
            {
                @continue = false;
            }

            return @continue;
        }

        private void executeTestSuite()
        {
            m_testTreeView.Execute();
        }

        private void stopTestSuite()
        {
            m_testTreeView.StopExecution();
        }

        private void setCaption()
        {
            var qtf = "Quintity TestEngineer";

            var testSuite = m_testTreeView.RootNode.TestScriptObjectAsTestSuite();

            if (string.IsNullOrEmpty(testSuite.FilePath))
            {
                Text = qtf;
            }
            else
            {
                Text = $"{Path.GetFileName(testSuite.FilePath)}  - {qtf}";
            }
        }

        private void resetViewerAndStatusBar()
        {
            m_traceViewer.Clear();
            m_resultsViewer.Clear();

            // Reset status bar labels.
            m_elapsedTimeStatusBarLabel.Text = "00:00:00";
            m_inprocessStatusBarLabel.Text = "0";
            m_inprocessStatusBarLabel.Tag = 0;
            m_passedStatusBarLabel.Text = "0";
            m_passedStatusBarLabel.Tag = 0;
            m_didnotexecuteStatusBarLabel.Text = "0";
            m_didnotexecuteStatusBarLabel.Tag = 0;
            m_failedStatusBarLabel.Text = "0";
            m_failedStatusBarLabel.Tag = 0;
            m_inconclusiveStatusBarLabel.Text = "0";
            m_inconclusiveStatusBarLabel.Tag = 0;
            m_erroredStatusBarLabel.Text = "0";
            m_erroredStatusBarLabel.Tag = 0;
            m_totalAvailableStatusBarLabel.Text = "0";
            m_totalAvailableStatusBarLabel.Tag = 0;

            updateStatusBarTooltips();
        }

        private void resetTagComboBox(bool updateLabels)
        {
            m_updateFilterLabels = updateLabels;

            List<string> tags = m_testTreeView.GatherAllTags();
            m_tagComboBox.Items.Clear();
            int index = m_tagComboBox.Items.Add(m_noSelectionString);
            m_tagComboBox.CheckBoxItems[index + 1].Tag = m_noSelectionString;  //Index is idiosyncrasy of custom combobox

            m_tagComboBox.Items.AddRange(tags.ToArray());

            bool untagged = tags.FindIndex(x => x == null) != -1 ? true : false;

            if (tags.Count > 0)
            {
                m_tagComboBox.Items.Add(m_untaggedOnlyString);
            }

            m_tagComboBox.CheckBoxItems[1].Checked = true;

            m_tagSelectorLabel.Text = m_tagSelectorString;
            m_samplingRateLabel.Text = m_samplingRateString;
            m_filterToolStripButton.Enabled = false;
            m_updateFilterLabels = true;
        }

        private List<string> getCheckedTags()
        {
            List<string> checkedTags = new List<string>();

            foreach (CheckedComboBoxItem item in m_tagComboBox.CheckBoxItems)
            {
                if (item.Tag == null)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        if (item.Text.Equals(m_untaggedOnlyString))
                        {
                            checkedTags.Add(null);
                        }
                        else
                        {
                            checkedTags.Add(item.Text);
                        }
                    }
                }
            }

            return checkedTags;
        }

        private void uncheckAllTags()
        {
            foreach (CheckedComboBoxItem item in m_tagComboBox.CheckBoxItems)
            {
                if (!item.Text.Equals(m_noSelectionString))
                {
                    item.CheckState = CheckState.Unchecked;
                }
            }
        }

        private void ShowSplash(bool timer)
        {
            SplashDialog splash = new SplashDialog(timer);
            splash.Owner = this;
            splash.Location = new Point(Location.X + Width / 2 - splash.Width / 2, Location.Y + Height / 2 - splash.Height / 2);
            splash.StartPosition = FormStartPosition.Manual;
            splash.Show();
        }

        #endregion

        private void m_testPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_testPropertiesGlobalEditor = new TestPropertiesGlobalEditor(_testPropertyOverrides);
            m_testPropertiesGlobalEditor.FormClosed += Editor_FormClosed;
            m_testPropertiesGlobalEditor.Location = 
                new Point(Location.X + Width / 2, Location.Y);
                        m_testPropertiesGlobalEditor.StartPosition = FormStartPosition.Manual;
            m_testPropertiesGlobalEditor.Show(this);
            m_testPropertiesToolStripMenuItem.Enabled = false;
        }

        private void Editor_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_testPropertiesToolStripMenuItem.Enabled = true;
        }

        private void testListenersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_listenersEditorDlg = new TestListenersEditorDialog(Program.TestListenersFile);
            m_listenersEditorDlg.Text = "Test Listeners Editor";
            m_listenersEditorDlg.FormClosed += m_listenersEditorDlg_FormClosed;
            m_listenersEditorDlg.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSplash(false);
        }

        private void m_stepOverButton_Click(object sender, EventArgs e)
        {
            this.m_testTreeView.StepOverBreakPoint();
        }

        private void m_stopToolStripButton_Click(object sender, EventArgs e)
        {
            stopTestSuite();
        }

        private void m_suiteStopExecuteMenuItem_Click(object sender, EventArgs e)
        {
            stopTestSuite();
        }

        private void m_suiteStepOverMenuItem_Click(object sender, EventArgs e)
        {
            this.m_testTreeView.StepOverBreakPoint();
        }

        private void m_suiteDeleteAllBreakpointsMenuItem_Click(object sender, EventArgs e)
        {
            this.m_testTreeView.DeleteAllBreakpoints();
        }

        private void m_suiteToggleBreakpointMenuItem_Click(object sender, EventArgs e)
        {
            this.m_testTreeView.ToggleBreakpoint();
        }

        private void m_suiteMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.m_suiteResetMenuItem.Enabled = this.m_testTreeView.HasTestScriptResults();

            this.m_suiteStepOverMenuItem.Enabled = this.m_testTreeView.IsBreakpointMode();

            this.m_suiteDeleteAllBreakpointsMenuItem.Enabled =
                this.m_suiteDisableAllBreakpointsMenuItem.Enabled = m_testTreeView.HasBreakpoints();
        }

        //void watcher_Deleted(object sender, FileSystemEventArgs e)
        //{
        //    int i = 1;
        //}

        //void watcher_Created(object sender, FileSystemEventArgs e)
        //{
        //    int i = 1;
        //}

        //void watcher_Changed(object sender, FileSystemEventArgs e)
        //{
        //    int i = 1;
        //}
    }
}
