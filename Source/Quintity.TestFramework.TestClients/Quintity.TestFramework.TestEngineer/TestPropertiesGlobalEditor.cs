using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestEngineer
{
    public partial class TestPropertiesGlobalEditor : Form
    {
        #region Data members


        private const int OverriddenColumn = 0;
        private const int ActiveColumn = 1;
        private const int NameColumn = 2;
        private const int TypeColumn = 3;
        private const int ValueColumn = 4;
        private const int DescriptionColumn = 5;

        private const string UserType = "User";
        private const string SystemType = "System";

        private bool _initialLoading = false;

        private List<TestPropertyOverride> _testPropertyOverrides = null;

#pragma warning disable 0414
        private bool _hasChanged = false;
#pragma warning restore 0414

        private bool _addingRow = false;

        #endregion

        #region Constructor

        public TestPropertiesGlobalEditor(List<TestPropertyOverride> testPropertyOverrides)
        {
            _initialLoading = true;
            _testPropertyOverrides = testPropertyOverrides;

            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void TestPropertiesGlobalEditor_Load(object sender, EventArgs e)
        {
            loadDataGrid();

            m_applyOverridesChk.Enabled = m_applyOverridesChk.Checked = TestProperties.HasTestPropertyOverrides();
        }

        private void setCaption()
        {
            var fileName = string.IsNullOrEmpty(TestProperties.TestPropertiesFile) ? "Default" : Path.GetFileName(TestProperties.TestPropertiesFile);
            this.Text = $"Test Properites Editor - {fileName}";
        }

        private void TestPropertiesGlobalEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            TestPropertyCollection collection = TestProperties.TestPropertyCollection;
        }

        private void m_testPropertiesDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!_initialLoading && !_addingRow)
            {
                m_saveButton.Enabled = _hasChanged = true;

                DataGridViewRow row = m_testPropertiesDataGridView.Rows[e.RowIndex];
                var testProperty = row.Tag as TestProperty;

                if (e.ColumnIndex == ActiveColumn)
                {
                    testProperty.Active = (bool)row.Cells[ActiveColumn].Value;
                }
                else if (e.ColumnIndex == NameColumn)
                {
                    testProperty.Name = row.Cells[NameColumn].Value as string;
                }
                else if (e.ColumnIndex == ValueColumn)
                {
                    testProperty.Value = row.Cells[ValueColumn].Value as string;
                }
                else if (e.ColumnIndex == DescriptionColumn)
                {
                    testProperty.Description = row.Cells[DescriptionColumn].Value as string;
                }
            }
        }

        private void m_testPropertiesDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (m_testPropertiesDataGridView.Rows[e.RowIndex].Tag is TestSystemProperty)
            {
                m_testPropertiesDataGridView.AllowUserToDeleteRows = false;
            }
            else
            {
                m_testPropertiesDataGridView.AllowUserToDeleteRows = true;
            }
        }
        /// <summary>
        /// Gets new Save As file name via common dialog
        /// </summary>
        /// <returns></returns>
        private string getNewFileName()
        {
            SaveFileDialog m_saveFileDialog = new SaveFileDialog();

            m_saveFileDialog.Title = "Save Test Properties";
            m_saveFileDialog.InitialDirectory = TestProperties.ExpandString(TestProperties.GetPropertyValueAsString("TestProperties"));
            m_saveFileDialog.RestoreDirectory = true;
            m_saveFileDialog.Filter = "Test properties (*.props)|*.props";
            m_saveFileDialog.FilterIndex = 1;

            DialogResult result = m_saveFileDialog.ShowDialog();

            return m_saveFileDialog.FileName;
        }

        private void saveAsToFile()
        {
            string fileName = getNewFileName();

            if (!string.IsNullOrEmpty(fileName))
            {
                setTestPropertiesFileName(fileName);

                saveToFile();
            }
        }

        private void setTestPropertiesFileName(string fileName)
        {
            TestProperties.TestPropertiesFile = fileName;
            m_saveButton.Enabled = true;
        }

        private void saveToFile()
        {
            try
            {
                bool includeOverrides = false;

                if (TestProperties.HasTestPropertyOverrides())
                {
                    // Query to update config file with override values (they become the defaults.
                    includeOverrides = DialogResult.Yes == MessageBox.Show(this, "Some of the test properties reflect environmental overrides.  " +
                        "Do you want to update the configuration file to include the override values?", "Quintity TestFramework",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) ? true : false;
                }

                // Collect new properties and assign properties
                TestProperties.SetTestPropertyCollection(collectTestPropertiesFromGrid(includeOverrides));

                // Save properties to fie.
                TestProperties.Save();
                setCaption();
                m_saveButton.Enabled = false;
            }
            catch (TestPropertyEditorException e)
            {
                m_testPropertiesDataGridView.CurrentCell = m_testPropertiesDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                MessageBox.Show(this, e.Message,
                            this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                m_testPropertiesDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                m_testPropertiesDataGridView.BeginEdit(false);
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_saveAsButton_Click(object sender, EventArgs e)
        {
            saveAsToFile();
        }

        private void m_saveButton_Click(object sender, EventArgs e)
        {
            saveToFile();
        }

        private TestPropertyCollection collectTestPropertiesFromGrid(bool includeOverrides)
        {
            var testProperties = new TestPropertyCollection();

            foreach (DataGridViewRow row in m_testPropertiesDataGridView.Rows)
            {
                var testProperty = row.Tag as TestProperty;

                if (!testProperty.Overridden || includeOverrides)
                {
                    testProperties.Add(testProperty);
                }
                else if (testProperty.OverriddenValue != null)
                {
                    testProperty.Value = testProperty.OverriddenValue;
                    testProperty.Description = testProperty.OverriddenDescription;
                    testProperties.Add(testProperty);
                }
            }

            return testProperties;
        }

        private void m_cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogResult.No;

            if (string.IsNullOrEmpty(TestProperties.TestPropertiesFile))
            {
                dialogResult = MessageBox.Show(this, "The current test properties have not been saved, do you wish to save them to file? " +
                    "All changes will be lost upon exiting the application.", "Quintity TestFramework",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

                if (dialogResult== DialogResult.Yes)
                {
                    saveAsToFile();
                }
            }

            if (dialogResult != DialogResult.Cancel)
            {
                Close();
            }
        }

        private void m_testPropertiesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            m_removeStripButton.Enabled = (m_testPropertiesDataGridView.SelectedRows.Count > 0 &&
                !(m_testPropertiesDataGridView.SelectedRows[0].Tag is TestSystemProperty));

            m_moveUpStripButton.Enabled = enableMoveUpButton();
            m_moveDownStripButton.Enabled = enableMoveDownButton();
        }

        private void m_removeStripButton_Click(object sender, EventArgs e)
        {
            var row = m_testPropertiesDataGridView.SelectedRows[0];
            int index = row.Index;
            m_testPropertiesDataGridView.Rows.Remove(row);

            if (m_testPropertiesDataGridView.RowCount == index)
            {
                m_testPropertiesDataGridView.Rows[index - 1].Selected = true;
            }
            else
            {
                m_testPropertiesDataGridView.Rows[index].Selected = true;
            }
        }

        private void m_moveUpStripButton_Click(object sender, EventArgs e)
        {
            var row = m_testPropertiesDataGridView.SelectedRows[0];
            moveTestPropertyRow(row, row.Index - 1);

        }

        private void m_moveDownStripButton_Click(object sender, EventArgs e)
        {
            var row = m_testPropertiesDataGridView.SelectedRows[0];
            moveTestPropertyRow(row, row.Index + 1);
        }

        private void m_addToolStripButton1_Click(object sender, EventArgs e)
        {
            _addingRow = true;

            var testProperty = new TestProperty()
            {
                Active = true,
                Value = String.Empty,
            };

            var row = addTestPropertyRow(testProperty);

            m_testPropertiesDataGridView.CurrentCell = row.Cells[NameColumn];

            _hasChanged = true;

            _addingRow = false;
        }

        #endregion

        #region Private methods

        private string getFriendlyNameFromType(object value)
        {
            string friendly = "Unknown";

            if (value is string)
            {
                friendly = "String";
            }
            else if (value is int)
            {
                friendly = "Integer";
            }
            else if (value is bool)
            {
                friendly = "Boolean";
            }
            else if (value is DateTime)
            {
                friendly = "DateTime";
            }

            return friendly;
        }

        private Type getTypeFromFriendlyName(string friendlyName)
        {
            Type type = null;

            if (friendlyName.Equals("String"))
            {
                type = Type.GetType("SYSTEM.STRING");
            }
            else if (friendlyName.Equals("Integer"))
            {
                type = Type.GetType("System.Int32");
            }
            else if (friendlyName.Equals("Boolean"))
            {
                type = null;
            }
            else if (friendlyName.Equals("DateTime"))
            {
                type = null;
            }

            return type;
        }

        private void m_testListnerPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //if (e.ChangedItem.Label.Equals("Name"))
            //{
            //    string newName = e.ChangedItem.Value as string;
            //    TreeNode node = m_testListenersTreeView.SelectedNode;

            //    var testListener = node.Tag as TestListenerDescriptor;

            //    if (!string.IsNullOrEmpty(newName.Trim()))
            //    {
            //        node.Text = testListener.Name;
            //    }
            //    else
            //    {
            //        MessageBox.Show("The test listener's name cannot be a null or empty value.", "Quintity TestFramework", MessageBoxButtons.OK);
            //        testListener.Name = (string)(e.OldValue);
            //    }
            //}
        }

        private DataGridViewRow addTestPropertyRow(TestProperty testProperty)
        {
            int index = m_testPropertiesDataGridView.Rows.Add();
            DataGridViewRow row = m_testPropertiesDataGridView.Rows[index];
            row.Tag = testProperty;

            // If TestSystemProperty, need to makes some fields readonly.
            if (testProperty is TestSystemProperty)
            {
                row.Cells[ActiveColumn].ReadOnly = true;
                row.Cells[NameColumn].ReadOnly = true;
                row.Cells[TypeColumn].ReadOnly = true;
                row.Cells[TypeColumn].Value = SystemType;
                row.Cells[DescriptionColumn].ReadOnly = true;
            }
            else
            {
                row.Cells[TypeColumn].Value = UserType;
            }

            row.Cells[OverriddenColumn].Value = testProperty.Overridden ? "*" : string.Empty;
            row.Cells[OverriddenColumn].ToolTipText =
                testProperty.Overridden ? $"Environmental override:  {testProperty.TestPropertyOverride.Environment}" : string.Empty;

            row.Cells[ActiveColumn].Value = testProperty.Active;
            row.Cells[NameColumn].Value = testProperty.Name;
            row.Cells[TypeColumn].ReadOnly = true;

            row.Cells[ValueColumn].ValueType = testProperty.Value.GetType();
            row.Cells[ValueColumn].Value = testProperty.Value;
            row.Cells[ValueColumn].ToolTipText =
                testProperty.Overridden && testProperty.OverriddenValue != null ?
                $"Original value: \"{testProperty.OverriddenValue}\"" : (string)testProperty.Value;

            row.Cells[DescriptionColumn].Value = testProperty.Description;
            row.Cells[DescriptionColumn].ToolTipText =
               testProperty.Overridden && !string.IsNullOrEmpty(testProperty.OverriddenDescription) ?
               $"Original description: \"{testProperty.OverriddenDescription}\"" : testProperty.Description;

            return row;
        }

        private void moveTestPropertyRow(DataGridViewRow row, int targetIndex)
        {
            m_testPropertiesDataGridView.Rows.Remove(row);
            m_testPropertiesDataGridView.Rows.Insert(targetIndex, row);
            m_testPropertiesDataGridView.CurrentCell = null;
            row.Selected = true;
        }

        private bool enableMoveUpButton()
        {
            bool enable = false;

            if (m_testPropertiesDataGridView.SelectedRows.Count > 0 &&
                !(m_testPropertiesDataGridView.SelectedRows[0].Tag is TestSystemProperty))
            {
                var row = m_testPropertiesDataGridView.SelectedRows[0];

                // If preceeding row is a system property, can't move upwards
                if (!(m_testPropertiesDataGridView.Rows[row.Index - 1].Tag is TestSystemProperty))
                {
                    enable = true;
                }
            }

            return enable;
        }

        private bool enableMoveDownButton()
        {
            bool enable = false;

            if (m_testPropertiesDataGridView.SelectedRows.Count > 0 &&
                !(m_testPropertiesDataGridView.SelectedRows[0].Tag is TestSystemProperty))
            {
                // If it is the last row, can't move down.
                var row = m_testPropertiesDataGridView.SelectedRows[0];

                if (m_testPropertiesDataGridView.RowCount - 1 != row.Index)
                {
                    enable = true;
                }
            }

            return enable;
        }

        #endregion

        private void m_newButton_Click(object sender, EventArgs e)
        {
            // If has changed, prompt to save changes
            if (_hasChanged)
            {
                var result = MessageBox.Show(this, "The current test properties have changed, do you wish to save them to file?", "Quintity TestFramework",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

                if (result == DialogResult.Yes)
                {
                    if (TestProperties.TestPropertiesFile is null)
                    {
                        saveAsToFile();
                    }
                    else
                    {
                        saveToFile();
                    }

                    newTestProperties();
                }
                else if (result == DialogResult.No)
                {
                    newTestProperties();
                }
            }
            else
            {
                newTestProperties();
            }
        }

        private void newTestProperties(string filePath = null)
        {
            if (filePath is null)
            {
                TestProperties.Initialize();
            }
            else

            {
                TestProperties.Initialize(filePath);
            }

            TestProperties.ApplyTestPropertyOverrides(_testPropertyOverrides);

            _initialLoading = true;
            loadDataGrid();

            this.m_cancelButton.Text = "Close";

        }

        private void m_openButton_Click(object sender, EventArgs e)
        {
            // If has changed, prompt to save changes
            if (_hasChanged)
            {
                var result = MessageBox.Show(this, "The current test properties have changed, do you wish to save them to file?", "Quintity TestFramework",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

                if (result == DialogResult.Yes)
                {
                    if (TestProperties.TestPropertiesFile is null)
                    {
                        saveAsToFile();
                    }
                    else
                    {
                        saveToFile();
                    }

                    openTestProperitesFile();
                }
                else if (result == DialogResult.No)
                {
                    openTestProperitesFile();
                }
            }
            else
            {
                openTestProperitesFile();
            }
        }

        private void openTestProperitesFile()
        {
            var openFileDialog = new OpenFileDialog();

            try
            {
                openFileDialog.Title = "Quintity TestFramework - Load Test Properties";
                openFileDialog.InitialDirectory = TestProperties.ExpandString("[TestHome]\\TestProperties");
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "Test properties (*.props)|*.props|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.FileName = null;

                if (DialogResult.OK == openFileDialog.ShowDialog())
                {
                    newTestProperties(openFileDialog.FileName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this, $"An error has occurred loading file \"{openFileDialog.FileName}\":" +
                    $"{Environment.NewLine}{Environment.NewLine}{e.Message}{Environment.NewLine}{Environment.NewLine}" +
                    "Please select another file.", "Quintity TestFramework",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void loadDataGrid()
        {
            try
            {
                m_testPropertiesDataGridView.SuspendLayout();
                m_testPropertiesDataGridView.Invalidate();

                m_testPropertiesDataGridView.Rows.Clear();

                // Making copies of actual test property in case user cancels action
                // the property can be discarded.  If not, the current property contents
                // change (forcing update, even after cancel.  Save process only goes
                // against tagged copy properties.
                foreach (TestProperty testProperty in TestProperties.TestPropertyCollection)
                {
                    // If TestSystemProperty, need to makes some field readonly.
                    if (testProperty is TestSystemProperty)
                    {
                        addTestPropertyRow(new TestSystemProperty(testProperty as TestSystemProperty));
                    }
                    else
                    {
                        addTestPropertyRow(new TestProperty(testProperty as TestProperty));
                    }
                }

                // If test properties file not specifed, disable save button.
                m_saveButton.Enabled = !string.IsNullOrEmpty(TestProperties.TestPropertiesFile) && this._hasChanged;

                setCaption();
            }
            catch
            { }
            finally
            {
                m_testPropertiesDataGridView.ResumeLayout();
                _initialLoading = false;
                m_testPropertiesDataGridView.Update();
            }
        }

        private void m_applyOverridesChk_Click(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                TestProperties.ReapplyTestPropertyOverrides();
                loadDataGrid();
            }
            else
            {
                TestProperties.RemoveTestPropertyOverrides();
                loadDataGrid();
            }
        }
    }
}
