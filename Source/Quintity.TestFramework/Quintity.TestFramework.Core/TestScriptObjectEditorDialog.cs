using System;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading;

namespace Quintity.TestFramework.Core
{
    public partial class TestScriptObjectEditorDialog : Form
    {
        #region Delegates and event declarations

        public class TestScriptObjectEditorActivatedArgs : EventArgs
        {
            private TestScriptObject m_testObject;

            public TestScriptObjectEditorActivatedArgs(TestScriptObject testObject)
            {
                m_testObject = testObject;
            }

            /// <summary>
            /// Gets the dialog's .
            /// </summary>
            public TestScriptObject TestScriptObject
            {
                get { return m_testObject; }
            }
        }

        public class TestScriptObjectEditorClosedArgs : EventArgs
        {
            private TestScriptObject m_testObject;
            private bool m_contentChanged;

            public TestScriptObjectEditorClosedArgs(TestScriptObject testObject, bool contentChanged)
            {
                m_testObject = testObject;
                m_contentChanged = contentChanged;
            }

            /// <summary>
            /// Gets the dialog's TestStep.
            /// </summary>
            public TestScriptObject TestScriptObject
            {
                get { return m_testObject; }
            }

            public bool ContentChanged
            {
                get { return m_contentChanged; }
            }
        }

        public delegate void TestScriptObjectEditorActivated(object sender, TestScriptObjectEditorActivatedArgs e);
        public static event TestScriptObjectEditorActivated OnTestScriptObjectEditorActivated;

        public delegate void TestScriptObjectEditorClosed(object sender, TestScriptObjectEditorClosedArgs e);
        public static event TestScriptObjectEditorClosed OnTestScriptObjectEditorClosed;

        public delegate void ExecuteTestStepHandler();
        public static event ExecuteTestStepHandler OnExecuteTestStep;

        internal static void fireExecuteTestStepEvent()
        {
            OnExecuteTestStep?.Invoke();
        }

        #endregion

        #region Data fields

        // Column header values.
        private const int ParameterColumn = 0;
        private const int DataTypeColumn = 1;
        private const int ValueColumn = 2;

        private bool m_contentChanged;
        private TestScriptObject m_testScriptObject;
        private StringCollection m_testAssemblyCache;

        #endregion

        #region Constructors

        public TestScriptObjectEditorDialog(TestScriptObject testScriptObject)
        {
            m_testScriptObject = testScriptObject;

            InitializeComponent();

            initializeDialogAppearance();
        }

        public TestScriptObjectEditorDialog(StringCollection cachedAssemblies, TestScriptObject testScriptObject)
        {
            m_testScriptObject = testScriptObject;
            m_testAssemblyCache = cachedAssemblies;

            InitializeComponent();

            initializeDialogAppearance();

            m_testAssemblytreeView.NodeMouseClick += m_testAssemblytreeView_NodeMouseClick;
            m_resetToolStripMenuItem.Click += m_resetToolStripMenuItem_Click;
            m_executeToolStripMenuItem.Click += m_executeToolStripMenuItem_Click;
        }

        private void m_resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            populateTestParametersGrid(m_testAssemblytreeView.SelectedNode.Tag as MethodInfo);
        }

        private void m_executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (m_testScriptObject is TestStep)
            //{
            //    _executionEvent = new ManualResetEvent(false);

            //    TestExecutor.OnExecutionComplete += _OnExecutionComplete;

            //    TestAutomationDefinition currentDefinition = null;

            //    var testStep = m_testScriptObject as TestStep;

            //    try
            //    {
            //        currentDefinition = testStep.TestAutomationDefinition;

            //        var newdefinition = getTestAutomationDefinition();

            //        testStep.TestAutomationDefinition = newdefinition;

            //        new TestExecutor().ExecuteTestStep(testStep, false);

            //        Cursor = Cursors.WaitCursor;
            //        _executionEvent.WaitOne();
            //        Cursor = Cursors.Default;

            //    }
            //    catch
            //    {
            //        throw;
            //    }
            //    finally  // Always want to reset.
            //    {
            //        testStep.TestAutomationDefinition = currentDefinition;
            //        TestExecutor.OnExecutionComplete -= _OnExecutionComplete;
            //    }
            //}
        }

        private void m_testAssemblytreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_testAssemblytreeView.SelectedNode = e.Node;
                e.Node.ContextMenuStrip = m_treeViewPopupMenu;
            }
        }

        #endregion

        #region Properties

        #endregion

        #region Public methods

        public void populateTestAssemblyTreeView()
        {
            if (m_testAssemblyCache != null)
            {
                foreach (string testAssembly in m_testAssemblyCache)
                {
                    if (isAssemblyLoaded(testAssembly) == null)
                    {
                        try
                        {
                            addTestAssembly(testAssembly);
                        }
                        catch  //Eat any loading exception and proceed to next.  This is just helper material.
                        {
                            ;
                        }
                    }
                }

                m_testAssemblytreeView.Nodes[0].Expand();
            }
        }

        public StringCollection GetCachedTestAssemblies()
        {
            m_testAssemblyCache.Clear();

            var nodes = m_testAssemblytreeView.Nodes[0].Nodes;

            foreach (TreeNode node in nodes)
            {
                m_testAssemblyCache.Add(((string)node.Tag));
            }

            return m_testAssemblyCache;
        }

        #endregion

        #region Private methods

        bool m_isInitializingDialog;

        private void initializeDialogAppearance()
        {
            m_isInitializingDialog = true;

            if (m_testScriptObject is TestSuite)
            {
                initializeDialogForTestSuite();
            }
            else if (m_testScriptObject is TestCase)
            {
                initializeDialogForTestCase();
            }
            if (m_testScriptObject is TestStep)
            {
                initializeDialogForTestStep();
            }
            else if (m_testScriptObject is TestProcessor)
            {
                initializeDialogForTestProcessor();
            }

            m_isInitializingDialog = false;
        }

        private List<string> readConfigCachedAssemblies()
        {
            List<string> assemblies = new List<string>();

            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = @"Quintity.TestFramework.TestWinFormApp.exe.config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
            ConfigurationSectionGroup appSettingsGroup = config.GetSectionGroup("applicationSettings");

            if (appSettingsGroup != null)
            {
                ConfigurationSectionCollection sections = appSettingsGroup.Sections;
                ClientSettingsSection section = (ClientSettingsSection)sections["Quintity.TestFramework.TestWinFormApp.Properties.Settings"];

                ConfigurationElement element = section.Settings.Get("TestAssemblies");

                if (null != element)
                {
                    StringBuilder sb = new StringBuilder();

                    string xml = ((SettingElement)element).Value.ValueXml.InnerXml;
                    XmlSerializer xs = new XmlSerializer(typeof(string[]));
                    string[] strings = (string[])xs.Deserialize(new XmlTextReader(xml, XmlNodeType.Element, null));
                    assemblies.AddRange(strings);
                }

            }

            return assemblies;
        }

        private void initializeDialogForTestSuite()
        {
            // Hide expected text field
            m_expectedLabel.Enabled = false;
            m_expectedLabel.Visible = false;
            m_expectedText.Enabled = false;
            m_expectedText.Visible = false;
            m_automatecheckBox.Enabled = false;
            m_automatecheckBox.Visible = false;

            // Need to allow description edit control to expand with dialog (since expected no longer appears).
            m_descText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right |
                System.Windows.Forms.AnchorStyles.Bottom)));

            // Enlarge text field
            m_descText.Height = 309;

            m_titleText.Text = m_testScriptObject.Title;
            m_descText.Text = m_testScriptObject.Description;

            setActivateCheckBox();

            // Remove test method tabpage until needed.
            m_editorTabs.Controls.Remove(m_automationDefinitionTab);
        }

        private void initializeDialogForTestCase()
        {
            // Hide expected text field
            m_expectedLabel.Enabled = false;
            m_expectedLabel.Visible = false;
            m_expectedText.Enabled = false;
            m_expectedText.Visible = false;

            // Set title field text.
            m_titleText.Text = m_testScriptObject.Title;

            // Enlarge text field
            m_descText.Height = 309;
            m_descText.Text = m_testScriptObject.Description;

            // Remove test method tabpage until needed.
            m_editorTabs.Controls.Remove(m_automationDefinitionTab);

            // Hide automated check box (not applicable).
            m_automatecheckBox.Checked = false;
            m_automatecheckBox.Visible = false;

            setActivateCheckBox();
        }

        private void initializeDialogForTestStep()
        {
            var testStep = m_testScriptObject as TestStep;

            // Hide expected text field
            m_expectedLabel.Enabled = true;
            m_expectedLabel.Visible = true;
            m_expectedText.Enabled = true;
            m_expectedText.Visible = true;

            // Enlarge text field
            // m_descText.Height = 309;

            // Set title to step title.
            m_titleText.Text = m_testScriptObject.Title;
            m_descText.Text = m_testScriptObject.Description;

            m_expectedText.Height = 195;
            m_expectedText.Text = ((TestStep)(m_testScriptObject)).ExpectedBehavior;

            // Need to allow description edit control to expand with dialog (since expected no longer appears).
            m_descText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right |
                System.Windows.Forms.AnchorStyles.Bottom)));

            // Remove test method tab page until needed.
            m_editorTabs.Controls.Remove(m_automationDefinitionTab);

            var automated = ((TestStep)m_testScriptObject).TestType == TestType.Automated && !m_automatecheckBox.Checked ? true : false;

            if (automated)
            {
                // Add assemblies from assembly cache.
                populateTestAssemblyTreeView();

                // Setup test method tab contents.
                initializeForAutomationDefinition(testStep.TestAutomationDefinition);

                // Show checkbox as activated.
                setActivateCheckBox();
                m_automatecheckBox.Checked = automated;
                m_editorTabs.SelectedTab = m_automationDefinitionTab;
            }
            else
            {
                m_editorTabs.SelectedTab = m_generalTab;
            }
        }

        private void initializeDialogForTestProcessor()
        {
            var testProcessor = m_testScriptObject as TestProcessor;

            // Hide expected text field
            m_expectedLabel.Enabled = false;
            m_expectedLabel.Visible = false;
            m_expectedText.Enabled = false;
            m_expectedText.Visible = false;

            // Enlarge text field
            m_descText.Height = 309;

            // Set processor's title
            this.m_titleText.Text = m_testScriptObject.Title;

            // Set description to processor's description
            m_descText.Text = m_testScriptObject.Description;

            //m_expectedText.Text = testStep.ExpectedVerdict.ToString();

            // Need to allow description edit control to expand with dialog (since expected no longer appears).
            m_descText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right |
                System.Windows.Forms.AnchorStyles.Bottom)));

            // Remove test method tab page until needed.
            m_editorTabs.Controls.Remove(m_automationDefinitionTab);

            // Setup test method tab contents.
            initializeForAutomationDefinition(testProcessor.TestAutomationDefinition);

            setActivateCheckBox();

            // If automated, check checkbox, else uncheck (i.e., manual).
            m_automatecheckBox.Checked =
                ((TestProcessor)m_testScriptObject).TestType == TestType.Automated && !m_automatecheckBox.Checked ? true : false;
        }

        private void setActivateCheckBox()
        {
            bool activated = (m_testScriptObject.Status == Status.Active || m_testScriptObject.Status == Status.Incomplete) ? true : false;

            if (m_testScriptObject.Status == Status.Active)
            {
                //m_activatecheckBox.Text = "Ac&tivated";
                m_activatecheckBox.Checked = true;
            }
            else if (m_testScriptObject.Status == Status.Inactive)
            {
                // m_activatecheckBox.Text = "Ac&tivate";
                m_activatecheckBox.Checked = false;
            }
            else if (m_testScriptObject.Status == Status.Incomplete)
            {
                //m_activatecheckBox.Text = "Ac&tivate";
                m_activatecheckBox.Enabled = true;
            }
            else if (m_testScriptObject.Status == Status.Obsolete)
            {
                // m_activatecheckBox.Text = "Ac&tivate";
                m_activatecheckBox.Enabled = false;
            }
        }

        private TreeNode _initialNode = null;

        private void initializeForAutomationDefinition(TestAutomationDefinition automationDefinition)
        {
            try
            {
                if (!string.IsNullOrEmpty(automationDefinition.TestAssembly))
                {
                    var testAssembly = TestProperties.ExpandString(automationDefinition.TestAssembly);

                    // If fails, will throw system.io exception.
                    TreeNode assemblyNode = isAssemblyLoaded(testAssembly);

                    if (assemblyNode == null)
                    {
                        assemblyNode = addTestAssembly(testAssembly);
                    }

                    TreeNode classNode = selectTestClass(assemblyNode, automationDefinition);

                    if (classNode != null)
                    {
                        TreeNode methodNode = selectTestMethod(classNode, automationDefinition.TestMethod);
                        _initialNode = methodNode;

                        if (methodNode != null)
                        {
                            validateTestParameters((MethodInfo)methodNode.Tag, automationDefinition.TestParameters);
                        }
                        else
                        {
                            MessageBox.Show(this,
                                string.Format("Unable to locate test method \"{0}\" in test class \"{1}\"",
                                    automationDefinition.TestMethod, automationDefinition.TestClass),
                                "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show(this,
                            string.Format("Unable to locate test class \"{0}\" in assembly \"{1}\"",
                                automationDefinition.TestClass, automationDefinition.TestAssembly),
                            "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(this,
                                "This test step is marked as automated, however the automation information is incompleted.",
                                "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                MessageBox.Show(this, e.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.IO.FileLoadException e)
            {
                MessageBox.Show(this, e.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void updateChangedUI(bool changed)
        {
            // Set dirty flag.
            m_contentChanged = changed;

            // Enable OK button.
            m_okBtn.Enabled = changed;

            m_okBtn.Enabled = true;
        }

        private TreeNode selectTestClass(TreeNode assemblyNode, TestAutomationDefinition automationDefinition)
        {
            TreeNode node = null;

            foreach (TreeNode testClassNode in assemblyNode.Nodes)
            {
                if (((Type)testClassNode.Tag).FullName.Equals(automationDefinition.TestClass))
                {
                    node = testClassNode;
                    m_testAssemblytreeView.SelectedNode = node;
                    break;
                }
            }

            return node;
        }

        private TreeNode selectTestMethod(TreeNode classNode, string testMethod)
        {
            TreeNode node = null;

            foreach (TreeNode methodNode in classNode.Nodes)
            {
                if (((MethodInfo)methodNode.Tag).Name.Equals(testMethod))
                {
                    m_testAssemblytreeView.SelectedNode = methodNode;
                    node = methodNode;
                    break;
                }
            }

            return node;
        }

        private bool validateTestParameters(MethodInfo method, TestParameterCollection testParameters)
        {
            bool validated = true;
            StringBuilder sb = new StringBuilder();
            ParameterInfo[] parameters = method.GetParameters();

            string errorMsg = "The test parameter's below no longer match that of the actual library's method signature.  " +
                "The system will attempt resolve if re-ordered or substitute default values.\r\n\r\n";

            for (int i = 0; i < parameters.Length; i++)
            {
                try
                {
                    if (parameters[i].ParameterType.Name != "Nullable`1")
                    {
                        Convert.ChangeType(testParameters[i].ValueAsString, parameters[i].ParameterType);
                    }
                    else
                    {
                        if (testParameters[i].ValueAsString != null)
                        {
                            Type underlying = Nullable.GetUnderlyingType(parameters[i].ParameterType);
                            Convert.ChangeType(testParameters[i].ValueAsString, underlying);
                        }
                    }
                }
                catch
                {
                    validated = false;
                }
            }

            if (!validated)
            {
                MessageBox.Show(this, errorMsg + sb.ToString(),
                "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return validated;
        }

        private void populateTestParametersGrid(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            m_testParameterGrid.Rows.Clear();

            foreach (ParameterInfo parameter in parameters)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Tag = parameter;
                row.CreateCells(m_testParameterGrid);
                row.Cells[ParameterColumn].Value = getTestParameterAlias(parameter);
                row.Cells[ParameterColumn].ToolTipText = getTestParameterDescription(parameter);

                row.Cells[DataTypeColumn].Value = getDataTypeAlias(parameter);
                row.Cells[DataTypeColumn].ToolTipText = string.Format("{0} : {1}",
                    parameter.Name, parameter.ParameterType);

                // Set cell value type (used for parameter value validation).
                //row.Cells[ValueColumn].ValueType = parameter.ParameterType;
                row.Cells[ValueColumn].ValueType = typeof(string);
                object defaultValue = getParameterDefaultValue(parameter);

                object matchingValue;

                if (setTestParameterValue(parameter, out matchingValue))
                {
                    row.Cells[ValueColumn].Value = row.Cells[ValueColumn].Tag = matchingValue;
                     
                }
                else
                {
                    row.Cells[ValueColumn].Value = row.Cells[ValueColumn].Tag = defaultValue;
                }

                row.Cells[ValueColumn].ToolTipText =
                    string.Format("Default value:  {0}",
                    null != defaultValue ? defaultValue.ToString() : "Null");
                m_testParameterGrid.Rows.Add(row);
            }
        }

        /// <summary>
        /// Gets the default value for the passed parameter.
        /// </summary>
        /// <param name="parameter">ParameterInfo object.</param>
        /// <param name="defaultObject">True, includes the default datatype object value, false otherwise.</param>
        /// <returns>Default parameter value.</returns>
        private object getParameterDefaultValue(ParameterInfo parameter)
        {
            object defaultValue = null;

            // Get default value if available.
            object[] attributes = parameter.GetCustomAttributes(true);

            // If available find the TestParameterAttribute
            if (attributes.Length > 0)
            {
                foreach (object attribute in attributes)
                {
                    // If found, get default value if avaiable.
                    if (attribute is TestParameterAttribute)
                    {
                        defaultValue = ((TestParameterAttribute)attribute).DefaultValue;
                    }
                }
            }

            // If still not set, get the default datatype value (if flag set).
            if (defaultValue == null)
            {
                defaultValue = getTypeDefaultValue(parameter.ParameterType);
            }

            return defaultValue;
        }

        /// <summary>
        /// Finds the matching method's combobox loaded parameters with the initial editing parameters.
        /// </summary>
        /// <param name="searchParameter">Parameter to look up.</param>
        /// <returns>Found loaded test parameter or null.</returns>
        private TestParameter findTestParameter(string searchParameter, TestAutomationDefinition testMethod)
        {
            TestParameter foundParameter = null;

            if (testMethod.TestParameters != null)
            {
                foreach (TestParameter parameter in testMethod.TestParameters)
                {
                    if (parameter.DisplayName == searchParameter)
                    {
                        foundParameter = parameter;
                        break;
                    }
                }
            }

            return foundParameter;
        }

        /// <summary>
        /// Cycles through data grid rows and constructs an arraylist of TestParameter objects.
        /// </summary>
        /// <returns>Returns ArrayList of AutoTestParameters.</returns>
        private TestParameterCollection collectParameters()
        {
            // Collect test parameters
            DataGridViewRowCollection rows = this.m_testParameterGrid.Rows;

            TestParameterCollection newParams = new TestParameterCollection();

            foreach (DataGridViewRow row in rows)
            {
                object @value = row.Cells[ValueColumn].Value != null ? row.Cells[ValueColumn].Value.ToString() : null;

                TestParameter testParameter = new TestParameter(
                    ((ParameterInfo)row.Tag).Name,
                    row.Cells[ParameterColumn].Value.ToString(),
                    row.Cells[ValueColumn].Value != null ? row.Cells[ValueColumn].Value : null,
                    ((ParameterInfo)row.Tag).ParameterType
                    );

                newParams.Add(testParameter);
            }

            return newParams;
        }

        /// <summary>
        /// Resets dialog to original values.
        /// </summary>
        private void resetDialog()
        {
            if (m_testScriptObject is TestStep)
            {
                //setupForTestStep();
            }
            else if (m_testScriptObject is TestScriptObjectContainer)
            {
                //setupForTestScriptContainer();
            }
            else if (m_testScriptObject is TestProcessor)
            {
                initializeDialogForTestProcessor();
            }
        }

        public string getDataTypeAlias(ParameterInfo parameter)
        {
            string alias = null;

            switch (parameter.ParameterType.ToString().ToUpper())
            {
                case "SYSTEM.STRING":
                    alias = "String";
                    break;
                case "SYSTEM.CHAR":
                    alias = "Char";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.CHAR]":
                    alias = "Char?";
                    break;
                case "SYSTEM.BYTE":
                    alias = "Byte";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.BYTE]":
                    alias = "Byte?";
                    break;
                case "SYSTEM.SBYTE":
                    alias = "SByte";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.SBYTE]":
                    alias = "SByte?";
                    break;
                case "SYSTEM.INT16":
                    alias = "Int16";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.INT16]":
                    alias = "Int16?";
                    break;
                case "SYSTEM.INT32":
                    alias = "Int32";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.INT32]":
                    alias = "Int32?";
                    break;
                case "SYSTEM.INT64":
                    alias = "Int64";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.INT64]":
                    alias = "Int64?";
                    break;
                case "SYSTEM.UINT16":
                    alias = "UInt16";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.UINT16]":
                    alias = "UInt16?";
                    break;
                case "SYSTEM.UINT32":
                    alias = "UInt32";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.UINT32]":
                    alias = "UInt32?";
                    break;
                case "SYSTEM.UINT64":
                    alias = "UInt64";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.UINT64]":
                    alias = "UInt64?";
                    break;
                case "SYSTEM.SINGLE":
                    alias = "Single";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.SINGLE]":
                    alias = "Single?";
                    break;
                case "SYSTEM.DOUBLE":
                    alias = "Double";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.DOUBLE]":
                    alias = "Double?";
                    break;
                case "SYSTEM.DECIMAL":
                    alias = "Decimal";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.DECIMAL]":
                    alias = "Decimal?";
                    break;
                case "SYSTEM.BOOLEAN":
                    alias = "Boolean";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.BOOLEAN]":
                    alias = "Boolean?";
                    break;
                case "SYSTEM.DATETIME":
                    alias = "DateTime";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.DATETIME]":
                    alias = "DateTime?";
                    break;
                case "SYSTEM.TIMESPAN":
                    alias = "TimeSpan";
                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.TIMESPAN]":
                    alias = "TimeSpan?";
                    break;
                default:
                    alias = parameter.ParameterType.ToString();
                    break;
            }

            return alias;
        }

        private string getTestClassAlias(Type testClass)
        {
            TestClassAttribute attribute = getTestClassAttribute(testClass);
            return attribute != null && attribute.Alias != null ? attribute.Alias : testClass.Name;
        }

        private string getTestClassDescription(Type testClass)
        {
            TestClassAttribute attribute = getTestClassAttribute(testClass);
            return attribute != null && attribute.Description != null ? attribute.Description : null;
        }

        private TestClassAttribute getTestClassAttribute(Type testClass)
        {
            TestClassAttribute testMethodAttribute = null;

            // Get default value if available.
            object[] attributes = testClass.GetCustomAttributes(true);

            // If available find the TestParameterAttribute
            if (attributes.Length > 0)
            {
                foreach (object attribute in attributes)
                {
                    if (attribute is TestClassAttribute)
                    {
                        testMethodAttribute = (TestClassAttribute)attribute;
                        break;
                    }
                }
            }

            return testMethodAttribute;
        }

        private string getTestMethodAlias()
        {
            return getTestMethodAlias((MethodInfo)m_testAssemblytreeView.SelectedNode.Tag);
        }

        private string getTestMethodAlias(MethodInfo method)
        {
            TestMethodAttribute attribute = getTestMethodAttribute(method);
            return attribute != null && attribute.Alias != null ? attribute.Alias : method.Name;
        }

        private string getTestMethodDescription()
        {
            return getTestMethodDescription((MethodInfo)m_testAssemblytreeView.SelectedNode.Tag);
        }

        private string getTestMethodDescription(MethodInfo method)
        {
            TestMethodAttribute attribute = getTestMethodAttribute(method);
            return attribute != null && attribute.Description != null ? attribute.Description : null;
        }

        private TestMethodAttribute getTestMethodAttribute(MethodInfo method)
        {
            TestMethodAttribute testMethodAttribute = null;

            // Get default value if available.
            object[] attributes = method.GetCustomAttributes(true);

            // If available find the TestParameterAttribute
            if (attributes.Length > 0)
            {
                foreach (object attribute in attributes)
                {
                    if (attribute is TestMethodAttribute)
                    {
                        testMethodAttribute = (TestMethodAttribute)attribute;
                        break;
                    }
                }
            }

            return testMethodAttribute;
        }

        private string getTestParameterAlias(ParameterInfo parameter)
        {
            TestParameterAttribute attribute = getTestParameterAttribute(parameter);
            return attribute != null ? attribute.Alias : parameter.Name;
        }

        /// <summary>
        /// Gets the TestParameterAttribute description value from the parameter.
        /// </summary>
        /// <param name="method">ParameterInfo object.</param>
        /// <returns>Description if available, null otherwise.</returns>
        private string getTestParameterDescription(ParameterInfo parameter)
        {
            TestParameterAttribute attribute = getTestParameterAttribute(parameter);
            return attribute != null ? attribute.Description : null;
        }

        private object getTestParameterDefaultValue(ParameterInfo parameter)
        {
            TestParameterAttribute attribute = getTestParameterAttribute(parameter);
            return attribute != null ? attribute.DefaultValue : null;
        }

        private TestParameterAttribute getTestParameterAttribute(ParameterInfo parameter)
        {
            TestParameterAttribute testParameterAttribute = null;

            // Get default value if available.
            object[] attributes = parameter.GetCustomAttributes(true);

            // If available find the TestParameterAttribute
            if (attributes.Length > 0)
            {
                foreach (object attribute in attributes)
                {
                    if (attribute is TestParameterAttribute)
                    {
                        testParameterAttribute = (TestParameterAttribute)attribute;
                        break;
                    }
                }
            }

            return testParameterAttribute;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Event handler for dialog being activated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoTestMethodDlg_Activated(object sender, EventArgs e)
        {
            // If client event handler defined...
            if (OnTestScriptObjectEditorActivated != null)
            {
                OnTestScriptObjectEditorActivated(this,
                    new TestScriptObjectEditorActivatedArgs(m_testScriptObject));
            }
        }

        /// <summary>
        /// Resets the initial state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_resetBtn_Click(object sender, EventArgs e)
        {
            resetDialog();
        }

        private void m_okBtn_Click(object sender, EventArgs e)
        {
            if (m_testScriptObject is TestSuite)
            {
                m_testScriptObject.Title = m_titleText.Text;
                m_testScriptObject.Description = m_descText.Text;
                m_testScriptObject.Status = m_activatecheckBox.Checked ? Status.Active : Status.Inactive;
            }
            else if (m_testScriptObject is TestCase)
            {
                m_testScriptObject.Title = m_titleText.Text;
                m_testScriptObject.Description = m_descText.Text;
                m_testScriptObject.Status = m_activatecheckBox.Checked ? Status.Active : Status.Inactive;
            }
            else if (m_testScriptObject is TestStep)
            {
                m_testScriptObject.Title = m_titleText.Text.Equals("Untitled test step") ? getTestMethodAlias() : m_titleText.Text;
                m_testScriptObject.Description = string.IsNullOrEmpty(m_descText.Text) ? getTestMethodDescription() : m_descText.Text;

                ((TestStep)m_testScriptObject).ExpectedBehavior = m_expectedText.Text;
                m_testScriptObject.Status = m_activatecheckBox.Checked ? Status.Active : Status.Inactive;

                ((TestStep)m_testScriptObject).TestType = (m_automatecheckBox.Checked) ? TestType.Automated : TestType.Manual;

                TestAutomationDefinition testDefinition = getTestAutomationDefinition();
                testDefinition.TestAssembly = TestProperties.FixupString(testDefinition.TestAssembly, "TestAssemblies");
                ((TestStep)m_testScriptObject).TestAutomationDefinition = testDefinition;
            }
            else if (m_testScriptObject is TestProcessor)
            {
                m_testScriptObject.Title = m_titleText.Text;
                m_testScriptObject.Description = m_descText.Text;
                m_testScriptObject.Status = m_activatecheckBox.Checked ? Status.Active : Status.Inactive;

                ((TestProcessor)m_testScriptObject).TestType = (m_automatecheckBox.Checked) ? TestType.Automated : TestType.Manual;
                ((TestProcessor)m_testScriptObject).TestAutomationDefinition = getTestAutomationDefinition();
            }

            Close();
        }

        /// <summary>
        /// Cancel button click event hander.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        //private ManualResetEvent _executionEvent = null;

        /// <summary>
        /// Execute button click event handler.  Simulates running the test step with the selected test method.
        /// Relies temporarily setting the step's automation definition with new from UI selection.  TestExecutor executes and
        /// resets to original post execution.  Uses a manual reset on thread to block until execution is complete 
        /// (signaled from OnExecutionComplete event handler).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_executeBtn_Click(object sender, EventArgs e)
        {
            //if (m_testScriptObject is TestStep)
            //{
            //    _executionEvent = new ManualResetEvent(false);

            //    TestExecutor.OnExecutionComplete += _OnExecutionComplete;

            //    TestAutomationDefinition currentDefinition = null;

            //    var testStep = m_testScriptObject as TestStep;

            //    try
            //    {
            //        currentDefinition = testStep.TestAutomationDefinition;

            //        var newdefinition = getTestAutomationDefinition();

            //        testStep.TestAutomationDefinition = newdefinition;

            //        new TestExecutor().ExecuteTestStep(testStep, false);

            //        Cursor = Cursors.WaitCursor;
            //        _executionEvent.WaitOne();
            //        Cursor = Cursors.Default;

            //    }
            //    catch
            //    {
            //        throw;
            //    }
            //    finally  // Always want to reset.
            //    {
            //        testStep.TestAutomationDefinition = currentDefinition;
            //        TestExecutor.OnExecutionComplete -= _OnExecutionComplete;
            //    }
            //}
        }

        //private void _OnExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        //{
        //    _executionEvent.Set();
        //}

        /// <summary>
        /// Event handler for title text box content change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_titleText_TextChanged(object sender, EventArgs e)
        {
            // Update window caption for changed title.
            if (m_testScriptObject is TestSuite)
            {
                Text = "Test Suite - " + m_titleText.Text;
            }
            else if (m_testScriptObject is TestCase)
            {
                Text = "Test Case - " + m_titleText.Text;
            }
            else if (m_testScriptObject is TestStep)
            {
                Text = "Test Method - " + m_titleText.Text;
            }
            //else if (m_testScriptObject is TestPreprocessor)
            //{
            //    Text = "Test Pre-processor - " + m_testScriptObject.Parent.Title;
            //}
            //else if (m_testScriptObject is TestPostprocessor)
            //{
            //    Text = "Test Post-processor - " + m_testScriptObject.Parent.Title;
            //}


            updateChangedUI(!m_isInitializingDialog);
        }

        /// <summary>
        /// Event handler for descripton box content change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_descText_TextChanged(object sender, EventArgs e)
        {
            updateChangedUI(!m_isInitializingDialog);
        }

        /// <summary>
        /// Event handler for when automate checkbox changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_automatedChk_CheckedChanged(object sender, EventArgs e)
        {
            // If check box is checked...
            if (((CheckBox)sender).Checked)
            {
                // Add test method tab to tab control.
                m_editorTabs.Controls.Add(m_automationDefinitionTab);
                m_editorTabs.SelectedTab = m_automationDefinitionTab;
                populateTestAssemblyTreeView();
            }
            else
            {
                // If manual test, hide test method tabpage on tab control.
                m_editorTabs.Controls.Remove(m_automationDefinitionTab); ;
            }

            updateChangedUI(!m_isInitializingDialog);
        }

        private void m_tsAddAssemblyButton_Click(object sender, EventArgs e)
        {
            addTestAssembly();
        }

        private void m_testParameterGrid_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            object temp;

            if (e.ColumnIndex == 2)
            {
                // Get tagged ParameterIfno
                ParameterInfo parameter = m_testParameterGrid.Rows[e.RowIndex].Tag as ParameterInfo;

                // Get cell value, if not null, get string value.
                var valueCell = m_testParameterGrid.Rows[e.RowIndex].Cells[ValueColumn];
                string valueAsString = valueCell.Value != null ? valueCell.Value.ToString() : null;

                if (!TestUtils.FromString(valueAsString, parameter, out temp))
                {
                    var dlgResult = MessageBox.Show(this, "The new value is not compatible with this parameter's datatype.",
                        "Quintity TestEngineer", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (dlgResult == DialogResult.Cancel)
                    {
                        valueCell.Value = valueCell.Tag;
                        m_testParameterGrid.UpdateCellValue(ValueColumn, e.RowIndex);
                    }
                }
                else
                {
                    valueCell.Tag = valueCell.Value;
                    m_okBtn.Enabled = true;
                }
            }
        }

        private void TestScriptObjectEditorDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            // If client event handler defined...
            if (OnTestScriptObjectEditorClosed != null)
            {
                OnTestScriptObjectEditorClosed(this,
                    new TestScriptObjectEditorClosedArgs(m_testScriptObject, m_contentChanged));
            }
        }

        private void m_activatecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (m_testScriptObject is TestStep)
            {
                var teststep = m_testScriptObject as TestStep;

                if (m_activatecheckBox.Checked && teststep.TestType == Core.TestType.Automated)
                {
                    if (!m_isInitializingDialog)
                    {
                        TestAutomationDefinition definition = getTestAutomationDefinition();

                        if (definition == null || !definition.IsCompleted())
                        {
                            MessageBox.Show(this,
                                "This test step's automation definition is incomplete and cannot be activated.  " +
                                "Please complete the definition and try again.",
                                "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            this.m_editorTabs.SelectedTab = m_automationDefinitionTab;
                            m_activatecheckBox.Checked = false;
                        }
                    }
                }
            }
        }

        private void TestScriptObjectEditDialog_Load(object sender, EventArgs e)
        { }

        private void m_descText_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void m_testAssemblytreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag is MethodInfo)
            {
                populateTestParametersGrid((MethodInfo)e.Node.Tag);

                // If selected node has changed, enable OK button.
                m_okBtn.Enabled = _initialNode != null && !_initialNode.Equals(e.Node);
                m_okBtn.Enabled = true;
            }
            else
            {
                m_testParameterGrid.Rows.Clear();
            }
        }

        private bool setTestParameterValue(ParameterInfo parameter, out object @value)
        {
            bool match = false;
            @value = null;

            if (m_testScriptObject != null)
            {
                TestAutomationDefinition definition = null;

                if (m_testScriptObject is TestStep)
                {
                    definition = ((TestStep)m_testScriptObject).TestAutomationDefinition;
                }
                else if (m_testScriptObject is TestProcessor)
                {
                    definition = ((TestProcessor)m_testScriptObject).TestAutomationDefinition;
                }

                // Have to do massaging of type to accomodate nullable values.
                var parameterType = parameter.ParameterType.ToString();
                parameterType = parameterType.Substring(0, parameterType.Length - 1).Replace("[", "");

                foreach (TestParameter testParameter in definition.TestParameters)
                {
                    var testParameterType = testParameter.TypeAsString.Replace("[", "");

                    if (parameter.Name.Equals(testParameter.Name) && testParameterType.StartsWith(parameterType))
                    {
                        @value = testParameter.ValueAsString;
                        match = true;
                        break;
                    }
                }
            }

            return match;
        }

        private void m_testAssemblytreeView_MouseUp(object sender, MouseEventArgs e)
        {
            TreeNode node = m_testAssemblytreeView.GetNodeAt(e.Location);

            if (node != null && node.Name.Equals("RootNode"))
            {
                m_addTestAssemblyContextMenuStrip.Show(m_testAssemblytreeView.PointToScreen(e.Location));
            }
        }

        private void m_miAddTestAssembly_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void m_miAddTestAssembly_Click(object sender, EventArgs e)
        {
            addTestAssembly();
        }

        private void addTestAssembly()
        {
            try
            {
                m_openFileDialog.InitialDirectory = TestProperties.TestAssemblies;
                // m_openFileDialog.RestoreDirectory = true;
                m_openFileDialog.Filter = "Test assembly (*.dll)|*.dll|All files (*.*)|*.*";
                m_openFileDialog.FilterIndex = 1;

                if (DialogResult.OK == m_openFileDialog.ShowDialog(this))
                {
                    TreeNode node = isAssemblyLoaded(m_openFileDialog.FileName);

                    if (node != null)
                    {
                        m_testAssemblytreeView.SelectedNode = node;
                        MessageBox.Show(this,
                            string.Format("The test assemblly \"{0}\" has already been added.", m_openFileDialog.FileName),
                            "Quintity TestEngineer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        node = addTestAssembly(m_openFileDialog.FileName);
                        m_testAssemblyCache.Add((string)node.Tag);
                    }
                }
            }
            catch (Exception ex)
            {
                {
                    if (ex is System.Reflection.ReflectionTypeLoadException)
                    {
                        var typeLoadException = ex as ReflectionTypeLoadException;
                        var loaderExceptions = typeLoadException.LoaderExceptions;

                        MessageBox.Show(this,
                            ex.Message + "  Verify this assembly and test framework dependent assemblies are built with the currently installed version of the test framework.",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private TreeNode addTestAssembly(string fileName)
        {
            Assembly assembly = TestReflection.LoadTestAssembly(fileName);

            // If friendly name specified (i.e., assembly Title), use it.
            object[] customAttributes = assembly.GetCustomAttributes
                            (typeof(AssemblyTitleAttribute), false);

            var title = customAttributes != null && customAttributes.Length > 0 ? ((AssemblyTitleAttribute)customAttributes[0]).Title : assembly.ManifestModule.Name;

            TreeNode assemblyNode = m_testAssemblytreeView.Nodes[0].Nodes.Add(title);
            assemblyNode.Tag = assembly.Location;
            assemblyNode.ToolTipText = assembly.Location;

            List<Type> types = TestReflection.GetTestClasses(assembly);

            if (types.Count != 0)
            {
                foreach (Type type in types)
                {
                    var typeNode = assemblyNode.Nodes.Add(getTestClassAlias(type));
                    typeNode.Expand();
                    typeNode.Tag = type;

                    string description = getTestClassDescription(type);
                    typeNode.ToolTipText = string.Format("{0}{1}",
                                description != null ? description + "\r\n" : null,
                                type.FullName);

                    List<MethodInfo> methods = TestReflection.GetTestMethods(type);

                    if (methods.Count != 0)
                    {
                        foreach (MethodInfo method in methods)
                        {
                            TreeNode nodeAdded = typeNode.Nodes.Add(getTestMethodAlias(method));
                            nodeAdded.Tag = method;

                            description = getTestMethodDescription(method);
                            nodeAdded.ToolTipText = string.Format("{0}{1}",
                                description != null ? description + "\r\n" : null,
                                method.ToString());
                        }
                    }
                    else
                    {
                        TreeNode node = new TreeNode();
                        node.Text = "Test class does not contain test methods.";
                        node.NodeFont = new Font("Microsoft Sans Serif", 8, FontStyle.Italic);
                        typeNode.Nodes.Add(node);
                        //m_testAssemblytreeView.SelectedNode = typeNode;
                    }
                }

                m_testAssemblytreeView.SelectedNode = assemblyNode;
            }
            else
            {
                TreeNode node = new TreeNode();
                node.Text = "Test assembly does not contain test classes.";
                node.NodeFont = new Font("Microsoft Sans Serif", 8, FontStyle.Italic);
                assemblyNode.Nodes.Add(node);
                m_testAssemblytreeView.SelectedNode = assemblyNode;
            }

            m_testAssemblytreeView.Sort();
            //m_testAssemblytreeView.ExpandAll();

            return assemblyNode;
        }

        private TreeNode isAssemblyLoaded(string fileName)
        {
            TreeNode loadedNode = null;

            Uri assyToLoadUri = new Uri(fileName);
            TreeNode root = m_testAssemblytreeView.Nodes[0];

            foreach (TreeNode node in root.Nodes)
            {
                Uri loadedUri = new Uri((string)node.Tag);

                if (assyToLoadUri == loadedUri)
                {
                    loadedNode = node;
                    break;
                }
            }

            return loadedNode;
        }

        /// <summary>
        /// [ <c>public static T GetDefault&lt; T &gt;()</c> ]
        /// <para></para>
        /// Retrieves the default value for a given Type
        /// </summary>
        /// <typeparam name="T">The Type for which to get the default value</typeparam>
        /// <returns>The default value for Type T</returns>
        /// <remarks>
        /// If a reference Type or a System.Void Type is supplied, this method always returns null.  If a value type 
        /// is supplied which is not publicly visible or which contains generic parameters, this method will fail with an 
        /// exception.
        /// </remarks>
        /// <seealso cref="getTypeDefaultValue(Type)"/>
        public T getTypeDefaultValue<T>()
        {
            return (T)getTypeDefaultValue(typeof(T));
        }

        /// <summary>
        /// [ <c>public static object GetDefault(Type type)</c> ]
        /// <para></para>
        /// Retrieves the default value for a given Type
        /// </summary>
        /// <param name="type">The Type for which to get the default value</param>
        /// <returns>The default value for <paramref name="type"/></returns>
        /// <remarks>
        /// If a null Type, a reference Type, or a System.Void Type is supplied, this method always returns null.  If a value type 
        /// is supplied which is not publicly visible or which contains generic parameters, this method will fail with an 
        /// exception.
        /// </remarks>
        /// <seealso cref="GetDefault&lt;T&gt;"/>
        public object getTypeDefaultValue(Type type)
        {
            // If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
            if (type == null || !type.IsValueType || type == typeof(void))
                return null;

            // If the supplied Type has generic parameters, its default value cannot be determined
            if (type.ContainsGenericParameters)
                throw new ArgumentException(
                    "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                    "> contains generic parameters, so the default value cannot be retrieved");

            // If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct), return a 
            //  default instance of the value type
            if (type.IsPrimitive || !type.IsNotPublic)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
                        "create a default instance of the supplied value type <" + type +
                        "> (Inner Exception message: \"" + e.Message + "\")", e);
                }
            }

            // Fail with exception
            throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                "> is not a publicly-visible type, so the default value cannot be retrieved");
        }

        private TestAutomationDefinition getTestAutomationDefinition()
        {
            TreeNode node = m_testAssemblytreeView.SelectedNode;

            TestAutomationDefinition definition = new TestAutomationDefinition();

            if (node != null && node.Tag != null && node.Tag is MethodInfo)
            {
                definition.TestAssembly = ((String)node.Parent.Parent.Tag);
                definition.TestClass = ((Type)node.Parent.Tag).FullName;
                definition.TestMethod = ((MethodInfo)node.Tag).Name;
                definition.TestParameters = collectParameters();
            }

            return definition;
        }

        #endregion

        private void m_expectedText_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void m_testAssemblytreeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var bob = m_testAssemblytreeView.SelectedNode;
            }
        }
    }
}