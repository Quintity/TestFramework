using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestEngineer
{
    [Serializable]
    public class TestTreeNode : TreeNode
    {
        #region Data memebers

        public TestScriptObject TestScriptObject
        { get; set; }

        public TestScriptResult TestScriptResult
        { get; set; }

        public bool IsExecuting
        { get; set; }

        public bool HasChanged
        { get; set; }

        public TestScriptObjectEditorDialog TestScriptEditorDialog
        { get; set; }

        #endregion

        #region Constructors

        public TestTreeNode(TestScriptObject testScriptObject)
            : this()
        {
            TestScriptObject = testScriptObject;
            HasChanged = false;
            UpdateTitleAndToolTip();
            UpdateUI();
        }

        public TestTreeNode()
        { }

        protected TestTreeNode(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        #endregion

        #region Public methods

        public static TestTreeNode Convert(TreeNode treeNode)
        {
            return treeNode as TestTreeNode;
        }

        private Object _thisLock = new Object();

        internal void UpdateUI()
        {
            if (IsTestSuite())
            {
                lock (_thisLock)
                {
                    updateUIForTestSuite();
                }
            }
            else if (IsTestCase())
            {
                lock (_thisLock)
                {
                    updateUIForTestCase();
                }
            }
            else if (IsTestStep())
            {
                lock (_thisLock)
                {
                    updateUIForTestStep();
                }
            }
        }

        internal void UpdateTitleAndToolTip()
        {
            string header = "Step";

            if (IsTestCase())
            {
                header = "Case";
            }
            else if (IsTestSuite())
            {
                header = "Suite";
            }

            Text = string.Format("{0}{1} - {2}{3}",
                header,
                TestScriptObject.UserID != null ? " " + TestScriptObject.UserID : null,
                TestScriptObject.Title,
                HasChanged ? "*" : null);

            UpdateToolTip();
        }



        /// <summary>
        /// Sets the node's TestScriptResult value to null;
        /// </summary>
        public void ResetResult()
        {
            if (TestScriptResult != null || IsExecuting)
            {
                IsExecuting = false;
                TestScriptResult = null;
                UpdateUI();
            }
        }

        public void ResetHasChanged()
        {
            if (HasChanged)
            {
                HasChanged = false;
                UpdateTitleAndToolTip();
            }
        }

        public bool IsTestSuite()
        {
            return TestScriptObject is TestSuite;
        }

        public bool IsTestCase()
        {
            return TestScriptObject is TestCase;
        }

        public bool IsTestStep()
        {
            return TestScriptObject is TestStep;
        }

        public string GetTestScriptObjectType()
        {
            string type = null;

            if (IsTestSuite())
            {
                type = "Test suite";
            }
            else if (IsTestCase())
            {
                type = "Test case";
            }
            else if (IsTestStep())
            {
                type = "Test step";
            }

            return type;
        }

        public TestScriptObjectContainer TestScriptObjectAsContainer()
        {
            return TestScriptObject as TestScriptObjectContainer;
        }

        public TestSuite TestScriptObjectAsTestSuite()
        {
            return TestScriptObject as TestSuite;
        }

        public TestCase TestScriptObjectAsTestCase()
        {
            return TestScriptObject as TestCase;
        }

        public TestStep TestScriptObjectAsTestStep()
        {
            return TestScriptObject as TestStep;
        }

        public TestSuiteResult TestScriptResultAsTestSuiteResult()
        {
            return TestScriptResult as TestSuiteResult;
        }

        public TestCaseResult TestScriptResultAsTestCaseResult()
        {
            return TestScriptResult as TestCaseResult;
        }

        public TestStepResult TestScriptResultAsTestStepResult()
        {
            return TestScriptResult as TestStepResult;
        }

        new public TestTreeNode Parent
        {
            get { return base.Parent as TestTreeNode; }
        }

        public void UpdateToolTip()
        {
            if (IsTestStep())
            {
                var testStep = TestScriptObject as TestStep;

                ToolTipText =
                    string.Format("Title:  {0}\r\nTest type:  {1}\r\nStatus:  {2}\r\nDescription:  {3}\r\nExpected:  {4}{5}",
                    testStep.Title,
                    testStep.TestType.ToString(),
                    testStep.Status,
                    testStep.Description != null ? testStep.Description : "None",
                    testStep.ExpectedBehavior != null ? testStep.ExpectedBehavior : "None",
                    testStep.TestType == TestType.Automated ? "\r\n\r\n" + testStep.TestAutomationDefinition.ToString() : null);
            }
            else
            {
                var fileText = TestScriptObject is TestSuite ? $"File:  {TestProperties.ExpandString(((TestSuite)TestScriptObject).FilePath)}" : string.Empty;

                ToolTipText = string.Format("Title:  {0}\r\nStatus:  {1}\r\n{2}\r\nDescription:  {3}",
                TestScriptObject.Title,
                TestScriptObject.Status,
                fileText,
                TestScriptObject.Description != null ? TestScriptObject.Description : "None");
            }
        }

        #endregion

        #region Private methods
        private void updateUIForTestSuite()
        {
            updateUIForTestObjectContainer();
        }

        private void updateUIForTestCase()
        {
            updateUIForTestObjectContainer();
        }

        private void updateUIForTestObjectContainer()
        {
            UpdateToolTip();

            if (TestScriptObject.Status == Status.Active)
            {
                if (IsExecuting)
                {
                    this.EnsureVisible();
                    ImageKey = SelectedImageKey = "execution.arrow.bmp";
                    ForeColor = Color.ForestGreen;
                }
                else
                {
                    if (TestScriptResult == null)
                    {
                        ImageKey = SelectedImageKey = IsExpanded ? "folder.open.bmp" : "folder.closed.bmp";
                        NodeFont = TestTreeView.ActiveFont;
                        ForeColor = Color.Black;
                    }
                    else
                    {
                        var testResult = ((TestScriptResultContainer)TestScriptResult);

                        if (testResult.TestVerdict == TestVerdict.Pass)
                        {
                            ImageKey = SelectedImageKey = IsExpanded ? "folder.open.pass.bmp" : "folder.closed.pass.bmp";
                            NodeFont = TestTreeView.ActiveFont;
                            ForeColor = Color.Black;
                        }
                        else if (testResult.TestVerdict == TestVerdict.DidNotExecute)
                        {
                            ImageKey = SelectedImageKey = IsExpanded ? "folder.open.didnotexecute.bmp" : "folder.closed.didnotexecute.bmp";
                            NodeFont = TestTreeView.ActiveFont;
                            ForeColor = Color.Gray;
                        }
                        else if (testResult.TestVerdict == TestVerdict.Fail)
                        {
                            ImageKey = SelectedImageKey = IsExpanded ? "folder.open.fail.bmp" : "folder.closed.fail.bmp";
                            NodeFont = TestTreeView.ActiveFont;
                            ForeColor = Color.DarkRed;
                        }
                    }
                }
            }
            else
            {
                ImageKey = SelectedImageKey = IsExpanded ? "folder.open.deactivated.bmp" : "folder.closed.deactivated.bmp";
                NodeFont = TestTreeView.InactiveFont;
                ForeColor = Color.Gray;
            }
        }

        private void updateUIForTestStep()
        {
            TestStep testStep = TestScriptObjectAsTestStep();

            if (IsExecuting)
            {
                this.EnsureVisible();
                ImageKey = SelectedImageKey = "execution.arrow.bmp";
                ForeColor = Color.ForestGreen;
            }
            else
            {
                if (this.TestScriptResult == null)
                {
                    ImageKey = SelectedImageKey = "teststep.active.bmp";

                    switch (TestScriptObject.Status)
                    {
                        case Status.Active:
                            ImageKey = testStep.TestType == TestType.Automated ? "teststep.active.bmp" : "teststep.manual.active.bmp";
                            NodeFont = TestTreeView.ActiveFont;
                            ForeColor = Color.Black;
                            break;
                        case Status.Inactive:
                            ImageKey = testStep.TestType == TestType.Automated ? "teststep.inactive.bmp" : "teststep.manual.inactive.bmp";
                            NodeFont = TestTreeView.InactiveFont;
                            ForeColor = Color.Gray;
                            break;
                        case Status.Incomplete:
                            ImageKey = testStep.TestType == TestType.Automated ? "teststep.incomplete.bmp" : "teststep.manual.inactive.bmp";
                            NodeFont = TestTreeView.UnavailableFont;
                            ForeColor = Color.Gray;
                            break;
                        case Status.Obsolete:
                            NodeFont = TestTreeView.UnavailableFont;
                            ForeColor = Color.Gray;
                            break;
                    }

                    SelectedImageKey = ImageKey;
                }
                else
                {
                    NodeFont = TestTreeView.ActiveFont;

                    switch (TestScriptResultAsTestStepResult().TestVerdict)
                    {
                        case TestVerdict.Pass:
                            ImageKey = "teststep.pass.bmp";
                            ForeColor = Color.Black;
                            break;
                        case TestVerdict.Fail:
                            ForeColor = Color.DarkRed;
                            ImageKey = "teststep.fail.bmp";
                            break;
                        case TestVerdict.Error:
                            ForeColor = Color.DarkRed;
                            ImageKey = "teststep.error.bmp";
                            break;
                        case TestVerdict.Inconclusive:
                            break;
                        case TestVerdict.DidNotExecute:
                            ForeColor = Color.Black;
                            ImageKey = "teststep.error.bmp";
                            break;
                        case TestVerdict.Unknown:
                            break;
                        default:
                            break;
                    }

                    SelectedImageKey = ImageKey;
                }
            }
        }
        #endregion
    }
}
