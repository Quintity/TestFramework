using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.Core
{
    public partial class TestListenersEditorDialog : Form
    {
        #region Data members

        private const string _dialogText = "Test Listeners Editor";
        private const string _newName = "Unnamed listener";
        private bool _changed = false;
        private string _filePath = null;
        private TestListenerCollection _testListeners;

        #endregion

        #region Class events

        public delegate void TestListenerFileChangedEvent(TestListenersEditorDialog testListenersEditorDialog, TestListenerFileChangedEventArgs args);
        public static event TestListenerFileChangedEvent OnTestListenerFileChanged;

        private void FireTestListenerFileChangedEvent(TestListenerFileChangedEventArgs args)
        {
            if (OnTestListenerFileChanged != null)
            {
                OnTestListenerFileChanged(this, args);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the test listeners editor dialog.
        /// </summary>
        /// <param name="filePath">File path of test listener's file.</param>
        public TestListenersEditorDialog(string filePath)
        {
            _filePath = filePath is null ? filePath : TestProperties.ExpandString(filePath);

            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void TestListenersEditorDialog_Load(object sender, EventArgs e)
        {
            setLabelColumnWidth(125);

            if (!string.IsNullOrEmpty(_filePath))
            {
                this.openFile(_filePath);
            }
            else
            {
                newFile();
            }
        }

        private void TestListenersEditorDialog_Shown(object sender, EventArgs e)
        {
            //setLabelColumnWidth(125);
            //newFile();
        }

        private void m_addToolStripButton_Click(object sender, EventArgs e)
        {
            var node = m_testListenersTreeView.SelectedNode;

            if (node != null && node.Tag != null)
            {
                var newNode = new TreeNode("New Listener");

                newNode.Tag = new TestListenerDescriptor()
                {
                    Name = "New Listener"
                };

                m_testListenersTreeView.Nodes[0].Nodes.Insert(node.Index, newNode);
                m_testListenersTreeView.SelectedNode = newNode;
            }
        }

        private void m_removeToolStripButton_Click(object sender, EventArgs e)
        {
            var node = m_testListenersTreeView.SelectedNode;

            if (node != null && node.Tag != null)
            {
                node.Remove();
            }
        }

        private void m_testListenersTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            m_testListenerPropertyGrid.SelectedObject = e.Node.Tag;
            m_testListenerPropertyGrid.Tag = e.Node;
            m_removeToolStripButton.Enabled = e.Node.Tag != null;
        }

        private void m_testListenerPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // If the name changes, need to update the name displayed in treeview.
            if (e.ChangedItem.PropertyDescriptor.Name == "Name")
            {
                TreeNode node = m_testListenerPropertyGrid.Tag as TreeNode;
                node.Text = e.ChangedItem.Value as string;
            }

            if (e.ChangedItem.PropertyDescriptor.Name == "Assembly")
            {
                var assembly = e.ChangedItem.Value as string;

                if (!verifyAssembly(e.ChangedItem.Value as string))
                {
                    resetTypeValue();
                    setTypeToReadOnly(true);

                    DisplayErrorMessage(
                        string.Format("Unable to listener assembly specified, please update the selection.",
                        assembly));
                }
                else
                {
                    setTypeToReadOnly(false);
                }
            }

            if (!_changed)
            {
                if (e.ChangedItem.PropertyDescriptor.Name == "Name" ||
                    e.ChangedItem.PropertyDescriptor.Name == "Description" ||
                    e.ChangedItem.PropertyDescriptor.Name == "Type")
                {
                    _changed = (string)(e.ChangedItem.Value) != (string)(e.OldValue) ? true : _changed;

                }
                else if (e.ChangedItem.PropertyDescriptor.Name == "Status")
                {
                    _changed = (Status)(e.ChangedItem.Value) != (Status)(e.OldValue) ? true : _changed;
                }
                else if (e.ChangedItem.PropertyDescriptor.Name == "OnFailure")
                {
                    _changed = (OnFailure)(e.ChangedItem.Value) != (OnFailure)(e.OldValue) ? true : _changed;
                }
                else if (e.ChangedItem.PropertyDescriptor.Name == "Assembly")
                {
                    string oldValue = e.OldValue as string;
                    string newValue = e.ChangedItem.Value as string;

                    if (!string.IsNullOrEmpty(oldValue) && !string.IsNullOrEmpty(newValue))
                    {
                        // Check to see if not same listener assembly.
                        var newAssembly = new Uri(e.ChangedItem.Value as string);
                        var oldAssembly = new Uri(e.OldValue as string);

                        if (!newAssembly.Equals(oldAssembly))
                        {
                            _changed = true;
                            ((TestListenerDescriptor)(m_testListenerPropertyGrid.SelectedObject)).Type = null;
                        }
                    }
                    else if (oldValue != newValue)
                    {
                        _changed = true;
                        ((TestListenerDescriptor)(m_testListenerPropertyGrid.SelectedObject)).Type = null;
                    }
                }

                if (_changed)
                {
                    markAsChanged();
                }
            }
        }

        private void m_testListenerPropertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
        {
            var descriptor = m_testListenerPropertyGrid.SelectedObject as TestListenerDescriptor;
            oldParameters = new Dictionary<string, string>(descriptor.Parameters);

            if (descriptor != null)
            {
                setTypeToReadOnly(!verifyAssembly(descriptor.Assembly));
            }
        }

        private void m_addToolStripButton_Click_1(object sender, EventArgs e)
        {
            // Create new descriptor
            var descriptor = new TestListenerDescriptor()
            {
                Name = _newName
            };

            // Add to collection
            _testListeners.Add(descriptor);

            // Add to tree view.
            var node = new TreeNode(descriptor.Name);
            node.Tag = descriptor;
            m_testListenersTreeView.Nodes[0].Nodes.Add(node);
            m_testListenersTreeView.SelectedNode = node;

            // Mark file as dirty.
            markAsChanged();
        }

        private void m_removeToolStripButton_Click_1(object sender, EventArgs e)
        {
            var descriptor = m_testListenersTreeView.SelectedNode.Tag as TestListenerDescriptor;
            _testListeners.Remove(descriptor);
            m_testListenersTreeView.Nodes.Remove(m_testListenersTreeView.SelectedNode);

            markAsChanged();
        }

        private void m_newBtn_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void m_openBtn_Click(object sender, EventArgs e)
        {
            openFile(null);
        }

        private void m_saveAsBtn_Click(object sender, EventArgs e)
        {
            saveAsFile();
        }

        private void m_saveBtn_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void m_cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.No;

            if (_changed)
            {
                result = promptToSave();
            }

            if (result != DialogResult.Cancel)
            {
                if (result == DialogResult.Yes)
                {
                    saveFile();
                }

                this.Close();
            }
        }

        #endregion

        #region Private methods

        private void newFile()
        {
            try
            {
                DialogResult result = DialogResult.No;

                if (_changed)
                {
                    // Prompt to save if currently loaded has changed.
                    result = promptToSave();
                }

                if (result != DialogResult.Cancel)
                {
                    if (result == DialogResult.Yes)
                    {
                        saveFile();
                    }

                    // Create new listener collection
                    _testListeners = new TestListenerCollection();

                    // Create new descriptor, add to collection and initialize tree view.
                    var descriptor = new TestListenerDescriptor()
                    {
                        Name = _newName
                    };

                    _testListeners.Add(descriptor);
                    displayListenerCollection(_testListeners);

                    markAsUnchanged();

                    // Override Save button disable
                    this.m_saveBtn.Enabled = true;
                }
            }
            catch (Exception e)
            {
                DisplayErrorMessage(e.Message);
            }
        }

        private void saveFile()
        {
            if (_filePath == null)
            {
                saveAsFile();
            }
            else
            {
                save();
            }
        }

        private void openFile(string filePath)
        {
            try
            {
                DialogResult result = DialogResult.No;

                if (_changed)
                {
                    // Prompt to save if currently loaded has changed.
                    result = promptToSave();
                }

                if (result != DialogResult.Cancel)
                {
                    if (result == DialogResult.Yes)
                    {
                        saveFile();
                    }

                    if (string.IsNullOrEmpty(filePath))
                    {
                        m_openFileDialog.InitialDirectory = TestProperties.ExpandString(TestProperties.TestConfigs);
                        result = this.m_openFileDialog.ShowDialog(this);
                        _filePath = m_openFileDialog.FileName;
                    }
                    else
                    {
                        result = DialogResult.OK;
                    }

                    if (result == DialogResult.OK)
                    {
                        var testListeners = TestListenerCollection.DeserializeFromFile(_filePath);
                        displayListenerCollection(testListeners);
                        _testListeners = testListeners;
                        m_testListenersTreeView.Nodes[0].ToolTipText = _filePath;
                        markAsUnchanged();
                    }
                }
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                DisplayErrorMessage(
                    string.Format("Unable to access file:\r\n\r\n\"{0}\".\r\n\r\nPlease verify it is valid test listener configuration file.",
                    m_openFileDialog.FileName));
            }
            catch (Exception exp)
            {
                DisplayErrorMessage(exp.Message);
            }
        }

        private void saveAsFile()
        {
            try
            {
                m_saveFileDialog.InitialDirectory = TestProperties.TestConfigs;
                DialogResult result = m_saveFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    _filePath = m_saveFileDialog.FileName;
                    save();
                }
            }
            catch (Exception e)
            {
                DisplayErrorMessage(e.Message);
            }
        }

        private void save()
        {
            try
            {
                TestListenerCollection.SerializeToFile(this._testListeners, _filePath);
                m_cancelButton.Text = "&Close";
                m_testListenersTreeView.Nodes[0].ToolTipText = _filePath;
                markAsUnchanged();

                var args = new TestListenerFileChangedEventArgs(_filePath);
                FireTestListenerFileChangedEvent(args);
            }
            catch (Exception e)
            {
                DisplayErrorMessage(e.Message);
            }
        }

        private void markAsChanged()
        {
            _changed = true;
            m_saveBtn.Enabled = true;
            Text = constructCaption();
        }

        private void markAsUnchanged()
        {
            _changed = false;
            m_saveBtn.Enabled = false;
            Text = constructCaption();
        }

        private DialogResult promptToSave()
        {
            return MessageBox.Show(this, "The current test listener file has changed, do you wish save it?",
                "Test Listener Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        private void displayListenerCollection(TestListenerCollection listenerCollection)
        {
            this.m_testListenersTreeView.Nodes.Clear();
            this.m_testListenersTreeView.Nodes.Add("Test Listeners");

            foreach (TestListenerDescriptor descriptor in listenerCollection)
            {
                var node = new TreeNode(descriptor.Name);
                node.Tag = descriptor;
                m_testListenersTreeView.Nodes[0].Nodes.Add(node);
            }

            if (listenerCollection.Count > 0)
            {
                this.m_testListenersTreeView.SelectedNode = m_testListenersTreeView.Nodes[0].Nodes[0];
            }
            else
            {
                this.m_testListenersTreeView.SelectedNode = m_testListenersTreeView.Nodes[0];
            }
        }

        private void setLabelColumnWidth(int width)
        {
            FieldInfo fieldInfo = m_testListenerPropertyGrid.GetType().GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);

            if (fieldInfo == null)
                return;

            Control view = fieldInfo.GetValue(m_testListenerPropertyGrid) as Control;
            if (view == null)
                return;

            MethodInfo mi = view.GetType().GetMethod("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic);
            if (mi == null)
                return;

            mi.Invoke(view, new object[] { width });
        }

        internal static DialogResult DisplayErrorMessage(string message)
        {
            return MessageBox.Show(message, "Test Listener Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private string constructCaption()
        {
            string fileName = null;

            if (_filePath != null)
            {
                var uri = new Uri(_filePath);
                fileName = uri.Segments[uri.Segments.Length - 1];

            }

            return string.Format("{0}{1}{2}",
                _dialogText,
                fileName != null ? " - " + fileName : "",
                _changed ? "*" : "");
        }

        private bool verifyAssembly(string filePath)
        {
            bool valid = false;

            if (!string.IsNullOrEmpty(filePath))
            {
                valid = File.Exists(filePath);
            }

            return valid;
        }

        private void setTypeToReadOnly(bool readOnly)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(TestListenerDescriptor));
            PropertyDescriptor prop = props["Type"];
            ReadOnlyAttribute attrib = (ReadOnlyAttribute)prop.Attributes[typeof(ReadOnlyAttribute)];
            FieldInfo isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
            isReadOnly.SetValue(attrib, readOnly);
        }

        private void resetTypeValue()
        {
            getSelectedListener().Type = null;
        }

        private TestListenerDescriptor getSelectedListener()
        {
            return m_testListenerPropertyGrid.SelectedObject as TestListenerDescriptor;
        }

        #endregion

        Dictionary<string, string> oldParameters = null;

        private void m_testListenerPropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (e.OldSelection != null && e.OldSelection.PropertyDescriptor.Name.Equals("Parameters"))
            {
                if (parametersChanged())
                {
                    markAsChanged();
                    oldParameters = new Dictionary<string, string>(getSelectedListener().Parameters);
                }
            }
        }

        private bool parametersChanged()
        {
            bool changed = false;

            var newParameters = getSelectedListener().Parameters;

            if (newParameters.Count == oldParameters.Count)
            {
                foreach (var oldParameter in oldParameters)
                {
                    string @value;

                    if (newParameters.TryGetValue(oldParameter.Key, out value))
                    {
                        if (!oldParameter.Value.Equals(value))
                        {
                            changed = true;
                            break;
                        };
                    }
                    else
                    {
                        changed = true;
                        break;
                    }
                }
            }
            else
            {
                changed = true;
            }

            return changed;
        }

        private void m_testListenerPropertyGrid_Leave(object sender, EventArgs e)
        {
            if (parametersChanged())
            {
                markAsChanged();
                oldParameters = new Dictionary<string, string>(getSelectedListener().Parameters);
            }
        }
    }
}
