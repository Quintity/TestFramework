using System.Threading;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.TestEngineer
{
    public partial class TestTreeView
    {
        #region Partial class data members

        delegate void OnTestExecutorExecutionBeginDelegate(TestExecutor testExecutor, TestExecutionBeginArgs args);
        delegate void OnTestExecutorExecutionCompleteDelegate(TestExecutor testExecutor, TestExecutionCompleteArgs args);
        delegate void OnTestScriptObjectExecutionCompleteDelegate(TestScriptObject testScriptObject, TestScriptResult testScriptObjectResult);
        delegate void OnTestScriptObjectExecutionBeginDelegate(TestScriptObject testScriptObject, TestScriptObjectBeginExecutionArgs args);
        delegate void OnTestTraceDelegate(string virtualUser, string traceMessage);
        delegate void OnTestWarningDelegate(TestWarning testWarning);
        delegate void OnTestCheckDelegate(TestCheck testCheck);

        #endregion

        #region Partial class private methods

        private void registerRuntimeEvents()
        {
            // TestExecutor events
            TestExecutor.OnExecutionBegin += TestExecutor_OnExecutionBegin;
            TestExecutor.OnExecutionComplete += TestExecutor_OnExecutionComplete;

            // TestSuite events
            TestSuite.OnExecutionBegin += TestSuite_OnExecutionBegin;
            TestSuite.OnTestPreprocessorBegin += TestSuite_OnTestPreprocessorBegin;
            TestSuite.OnTestPreprocessorComplete += TestSuite_OnTestPreprocessorComplete;
            TestSuite.OnTestPostprocessorBegin += TestSuite_OnTestPostprocessorBegin;
            TestSuite.OnTestPostprocessorComplete += TestSuite_OnTestPostprocessorComplete;
            TestSuite.OnExecutionComplete += TestSuite_OnExecutionComplete;

            // TestCase events
            TestCase.OnExecutionBegin += TestCase_OnExecutionBegin;
            TestCase.OnExecutionComplete += TestCase_OnExecutionComplete;

            // TestStep events
            TestStep.OnExecutionBegin += TestStep_OnExecutionBegin;
            TestStep.OnExecutionComplete += TestStep_OnExecutionComplete;

            // Runtime events
            TestCheck.OnTestCheck += TestCheck_OnTestCheck;
            TestWarning.OnTestWarning += TestWarning_OnTestWarning;
            TestTrace.OnTestTrace += TestTrace_OnTestTrace;
        }

        private void unregisterRuntimeEvents()
        {
            // TestExecutor events
            TestExecutor.OnExecutionBegin -= TestExecutor_OnExecutionBegin;
            TestExecutor.OnExecutionComplete -= TestExecutor_OnExecutionComplete;

            // TestSuite events
            TestSuite.OnExecutionBegin -= TestSuite_OnExecutionBegin;
            TestSuite.OnTestPreprocessorBegin -= TestSuite_OnTestPreprocessorBegin;
            TestSuite.OnTestPreprocessorComplete -= TestSuite_OnTestPreprocessorComplete;
            TestSuite.OnTestPostprocessorBegin -= TestSuite_OnTestPostprocessorBegin;
            TestSuite.OnTestPostprocessorComplete -= TestSuite_OnTestPostprocessorComplete;
            TestSuite.OnExecutionComplete -= TestSuite_OnExecutionComplete;

            // TestCase events
            TestCase.OnExecutionBegin -= TestCase_OnExecutionBegin;
            TestCase.OnExecutionComplete -= TestCase_OnExecutionComplete;

            // TestStep events
            TestStep.OnExecutionBegin -= TestStep_OnExecutionBegin;
            TestStep.OnExecutionComplete -= TestStep_OnExecutionComplete;

            // Runtime events
            TestCheck.OnTestCheck -= TestCheck_OnTestCheck;
            TestWarning.OnTestWarning -= TestWarning_OnTestWarning;
            TestTrace.OnTestTrace -= TestTrace_OnTestTrace;
        }

        #endregion

        #region Runtime event handlers

        private void TestExecutor_OnExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {

            if (this.InvokeRequired)
            {
                OnTestExecutorExecutionBeginDelegate d = new OnTestExecutorExecutionBeginDelegate(onBeginningTestExecution);
                BeginInvoke(d, new object[] { testExecutor, args });
            }
            else
            {
                onBeginningTestExecution(testExecutor, args);
            }
        }

        private void TestSuite_OnExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            if (this.InvokeRequired)
            {
                OnTestScriptObjectExecutionBeginDelegate d = new OnTestScriptObjectExecutionBeginDelegate(onBeginningTestScriptObjectExecution);
                BeginInvoke(d, new object[] { testSuite, args });
            }
            else
            {
                onBeginningTestScriptObjectExecution(testSuite, args);
            }
        }

        private void TestSuite_OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        { }

        private void TestSuite_OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        { }

        private void TestSuite_OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        { }

        private void TestSuite_OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        { }

        private void TestSuite_OnExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            if (this.InvokeRequired)
            {
                OnTestScriptObjectExecutionCompleteDelegate d = new OnTestScriptObjectExecutionCompleteDelegate(onTestScriptObjectExecutionComplete);
                BeginInvoke(d, new object[] { testSuite, testSuiteResult });
            }
            else
            {
                onTestScriptObjectExecutionComplete(testSuite, testSuiteResult);
            }
        }

        private void TestCase_OnExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args)
        {
            if (this.InvokeRequired)
            {
                OnTestScriptObjectExecutionBeginDelegate d = new OnTestScriptObjectExecutionBeginDelegate(onBeginningTestScriptObjectExecution);
                BeginInvoke(d, new object[] { testCase, args });
            }
            else
            {
                onBeginningTestScriptObjectExecution(testCase, args);
            }
        }

        private void TestCase_OnExecutionComplete(TestCase testCase, TestCaseResult testCaseResult)
        {
            if (this.InvokeRequired)
            {
                OnTestScriptObjectExecutionCompleteDelegate d = new OnTestScriptObjectExecutionCompleteDelegate(onTestScriptObjectExecutionComplete);
                BeginInvoke(d, new object[] { testCase, testCaseResult });
            }
            else
            {
                onTestScriptObjectExecutionComplete(testCase, testCaseResult);
            }
        }

        private void TestStep_OnExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            if (this.InvokeRequired)
            {
                OnTestScriptObjectExecutionBeginDelegate d = new OnTestScriptObjectExecutionBeginDelegate(onBeginningTestScriptObjectExecution);
                BeginInvoke(d, new object[] { testStep, args });
            }
            else
            {
                onBeginningTestScriptObjectExecution(testStep, args);
            }
        }

        private void TestStep_OnExecutionComplete(TestStep testStep, TestStepResult testStepResult)
        {
            if (this.InvokeRequired)
            {
                OnTestScriptObjectExecutionCompleteDelegate d = new OnTestScriptObjectExecutionCompleteDelegate(onTestScriptObjectExecutionComplete);
                BeginInvoke(d, new object[] { testStep, testStepResult });
            }
            else
            {
                onTestScriptObjectExecutionComplete(testStep, testStepResult);
            }
        }

        private void TestCheck_OnTestCheck(TestCheck testCheck)
        {
            if (this.InvokeRequired)
            {
                OnTestCheckDelegate d = new OnTestCheckDelegate(onTestCheck);
                BeginInvoke(d, new object[] { testCheck });
            }
            else
            {
                onTestCheck(testCheck);
            }
        }

        private void TestWarning_OnTestWarning(TestWarning testWarning)
        {
            if (this.InvokeRequired)
            {
                OnTestWarningDelegate d = new OnTestWarningDelegate(onTestWarning);
                BeginInvoke(d, new object[] { testWarning });
            }
            else
            {
                onTestWarning(testWarning);
            }
        }

        private void TestTrace_OnTestTrace(string virtualUser, string traceMessage)
        {
            if (this.InvokeRequired)
            {
                OnTestTraceDelegate d = new OnTestTraceDelegate(onTestTrace);
                BeginInvoke(d, new object[] { virtualUser, traceMessage });
            }
            else
            {
                onTestTrace(virtualUser, traceMessage);
            }
        }


        private void TestExecutor_OnExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            if (this.InvokeRequired)
            {
                OnTestExecutorExecutionCompleteDelegate d = new OnTestExecutorExecutionCompleteDelegate(onTestExecutionComplete);
                BeginInvoke(d, new object[] { testExecutor, args });
            }
            else
            {
                onTestExecutionComplete(testExecutor, args);
            }
        }

        #endregion

        #region Runtime event methods

        private void onBeginningTestExecution(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            _isExecuting = true;
        }

        private void onTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            _isExecuting = false;

            // Give a little time to let UI processing to complete.
            Thread.Sleep(1000);

            // Set testtree to initial node executed.
            SelectedNode = FindNode(args.TestScriptObject);
        }

        private void onTestWarning(TestWarning testWarning)
        { }

        private void onTestCheck(TestCheck testCheck)
        { }

        private void onTestTrace(string virtualUser, string traceMessage)
        { }

        private void onBeginningTestScriptObjectExecution(TestScriptObject testScriptObject, TestScriptObjectBeginExecutionArgs args)
        {
            TestTreeNode node = FindNode(testScriptObject);
            node.IsExecuting = true;
            node.UpdateUI();
            SelectedNode = node;
        }

        private void onTestScriptObjectExecutionComplete(TestScriptObject testScriptObject, TestScriptResult testScriptObjectResult)
        {
            TestTreeNode node = FindNode(testScriptObject);

            if (testScriptObjectResult.TestVerdict == TestVerdict.Pass)
            {
                node.Collapse(false);
            }

            node.IsExecuting = false;
            node.TestScriptResult = testScriptObjectResult;
            node.UpdateUI();

            // If completed following a step over, turn step mode off.
            TestBreakpoints.BreakStepMode = false;
        }

        #endregion
    }
}
