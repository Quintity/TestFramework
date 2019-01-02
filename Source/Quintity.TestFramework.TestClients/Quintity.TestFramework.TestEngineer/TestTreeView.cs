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
            if (OnTestTreeNodeAdded != null)
            {
                OnTestTreeNodeAdded(this, nodeAdded);
            }
        }

        public delegate void TestTreeNodeRemoved(TestTreeView testTreeView, TestTreeNode nodeRemoved);
        public event TestTreeNodeRemoved OnTestTreeNodeRemoved;

        internal void fireTestTreeNodeRemovedEvent(TestTreeNode nodeRemoved)
        {
            if (OnTestTreeNodeRemoved != null)
            {
                OnTestTreeNodeRemoved(this, nodeRemoved);
            }
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

        private Dictionary<Guid, TestTreeNode> m_nodeMapping;

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
        private ToolStripMenuItem m_miReloadTestSuite;
        private ToolStripMenuItem m_miRename;
        private bool m_ignoreNodeChanges;

        #endregion

        #region Constructors

        public TestTreeView()
            : base()
        {
            m_ignoreNodeChanges = false;

            InitializeComponent();

            m_nodeMapping = new Dictionary<Guid, TestTreeNode>();

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
            this.m_miReloadTestSuite.Click += m_miReloadTestSuite_Click;
            this.m_miNewTestSuite.Click += m_miNewTestSuite_Click;
            this.m_miNewTestCase.Click += m_miNewTestCase_Click;
            this.m_miNewTestStep.Click += m_miNewTestStep_Click;
            this.m_miCut.Click += m_miCut_Click;
            this.m_miCopy.Click += m_miCopy_Click;
            this.m_miPaste.Click += m_miPaste_Click;
            this.m_miDelete.Click += m_miDelete_Click;
            this.m_miRename.Click += m_miRename_Click;

            // Set to catch timer system messages
            SetStyle(ControlStyles.EnableNotifyMessage, true);

            TestScriptObject.OnTestPropertyChanged += TestScriptObject_OnTestPropertyChanged;
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

        #endregion

        #region Public methods

        public void DisplayDefaultScreen()
        {
            this.ShowLines = false;

            var node = new TestTreeDefaultNode(
                @"                              " +
                "Select File->New to create a new test suite or File->Open to open an existing suite.");

            this.Nodes.Insert(0, node);

            this.ExpandAll();
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
            m_saveFileDialog.Title = "Save Test Suite As";
            m_saveFileDialog.InitialDirectory = TestProperties.TestSuites;
            m_saveFileDialog.RestoreDirectory = true;
            m_saveFileDialog.Filter = "Test suites (*.ste)|*.ste";
            m_saveFileDialog.FilterIndex = 1;

            TestTreeNode newNode = null;
            DialogResult result = m_saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                TestSuite testSuite = new TestSuite(
                    Path.GetFileNameWithoutExtension(m_saveFileDialog.FileName), m_saveFileDialog.FileName);

                var testcase = testSuite.AddTestCase(new TestCase("Untitled test case"));
                testcase.AddTestStep(new TestStep("Untitled test step"));

                insertTestSuite(testSuite, currentNode);
            }

            return newNode;
        }

        public TestTreeNode AddNewTestCase(TestTreeNode currentNode, bool recordHistory = true)
        {
            TestTreeNode newNode = null;
            TestCase testCase = new TestCase("Untitled test case");

            if (currentNode.IsTestSuite())
            {
                newNode = InsertNode(testCase, currentNode, -1, recordHistory);
            }
            else if (currentNode.IsTestCase())
            {
                newNode = InsertNode(testCase, currentNode.Parent, currentNode.Index + 1, recordHistory);
            }

            // Initialize test case with a single test step.
            AddNewTestStep(newNode, false);

            return newNode;
        }

        public TestTreeNode AddNewTestStep(TestTreeNode currentNode, bool recordHistory = true)
        {
            TestTreeNode newNode = null;

            // Create test step
            TestStep testStep = new TestStep("Untitled test step");

            if (currentNode.IsTestCase())
            {
                newNode = InsertNode(testStep, currentNode, -1, recordHistory);

            }
            else if (currentNode.IsTestStep())
            {
                newNode = InsertNode(testStep, currentNode.Parent, currentNode.Index + 1, recordHistory);
            }

            return newNode;
        }

        public TestTreeNode InsertNode(TestScriptObject testScriptObject, TestTreeNode parentNode, int nodeIndex, bool recordHistory = true)
        {
            TestScriptObjectContainer parentContainer = null;
            int newContainerIndex = 0;

            // Create new node for test script object.
            TestTreeNode newNode = new TestTreeNode(testScriptObject);

            if (null == parentNode)  // Must be root node.
            {
                Nodes.Clear();
                this.Nodes.Insert(0, newNode);
            }
            else
            {
                // Get parent's container object.
                parentContainer = parentNode.TestScriptObjectAsContainer();

                // Get previous sibling node's container index
                if (nodeIndex != -1)
                {
                    if (nodeIndex != 0)
                    {
                        TestTreeNode siblingNode = parentNode.Nodes[nodeIndex - 1] as TestTreeNode;  // Previous?

                        TestScriptObject siblingObject = siblingNode.TestScriptObject;

                        newContainerIndex = parentContainer.FindTestScriptObjectIndex(siblingObject) + 1;

                        nodeIndex = siblingNode.Index + 1;
                    }
                }
                else
                {
                    nodeIndex++;
                }

                // Add test script object to container to follow sibling in collection.
                parentContainer.AddTestScriptObject(testScriptObject, newContainerIndex);

                // Add to tree view parent node at node index.
                parentNode.Nodes.Insert(nodeIndex, newNode);
            }

            // Update UI
            markAsChanged(newNode);
            this.SelectedNode = newNode;
            //this.m_nodeMapping.Add(testScriptObject.SystemID, newNode);

            // Create change event with parents container object and index.
            if (recordHistory)
            {
                TestScriptObjectLocation location = new TestScriptObjectLocation(parentContainer, newContainerIndex);
                m_changeHistory.RecordEvent(new TestChangeEvent(testScriptObject, ChangeType.Add, location, null, newNode));
            }

            fireTestTreeNodeAddedEvent(newNode);

            return newNode;
        }

        public TestTreeNode CopyNode(TestTreeNode nodeToCopy, TestTreeNode targetNode, int nodeIndex, bool recordHistory = true)
        {
            // Get parent's container object.
            TestScriptObjectContainer parentContainer = nodeToCopy.Parent.TestScriptObjectAsContainer();
            int index = parentContainer.FindTestScriptObjectIndex(nodeToCopy.TestScriptObject);

            // Create copy of node's test script object
            TestScriptObject objectCopy = null;

            if (nodeToCopy.IsTestSuite())
            {
                objectCopy = new TestSuite(nodeToCopy.TestScriptObject as TestSuite, null, null);
            }
            else if (nodeToCopy.IsTestCase())
            {
                objectCopy = new TestCase(nodeToCopy.TestScriptObject as TestCase, null);
            }
            else if (nodeToCopy.IsTestStep())
            {
                objectCopy = new TestStep(nodeToCopy.TestScriptObject as TestStep, null);
            }

            // Change the singular item (not it's children if container) to indicate copy.
            objectCopy.Title = "Copy of " + objectCopy.Title;

            // Add node to tree
            TestTreeNode copyNode = InsertNode(objectCopy, targetNode, nodeIndex, recordHistory);

            // Add copied objects children to copyNode;
            constructNodeTreeFragment(copyNode, objectCopy);

            return copyNode;
        }

        public void RemoveNode(TestTreeNode nodeToRemove, bool recordHistory = true)
        {
            // Get parent's container object.
            TestTreeNode parentNode = nodeToRemove.Parent;
            TestScriptObjectContainer parentContainer = nodeToRemove.Parent.TestScriptObjectAsContainer();
            int index = parentContainer.FindTestScriptObjectIndex(nodeToRemove.TestScriptObject);

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
                TestScriptObjectLocation location = new TestScriptObjectLocation(parentContainer, index);
                m_changeHistory.RecordEvent(new TestChangeEvent(nodeToRemove.TestScriptObject, ChangeType.Remove, null, location, nodeToRemove));
            }
        }

        public void MoveNode(TestTreeNode nodeToMove, TestTreeNode parentNode, int nodeIndex, bool recordHistory = true)
        {
            // Get original parent's container object.
            TestTreeNode originalParentNode = nodeToMove.Parent;
            TestScriptObjectContainer originalParentContainer = originalParentNode.TestScriptObjectAsContainer();
            int oldIndex = originalParentContainer.FindTestScriptObjectIndex(nodeToMove.TestScriptObject);

            // Get parent's container object.
            TestScriptObjectContainer targetContainer = parentNode.TestScriptObjectAsContainer();

            // Get target previous sibling node's container index
            int newContainerIndex = 0;

            if (nodeIndex != -1)
            {
                // Previous sibling node
                TestTreeNode siblingNode = parentNode.Nodes[nodeIndex] as TestTreeNode;  // Previous?

                // Get siblings container object index, will insert directly after.
                TestScriptObject siblingObject = siblingNode.TestScriptObject;

                // Remove test script object from original parent (may be same parent).
                originalParentContainer.RemoveTestScriptObject(nodeToMove.TestScriptObject);

                // Determine new index (removed script may preceed in same collection).
                newContainerIndex = targetContainer.FindTestScriptObjectIndex(siblingObject) + 1;

                // Remove node from original parent node and reset node index (in case node removed in same collection).
                parentNode.Nodes.Remove(nodeToMove);
                nodeIndex = siblingNode.Index + 1;  // Insert after sibling.
            }
            else
            {
                // Remove test script object from original parent (may be same parent).
                originalParentContainer.RemoveTestScriptObject(nodeToMove.TestScriptObject);

                // Remove node from original parent node
                parentNode.Nodes.Remove(nodeToMove);

                nodeIndex++;
            }

            // Add test script object to container to follow sibling in collection.
            targetContainer.AddTestScriptObject(nodeToMove.TestScriptObject, newContainerIndex);

            // Add node to move to new parent.
            parentNode.Nodes.Insert(nodeIndex, nodeToMove);

            // Update UI for moved node and previous parent
            markAsChanged(originalParentNode);
            markAsChanged(nodeToMove);

            this.SelectedNode = nodeToMove;

            // Create change event with parents container object and index.
            if (recordHistory)
            {
                // Create old location object
                TestScriptObjectLocation oldLocation = new TestScriptObjectLocation(originalParentContainer, oldIndex);

                // Create new location object
                TestScriptObjectLocation newLocation = new TestScriptObjectLocation(targetContainer, newContainerIndex);

                m_changeHistory.RecordEvent(new TestChangeEvent(nodeToMove.TestScriptObject, ChangeType.Move, newLocation, oldLocation, nodeToMove));
            }
        }

        public TestTreeNode SetTestSuite(TestSuite testSuite)
        {
            m_testSuite = testSuite;
            m_nodeMapping.Clear();

            BeginUpdate();

            Nodes.Clear();

            m_ignoreNodeChanges = true;

            var newNode = new TestTreeNode(testSuite);
            this.Nodes.Add(newNode);
            // this.m_nodeMapping.Add(testSuite.SystemID, newNode);

            var start = DateTime.Now;
            constructNodeTreeFragment(RootNode, testSuite);
            var elapsed = DateTime.Now - start;
            var count = m_nodeMapping.Count;

            m_ignoreNodeChanges = false;

            ShowAllTestCases();
            SelectedNode = RootNode;
            RootNode.EnsureVisible();

            EndUpdate();

            return newNode;
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

        private TestTreeNode getNodeMapping(TestScriptObject testScriptObject)
        {
            return getNodeMapping(testScriptObject.SystemID);
        }

        private TestTreeNode getNodeMapping(Guid systemId)
        {
            TestTreeNode mappedNode;

            m_nodeMapping.TryGetValue(systemId, out mappedNode);

            return mappedNode;

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

        public bool RedoAvaible()
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
            var testPropertyOverrides = getTestProperityOverrides("LocalTest");

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
            catch(Exception e)
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

        private TestFilter m_currentFilter;
        private List<TestCase> m_qualifiedTestCases;

        private bool filterTestTree(TestScriptObject testScriptObject, object tag)
        {
            TestTreeNode currentNode = getNodeMapping(testScriptObject);
            TestTreeNode parentNode = getNodeMapping(testScriptObject.ParentID);

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

            //if (e.Button == MouseButtons.Left)
            //{
            //    if (SelectedNode == GetNodeAt(e.X, e.Y))
            //    {
            //        if (_wasDoubleClick)
            //        {
            //            _wasDoubleClick = false;
            //        }
            //        else
            //        {
            //            _triggerLabelEdit = true;
            //        }
            //    }
            //}

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
        { }

        protected override void OnDragEnter(DragEventArgs e)
        {
            var node = nodeFromSystemPoint(new Point(e.X, e.Y));

            if (e.KeyState == 1)
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.KeyState == 9)
            {
                e.Effect = DragDropEffects.Copy;
            }

            string[] formats = e.Data.GetFormats();

            object spud = e.Data.GetData(formats[0]);
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            TestTreeNode targetNode = nodeFromSystemPoint(new Point(e.X, e.Y));

            TestTreeNode sourceNode = e.Data.GetData(typeof(TestTreeNode)) as TestTreeNode;
            Debug.WriteLine(string.Format("OnDragOver - Node:  {0}",
                targetNode != null ? targetNode.TestScriptObject.Title : "Not over tree node."));

            SelectedNode = targetNode;

            if (sourceNode.Parent == null || (sourceNode.IsTestStep() && targetNode.IsTestSuite()))
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                if (e.KeyState == 1)
                {
                    e.Effect = DragDropEffects.Move;
                }
                else if (e.KeyState == 9)
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
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
                TestTreeNode targetNode = nodeFromSystemPoint(new Point(e.X, e.Y));
                TestTreeNode sourceNode = e.Data.GetData(typeof(TestTreeNode)) as TestTreeNode;
                performTreeEdit(
                    e.Data.GetData(typeof(TestTreeNode)) as TestTreeNode,  // Source node.
                    nodeFromSystemPoint(new Point(e.X, e.Y)), // Target node
                    action);  // Action
            }
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
            this.m_miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_treeViewImages = new System.Windows.Forms.ImageList(this.components);
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_miReloadTestSuite = new System.Windows.Forms.ToolStripMenuItem();
            this.m_miRename = new System.Windows.Forms.ToolStripMenuItem();
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
            this.m_miReloadTestSuite,
            this.m_toolStripSeparator5,
            this.m_miCut,
            this.m_miCopy,
            this.m_miPaste,
            this.m_miDelete,
            this.m_miRename});
            this.m_treeViewContextMenu.Name = "m_treeViewContextMenu";
            this.m_treeViewContextMenu.Size = new System.Drawing.Size(202, 386);
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
            this.m_miExecute.Name = "m_miExecute";
            this.m_miExecute.Size = new System.Drawing.Size(201, 22);
            this.m_miExecute.Text = "Execute";
            // 
            // m_miResetResults
            // 
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
            // m_miCut
            // 
            this.m_miCut.Name = "m_miCut";
            this.m_miCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.m_miCut.Size = new System.Drawing.Size(201, 22);
            this.m_miCut.Text = "Cut";
            // 
            // m_miCopy
            // 
            this.m_miCopy.Name = "m_miCopy";
            this.m_miCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.m_miCopy.Size = new System.Drawing.Size(201, 22);
            this.m_miCopy.Text = "Copy";
            // 
            // m_miPaste
            // 
            this.m_miPaste.Name = "m_miPaste";
            this.m_miPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.m_miPaste.Size = new System.Drawing.Size(201, 22);
            this.m_miPaste.Text = "Paste";
            // 
            // m_miDelete
            // 
            this.m_miDelete.Name = "m_miDelete";
            this.m_miDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.m_miDelete.Size = new System.Drawing.Size(201, 22);
            this.m_miDelete.Text = "Del";
            // 
            // m_miReloadTestSuite
            // 
            this.m_miReloadTestSuite.Name = "m_miReloadTestSuite";
            this.m_miReloadTestSuite.Size = new System.Drawing.Size(201, 22);
            this.m_miReloadTestSuite.Text = "Reload Test Suite...";
            this.m_miReloadTestSuite.ToolTipText = "Reload an existing test suite.";
            // 
            // m_miRename
            // 
            this.m_miRename.Name = "m_miRename";
            this.m_miRename.ShortcutKeys = Keys.F2;
            this.m_miRename.Size = new System.Drawing.Size(201, 22);
            this.m_miRename.Text = "Rename";
            this.m_miRename.ToolTipText = "Rename the selected item.";
            // 
            // m_treeViewImages
            // 
            this.m_treeViewImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_treeViewImages.ImageStream")));
            this.m_treeViewImages.TransparentColor = System.Drawing.Color.Transparent;
            this.m_treeViewImages.Images.SetKeyName(0, "folder.closed.bmp");
            this.m_treeViewImages.Images.SetKeyName(1, "folder.open.bmp");
            this.m_treeViewImages.Images.SetKeyName(2, "teststep.active.bmp");
            this.m_treeViewImages.Images.SetKeyName(3, "teststep.error.bmp");
            this.m_treeViewImages.Images.SetKeyName(4, "teststep.inactive.bmp");
            this.m_treeViewImages.Images.SetKeyName(5, "teststep.fail.bmp");
            this.m_treeViewImages.Images.SetKeyName(6, "teststep.pass.bmp");
            this.m_treeViewImages.Images.SetKeyName(7, "teststep.incomplete.bmp");
            this.m_treeViewImages.Images.SetKeyName(8, "folder.closed.bmp");
            this.m_treeViewImages.Images.SetKeyName(9, "folder.closed.deactivated.bmp");
            this.m_treeViewImages.Images.SetKeyName(10, "folder.closed.fail.bmp");
            this.m_treeViewImages.Images.SetKeyName(11, "folder.closed.pass.bmp");
            this.m_treeViewImages.Images.SetKeyName(12, "folder.open.bmp");
            this.m_treeViewImages.Images.SetKeyName(13, "folder.open.deactivated.bmp");
            this.m_treeViewImages.Images.SetKeyName(14, "folder.open.fail.bmp");
            this.m_treeViewImages.Images.SetKeyName(15, "folder.open.pass.bmp");
            this.m_treeViewImages.Images.SetKeyName(16, "execution.arrow.bmp");
            this.m_treeViewImages.Images.SetKeyName(17, "teststep.manual.active.bmp");
            this.m_treeViewImages.Images.SetKeyName(18, "teststep.manual.error.bmp");
            this.m_treeViewImages.Images.SetKeyName(19, "teststep.manual.fail.bmp");
            this.m_treeViewImages.Images.SetKeyName(20, "teststep.manual.inactive.bmp");
            this.m_treeViewImages.Images.SetKeyName(21, "teststep.manual.pass.bmp");
            this.m_treeViewImages.Images.SetKeyName(22, "folder.closed.didnotexecute.bmp");
            this.m_treeViewImages.Images.SetKeyName(23, "folder.open.didnotexecute.bmp");

            // 
            // TestTreeView
            // 
            this.LineColor = System.Drawing.Color.Black;
            this.DoubleClick += new System.EventHandler(this.TestTreeView_DoubleClick);
            this.m_treeViewContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

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
            if (sourceNode.IsTestSuite())
            {
                if (targetNode.IsTestSuite())
                {
                    // Add/Copy as first child of target suite.
                    if (action == ChangeType.Move)
                    {
                        MoveNode(sourceNode, targetNode, -1);
                    }
                    else if (action == ChangeType.Copy)
                    {
                        CopyNode(sourceNode, targetNode, -1);
                    }
                }
                else if (targetNode.IsTestCase())
                {
                    // Add/Copy immediately after target test case.
                    if (action == ChangeType.Move)
                    {
                        MoveNode(sourceNode, targetNode.Parent, targetNode.Index + 1);
                    }
                    else if (action == ChangeType.Copy)
                    {
                        CopyNode(sourceNode, targetNode.Parent, targetNode.Index + 1);
                    }
                }
                //else if (targetNode.IsTestStep())
                //{
                //    // Add/Copy immediately after target steps parent case's parent.
                //    if (action == ChangeType.Move)
                //    {
                //        MoveNode(sourceNode, targetNode.Parent.Parent, targetNode.Index);
                //    }
                //    else if (action == ChangeType.Copy)
                //    {
                //        CopyNode(sourceNode, targetNode.Parent.Parent, targetNode.Index);
                //    }
                //}
            }
            else if (sourceNode.IsTestCase())
            {
                if (targetNode.IsTestSuite())
                {
                    // Add/Copy as first child of target suite.
                    if (action == ChangeType.Move)
                    {
                        MoveNode(sourceNode, targetNode, -1);
                    }
                    else if (action == ChangeType.Copy)
                    {
                        CopyNode(sourceNode, targetNode, -1);
                    }
                }
                else if (targetNode.IsTestCase())
                {
                    if (action == ChangeType.Move)
                    {
                        MoveNode(sourceNode, targetNode.Parent, targetNode.Index + 1);
                    }
                    else if (action == ChangeType.Copy)
                    {
                        CopyNode(sourceNode, targetNode.Parent, targetNode.Index + 1);
                    }
                }
                else if (targetNode.IsTestStep())
                {
                    // Add/Copy immediately after target steps parent case.
                    if (action == ChangeType.Move)
                    {
                        MoveNode(sourceNode, targetNode.Parent.Parent, targetNode.Parent.Index);
                    }
                    else if (action == ChangeType.Copy)
                    {
                        CopyNode(sourceNode, targetNode.Parent.Parent, targetNode.Parent.Index);
                    }

                    SelectedNode = sourceNode;
                }
            }
            else if (sourceNode.IsTestStep())
            {
                if (targetNode.IsTestCase())
                {
                    // Add/Copy as first child of target case.
                    if (action == ChangeType.Move)
                    {
                        MoveNode(sourceNode, targetNode, -1);
                    }
                    else if (action == ChangeType.Copy)
                    {
                        CopyNode(sourceNode, targetNode, -1);
                    }
                }
                else if (targetNode.IsTestStep())
                {
                    // Add/Copy immediately after target step.
                    if (action == ChangeType.Move)
                    {
                        MoveNode(sourceNode, targetNode.Parent, targetNode.Index + 1);
                    }
                    else if (action == ChangeType.Copy)
                    {
                        CopyNode(sourceNode, targetNode.Parent, targetNode.Index + 1);
                    }
                }
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

        private void removeTestSuiteNode(TestTreeNode testSuiteNode)
        {
            var testSuite = testSuiteNode.Parent.TestScriptObjectAsTestSuite();

            int index = testSuite.TestScriptObjects.FindIndex(x => x.SystemID == testSuiteNode.TestScriptObject.SystemID);

            var currentLocation = new TestScriptObjectLocation(testSuiteNode.Parent.TestScriptObjectAsContainer(), index);

            removeNode(testSuiteNode);

            if (m_recordHistory)
            {
                m_changeHistory.RecordEvent(new TestChangeEvent(testSuiteNode.TestScriptObject, ChangeType.Remove, currentLocation, testSuiteNode));
            }
        }

        private void removeTestCaseNode(TestTreeNode testCaseNode)
        {
            var testSuite = testCaseNode.Parent.TestScriptObjectAsTestSuite();

            int index = testSuite.TestScriptObjects.FindIndex(x => x.SystemID == testCaseNode.TestScriptObject.SystemID);

            var currentLocation = new TestScriptObjectLocation(testCaseNode.Parent.TestScriptObjectAsContainer(), index);

            removeNode(testCaseNode);

            if (m_recordHistory)
            {
                m_changeHistory.RecordEvent(new TestChangeEvent(testCaseNode.TestScriptObject, ChangeType.Remove, currentLocation, testCaseNode));
            }
        }

        private void removeTestStepNode(TestTreeNode testStepNode)
        {
            // Get parent test case.
            var testCase = testStepNode.Parent.TestScriptObjectAsTestCase();

            int index = testCase.TestSteps.FindIndex(x => x.SystemID == testStepNode.TestScriptObject.SystemID);

            // Get current location for change history
            var currentLocation = new TestScriptObjectLocation(testStepNode.Parent.TestScriptObjectAsContainer(), index);

            // Remove node from tree
            removeNode(testStepNode);

            if (m_recordHistory)
            {
                m_changeHistory.RecordEvent(new TestChangeEvent(testStepNode.TestScriptObject, ChangeType.Remove, currentLocation, testStepNode));
            }
        }

        private void removeNode(TestTreeNode nodeToRemove)
        {
            var parentNode = FindNode(nodeToRemove.TestScriptObject.ParentID);
            var parent = parentNode.TestScriptObject as TestScriptObjectContainer;
            parent.RemoveTestScriptObject(nodeToRemove.TestScriptObject);
            parentNode.Nodes.Remove(nodeToRemove);
            markAsChanged(parentNode);
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

        private void m_changeHistory_OnTestHistoryUndoEvent(TestChangeEvent e)
        {
            undoChangeEvent(e);
        }

        private void m_changeHistory_OnTestHistoryRedoEvent(TestChangeEvent e)
        {
            redoChangeEvent(e);
        }

        private void undoChangeEvent(TestChangeEvent changeEvent)
        {
            TestTreeNode changeEventNode = changeEvent.Tag as TestTreeNode;

            m_recordHistory = false;

            BeginUpdate();

            switch (changeEvent.ChangeType)
            {
                case ChangeType.Update:
                    {
                        undoRedoNodePropertyChange(changeEvent);
                    }
                    break;
                case ChangeType.Add:  // If node was added, need to remove it.
                    {
                        RemoveNode(changeEventNode, false);
                    }

                    break;
                case ChangeType.Remove:
                    {
                        TestScriptObjectLocation location = changeEvent.FormerValue as TestScriptObjectLocation;
                        location.Parent.AddTestScriptObject(changeEvent.TestScriptObject, location.Index);
                        Filter(m_currentFilter);

                        TestTreeNode parentNode = FindNode(location.Parent);

                        if (parentNode != null)
                        {
                            parentNode.Expand();
                            markAsChanged(changeEventNode);
                            SelectedNode = changeEventNode;
                        }
                    }
                    break;
                case ChangeType.Copy:
                    {
                        RemoveNode(changeEventNode, false);
                    }

                    break;
                case ChangeType.Move:
                    {
                        TestScriptObjectLocation formerLocation = changeEvent.FormerValue as TestScriptObjectLocation;
                        // MoveNode(changeEventNode, formerLocation.Parent, formerLocation.Index);
                    }
                    break;
                default:
                    { }
                    break;
            }

            m_recordHistory = true;

            EndUpdate();
        }

        private void redoChangeEvent(TestChangeEvent changeEvent)
        {
            TestTreeNode changeEventNode = getNodeMapping(changeEvent.TestScriptObject);

            switch (changeEvent.ChangeType)
            {
                case ChangeType.Update:
                    {
                        undoRedoNodePropertyChange(changeEvent, false);
                    }
                    break;
                case ChangeType.Add:  // If node was added, need to remove it.
                    {
                        TestScriptObjectLocation location = changeEvent.CurrentValue as TestScriptObjectLocation;
                        location.Parent.AddTestScriptObject(changeEvent.TestScriptObject, location.Index);

                        Filter(m_currentFilter);

                        TestTreeNode parentNode = FindNode(location.Parent);

                        if (parentNode != null)
                        {
                            parentNode.Expand();
                            SelectedNode = changeEventNode;
                        }
                    }

                    break;
                case ChangeType.Remove:
                    {
                        removeNode(changeEventNode);
                    }

                    break;
                case ChangeType.Copy:
                    {
                        TestScriptObjectLocation location = changeEvent.CurrentValue as TestScriptObjectLocation;
                        //location.Parent.Nodes.Insert(location.Index, changeEventNode);
                        SelectedNode = changeEventNode;
                    }

                    break;
                case ChangeType.Move:
                    {
                        TestScriptObjectLocation location = changeEvent.CurrentValue as TestScriptObjectLocation;
                        //MoveNode(changeEventNode, location.Parent, location.Index);
                    }
                    break;
                default:
                    { }
                    break;
            }
        }

        private void undoRedoNodePropertyChange(TestChangeEvent changeEvent, bool undo = true)
        {
            // Want to provide visual response to user.
            SelectedNode = getNodeMapping(changeEvent.TestScriptObject);
            TestScriptObject testScriptObject = null;

            if (changeEvent.Tag is TestPreprocessor)
            {
                testScriptObject = changeEvent.Tag as TestScriptObject;

            }
            else if (changeEvent.Tag is TestPostprocessor)
            {
                testScriptObject = changeEvent.Tag as TestScriptObject;
            }
            else
            {
                testScriptObject = changeEvent.TestScriptObject;
            }

            // Get changed property through reflection.
            Type type = testScriptObject.GetType();
            PropertyInfo pinfo = type.GetProperty(changeEvent.Property);

            // Set property to old value.
            object value = undo ? changeEvent.FormerValue : changeEvent.CurrentValue;
            pinfo.SetValue(testScriptObject, value, null);

            // Give a moment a second for user to catch up.
            Thread.Sleep(125);

            // Bit of a hack, forces node to repaint so user can see change occur.
            SelectedNode = RootNode;
            SelectedNode = getNodeMapping(changeEvent.TestScriptObject);

            // Update node UI if necessary.
            SelectedNode.UpdateTitleAndToolTip();
            SelectedNode.UpdateUI();
        }
        #endregion

        private void TestScriptObject_OnTestPropertyChanged(TestScriptObject testScriptObject, TestPropertyChangedEventArgs args)
        {
            TestTreeNode node = null;

            if (testScriptObject is TestProcessor)
            {
                node = FindNode(testScriptObject.ParentID);
            }
            else
            {
                node = FindNode(testScriptObject);
            }

            markAsChanged(node);

            if (m_recordHistory)
            {
                m_changeHistory.RecordEvent(new TestChangeEvent(testScriptObject, ChangeType.Update,
                    args.Property, args.CurrentValue, args.FormerValue, testScriptObject));
            }
        }

        private void TestTreeView_OnTestTreeChanged(TestTreeView testTreeView, TestTreeChangedEventArgs args)
        {
            switch (args.NodeAction)
            {
                case ChangeType.Add:
                    break;
                case ChangeType.Remove:
                    break;
                case ChangeType.Move:
                    break;
                default:
                    break;
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
                //m_nodeMapping.Add(testScriptObject.SystemID, newNode);
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
                parentContainer.AddTestScriptObject(testTreeNode.TestScriptObject);
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
            m_miReloadTestSuite.Visible = selectedNode.IsTestSuite() ? true : false;

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

            bool containsData = Clipboard.ContainsData("SystemID");

            if (containsData && !unavailable)
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
            else
            {
                m_miPaste.Enabled = false;
                m_miPaste.ToolTipText = "The application clipboard is empty.";
            }

            m_miResetResults.Enabled = selectedNode.TestScriptResult != null ? true : false;
            m_miExecute.Enabled = selectedNode.TestScriptObject.Status == Status.Active ? true : false;
            m_miSaveResults.Enabled = selectedNode.TestScriptResult != null ? true : false;
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
            m_openFileDialog.Reset();
            m_openFileDialog.Title = "Select test suite to add";
            m_openFileDialog.InitialDirectory = TestProperties.TestSuites;
            m_openFileDialog.Filter = "Test suites (*.ste)|*.ste";
            m_openFileDialog.FilterIndex = 1;

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
                        insertTestSuite(testSuite, SelectedNode);
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
        }

        private void m_miReloadTestSuite_Click(object sender, EventArgs e)
        {
            var targetNode = SelectedNode;
            var testSuite = targetNode.TestScriptObject as TestSuite;
            testSuite = TestSuite.ReadFromFile(testSuite.FilePath);

            //if (testSuite.Status != Status.Unavailable)
            //{
            BeginUpdate();
            insertTestSuite(testSuite, targetNode, false);
            removeTestSuiteNode(targetNode);
            EndUpdate();
            //}
            //else
            //{
            // In case load is failing to another reason (update).
            //targetNode.TestScriptObject = testSuite;
            //targetNode.UpdateToolTip();
            //}
        }

        /// <summary>
        /// Insert a test into the treeview.  If the target node is test suite,
        /// the user is prompted whether insert as child or sibling of target suite.
        /// If the target is a test case, the suite is insert below the target case
        /// as a sibling.
        /// </summary>
        /// <param name="testSuite">Test suite object to insert.</param>
        /// <param name="targetNode">Target node to insert.</param>
        private void insertTestSuite(TestSuite testSuite, TestTreeNode targetNode, bool relationshipPrompt = true)
        {
            TestTreeNode newNode = null;

            // Turn painting off
            BeginUpdate();

            if (null == targetNode)  // Must be root node.
            {
                newNode = InsertNode(testSuite, null, -1, false);
            }
            else if (targetNode.IsTestSuite())
            {
                if (!isRootNode(targetNode))
                {
                    var sibling = true;

                    if (relationshipPrompt)
                    {
                        var relationshipDlg = new TestsuiteRelationshipDialog();
                        var result = relationshipDlg.ShowDialog(this);
                        sibling = result == DialogResult.Yes ? true : false;
                    }

                    newNode = sibling ? newNode = InsertNode(testSuite, targetNode.Parent, targetNode.Index + 1, true) : InsertNode(testSuite, targetNode, -1, true);

                    //if (result == DialogResult.No)  // No adds as child
                    //{
                    //    newNode = InsertNode(testSuite, targetNode, -1, true);
                    //}
                    //else if (result == DialogResult.Yes)  // Yes adds as sibling
                    //{
                    //    newNode = InsertNode(testSuite, targetNode.Parent, targetNode.Index + 1, true);
                    //}
                }
                else  // If root, don't prompt, have to add as child.
                {
                    newNode = InsertNode(testSuite, targetNode, targetNode.Nodes.Count, true);
                }
            }
            else if (targetNode.IsTestCase())
            {
                newNode = InsertNode(testSuite, targetNode.Parent, targetNode.Index + 1, true);
            }

            if (newNode != null)
            {
                constructNodeTreeFragment(newNode, testSuite);
                newNode.Expand();
                SelectedNode = newNode;
            }

            EndUpdate();
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

        private void m_miCut_Click(object sender, EventArgs e)
        {
            if (SelectedNode.TestScriptObject.Status != Status.Unavailable)
            {
                var systemId = SelectedNode.TestScriptObject.SystemID;
                Clipboard.SetData("SystemID", systemId);
                m_clipboardAction = ChangeType.Move;
            }
        }

        private void m_miCopy_Click(object sender, EventArgs e)
        {
            if (SelectedNode.TestScriptObject.Status != Status.Unavailable)
            {
                var systemId = SelectedNode.TestScriptObject.SystemID;
                Clipboard.SetData("SystemID", systemId);
                m_clipboardAction = ChangeType.Copy;
            }
        }

        private void m_miPaste_Click(object sender, EventArgs e)
        {
            TestTreeNode target = SelectedNode;

            Guid systemID = (Guid)Clipboard.GetData("SystemID");

            performTreeEdit(systemID, target, m_clipboardAction);

            if (m_clipboardAction == ChangeType.Move)
            {
                m_clipboardAction = ChangeType.Unknown;
            }
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

        #endregion
    }
}
