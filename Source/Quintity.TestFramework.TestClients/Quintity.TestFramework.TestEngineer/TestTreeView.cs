using System;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;
using System.Linq;
using System.Drawing.Imaging;

namespace Quintity.TestFramework.TestEngineer
{
    /// <summary>
    /// For label edit code:
    /// http://www.codeproject.com/Articles/11931/Enhancing-TreeView-Customizing-LabelEdit
    /// </summary>
    public partial class TestTreeView : TreeView
    {
        #region Delegates and events

        public delegate void TestTreeNodeAdded(TestTreeView testTreeView, TestTreeNode nodeAdded);
        public event TestTreeNodeAdded OnTestTreeNodeAdded;

        internal void fireTestTreeNodeAddedEvent(TestTreeNode nodeAdded)
        {
            OnTestTreeNodeAdded?.Invoke(this, nodeAdded);
        }

        public delegate void TestTreeNodeRemoved(TestTreeView testTreeView, TestTreeNode nodeRemoved);
        public event TestTreeNodeRemoved OnTestTreeNodeRemoved;

        internal void fireTestTreeNodeRemovedEvent(TestTreeNode nodeRemoved)
        {
            OnTestTreeNodeRemoved?.Invoke(this, nodeRemoved);
        }

        // Delegate and method to traverse all tree nodes starting at name node.
        public delegate bool TraverseNodesDelegate(TestTreeNode testTreeNode, object tag);

        public void TraverseNodes(TestTreeNode startNode, TraverseNodesDelegate traverseNodesDelegate)
        {
            TraverseNodes(startNode, traverseNodesDelegate, null);
        }

        public void TraverseNodes(TestTreeNode startNode, TraverseNodesDelegate traverseNodesDelegate, object tag)
        {
            traverseNodes(startNode, traverseNodesDelegate, tag);
        }

        private void traverseNodes(TestTreeNode startNode, TraverseNodesDelegate traverseNodesDelegate, object tag)
        {
            bool @continue = traverseNodesDelegate(startNode, tag);

            if (@continue)
            {
                foreach (TestTreeNode treeNode in startNode.Nodes)
                {
                    traverseNodes(treeNode, traverseNodesDelegate, tag);
                }
            }
        }

        #endregion

        #region Data members

        // Flag set in response to test execution beginning and ending events.
        private bool _isExecuting;

        public StringCollection CachedTestAssemblies
        { get; set; }

        public TestProfile TestProfile
        { get; set; }

        public TestListeners TestListeners
        { get; set; }

        public bool SuppressExecution
        { get; set; }

        // Font to be used by active/inactive nodes.
        private static Font m_activeFont;
        internal static Font ActiveFont
        { get { return m_activeFont; } }

        private static Font m_executionFont;
        internal static Font ExecutionFont
        { get { return m_executionFont; } }

        private static Font m_inactiveFont;
        internal static Font InactiveFont
        { get { return m_inactiveFont; } }

        private static Font m_unavailableFont;
        internal static Font UnavailableFont
        { get { return m_unavailableFont; } }


        // TestTreeView change history container for undo/redo.
        private TestChangeEventHistories m_changeHistory;
        bool m_recordHistory = true;
        private ChangeType m_clipboardAction = ChangeType.Unknown;

        private const int WM_TIMER = 0x0113;
        private bool _triggerLabelEdit = false;
        private bool _wasDoubleClick = false;
        private TestSuite m_testSuite;
        private ContextMenuStrip m_treeViewContextMenu;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem m_miOpenEditor;
        private ToolStripMenuItem m_miExecute;
        private ToolStripSeparator m_toolStripSeparator1;
        private ToolStripMenuItem m_miSaveResults;
        private ToolStripMenuItem m_miResetResults;
        private ToolStripSeparator m_mitoolStripSeparator2;
        private ToolStripMenuItem m_miActivate;
        private ToolStripMenuItem m_miActivateAll;
        private ToolStripSeparator m_toolStripSeparator3;
        private ToolStripMenuItem m_miNewTestSuite;
        private ToolStripMenuItem m_miNewTestCase;
        private ToolStripMenuItem m_miNewTestStep;
        private ToolStripSeparator m_toolStripSeparator4;
        private ToolStripMenuItem m_miCut;
        private ToolStripMenuItem m_miCopy;
        private ToolStripMenuItem m_miPaste;
        private SaveFileDialog m_saveFileDialog;
        private ImageList m_treeViewImages;
        private ToolStripMenuItem m_miDelete;
        private ToolStripMenuItem m_miAddTestSuite;
        private ToolStripSeparator m_toolStripSeparator5;
        private OpenFileDialog m_openFileDialog;
        private ToolStripMenuItem m_miRename;
        private ToolStripSeparator m_toolStripSeparator6;
        private ToolStripMenuItem m_miBreakpoint;
        private ToolStripMenuItem m_miInsertBreakpoint;
        private ToolStripMenuItem m_miDeleteBreakpoint;
        private ToolStripMenuItem m_miChangeBreakpointState;
        private bool m_ignoreNodeChanges;

        #endregion

        #region Constructors

        public TestTreeView()
            : base()
        {
            m_ignoreNodeChanges = false;

            InitializeComponent();

            Clipboard.Clear();

            registerRuntimeEvents();

            // Create font for inactive nodes.
            m_activeFont = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
            m_executionFont = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            m_inactiveFont = new Font("Microsoft Sans Serif", 8, FontStyle.Italic);
            m_unavailableFont = new Font("Microsoft Sans Serif", 8, FontStyle.Italic);

            TestSuite.OnUnavailableReads += TestSuite_OnUnavailableReads;

            // Instantiate change history to support Undo\Redo.
            m_changeHistory = new TestChangeEventHistories();
            m_changeHistory.OnTestHistoryRedoEvent += m_changeHistory_OnTestHistoryRedoEvent;
            m_changeHistory.OnTestHistoryUndoEvent += m_changeHistory_OnTestHistoryUndoEvent;

            // Test script editor dialog event handlers
            TestScriptObjectEditorDialog.OnTestScriptObjectEditorActivated += TestScriptObjectEditorDialog_OnTestScriptObjectEditorActivated;
            TestScriptObjectEditorDialog.OnTestScriptObjectEditorClosed += TestScriptObjectEditorDialog_OnTestScriptObjectEditorClosed;

            this.ImageList = this.m_treeViewImages;
            this.ShowNodeToolTips = true;
            this.AllowDrop = true;
            this.HideSelection = false;

            this.m_miOpenEditor.Click += m_miOpenEditor_Click;
            this.m_miExecute.Click += m_miExecute_Click;
            this.m_miSaveResults.Click += m_miSaveResults_Click;
            this.m_miResetResults.Click += m_miReset_Click;
            this.m_miActivate.Click += m_miActivate_Click;
            this.m_miActivateAll.Click += m_miActivateAll_Click;
            this.m_miAddTestSuite.Click += m_miAddTestSuite_Click;
            this.m_miNewTestSuite.Click += m_miNewTestSuite_Click;
            this.m_miNewTestCase.Click += m_miNewTestCase_Click;
            this.m_miNewTestStep.Click += m_miNewTestStep_Click;
            this.m_miInsertBreakpoint.Click += m_miInsertBreakpoint_Click;
            this.m_miDeleteBreakpoint.Click += m_miDeleteBreakpoint_Click;
            this.m_miChangeBreakpointState.Click += m_miChangeBreakpointState_Click;
            this.m_miCut.Click += m_miCut_Click;
            this.m_miCopy.Click += m_miCopy_Click;
            this.m_miPaste.Click += m_miPaste_Click;
            this.m_miDelete.Click += m_miDelete_Click;
            this.m_miRename.Click += m_miRename_Click;

            // Set to catch timer system messages
            SetStyle(ControlStyles.EnableNotifyMessage, true);

            // Add images for item breakpoint mode
            addBreakpointImages("breakpoint.enabled");
            addBreakpointImages("breakpoint.disabled");

            TestScriptObject.OnTestPropertyChanged += TestScriptObject_OnTestPropertyChanged;
        }

        private void addBreakpointImages(string overlay)
        {
            var overlayImage = m_treeViewImages.Images[overlay];

            for (int index = 0; index <= 23; index++)
            {
                var key = m_treeViewImages.Images.Keys[index];
                var newKey = $"{key}.{overlay}";
                this.ImageList.Images.Add(newKey, mergeImages(m_treeViewImages.Images[key], overlayImage));
            }
        }

        UnavailableReadsEventArgs unavailableReadsEventArgs = null;

        private void TestSuite_OnUnavailableReads(TestSuite testSuite, UnavailableReadsEventArgs args)
        {
            unavailableReadsEventArgs = args;
        }

        #endregion

        #region Public properties

        new public TestTreeNode SelectedNode
        {
            get
            {
                return base.SelectedNode is TestTreeNode ? base.SelectedNode as TestTreeNode : null;
            }
            set
            {
                base.SelectedNode = value;
            }
        }

        public bool IsTestTreeNode
        {
            get { return Nodes.Count > 0 && Nodes[0] is TestTreeNode; }
        }

        public bool IsTestTreeDefaultNode
        {
            get { return Nodes.Count > 0 && Nodes[0] is TestTreeDefaultNode; }
        }

        public TestTreeNode RootNode
        {
            get { return (TestTreeNode)Nodes[0]; }
        }

        public bool HasChanged
        {
            get { return RootNode.HasChanged; }
        }

        public bool IsStepMode
        {
            get { return TestBreakpoints.StepOverMode; }
        }

        #endregion

        #region Public and internal methods

        /// <summary>
        /// Traverses TestTreeView for all nodes.
        /// </summary>
        /// <param name="startNode">Starting node position</param>
        /// <returns>Collection of all nodes from starting position</returns>
        public List<TestTreeNode> GetTestTreeNodes(TestTreeNode startNode)
        {
            var testTreeNodes = new List<TestTreeNode>();
            TraverseNodes(startNode, getTestTreeNodesDelegate, testTreeNodes);

            return testTreeNodes;
        }

        /// <summary>
        /// Delegate for GetTestTreeNodes method.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private bool getTestTreeNodesDelegate(TestTreeNode node, object tag)
        {
            var nodes = tag as List<TestTreeNode>;
            nodes.Add(node);
            return true;
        }

        public TestTreeNode OpenExistingTestSuite(TestTreeNode currentNode, bool recordHistory = true)
        {
            TestSuite testSuite = null;
            TestTreeNode testSuiteNode = null;

            try
            {
                m_openFileDialog.Title = "Open Test Suite";
                m_openFileDialog.InitialDirectory = TestProperties.TestSuites;
                m_openFileDialog.Filter = "Test suites (*.ste)|*.ste";
                m_openFileDialog.FilterIndex = 1;

                if (DialogResult.OK == m_openFileDialog.ShowDialog())
                {
                    testSuite = TestSuite.ReadFromFile(m_openFileDialog.FileName);
                    testSuiteNode = SetTestSuite(testSuite);

                    if (unavailableReadsEventArgs.TestSuites.Count > 0)
                    {
                        MessageBox.Show($"One or more errors occurred while opening the test suite \"{testSuite.FilePath}\".", "Quintity TestFramework",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        unavailableReadsEventArgs = null;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(
                    string.Format("Unable to locate test suite file \"{0}\".\r\n\r\nPlease verify the file path.", m_openFileDialog.FileName),
                    "Quintity TestFramework", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (SerializationException)
            {
                MessageBox.Show(
                    string.Format("Unable to read test suite \"{0}\".\r\n\r\nPlease verify the file is a valid test suite file.", m_openFileDialog.FileName),
                    "Quintity TestFramework", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (UriFormatException)
            {
                MessageBox.Show(
                    string.Format("Unable to read test suite \"{0}\".\r\n\r\nPlease verify the file is a valid test suite file.", m_openFileDialog.FileName),
                    "Quintity TestFramework", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Quintity TestFramework", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return testSuiteNode;
        }

        public TestTreeNode AddNewTestSuite(TestTreeNode currentNode, bool recordHistory = true)
        {
            DialogResult result = DialogResult.OK;
            TestTreeNode newNode = null;

            if (currentNode != null)
            {
                m_saveFileDialog.Title = "Save Test Suite As";
                m_saveFileDialog.InitialDirectory = TestProperties.TestSuites;
                m_saveFileDialog.RestoreDirectory = true;
                m_saveFileDialog.Filter = "Test suites (*.ste)|*.ste";
                m_saveFileDialog.FilterIndex = 1;

                result = m_saveFileDialog.ShowDialog();
            }

            if (result == DialogResult.OK)
            {
                var testSuite = new TestSuite(
                    string.IsNullOrEmpty(m_saveFileDialog.FileName) ? "Untitled test suite" : Path.GetFileNameWithoutExtension(m_saveFileDialog.FileName),
                    m_saveFileDialog.FileName);

                var testcase = testSuite.AddTestCase(new TestCase("Untitled test case"));
                testcase.AddTestStep(new TestStep("Untitled test step"));
                newNode = new TestTreeNode(testSuite);
                constructNodeTreeFragment(newNode, testSuite);
                InsertNodeExt(newNode, currentNode, false);

                if (recordHistory)
                {
                    m_changeHistory.RecordChangeEvent(new TestChangeEvent(ChangeType.Add, newNode, currentNode));
                }
            }

            return newNode;
        }

        public TestTreeNode AddExistingTestSuite(TestTreeNode currentNode, bool recordHistory = true)
        {
            m_openFileDialog.Reset();
            m_openFileDialog.Title = "Select test suite to add";
            m_openFileDialog.InitialDirectory = TestProperties.TestSuites;
            m_openFileDialog.Filter = "Test suites (*.ste)|*.ste";
            m_openFileDialog.FilterIndex = 1;

            TestTreeNode newNode = null;

            try
            {
                if (DialogResult.OK == m_openFileDialog.ShowDialog())
                {
                    var testSuite = TestSuite.ReadFromFile(m_openFileDialog.FileName);

                    Cursor = Cursors.WaitCursor;
                    var existingNode = FindNode(testSuite);
                    Cursor = Cursors.Default;

                    if (existingNode == null)
                    {
                        newNode = new TestTreeNode(testSuite);
                        constructNodeTreeFragment(newNode, testSuite);
                        newNode.Expand();
                        InsertNodeExt(newNode, currentNode, false);

                        if (recordHistory)
                        {
                            m_changeHistory.RecordChangeEvent(new TestChangeEvent(ChangeType.Add, newNode, currentNode));
                        }
                    }
                    else
                    {
                        SelectedNode = existingNode;

                        MessageBox.Show(
                          string.Format("Test suite \"{0}\" is already a test suite member.", testSuite.Title),
                          "Quintity TestFramework", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                MessageBox.Show(this,
                    string.Format(" \"{0}\" does not appear to be a valid test suite file", m_openFileDialog.FileName),
                    "Quintity TestFramework",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message,
                    "Quintity TestFramework",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return newNode;
        }

        public TestTreeNode AddNewTestCase(TestTreeNode currentNode, bool recordHistory = true)
        {
            var newNode = InsertNodeExt(new TestCase("Untitled test case"), currentNode, false);

            // Initialize test case with a single test step.
            AddNewTestStep(newNode, false);

            newNode.Expand();

            if (recordHistory)
            {
                m_changeHistory.RecordChangeEvent(new TestChangeEvent(ChangeType.Add, newNode, currentNode));
            }

            return newNode;
        }

        public TestTreeNode AddNewTestStep(TestTreeNode currentNode, bool recordHistory = true)
        {
            var newNode = InsertNodeExt(new TestStep("Untitle test step"), currentNode, false);

            if (recordHistory)
            {
                m_changeHistory.RecordChangeEvent(new TestChangeEvent(ChangeType.Add, newNode, currentNode));
            }

            return newNode;
        }

        public void RemoveNode()
        {
            RemoveNode(SelectedNode);
        }

        public void RemoveNode(TestScriptObject testScriptObject, bool recordHistory = true)
        {
            var nodeToRemove = FindNode(testScriptObject);
            RemoveNode(nodeToRemove, recordHistory);
        }

        public void RemoveNode(TestTreeNode nodeToRemove, bool recordHistory = true)
        {
            // Get parent's container object.
            TestTreeNode parentNode = nodeToRemove.Parent;
            TestScriptObjectContainer parentContainer = nodeToRemove.Parent.TestScriptObjectAsContainer();
            int index = parentContainer.FindTestScriptObjectIndex(nodeToRemove.TestScriptObject);

            // Need to determine for undo purposes (will beocme the undo target node.
            var undoTargetNode = nodeToRemove.Index != 0 ? parentNode.Nodes[nodeToRemove.Index - 1] : parentNode;

            // Remove from parent container
            parentContainer.RemoveTestScriptObject(nodeToRemove.TestScriptObject);

            // Remove node from tree
            nodeToRemove.Remove();

            fireTestTreeNodeRemovedEvent(nodeToRemove);

            // Update UI
            markAsChanged(parentNode);
            this.SelectedNode = parentNode;

            // Create change event with parents container object and index.
            if (recordHistory)
            {
                m_changeHistory.RecordChangeEvent(new TestChangeEvent(ChangeType.Remove, nodeToRemove, undoTargetNode));
            }
        }

        private struct TargetInsertInfo
        {
            public TestTreeNode TargetContainerNode;
            public int InsertIndex;
        }

        public TestTreeNode CopyNode(TestTreeNode sourceNode, TestTreeNode targetNode, bool recordHistory = true)
        {
            return CopyNode(sourceNode, targetNode, null, recordHistory);
        }

        /// <summary>
        /// Moves the selected "Cut" source object to the target node location.
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="targetNode"></param>
        public TestTreeNode CopyNode(TestTreeNode sourceNode, TestTreeNode targetNode, TestTreeNode copyNode, bool recordHistory = true)
        {
            var testScriptObjectCopy = copyNode?.TestScriptObject;

            // If copied node is not supplied (doesn't already exist), create it.
            if (testScriptObjectCopy is null)
            {
                if (sourceNode.IsTestSuite())
                {
                    testScriptObjectCopy = new TestSuite(sourceNode.TestScriptObject as TestSuite, null, null);
                }
                else if (sourceNode.IsTestCase())
                {
                    testScriptObjectCopy = new TestCase(sourceNode.TestScriptObject as TestCase, null);
                }
                else if (sourceNode.IsTestStep())
                {
                    testScriptObjectCopy = new TestStep(sourceNode.TestScriptObject as TestStep, null);
                }

                // Change title to reflect is copy (suppress undo event)
                m_recordHistory = false;
                testScriptObjectCopy.Title = "Copy of " + testScriptObjectCopy.Title;
                m_recordHistory = true;

                // Create new node for test script object.
                copyNode = new TestTreeNode(testScriptObjectCopy);

                if (copyNode.IsTestSuite())
                {
                    if (promptToSaveTestSuite(copyNode, false) == DialogResult.Cancel)
                    {
                        return null;
                    }
                }

                // Add copied objects children to copyNode;
                constructNodeTreeFragment(copyNode, testScriptObjectCopy);
            }

            // Get new insertion info (based on rules).
            var targetInsertInfo = GetTargetInsertInfo(copyNode, targetNode);

            // Insert into parent containertree node accordingly
            targetInsertInfo.TargetContainerNode.Nodes.Insert(targetInsertInfo.InsertIndex, copyNode);

            // Insert into new test script object target container
            targetInsertInfo.TargetContainerNode.TestScriptObjectAsContainer().InsertTestScriptObject(testScriptObjectCopy, targetInsertInfo.InsertIndex);

            // Update UI for moved node and previous parent
            markAsChanged(copyNode);

            this.SelectedNode = copyNode;

            // Create change event with parents container object and index.
            if (recordHistory)
            {
                m_changeHistory.RecordChangeEvent(new TestChangeEvent(ChangeType.Copy, copyNode, sourceNode, targetNode));
            }

            fireTestTreeNodeAddedEvent(copyNode);

            return copyNode;
        }

        private DialogResult promptToSaveTestSuite(TestTreeNode currentNode, bool recordHistory = true)
        {
            DialogResult result = DialogResult.OK;

            if (currentNode != null)
            {
                m_saveFileDialog.Title = "Save Test Suite As";
                m_saveFileDialog.InitialDirectory = TestProperties.TestSuites;
                m_saveFileDialog.RestoreDirectory = true;
                m_saveFileDialog.Filter = "Test suites (*.ste)|*.ste";
                m_saveFileDialog.FilterIndex = 1;

                result = m_saveFileDialog.ShowDialog();
            }

            if (result == DialogResult.OK)
            {
                var testSuite = currentNode.TestScriptObjectAsTestSuite();
                testSuite.FilePath = TestProperties.FixupString(m_saveFileDialog.FileName, "TestSuites");
                testSuite.Title = Path.GetFileNameWithoutExtension(m_saveFileDialog.FileName);
            }

            return result;
        }

        public void MoveNode(TestTreeNode sourceNode, TestScriptObjectContainer targetContainer, int index, bool recordHistory = true)
        {
            var parentNode = FindNode(targetContainer);

            // If parent nodes is empty, target is parent
            var targetNode = parentNode.Nodes.Count != 0 ? parentNode.Nodes[index] as TestTreeNode : parentNode;

            MoveNode(sourceNode, targetNode, recordHistory);
        }
        /// <summary>
        /// Moves the selected "Cut" source object to the target node location.
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="targetNode"></param>
        public void MoveNode(TestTreeNode sourceNode, TestTreeNode targetNode, bool recordHistory = true)
        {
            var sourceNodeParent = sourceNode.Parent;
            var sourceParentScriptObject = sourceNode.Parent.TestScriptObject as TestScriptObjectContainer;
            var sourceScriptObject = sourceNode.TestScriptObject;

            // Need to determine for undo purposes (will beocme the undo target node.
            var undoTargetNode = sourceNode.Index != 0 ? sourceNodeParent.Nodes[sourceNode.Index - 1] : sourceNodeParent;

            // Remove from source container node
            sourceNodeParent.Nodes.Remove(sourceNode);
            fireTestTreeNodeRemovedEvent(sourceNode);

            // Get new insertion info (based on rules).
            var targetInsertInfo = GetTargetInsertInfo(sourceNode, targetNode);

            // Insert into parent containertree node accordingly
            targetInsertInfo.TargetContainerNode.Nodes.Insert(targetInsertInfo.InsertIndex, sourceNode);

            // Remove from old test script object container
            var success = sourceParentScriptObject.RemoveTestScriptObject(sourceScriptObject);

            // Insert into new test script object target container
            targetInsertInfo.TargetContainerNode.TestScriptObjectAsContainer().InsertTestScriptObject(sourceScriptObject, targetInsertInfo.InsertIndex);

            // Update UI for moved node and previous parent
            markAsChanged(sourceNodeParent);
            markAsChanged(sourceNode);

            this.SelectedNode = sourceNode;
            
            // Create change event with parents container object and index.
            if (recordHistory)
            {
                m_changeHistory.RecordChangeEvent(new TestChangeEvent(ChangeType.Move, sourceNode, targetNode, undoTargetNode));
            }

            Clipboard.Clear();
        }

        internal void ResetChangeEventHistory() => m_changeHistory.Reset();

        public TestTreeNode InsertNodeExt(TestScriptObject testScriptObject, TestTreeNode targetNode, bool recordHistory = true)
        {
            return InsertNodeExt(new TestTreeNode(testScriptObject), targetNode, recordHistory);
        }

        /// <summary>
        /// Inserts an existing node in relationship to target node.  Relationship defined by type of node (e.g., suite, case or step)
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="targetNode"></param>
        public TestTreeNode InsertNodeExt(TestTreeNode sourceNode, TestTreeNode targetNode, bool recordHistory = true)
        {
            BeginUpdate();

            var sourceNodeParent = sourceNode.Parent;
            var sourceScriptObject = sourceNode.TestScriptObject;

            // Get new insertion info (based on rules).
            var targetInsertInfo = GetTargetInsertInfo(sourceNode, targetNode);

            // Insert into parent containertree node accordingly
            targetInsertInfo.TargetContainerNode.Nodes.Insert(targetInsertInfo.InsertIndex, sourceNode);

            // Insert into new test script object target container
            targetInsertInfo.TargetContainerNode.TestScriptObjectAsContainer().InsertTestScriptObject(sourceScriptObject, targetInsertInfo.InsertIndex);

            // Node and parents as changed.
            markAsChanged(sourceNode);

            this.SelectedNode = sourceNode;

            Clipboard.Clear();

            EndUpdate();

            return sourceNode;
        }

        private TargetInsertInfo GetTargetInsertInfo(TestTreeNode sourceNode, TestTreeNode targetNode)
        {
            TestTreeNode targetContainerNode = null;
            int insertIndex = -1;

            if (sourceNode.IsTestSuite())
            {
                if (targetNode.IsTestSuite())
                {
                    targetContainerNode = targetNode;
                    insertIndex = 0;
                }
                else if (targetNode.IsTestCase())
                {
                    targetContainerNode = targetNode.Parent;
                    insertIndex = targetNode.Index + 1;
                }
            }
            else if (sourceNode.IsTestCase())
            {
                if (targetNode.IsTestCase())
                {
                    targetContainerNode = targetNode.Parent;
                    insertIndex = targetNode.Index + 1;
                }
                else if (targetNode.IsTestSuite())
                {
                    targetContainerNode = targetNode;
                    insertIndex = 0;
                }
            }
            else if (sourceNode.IsTestStep())
            {
                if (targetNode.IsTestStep())
                {
                    targetContainerNode = targetNode.Parent;
                    insertIndex = targetNode.Index + 1;
                }
                else if (targetNode.IsTestCase())
                {
                    targetContainerNode = targetNode;
                    insertIndex = 0;
                }
            }

            return new TargetInsertInfo() { TargetContainerNode = targetContainerNode, InsertIndex = insertIndex };
        }

        public TestTreeNode SetTestSuite(TestSuite testSuite)
        {
            m_testSuite = testSuite;

            BeginUpdate();

            Nodes.Clear();

            m_ignoreNodeChanges = true;

            var newNode = new TestTreeNode(testSuite);
            this.Nodes.Add(newNode);

            // Populate tree view with suite nodes.
            var start = DateTime.Now;
            constructNodeTreeFragment(RootNode, testSuite);
            var elapsed = DateTime.Now - start;

            // Set breakpoints for newly loaded test suite.
            setTestBreakpoints();

            m_ignoreNodeChanges = false;

            ShowAllTestCases();
            SelectedNode = RootNode;
            RootNode.EnsureVisible();

            EndUpdate();

            return newNode;
        }

        private void setTestBreakpoints()
        {
            var breakpoints = TestBreakpoints.GetBreakpoints();

            foreach (var breakpoint in breakpoints)
            {
                var node = FindNode(breakpoint.TestScriptObjectID);

                if (node != null)
                {
                    node.TestBreakpoint = breakpoint;
                    node.UpdateUI();
                }

            }
        }

        public TestSuite GetTestSuite()
        {
            return RootNode.TestScriptObject as TestSuite;
        }

        public TestTreeNode NewRootTestSuite()
        {
            return AddNewTestSuite(null, false);
        }

        public void Deactivate(TestTreeNode node)
        {
            if (node.TestScriptObject.Status == Status.Active)
            {
                node.TestScriptObject.Status = Status.Inactive;
                node.UpdateUI();
            }
        }

        public void Activate(TestTreeNode node)
        {
            if (node.TestScriptObject.Status == Status.Inactive)
            {
                node.TestScriptObject.Status = Status.Active;
                node.UpdateUI();
            }
        }

        public void ActivateAll(TestTreeNode startNode)
        {
            BeginUpdate();

            Activate(startNode);

            foreach (TestTreeNode node in startNode.Nodes)
            {
                Activate(node);
            }

            EndUpdate();
        }

        public void DeactivateAll(TestTreeNode startNode)
        {
            BeginUpdate();

            Deactivate(startNode);

            foreach (TestTreeNode node in startNode.Nodes)
            {
                Deactivate(node);
            }

            EndUpdate();
        }

        public void ResetResults()
        {
            ResetResults(RootNode);
        }

        public void ResetResults(TestTreeNode startNode)
        {
            TraverseNodes(startNode, resetResult);
        }

        public void ResetHasChangedFlags()
        {
            TraverseNodes(RootNode, resetHasChangedFlag);
        }

        new public TestTreeNode GetNodeAt(int x, int y)
        {
            return base.GetNodeAt(x, y) as TestTreeNode;
        }

        new public TestTreeNode GetNodeAt(Point point)
        {
            return base.GetNodeAt(point) as TestTreeNode;
        }

        public bool IsTestSuiteLoaded()
        {
            return Nodes.Count > 0 ? true : false;
        }

        public List<TestTreeNode> GetAllNotes(TestTreeNode startNode)
        {
            var allNodes = new List<TestTreeNode>();

            TraverseNodes(startNode, getAllNodes, allNodes);

            return allNodes;
        }

        private bool getAllNodes(TestTreeNode node, object tag)
        {
            var allNodes = tag as List<TestTreeNode>;
            allNodes.Add(node);

            return true;
        }

        public void ShowAllTestCases()
        {
            TraverseNodes(RootNode, showAllTestCases);
        }

        public void ShowAllTestCases(TestTreeNode startNode)
        {
            TraverseNodes(startNode, showAllTestCases);
        }

        public void HideEmptyTestSuites()
        {
            TraverseNodes(RootNode, hideEmptyTestSuites);
        }

        public void HideEmptyTestSuites(TestTreeNode startNode)
        {
            TraverseNodes(startNode, hideEmptyTestSuites);
        }

        private bool hideEmptyTestSuites(TestTreeNode node, object tag)
        {
            //if (node != null && node.IsTestSuite() && !isRootNode(node) && node.Nodes.Count == 0)
            //{
            //    node.Remove();
            //}

            return true;
        }

        public void Undo()
        {
            m_changeHistory.Undo();
        }

        public bool UndoAvailable()
        {
            return m_changeHistory.CanUndo();
        }

        public void Redo()
        {
            m_changeHistory.Redo();
        }

        public bool RedoAvailable()
        {
            return m_changeHistory.CanRedo();
        }

        public void Execute()
        {
            Execute(RootNode);
        }

        private TestExecutor _executor;

        public void Execute(TestTreeNode testTreeNode)
        {
            try
            {
                ResetResults(testTreeNode);

                _executor = new TestExecutor();

                if (testTreeNode.TestScriptObject is TestSuite)
                {
                    _executor.ExecuteTestSuite(testTreeNode.TestScriptObject as TestSuite, m_qualifiedTestCases, TestProfile,
                        TestListeners.TestListenerCollection, SuppressExecution);
                }
                else if (testTreeNode.TestScriptObject is TestCase)
                {
                    _executor.ExecuteTestCase(testTreeNode.TestScriptObject as TestCase, SuppressExecution);
                }
                else if (testTreeNode.TestScriptObject is TestStep)
                {
                    _executor.ExecuteTestStep(testTreeNode.TestScriptObject as TestStep, SuppressExecution);
                }
            }
            catch (TestListenerInitializeException e)
            {
                MessageBox.Show(e.Message, "Quintity TestFramework",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Quintity TestFramework",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Hashtable getTestProperityOverrides(string targetEnvironment)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var testPropertyOverrides = new Hashtable();

            string section = $@"Environments/{targetEnvironment}/testPropertyOverrides";

            testPropertyOverrides = (Hashtable)ConfigurationManager.GetSection(section);

            return testPropertyOverrides;
        }

        public void StopExecution()
        {
            try
            {
                _executor.StopExecution(null, TerminationReason.UserInitiated);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Quintity TestFramework",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal bool HasTestScriptResults()
        {
            return GetTestTreeNodesWithResults().Count > 0 ? true : false;
        }

        internal List<TestTreeNode> GetTestTreeNodesWithResults()
        {
            var nodes = GetTestTreeNodes(RootNode);

            return new List<TestTreeNode>(nodes.Where<TestTreeNode>(x => x.TestScriptResult != null));
        }

        // Breakpoint methods
        public bool HasBreakpoints()
        {
            return GetAllBreakpoints().Count > 0 ? true : false;
        }
        public List<TestBreakpoint> GetAllBreakpoints()
        {
            var nodes = GetTestTreeNodes(RootNode);

            var breakpoints = (from z in nodes
                               where z.TestBreakpoint != null
                               select z.TestBreakpoint);

            return new List<TestBreakpoint>(breakpoints);
        }

        public void InsertBreakpoint(TestTreeNode currentNode)
        {
            TestBreakpoints.InsertBreakpoint(currentNode.TestScriptObject);
        }

        private Image mergeImages(Image backgroundImage, Image overlayImage)
        {
            Image mergedImage = backgroundImage;

            if (null != overlayImage)
            {
                Image theOverlay = overlayImage;

                if (PixelFormat.Format32bppArgb != overlayImage.PixelFormat)
                {
                    theOverlay = new Bitmap(overlayImage.Width,
                                            overlayImage.Height,
                                            PixelFormat.Format32bppArgb);
                    using (Graphics graphics = Graphics.FromImage(theOverlay))
                    {
                        graphics.DrawImage(overlayImage,
                                           new Rectangle(0, 0, theOverlay.Width, theOverlay.Height),
                                           new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                                           GraphicsUnit.Pixel);
                    }

                    ((Bitmap)theOverlay).MakeTransparent();
                }

                using (Graphics graphics = Graphics.FromImage(mergedImage))
                {
                    graphics.DrawImage(theOverlay,
                                       new Rectangle(0, 0, mergedImage.Width, mergedImage.Height),
                                       new Rectangle(0, 0, theOverlay.Width, theOverlay.Height),
                                       GraphicsUnit.Pixel);
                }
            }

            return mergedImage;
        }

        internal void ToggleBreakpoint()
        {
            var breakpoint = TestBreakpoints.GetBreakpoint(SelectedNode.TestScriptObject);

            if (breakpoint is null)
            {
                TestBreakpoints.InsertBreakpoint(SelectedNode.TestScriptObject);
            }
            else
            {
                if (breakpoint.CurrentState == TestBreakpoint.State.Enabled)
                {
                    TestBreakpoints.DeleteBreakpoint(breakpoint);
                }
                else
                {
                    TestBreakpoints.ChangeBreakpointState(breakpoint, TestBreakpoint.State.Enabled);
                }
            }
        }

        public void ToggleBreakpointState(TestTreeNode currentNode)
        {
            var breakpoint = TestBreakpoints.GetBreakpoint(currentNode.TestScriptObject);

            if (breakpoint != null)
            {
                TestBreakpoints.ChangeBreakpointState(breakpoint, breakpoint.CurrentState ==
                    TestBreakpoint.State.Enabled ? TestBreakpoint.State.Disabled : TestBreakpoint.State.Enabled);
            }
        }

        public void DisableAllBreakpoints()
        {
            TestBreakpoints.ChangeBreakpointStates(GetAllBreakpoints(), TestBreakpoint.State.Disabled);
        }

        public void EnableAllBreakpoints()
        {
            TestBreakpoints.ChangeBreakpointStates(GetAllBreakpoints(), TestBreakpoint.State.Enabled);
        }

        public void DeleteBreakpoint(TestTreeNode currentNode)
        {
            TestBreakpoints.DeleteBreakpoint(currentNode.TestScriptObject);
        }

        internal void DeleteAllBreakpoints()
        {
            if (DialogResult.Yes == MessageBox.Show(this, "Do you want to delete all breakpoints?", "Quintity TestFramework",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                TestBreakpoints.DeleteBreakpoints(new List<TestBreakpoint>(GetAllBreakpoints()));
            }
        }

        internal void StepOverBreakPoint()
        {
            TestBreakpoints.StepOverExecution();
        }

        internal void ContinueExecution()
        {
            TestBreakpoints.ContinueExecution();
        }

        public bool IsBreakpointMode()
        {
            return TestBreakpoints.CurrentBreakpoint is null ? false : true;
        }

        /// <summary>
        /// Refreshes the test object based on treeview structure.  Including
        /// rebuilding test suite's or case's container objects and updating parent object
        /// references.
        /// </summary>
        /// <param name="testTreeNode">Tree node whose script object is to be rebuilt.</param>
        /// <returns></returns>
        public TestScriptObject RefreshTestScriptObject(TestTreeNode testTreeNode)
        {
            m_refreshedTestScriptObject = testTreeNode.TestScriptObject;

            if (m_refreshedTestScriptObject is TestScriptObjectContainer)
            {
                var testScriptObjectContainer = testTreeNode.TestScriptObject as TestScriptObjectContainer;
                testScriptObjectContainer.TestScriptObjects.Clear();
                TraverseNodes(testTreeNode, new TraverseNodesDelegate(refreshTestScriptObjectContainer), testScriptObjectContainer);
            }

            return m_refreshedTestScriptObject;
        }

        public TestTreeNode FindNode(TestScriptObject testScriptObject)
        {
            return FindNode(testScriptObject.SystemID);
        }

        public TestTreeNode FindNode(Guid systemId)
        {
            m_foundNode = null;

            if (systemId != Guid.Empty)
            {
                TraverseNodes(RootNode, new TraverseNodesDelegate(findNode), systemId);
                //m_foundNode = getNodeMapping(systemId);
            }

            return m_foundNode;
        }

        public TestTreeNode FindNode(TestTreeNode startNode, Guid systemId)
        {
            m_foundNode = null;

            if (systemId != Guid.Empty)
            {
                TraverseNodes(startNode, new TraverseNodesDelegate(findNode), systemId);
                //m_foundNode = getNodeMapping(systemId);
            }

            return m_foundNode;
        }

        //public List<string> GatherAllTags()
        //{
        //    return m_testSuite.GatherAllTags();
        //}

        private List<string> _allTags;

        public List<string> GatherAllTags()
        {
            _allTags = new List<string>();

            TestSuite.TraverseTestTree(m_testSuite, new TestSuite.TraverseTestTreeDelegate(gatherAllTags));

            return _allTags.Distinct<string>().ToList<string>();
        }

        private bool gatherAllTags(TestScriptObject testScriptObject, object tag)
        {
            if (testScriptObject is TestCase)
            {
                string tags = ((TestCase)(testScriptObject)).Tags;

                if (null != tags)
                {
                    _allTags.AddRange(TestUtils.ToList(tags));
                }
            }

            return true;
        }

        public void Filter(TestFilter filter)
        {
            BeginUpdate();

            m_currentFilter = filter;
            RootNode.Nodes.Clear();

            m_qualifiedTestCases = m_testSuite.Filter(filter);

            TestSuite.TraverseTestTree(m_testSuite, new TestSuite.TraverseTestTreeDelegate(filterTestTree));

            HideEmptyTestSuites();

            ShowAllTestCases();

            EndUpdate();
        }

        internal void ClipboardCut()
        {
            if (SelectedNode.TestScriptObject.Status != Status.Unavailable)
            {
                var systemId = SelectedNode.TestScriptObject.SystemID;
                Clipboard.SetData("SystemID", systemId);
                m_clipboardAction = ChangeType.Move;
            }
        }

        internal void ClipboardCopy()
        {
            if (SelectedNode.TestScriptObject.Status != Status.Unavailable)
            {
                var systemId = SelectedNode.TestScriptObject.SystemID;
                Clipboard.SetData("SystemID", systemId);
                m_clipboardAction = ChangeType.Copy;
            }
        }

        internal void ClipboardPaste()
        {
            TestTreeNode target = SelectedNode;

            Guid systemID = (Guid)Clipboard.GetData("SystemID");

            performTreeEdit(systemID, target, m_clipboardAction);

            if (m_clipboardAction == ChangeType.Move)
            {
                m_clipboardAction = ChangeType.Unknown;
            }
        }

        private TestFilter m_currentFilter;
        private List<TestCase> m_qualifiedTestCases;

        private bool filterTestTree(TestScriptObject testScriptObject, object tag)
        {
            TestTreeNode currentNode = FindNode(testScriptObject);
            TestTreeNode parentNode = FindNode(testScriptObject.ParentID);

            // If test suite node, clear it nodes...
            if (currentNode.IsTestSuite() && !isRootNode(currentNode))
            {
                currentNode.Nodes.Clear();
                parentNode.Nodes.Add(currentNode);
            }
            else if (currentNode.IsTestCase())// && isQualified(currentNode.TestScriptObject))
            {
                currentNode.Nodes.Clear();

                if (isQualified(currentNode.TestScriptObject))
                {
                    parentNode.Nodes.Add(currentNode);
                }
            }
            else if (currentNode.IsTestStep())
            {
                parentNode.Nodes.Add(currentNode);
            }

            return true;
        }

        private bool isQualified(TestScriptObject testScriptObject)
        {

            return m_qualifiedTestCases.FindIndex(x => x.SystemID.Equals(testScriptObject.SystemID)) != -1 ? true : false;
        }

        #endregion

        #region Protected event handler methods

        protected override void OnNotifyMessage(Message m)
        {
            if (_triggerLabelEdit)
            {
                if (m.Msg == WM_TIMER)
                {
                    _triggerLabelEdit = false;
                    startLabelEdit();
                }
            }

            base.OnNotifyMessage(m);
        }

        protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
        {
            TestTreeNode node = SelectedNode;
            node.Text = node.TestScriptObject.Title;
        }

        protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
        {
            LabelEdit = false;
            e.CancelEdit = true;

            var node = SelectedNode;

            if (e.Label != null)
            {
                node.TestScriptObject.Title = e.Label;
            }

            node.UpdateTitleAndToolTip();
            base.OnAfterLabelEdit(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            Debug.WriteLine(string.Format("OnKeyUp - Data:  {0}, Code:  {1}, Value:  {2}, Modifiers:  {3}", e.KeyData, e.KeyCode, e.KeyValue, e.Modifiers));

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    {
                        RemoveNode(SelectedNode);
                        break;
                    }
                case Keys.C:
                    {
                        if (e.Control)
                        {
                            m_miCopy_Click(null, null);
                        }

                        break;
                    }

                case Keys.V:
                    {
                        if (e.Control)
                        {
                            m_miPaste_Click(null, null);
                        }

                        break;
                    }
                case Keys.X:
                    {
                        if (e.Control)
                        {
                            m_miCut_Click(null, null);
                        }

                        break;
                    }
                default:
                    { break; }
            }

            base.OnKeyUp(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            Debug.WriteLine(string.Format("OnKeyDown - Data:  {0}, Code:  {1}, Value:  {2}, Modifiers:  {3}", e.KeyData, e.KeyCode, e.KeyValue, e.Modifiers));

            if (!_isExecuting)
            {
                switch (e.KeyCode)
                {
                    case Keys.F2:
                        {
                            startLabelEdit();
                            break;
                        }
                    case Keys.Delete:
                        {
                            RemoveNode(SelectedNode);
                            break;
                        }
                    case Keys.Enter:
                        {
                            TestTreeNode nodeAdded = null;

                            if (SelectedNode.IsTestSuite())
                            {
                                nodeAdded = AddNewTestSuite(SelectedNode);
                            }
                            else if (SelectedNode.IsTestCase())
                            {
                                nodeAdded = AddNewTestCase(SelectedNode);
                            }
                            else if (SelectedNode.IsTestStep())
                            {
                                nodeAdded = AddNewTestStep(SelectedNode);
                            }

                            this.SelectedNode = nodeAdded;
                            startLabelEdit();

                            break;
                        }

                    case Keys.C:
                        {
                            if (e.Control)
                            {
                                m_miCopy_Click(null, null);
                            }

                            break;
                        }

                    case Keys.V:
                        {
                            if (e.Control)
                            {
                                m_miPaste_Click(null, null);
                            }

                            break;
                        }
                    case Keys.X:
                        {
                            if (e.Control)
                            {
                                m_miCut_Click(null, null);
                            }

                            break;
                        }
                    default:
                        { break; }
                }

                base.OnKeyDown(e);
            }

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var node = GetNodeAt(e.X, e.Y);

                if (node != null)
                {
                    SelectedNode = node;
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            TestTreeNode node = GetNodeAt(e.X, e.Y);

            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        if (null != node && !_isExecuting)
                        {
                            SelectedNode = node;
                            this.m_treeViewContextMenu.Show(PointToScreen(e.Location));
                        }
                    }

                    break;
                case MouseButtons.Left:
                    {
                        if (SelectedNode == node)
                        {
                            if (_wasDoubleClick)
                            {
                                _wasDoubleClick = false;
                            }
                            else
                            {
                                _triggerLabelEdit = true;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            base.OnMouseUp(e);
        }

        protected override void OnClick(EventArgs e)
        {
            _triggerLabelEdit = false;
            base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            _wasDoubleClick = true;
            base.OnDoubleClick(e);
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            Debug.WriteLine("OnItemDrag");

            DoDragDrop(e.Item, DragDropEffects.All);
            base.OnItemDrag(e);
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            Debug.WriteLine(string.Format("OnGiveFeedback:  effect:  {0}", e.Effect));
            base.OnGiveFeedback(e);
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent)
        {
            int i = 1;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if (e.KeyState == 1)
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.KeyState == 9)
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            TestTreeNode targetNode = nodeFromSystemPoint(new Point(e.X, e.Y));

            if (targetNode is null)
            {
                int i = 1;
                return;
            }
            TestTreeNode sourceNode = e.Data.GetData(typeof(TestTreeNode)) as TestTreeNode;

            Debug.WriteLine(string.Format("OnDragOver - Node:  {1} {0}",
                targetNode != null ? targetNode.TestScriptObject.GetType().ToString() : "Bonkers",
                targetNode != null ? targetNode.TestScriptObject.Title : "Not over tree node."));

            SelectedNode = targetNode;

            if (e.KeyState == 1)
            {
                e.Effect = isValidPaste(sourceNode, targetNode, ChangeType.Move) ? DragDropEffects.Move : DragDropEffects.None;
            }
            else if (e.KeyState == 9)
            {
                e.Effect = isValidPaste(sourceNode, targetNode, ChangeType.Copy) ? DragDropEffects.Move : DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            Debug.WriteLine(string.Format("OnDragDrop:  keystate:  {0}", e.KeyState));

            ChangeType action = ChangeType.Unknown;

            if (e.KeyState == 0)
            {
                action = ChangeType.Move;
            }
            else if (e.KeyState == 8)
            {
                action = ChangeType.Copy;
            }

            if (action != ChangeType.Unknown)
            {
                var targetNode = nodeFromSystemPoint(new Point(e.X, e.Y));
                var sourceNode = e.Data.GetData(typeof(TestTreeNode)) as TestTreeNode;

                if (targetNode != null)
                {
                    performTreeEdit(sourceNode, targetNode, action);
                }
            }
        }

        #endregion

        #region Traverse test tree delegate methods

        private bool constructNodeTreeFragmentDelegate(TestScriptObject testScriptObject, object tag)
        {
            Debug.WriteLine(string.Format("Type:  {0}, Title: {1}, Parent:  {2}",
                testScriptObject.GetType().ToString(), testScriptObject.Title, testScriptObject.ParentID));

            if (m_parentNode.TestScriptObject.SystemID != testScriptObject.SystemID)
            {
                TestTreeNode newNode = new TestTreeNode(testScriptObject);
                TestTreeNode parentNode = FindNode(m_parentNode, testScriptObject.ParentID);
                parentNode.Nodes.Add(newNode);
            }

            return true;
        }

        private TestTreeNode m_foundNode;

        private bool findNode(TestTreeNode testTreeNode, object tag)
        {
            bool @continue = true;

            if (testTreeNode.TestScriptObject.SystemID.Equals((Guid)tag))
            {
                m_foundNode = testTreeNode;
                @continue = false;
            }

            return @continue;
        }

        private TestScriptObject m_refreshedTestScriptObject;

        private bool refreshTestScriptObjectContainer(TestTreeNode testTreeNode, object tag)
        {
            TestScriptObjectContainer primaryContainer = tag as TestScriptObjectContainer;

            if (m_refreshedTestScriptObject.SystemID != testTreeNode.TestScriptObject.SystemID)
            {
                if (testTreeNode.IsTestSuite() || testTreeNode.IsTestCase())
                {
                    ((TestScriptObjectContainer)testTreeNode.TestScriptObject).TestScriptObjects.Clear();
                }

                TestScriptObjectContainer parentContainer =
                    primaryContainer.Find(testTreeNode.Parent.TestScriptObject.SystemID) as TestScriptObjectContainer;

                testTreeNode.TestScriptObject.SetParent(parentContainer);
                parentContainer.InsertTestScriptObject(testTreeNode.TestScriptObject);
            }

            return true;
        }

        private bool resetResult(TestTreeNode currentNode, object tag)
        {
            currentNode.ResetResult();

            return true;
        }

        private bool resetHasChangedFlag(TestTreeNode currentNode, object tag)
        {
            currentNode.ResetHasChanged();

            return true;
        }

        private bool refreshTree(TestTreeNode currentNode, object tag)
        {
            if (currentNode.IsTestSuite())
            {
                TestSuite currentSuite = currentNode.TestScriptObjectAsTestSuite();

                currentSuite.ClearTestScriptObjectCollection();

                if (currentNode.Parent != null)
                {
                    TestSuite parentSuite = currentNode.Parent.TestScriptObjectAsTestSuite();
                    parentSuite.AddTestSuite(currentSuite);
                }
                else
                {
                    m_testSuite = currentSuite;
                }
            }
            else if (currentNode.IsTestCase())
            {
                TestCase currentCase = currentNode.TestScriptObjectAsTestCase();
                TestSuite parentSuite = currentNode.Parent.TestScriptObjectAsTestSuite();
                currentCase.ClearTestSteps();
                parentSuite.AddTestCase(currentCase);
            }
            else if (currentNode.IsTestStep())
            {
                TestCase parentCase = currentNode.Parent.TestScriptObjectAsTestCase();
                parentCase.AddTestStep(currentNode.TestScriptObjectAsTestStep());
            }

            return true;
        }

        private bool showAllTestCases(TestTreeNode node, object tag)
        {
            if (node.IsTestSuite())
            {
                node.Expand();
            }
            else if (node.IsTestCase())
            {
                node.Collapse();
            }

            return true;
        }

        #endregion

        #region Context menu event handlers

        private void m_treeViewContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TestTreeNode selectedNode = SelectedNode;

            // Unavailable refers to a failed test suite open/load.
            var unavailable = selectedNode.TestScriptObject.Status == Status.Unavailable ? true : false;

            if (!selectedNode.HasBreakpoint())
            {
                m_miDeleteBreakpoint.Visible = false;
                m_miDeleteBreakpoint.Enabled = false;

                m_miChangeBreakpointState.Visible = false;
                m_miChangeBreakpointState.Enabled = false;

                m_miInsertBreakpoint.Visible = true;
                m_miInsertBreakpoint.Enabled = true;
            }
            else
            {
                m_miDeleteBreakpoint.Visible = true;
                m_miDeleteBreakpoint.Enabled = true;

                m_miChangeBreakpointState.Text = selectedNode.TestBreakpoint.CurrentState == TestBreakpoint.State.Enabled ? "Disable Breakpoint" : "Enable Breakpoint";
                m_miChangeBreakpointState.Visible = true;
                m_miChangeBreakpointState.Enabled = true;

                m_miInsertBreakpoint.Visible = false;
                m_miInsertBreakpoint.Enabled = false;
            }

            if (selectedNode.TestScriptObject.Status == Status.Active)
            {
                m_miActivate.Enabled = true;
                m_miActivate.Text = "Deactivate";

                m_miActivateAll.Visible = selectedNode.TestScriptObject is TestScriptObjectContainer;
                m_miActivateAll.Enabled = true;
                m_miActivateAll.Text = "Deactivate All";
            }
            else if (selectedNode.TestScriptObject.Status == Status.Inactive)
            {
                m_miActivate.Enabled = true;
                m_miActivate.Text = "Activate";

                m_miActivateAll.Visible = selectedNode.TestScriptObject is TestScriptObjectContainer;
                m_miActivateAll.Enabled = true;
                m_miActivateAll.Text = "Activate All";
            }
            else
            {
                m_miActivate.Enabled = false;
                m_miActivateAll.Visible = false;
            }

            m_miOpenEditor.Enabled = unavailable ? false : true;

            if (selectedNode.IsTestSuite())
            {
                if (!unavailable)
                {
                    m_miNewTestSuite.Enabled = true;
                    m_miNewTestCase.Enabled = true;
                    m_miNewTestStep.Enabled = false;
                    m_miAddTestSuite.Enabled = true;
                }
                else
                {
                    m_miAddTestSuite.Enabled = false;
                    m_miNewTestSuite.Enabled = false;
                    m_miNewTestCase.Enabled = false;
                    m_miNewTestStep.Enabled = false;
                }

                configureTestSuiteDelete();
            }
            else if (selectedNode.IsTestCase())
            {
                m_miNewTestSuite.Enabled = true;
                m_miNewTestCase.Enabled = true;
                m_miNewTestStep.Enabled = true;
                m_miAddTestSuite.Enabled = true;
                configureDelete();
            }
            else if (selectedNode.IsTestStep())
            {
                m_miNewTestStep.Enabled = true;
                m_miNewTestSuite.Enabled = false;
                m_miNewTestCase.Enabled = false;
                m_miAddTestSuite.Enabled = false;
                configureDelete();
            }

            if (isRootNode(selectedNode))
            {
                m_miCut.Enabled = false;
                m_miCopy.Enabled = false;
                m_miDelete.Enabled = false;
                m_miCut.ToolTipText = null;
                m_miCopy.ToolTipText = null;
                m_miDelete.ToolTipText = null;
            }
            else
            {
                m_miDelete.Enabled = true;
                m_miCut.Enabled = unavailable ? false : true;
                m_miCopy.Enabled = unavailable ? false : true; ;
                m_miRename.Enabled = unavailable ? false : true; ;

                if (!unavailable)
                {
                    m_miCopy.ToolTipText = string.Format("Copy node \"{0}\" to clipboard", selectedNode.Text);
                    m_miCut.ToolTipText = string.Format("Cut node \"{0}\" to clipboard", selectedNode.Text);
                    m_miDelete.ToolTipText = string.Format("Delete node \"{0}\"", selectedNode.Text);
                }
            }

            if (Clipboard.ContainsData("SystemID"))
            {
                var sourceNode = this.FindNode((Guid)Clipboard.GetData("SystemID"));

                if (isValidPaste(sourceNode, SelectedNode, m_clipboardAction) && !unavailable)
                {
                    m_miPaste.Enabled = true;

                    if (m_clipboardAction == ChangeType.Copy)
                    {
                        m_miPaste.ToolTipText = string.Format("Copy node \"{0}\" to selected location.", selectedNode.Text);
                    }
                    else if (m_clipboardAction == ChangeType.Move)
                    {
                        m_miPaste.ToolTipText = string.Format("Move node \"{0}\" to selected location.", selectedNode.Text);
                    }
                }
            }
            else
            {
                m_miPaste.Enabled = false;
                m_miPaste.ToolTipText = "The application clipboard is empty.";
            }

            m_miResetResults.Enabled = selectedNode.TestScriptResult != null ? true : false;
            m_miExecute.Enabled = selectedNode.TestScriptObject.Status == Status.Active ? true : false;
            m_miSaveResults.Enabled = selectedNode.TestScriptResult != null ? true : false;
        }

        private void m_miOpenEditor_Click(object sender, EventArgs e)
        {
            displayTestScriptObjectEditorDialog();
        }

        private void m_miExecute_Click(object sender, EventArgs e)
        {
            Execute(SelectedNode);
        }

        private void m_miReset_Click(object sender, EventArgs e)
        {
            ResetResults(SelectedNode);
        }

        private void m_miSaveResults_Click(object sender, EventArgs e)
        {
            TestTreeNode node = SelectedNode;

            m_saveFileDialog.Title = "Save Test Result As";
            m_saveFileDialog.InitialDirectory = TestProperties.TestOutput;
            m_saveFileDialog.FileName = node.TestScriptObject.Title;
            m_saveFileDialog.ValidateNames = true;
            m_saveFileDialog.RestoreDirectory = true;
            m_saveFileDialog.Filter =
                "XML results (*.xml)|*.xml|Webpage results (*.html) | *.html|Text results (*.txt) | *.txt|All files (*.*)|*.*";
            m_saveFileDialog.FilterIndex = 1;

            DialogResult dlgResult = m_saveFileDialog.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                var result = SelectedNode.TestScriptResult;

                if (result is TestSuiteResult)
                {
                    TestSuiteResult.SerializeToFile((TestSuiteResult)result, m_saveFileDialog.FileName);
                }
                else if (result is TestCaseResult)
                {
                    TestCaseResult.SerializeToFile((TestCaseResult)result, m_saveFileDialog.FileName);
                }
                else if (result is TestStepResult)
                {
                    TestStepResult.SerializeToFile((TestStepResult)result, m_saveFileDialog.FileName);
                }
            }
        }

        private void m_miActivate_Click(object sender, EventArgs e)
        {
            if (SelectedNode.TestScriptObject.Status == Status.Inactive)
            {
                Activate(SelectedNode);
            }
            else if (SelectedNode.TestScriptObject.Status == Status.Active)
            {
                Deactivate(SelectedNode);
            }
        }

        private void m_miActivateAll_Click(object sender, EventArgs e)
        {
            if (SelectedNode.TestScriptObject.Status == Status.Inactive)
            {
                ActivateAll(SelectedNode);
            }
            else if (SelectedNode.TestScriptObject.Status == Status.Active)
            {
                DeactivateAll(SelectedNode);
            }
        }

        private void m_miAddTestSuite_Click(object sender, EventArgs e)
        {
            AddExistingTestSuite(SelectedNode, true);
        }

        private void m_miNewTestSuite_Click(object sender, EventArgs e)
        {
            TestTreeNode newNode = AddNewTestSuite(SelectedNode);

            if (null != newNode)
            {
                newNode.Expand();
                SelectedNode = newNode;
            }
        }

        private void m_miNewTestCase_Click(object sender, EventArgs e)
        {
            TestTreeNode newNode = AddNewTestCase(SelectedNode);

            if (null != newNode)
            {
                newNode.Expand();
                SelectedNode = newNode;
            }
        }

        private void m_miNewTestStep_Click(object sender, EventArgs e)
        {
            SelectedNode = AddNewTestStep(SelectedNode);
        }

        private void m_miChangeBreakpointState_Click(object sender, EventArgs e)
        {
            ToggleBreakpointState(SelectedNode);
        }

        private void m_miDeleteBreakpoint_Click(object sender, EventArgs e)
        {
            DeleteBreakpoint(SelectedNode);
        }

        private void m_miInsertBreakpoint_Click(object sender, EventArgs e)
        {
            InsertBreakpoint(SelectedNode);
        }

        private void m_miCut_Click(object sender, EventArgs e)
        {
            ClipboardCut();
        }

        private void m_miCopy_Click(object sender, EventArgs e)
        {
            ClipboardCopy();
        }

        private void m_miPaste_Click(object sender, EventArgs e)
        {
            ClipboardPaste();
        }


        private void m_miDelete_Click(object sender, EventArgs e)
        {
            RemoveNode(SelectedNode);
        }

        private void m_miRename_Click(object sender, EventArgs e)
        {
            startLabelEdit();
        }

        #endregion

        #region Other event handlers

        private void m_changeHistory_OnTestHistoryUndoEvent(TestChangeEvent e)
        {
            undoChangeEvent(e);
        }

        private void m_changeHistory_OnTestHistoryRedoEvent(TestChangeEvent e)
        {
            redoChangeEvent(e);
        }

        private void TestScriptObject_OnTestPropertyChanged(TestScriptObject testScriptObject, TestPropertyChangedEventArgs args)
        {
            TestTreeNode testTreeNode = testScriptObject is TestProcessor ? FindNode(testScriptObject.ParentID) : FindNode(testScriptObject);

            markAsChanged(testTreeNode);

            if (m_recordHistory)
            {
                m_changeHistory.RecordChangeEvent(new TestChangeEvent(ChangeType.Update, testTreeNode,
                    testScriptObject, args.Property, args.CurrentValue, args.FormerValue));
            }
        }

        void TestScriptObjectEditorDialog_OnTestScriptObjectEditorClosed(object sender, TestScriptObjectEditorDialog.TestScriptObjectEditorClosedArgs e)
        {
            if (!(e.TestScriptObject is TestProcessor))
            {
                var node = FindNode(e.TestScriptObject.SystemID);
                CachedTestAssemblies = node.TestScriptEditorDialog.GetCachedTestAssemblies();
                node.TestScriptEditorDialog = null;
            }
        }

        void TestScriptObjectEditorDialog_OnTestScriptObjectEditorActivated(object sender, TestScriptObjectEditorDialog.TestScriptObjectEditorActivatedArgs e)
        {
            Guid systemId = e.TestScriptObject is TestProcessor ? ((TestProcessor)e.TestScriptObject).ParentID : e.TestScriptObject.SystemID;

            SelectedNode = FindNode(systemId);
        }

        private void TestTreeView_DoubleClick(object sender, EventArgs e)
        {
            displayTestScriptObjectEditorDialog();
        }

        private void TestTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (SelectedNode != null)
                SelectedNode.UpdateUI();

            var bob = this.m_treeViewImages.Images;
        }

        private void TestTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (SelectedNode != null)
                SelectedNode.UpdateUI();
        }

        #endregion

        #region Private methods

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestTreeView));
            this.m_treeViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_miOpenEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_miExecute = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miResetResults = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miSaveResults = new System.Windows.Forms.ToolStripMenuItem();
            this.m_mitoolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_miActivate = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miActivateAll = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.m_miNewTestSuite = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miNewTestCase = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miNewTestStep = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.m_miAddTestSuite = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.m_miBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miInsertBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miDeleteBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miChangeBreakpointState = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.m_miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miRename = new System.Windows.Forms.ToolStripMenuItem();
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_treeViewImages = new System.Windows.Forms.ImageList(this.components);
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_treeViewContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_treeViewContextMenu
            // 
            this.m_treeViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_miOpenEditor,
            this.m_toolStripSeparator1,
            this.m_miExecute,
            this.m_miResetResults,
            this.m_miSaveResults,
            this.m_mitoolStripSeparator2,
            this.m_miActivate,
            this.m_miActivateAll,
            this.m_toolStripSeparator3,
            this.m_miNewTestSuite,
            this.m_miNewTestCase,
            this.m_miNewTestStep,
            this.m_toolStripSeparator4,
            this.m_miAddTestSuite,
            this.m_toolStripSeparator5,
            this.m_miBreakpoint,
            this.m_toolStripSeparator6,
            this.m_miCut,
            this.m_miCopy,
            this.m_miPaste,
            this.m_miDelete,
            this.m_miRename});
            this.m_treeViewContextMenu.Name = "m_treeViewContextMenu";
            this.m_treeViewContextMenu.Size = new System.Drawing.Size(202, 414);
            this.m_treeViewContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.m_treeViewContextMenu_Opening);
            // 
            // m_miOpenEditor
            // 
            this.m_miOpenEditor.Name = "m_miOpenEditor";
            this.m_miOpenEditor.Size = new System.Drawing.Size(201, 22);
            this.m_miOpenEditor.Text = "Open Editor...";
            // 
            // m_toolStripSeparator1
            // 
            this.m_toolStripSeparator1.Name = "m_toolStripSeparator1";
            this.m_toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // m_miExecute
            // 
            this.m_miExecute.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.StartExecution;
            this.m_miExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_miExecute.Name = "m_miExecute";
            this.m_miExecute.Size = new System.Drawing.Size(201, 22);
            this.m_miExecute.Text = "Execute";
            // 
            // m_miResetResults
            // 
            this.m_miResetResults.Image = ((System.Drawing.Image)(resources.GetObject("m_miResetResults.Image")));
            this.m_miResetResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_miResetResults.Name = "m_miResetResults";
            this.m_miResetResults.Size = new System.Drawing.Size(201, 22);
            this.m_miResetResults.Text = "Reset Results";
            // 
            // m_miSaveResults
            // 
            this.m_miSaveResults.Name = "m_miSaveResults";
            this.m_miSaveResults.Size = new System.Drawing.Size(201, 22);
            this.m_miSaveResults.Text = "Save Results...";
            // 
            // m_mitoolStripSeparator2
            // 
            this.m_mitoolStripSeparator2.Name = "m_mitoolStripSeparator2";
            this.m_mitoolStripSeparator2.Size = new System.Drawing.Size(198, 6);
            // 
            // m_miActivate
            // 
            this.m_miActivate.Name = "m_miActivate";
            this.m_miActivate.Size = new System.Drawing.Size(201, 22);
            this.m_miActivate.Text = "Activate";
            // 
            // m_miActivateAll
            // 
            this.m_miActivateAll.Name = "m_miActivateAll";
            this.m_miActivateAll.Size = new System.Drawing.Size(201, 22);
            this.m_miActivateAll.Text = "Activate All";
            // 
            // m_toolStripSeparator3
            // 
            this.m_toolStripSeparator3.Name = "m_toolStripSeparator3";
            this.m_toolStripSeparator3.Size = new System.Drawing.Size(198, 6);
            // 
            // m_miNewTestSuite
            // 
            this.m_miNewTestSuite.Name = "m_miNewTestSuite";
            this.m_miNewTestSuite.Size = new System.Drawing.Size(201, 22);
            this.m_miNewTestSuite.Text = "New Test Suite...";
            this.m_miNewTestSuite.ToolTipText = "Add a new test suite";
            // 
            // m_miNewTestCase
            // 
            this.m_miNewTestCase.Name = "m_miNewTestCase";
            this.m_miNewTestCase.Size = new System.Drawing.Size(201, 22);
            this.m_miNewTestCase.Text = "New Test Case";
            // 
            // m_miNewTestStep
            // 
            this.m_miNewTestStep.Name = "m_miNewTestStep";
            this.m_miNewTestStep.Size = new System.Drawing.Size(201, 22);
            this.m_miNewTestStep.Text = "New Test Step";
            // 
            // m_toolStripSeparator4
            // 
            this.m_toolStripSeparator4.Name = "m_toolStripSeparator4";
            this.m_toolStripSeparator4.Size = new System.Drawing.Size(198, 6);
            // 
            // m_miAddTestSuite
            // 
            this.m_miAddTestSuite.Name = "m_miAddTestSuite";
            this.m_miAddTestSuite.Size = new System.Drawing.Size(201, 22);
            this.m_miAddTestSuite.Text = "Add Existing Test Suite...";
            this.m_miAddTestSuite.ToolTipText = "Add an existing test suite";
            // 
            // m_toolStripSeparator5
            // 
            this.m_toolStripSeparator5.Name = "m_toolStripSeparator5";
            this.m_toolStripSeparator5.Size = new System.Drawing.Size(198, 6);
            // 
            // m_miBreakpoint
            // 
            this.m_miBreakpoint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_miInsertBreakpoint,
            this.m_miDeleteBreakpoint,
            this.m_miChangeBreakpointState});
            this.m_miBreakpoint.Name = "m_miBreakpoint";
            this.m_miBreakpoint.Size = new System.Drawing.Size(201, 22);
            this.m_miBreakpoint.Text = "Breakpoint";
            // 
            // m_miInsertBreakpoint
            // 
            this.m_miInsertBreakpoint.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.BreakpointEnable1;
            this.m_miInsertBreakpoint.Name = "m_miInsertBreakpoint";
            this.m_miInsertBreakpoint.Size = new System.Drawing.Size(172, 22);
            this.m_miInsertBreakpoint.Text = "Insert Breakpoint";
            this.m_miInsertBreakpoint.ToolTipText = "Inserts new breakpoint";
            // 
            // m_miDeleteBreakpoint
            // 
            this.m_miDeleteBreakpoint.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.DeleteBreakpoint;
            this.m_miDeleteBreakpoint.Name = "m_miDeleteBreakpoint";
            this.m_miDeleteBreakpoint.Size = new System.Drawing.Size(172, 22);
            this.m_miDeleteBreakpoint.Text = "Delete Breakpoint";
            // 
            // m_miChangeBreakpointState
            // 
            this.m_miChangeBreakpointState.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.ToggleAllBreakpoints;
            this.m_miChangeBreakpointState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_miChangeBreakpointState.Name = "m_miChangeBreakpointState";
            this.m_miChangeBreakpointState.Size = new System.Drawing.Size(172, 22);
            this.m_miChangeBreakpointState.Text = "Disable Breakpoint";
            // 
            // m_toolStripSeparator6
            // 
            this.m_toolStripSeparator6.Name = "m_toolStripSeparator6";
            this.m_toolStripSeparator6.Size = new System.Drawing.Size(198, 6);
            // 
            // m_miCut
            // 
            this.m_miCut.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Cut;
            this.m_miCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_miCut.Name = "m_miCut";
            this.m_miCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.m_miCut.Size = new System.Drawing.Size(201, 22);
            this.m_miCut.Text = "Cut";
            // 
            // m_miCopy
            // 
            this.m_miCopy.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Copy;
            this.m_miCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_miCopy.Name = "m_miCopy";
            this.m_miCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.m_miCopy.Size = new System.Drawing.Size(201, 22);
            this.m_miCopy.Text = "Copy";
            // 
            // m_miPaste
            // 
            this.m_miPaste.Enabled = false;
            this.m_miPaste.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Paste;
            this.m_miPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_miPaste.Name = "m_miPaste";
            this.m_miPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.m_miPaste.Size = new System.Drawing.Size(201, 22);
            this.m_miPaste.Text = "Paste";
            // 
            // m_miDelete
            // 
            this.m_miDelete.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Delete;
            this.m_miDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_miDelete.Name = "m_miDelete";
            this.m_miDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.m_miDelete.Size = new System.Drawing.Size(201, 22);
            this.m_miDelete.Text = "Del";
            // 
            // m_miRename
            // 
            this.m_miRename.Image = global::Quintity.TestFramework.TestEngineer.Properties.Resources.Rename;
            this.m_miRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_miRename.Name = "m_miRename";
            this.m_miRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.m_miRename.Size = new System.Drawing.Size(201, 22);
            this.m_miRename.Text = "Rename";
            this.m_miRename.ToolTipText = "Rename the selected item.";
            // 
            // m_treeViewImages
            // 
            this.m_treeViewImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_treeViewImages.ImageStream")));
            this.m_treeViewImages.TransparentColor = System.Drawing.Color.Transparent;
            this.m_treeViewImages.Images.SetKeyName(0, "folder.closed");
            this.m_treeViewImages.Images.SetKeyName(1, "folder.open");
            this.m_treeViewImages.Images.SetKeyName(2, "teststep.active");
            this.m_treeViewImages.Images.SetKeyName(3, "teststep.error");
            this.m_treeViewImages.Images.SetKeyName(4, "teststep.inactive");
            this.m_treeViewImages.Images.SetKeyName(5, "teststep.fail");
            this.m_treeViewImages.Images.SetKeyName(6, "teststep.pass");
            this.m_treeViewImages.Images.SetKeyName(7, "teststep.incomplete");
            this.m_treeViewImages.Images.SetKeyName(8, "folder.closed");
            this.m_treeViewImages.Images.SetKeyName(9, "folder.closed.deactivated");
            this.m_treeViewImages.Images.SetKeyName(10, "folder.closed.fail");
            this.m_treeViewImages.Images.SetKeyName(11, "folder.closed.pass");
            this.m_treeViewImages.Images.SetKeyName(12, "folder.open");
            this.m_treeViewImages.Images.SetKeyName(13, "folder.open.deactivated");
            this.m_treeViewImages.Images.SetKeyName(14, "folder.open.fail");
            this.m_treeViewImages.Images.SetKeyName(15, "folder.open.pass");
            this.m_treeViewImages.Images.SetKeyName(16, "execution.arrow");
            this.m_treeViewImages.Images.SetKeyName(17, "teststep.manual.active");
            this.m_treeViewImages.Images.SetKeyName(18, "teststep.manual.error");
            this.m_treeViewImages.Images.SetKeyName(19, "teststep.manual.fail");
            this.m_treeViewImages.Images.SetKeyName(20, "teststep.manual.inactive");
            this.m_treeViewImages.Images.SetKeyName(21, "teststep.manual.pass");
            this.m_treeViewImages.Images.SetKeyName(22, "folder.closed.didnotexecute");
            this.m_treeViewImages.Images.SetKeyName(23, "folder.open.didnotexecute");
            this.m_treeViewImages.Images.SetKeyName(24, "breakpoint.enabled");
            this.m_treeViewImages.Images.SetKeyName(25, "breakpoint.disabled");
            this.m_treeViewImages.Images.SetKeyName(26, "BreakpointEnable.png");
            // 
            // TestTreeView
            // 
            this.ContextMenuStrip = this.m_treeViewContextMenu;
            this.FullRowSelect = true;
            this.ImageIndex = 0;
            this.ImageList = this.m_treeViewImages;
            this.LineColor = System.Drawing.Color.Black;
            this.SelectedImageIndex = 0;
            this.ShowNodeToolTips = true;
            this.StateImageList = this.m_treeViewImages;
            this.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TestTreeView_AfterCollapse);
            this.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TestTreeView_AfterExpand);
            this.DoubleClick += new System.EventHandler(this.TestTreeView_DoubleClick);
            this.m_treeViewContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private bool isValidPaste(TestTreeNode sourceNode, TestTreeNode targetNode, ChangeType changeType)
        {
            bool isValid = false;

            // Make sure we are not trying to paste item into the cut fragment.
            if (changeType == ChangeType.Move && !sourceNode.IsTestStep() ? isDescendantNode(sourceNode, targetNode) : false)
            {
                // Can't paste into its deleted self.
                return isValid;
            }

            if (sourceNode.IsTestSuite())
            {
                isValid = targetNode.IsTestStep() ? false : true;
            }
            else if (sourceNode.IsTestCase())
            {
                isValid = targetNode.IsTestStep() ? false : true;
            }
            else if (sourceNode.IsTestStep())
            {
                isValid = targetNode.IsTestSuite() ? false : true;
            }

            return isValid;
        }

        private bool isDescendantNode(TestTreeNode startNode, TestTreeNode queryNode)
        {
            var nodes = GetAllNotes(startNode);

            return nodes.FindAll(x => x.TestScriptObject.SystemID.Equals(queryNode.TestScriptObject.SystemID)).Count > 0 ? true : false;
        }

        private void configureDelete()
        {
            m_miDelete.Text = "Del";
            m_miDelete.ShowShortcutKeys = true;
        }

        private void configureTestSuiteDelete()
        {
            m_miDelete.Text = "Remove";
            m_miDelete.ShowShortcutKeys = false;
        }

        private void performTreeEdit(Guid sourceSystemId, TestTreeNode targetNode, ChangeType action)
        {
            var sourceNode = FindNode(sourceSystemId);

            if (sourceNode != null)
            {
                performTreeEdit(sourceNode, targetNode, action);
            }
        }

        private void performTreeEdit(TestTreeNode sourceNode, TestTreeNode targetNode, ChangeType action)
        {
            if (action == ChangeType.Move)
            {
                MoveNode(sourceNode, targetNode);
            }
            else if (action == ChangeType.Copy)
            {
                CopyNode(sourceNode, targetNode);
            }
        }

        private TestTreeNode nodeFromSystemPoint(Point point)
        {
            return GetNodeAt(PointToClient(point));
        }

        private bool isValidDrop(TestTreeNode source, TestTreeNode target)
        {
            bool isValid = false;

            if (source.IsTestSuite())
            {
                isValid = !target.IsTestStep() ? true : false;
            }
            else if (source.IsTestCase())
            {
            }
            else if (source.IsTestStep())
            {
            }

            return isValid;
        }

        /// <summary>
        /// Marks the current node and nodes up tree chain as changed and updates UI.
        /// </summary>
        /// <param name="node"></param>
        private void markAsChanged(TestTreeNode node)
        {
            if (!m_ignoreNodeChanges)
            {
                while (node != null)
                {
                    // Don't change order below.
                    node.HasChanged = true;
                    node.UpdateTitleAndToolTip();
                    node = node.Parent;
                }
            }
        }

        private void startLabelEdit()
        {
            TestTreeNode node = SelectedNode;

            if (node != null && node.TestScriptObject.Status != Status.Unavailable)
            {
                node.Text = node.TestScriptObject.Title;

                NodeLabelEditEventArgs e = new NodeLabelEditEventArgs(node);

                base.OnBeforeLabelEdit(e);
                LabelEdit = true;
                node.BeginEdit();
            }
        }

        private bool isRootNode(TestTreeNode node)
        {
            return node == this.Nodes[0] ? true : false;
        }

        private void resetChangeEventHistories()
        {
            m_changeHistory.Reset();
        }

        private void displayTestScriptObjectEditorDialog()
        {
            TestTreeNode node = SelectedNode;

            if (null != node)
            {
                Point position = Cursor.Position;

                if (node.TestScriptEditorDialog != null)
                {
                    node.TestScriptEditorDialog.Location = position;
                    node.TestScriptEditorDialog.Activate();
                }
                else
                {
                    node.TestScriptEditorDialog = new TestScriptObjectEditorDialog(CachedTestAssemblies, node.TestScriptObject);
                    node.TestScriptEditorDialog.Location = position;
                    node.TestScriptEditorDialog.Show();
                }
            }
        }

        private TestTreeNode m_parentNode;

        private TestTreeNode constructNodeTreeFragment(TestTreeNode parentNode, TestScriptObject startingTestScriptObject)
        {
            m_parentNode = parentNode;

            TestSuite.TraverseTestTree(startingTestScriptObject, new TestSuite.TraverseTestTreeDelegate(constructNodeTreeFragmentDelegate));

            return m_parentNode;
        }

        #region Undo/Redo methods

        private void undoChangeEvent(TestChangeEvent changeEvent)
        {
            BeginUpdate();

            switch (changeEvent.ChangeType)
            {
                case ChangeType.Update:
                    {
                        // Change property back to original value.
                        undoRedoNodePropertyChange(changeEvent);
                    }
                    break;
                case ChangeType.Add:  // If node was added, need to remove it.
                    {
                        //Removed recently added node
                        RemoveNode(changeEvent.ChangeObject, false);
                    }
                    break;
                case ChangeType.Remove:
                    {
                        // Reinsert recently removed node.
                        InsertNodeExt(changeEvent.ChangeObject, changeEvent.ChangeValues[0], false);
                    }
                    break;
                case ChangeType.Copy:
                    {
                        RemoveNode(changeEvent.ChangeObject, false);
                    }

                    break;
                case ChangeType.Move:
                    {
                        // Reverse the move.
                        MoveNode(changeEvent.ChangeObject, changeEvent.ChangeValues[1], false);
                    }
                    break;
                default:
                    { }
                    break;
            }

            // If not more changes, reset change flags and UI
            if (!m_changeHistory.CanUndo())
            {
                ResetHasChangedFlags();
            }

            m_recordHistory = true;

            EndUpdate();
        }

        private void redoChangeEvent(TestChangeEvent changeEvent)
        {
            BeginUpdate();

            switch (changeEvent.ChangeType)
            {
                case ChangeType.Update:
                    {
                        // Redo previously undone property change.
                        undoRedoNodePropertyChange(changeEvent, false);
                    }
                    break;
                case ChangeType.Add:  // If node was added, need to remove it.
                    {
                        InsertNodeExt(changeEvent.ChangeObject, changeEvent.ChangeValues[0], false);
                    }

                    break;
                case ChangeType.Remove:
                    {
                        RemoveNode(changeEvent.ChangeObject, false);
                    }

                    break;
                case ChangeType.Copy:
                    {
                        SelectedNode = CopyNode(changeEvent.ChangeValues[0],
                            changeEvent.ChangeValues[1], changeEvent.ChangeObject, false);
                    }

                    break;
                case ChangeType.Move:
                    {
                        MoveNode(changeEvent.ChangeObject, changeEvent.ChangeValues[0], false);
                    }
                    break;
                default:
                    { }
                    break;
            }

            EndUpdate();
        }

        private void undoRedoNodePropertyChange(TestChangeEvent changeEvent, bool isUndo = true)
        {
            // Turn painting off
            BeginUpdate();

            SelectedNode = changeEvent.ChangeObject;

            // Get changed property through reflection.
            Type type = changeEvent.ChangeValues[0].GetType();
            PropertyInfo pinfo = type.GetProperty(changeEvent.ChangeValues[1]);

            // Set property to old value.
            object value = isUndo ? changeEvent.ChangeValues[3] : changeEvent.ChangeValues[2];

            m_recordHistory = false;
            pinfo.SetValue(changeEvent.ChangeValues[0], value, null);
            m_recordHistory = true;

            // Update node UI if necessary.
            changeEvent.ChangeObject.UpdateTitleAndToolTip();
            changeEvent.ChangeObject.UpdateUI();

            // Turn painting on.
            EndUpdate();
        }

        #endregion

        #endregion  Private methods
    }
}
