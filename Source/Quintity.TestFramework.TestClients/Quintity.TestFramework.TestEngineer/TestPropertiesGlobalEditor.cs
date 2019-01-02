using System;
using System.Linq;
using System.Windows.Forms;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestEngineer
{
    public partial class TestPropertiesGlobalEditor : Form
    {
        #region Data members

        private const int ActiveColumn = 0;
        private const int NameColumn = 1;
        private const int TypeColumn = 2;
        private const int ValueColumn = 3;
        private const int DescriptionColumn = 4;

        private const string UserType = "User";
        private const string SystemType = "System";

        private bool _initialLoading = false;

        #pragma warning disable 0414
        private bool _hasChanged = false;
        #pragma warning restore 0414

        private bool _addingRow = false;

        #endregion

        #region Constructor

        public TestPropertiesGlobalEditor()
        {
            _initialLoading = true;

            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void TestPropertiesGlobalEditor_Load(object sender, EventArgs e)
        {
            try
            {
                m_testPropertiesDataGridView.SuspendLayout();

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
                m_saveButton.Enabled = !string.IsNullOrEmpty(TestProperties.TestPropertiesFile);
            }
            catch
            { }
            finally
            {
                m_testPropertiesDataGridView.ResumeLayout();
                _initialLoading = false;
            }
        }

        private void TestPropertiesGlobalEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            TestPropertyCollection collection = TestProperties.TestPropertyCollection;
        }

        private void m_testPropertiesDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!_initialLoading && !_addingRow)
            {
                _hasChanged = true;

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

        private string getFileName()
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

        private void setTestPropertiesFileName(string fileName)
        {
            TestProperties.TestPropertiesFile = fileName;
            m_saveButton.Enabled = true;
        }

        private void saveAsToFile()
        {
            string fileName = getFileName();

            if (!string.IsNullOrEmpty(fileName))
            {
                setTestPropertiesFileName(fileName);

                saveToFile();
            }
        }

        private void saveToFile()
        {
            try
            {
                // Collect new properties and assign properties
                TestProperties.SetTestPropertyCollection(collectTestProperties());

                // Save properties to fie.
                TestProperties.Save();
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
            finally
            {
                Close();
            }
        }



        private void m_saveAsButton_Click(object sender, EventArgs e)
        {
            saveAsToFile();
        }

        //private void saveAs()
        //{
        //    string fileName = getFileName();

        //    if (!string.IsNullOrEmpty(fileName))
        //    {
        //        TestProperties.TestPropertiesFile = fileName;

        //        saveToFile();
        //    }
        //}

        //private void save()
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(TestProperties.TestPropertiesFile))
        //        {
        //            saveAs();
        //        }
        //        else
        //        {
        //            TestProperties.SetTestPropertyCollection(collectTestProperties());
        //            this.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message, "Save Test Properties", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //}

        //private void saveAs()
        //{
        //    try
        //    {
        //        SaveFileDialog m_saveFileDialog = new SaveFileDialog();

        //        m_saveFileDialog.Title = "Save Test Properties";
        //        m_saveFileDialog.InitialDirectory = TestProperties.ExpandPropertyValue(TestProperties.GetPropertyValueAsString("TestProperties"));
        //        m_saveFileDialog.RestoreDirectory = true;
        //        m_saveFileDialog.Filter = "Test properties (*.props)|*.props";
        //        m_saveFileDialog.FilterIndex = 1;

        //        DialogResult result = m_saveFileDialog.ShowDialog();

        //        if (result == DialogResult.OK)
        //        {
        //            TestProperties.SetTestPropertyCollection(collectTestProperties());
        //            TestProperties.TestPropertiesFile = m_saveFileDialog.FileName;
        //            TestProperties.Save();
        //        }

        //        this.Close();
        //    }
        //    catch (TestPropertyEditorException exp)
        //    {
        //        m_testPropertiesDataGridView.CurrentCell = m_testPropertiesDataGridView.Rows[exp.RowIndex].Cells[exp.ColumnIndex];

        //        MessageBox.Show(this, exp.Message,
        //                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        //        m_testPropertiesDataGridView.Rows[exp.RowIndex].Cells[exp.ColumnIndex].Selected = true;
        //        m_testPropertiesDataGridView.BeginEdit(false);
        //    } 
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message, "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void m_saveButton_Click(object sender, EventArgs e)
        {
            saveToFile();
            //try
            //{
            //    TestProperties.SetTestPropertyCollection(collectTestProperties());
            //    this.Close();
            //}
            //catch (TestPropertyEditorException exp)
            //{
            //    m_testPropertiesDataGridView.CurrentCell = m_testPropertiesDataGridView.Rows[exp.RowIndex].Cells[exp.ColumnIndex];

            //    MessageBox.Show(this, exp.Message,
            //                this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //    m_testPropertiesDataGridView.Rows[exp.RowIndex].Cells[exp.ColumnIndex].Selected = true;
            //    m_testPropertiesDataGridView.BeginEdit(false);
            //}
        }

        private TestPropertyCollection collectTestProperties()
        {
            var testProperties = new TestPropertyCollection();

            foreach (DataGridViewRow row in m_testPropertiesDataGridView.Rows)
            {
                var testProperty = row.Tag as TestProperty;

                if (!string.IsNullOrEmpty(testProperty.Name))
                {
                    testProperties.Add(testProperty);
                }
                else
                {
                    throw new TestPropertyEditorException(row.Index, NameColumn, "A test property name must be specified.");
                }
            }

            return testProperties;
        }

        private void m_cancelButton_Click(object sender, EventArgs e)
        {
            Close();
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

            row.Cells[ActiveColumn].Value = testProperty.Active;
            row.Cells[NameColumn].Value = testProperty.Name;
            row.Cells[TypeColumn].ReadOnly = true;
            row.Cells[ValueColumn].ValueType = testProperty.Value.GetType();
            row.Cells[ValueColumn].Value = testProperty.Value;
            row.Cells[DescriptionColumn].Value = testProperty.Description;

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
    }
}
