using System;
using System.Diagnostics;
using System.Windows.Forms;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.TestEngineer
{
    public partial class MainForm
    {
        #region Partial class data members

        delegate void OnTestExecutorExecutionBeginDelegate(TestExecutor testExecutor, TestExecutionBeginArgs args);
        delegate void OnTestExecutorExecutionCompleteDelegate(TestExecutor testExecutor, TestExecutionCompleteArgs args);
        delegate void OnTestExecutorExecutionFinalizedDelegate();
        delegate void OnTestScriptObjectExecutionCompleteDelegate(TestScriptObject testScriptObject, TestScriptResult testScriptObjectResult);
        delegate void OnTestScriptObjectExecutionBeginDelegate(TestScriptObject testScriptObject, TestScriptObjectBeginExecutionArgs args);
        delegate void OnTestTraceDelegate(string virtualUser, string traceMessage);
        delegate void OnTestWarningDelegate(TestWarning testWarning);
        delegate void OnTestCheckDelegate(TestCheck testCheck);

        #endregion

        #region Partial class private methods

        private void registerRuntimeEvents()
        {
            TestBreakpoints.OnTestBreakpointEnter += TestBreakpoints_OnTestBreakpointEnter1;
            TestBreakpoints.OnTestBreakpointExit += TestBreakpoints_OnTestBreakpointExit;

            // TestExecutor events
            TestExecutor.OnExecutionBegin += TestExecutor_OnExecutionBegin;
            TestExecutor.OnExecutionComplete += TestExecutor_OnExecutionComplete;
            TestExecutor.OnTestExecutionFinalizedEvent += TestExecutor_OnTestExecutionFinalizedEvent;

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

        private void TestBreakpoints_OnTestBreakpointEnter1(TestBreakpoint testBreakpoint, TestBreakPointArgs args)
        {
            if (this.InvokeRequired)
            {
                var @delegate = new TestBreakpoints.TestBreakpointEnterHandler(onTestBreakpointEnter);
                BeginInvoke(@delegate, new object[] { testBreakpoint, args });
            }
            else
            {
                onTestBreakpointEnter(testBreakpoint, args);
            }
        }

        private void TestBreakpoints_OnTestBreakpointExit(TestBreakpoint testBreakpoint, TestBreakPointArgs args)
        {
            if (this.InvokeRequired)
            {
                var @delegate = new TestBreakpoints.TestBreakpointExitHandler(onTestBreakPointExit);
                BeginInvoke(@delegate, new object[] { testBreakpoint, args });
            }
            else
            {
                onTestBreakPointExit(testBreakpoint, args);
            }
        }

        private void onTestBreakpointEnter(TestBreakpoint testBreakpoint, TestBreakPointArgs args)
        {
            this.m_executeToolStripButton.Enabled = true;
            this.m_stepOverButton.Enabled = true;
            // Bring app to forefront upon completiong
            Activate();
        }

        private void onTestBreakPointExit(TestBreakpoint testBreakpoint, TestBreakPointArgs args)
        {
            this.m_executeToolStripButton.Enabled = false;
            this.m_stepOverButton.Enabled = false;
        }

        private void unregisterRuntimeEvents()
        {
            TestBreakpoints.OnTestBreakpointEnter -= TestBreakpoints_OnTestBreakpointEnter1;
            TestBreakpoints.OnTestBreakpointExit -= TestBreakpoints_OnTestBreakpointExit;

            // TestExecutor events
            TestExecutor.OnExecutionBegin -= TestExecutor_OnExecutionBegin;
            TestExecutor.OnExecutionComplete -= TestExecutor_OnExecutionComplete;
            TestExecutor.OnTestExecutionFinalizedEvent -= TestExecutor_OnTestExecutionFinalizedEvent;

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

        private void TestExecutor_OnTestExecutionFinalizedEvent()
        {
            if (this.InvokeRequired)
            {
                OnTestExecutorExecutionFinalizedDelegate d = new OnTestExecutorExecutionFinalizedDelegate(onTestExecutionFinalized);
                BeginInvoke(d);
            }
            else
            {
                onTestExecutionFinalized();
            }
        }


        #endregion

        #region Runtime event methods
        Stopwatch m_stopWatch;

        private void onBeginningTestExecution(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            _isExecuting = true;

            // Start timers
            m_stopWatch = Stopwatch.StartNew();
            m_executionTimer.Start();

            // Set execution UI
            //Cursor.Current = Cursors.WaitCursor;
            setExecutionUI(true);
            resetViewerAndStatusBar();
            m_viewersTabControl.SelectedTab = m_traceViewerTabPage;

            int availableCases = 0;

            if (testExecutor.TestScriptObject is TestSuite)
            {
                availableCases = ((TestSuite)testExecutor.TestScriptObject).AvailableTestCases();

                if (m_testTreeView.TestProfile != null)
                {
                    availableCases = availableCases * m_testTreeView.TestProfile.VirtualUsers;
                }
            }
            else if (testExecutor.TestScriptObject is TestCase)
            {
                availableCases = 1;
            }

            m_totalAvailableStatusBarLabel.Tag = availableCases;
            m_totalAvailableStatusBarLabel.Text = availableCases.ToString();
            m_totalAvailableStatusBarLabel.ToolTipText = string.Format(m_totalLabelFormat, getPercentageString(100, 100));

            m_viewersTabControl.SelectedTab = m_traceViewerTabPage;

            m_traceViewer.AppendText(string.Format("Beginning execution ({0} virtual user(s)).\n\n",
                m_testTreeView.TestProfile != null ? m_testTreeView.TestProfile.VirtualUsers : 1));
            m_traceViewer.ScrollToCaret();
        }

        private void onTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            _isExecuting = false;

            // Stop timers
            m_stopWatch.Stop();
            m_executionTimer.Stop();

            // Update trace viewer
            var explanation = args.Explanation ?? "None";
            m_traceViewer.AppendText($"Execution complete.  Source: {args.TerminationSource}, Explanation:  {explanation}\n\n");
            m_traceViewer.ScrollToCaret();

            var node = m_testTreeView.FindNode(args.TestScriptObject);
            m_testTreeView.SelectedNode = node;

            if (node.TestScriptResult != null)
            {
                m_resultsViewer.Text = node.TestScriptResult.ToString();
            }

            // Bring app to forefront
            Activate();
        }

        private void onTestExecutionFinalized()
        {
            // Update trace viewer
            m_traceViewer.AppendText("Execution finalized\n\n");
            m_traceViewer.ScrollToCaret();

            // Flip viewer tab control to results viewer tab.
            m_viewersTabControl.SelectedTab = m_resultsViewerTabPage;

            // Update UI accordingly.
            setExecutionUI(false);
        }

        private void setExecutionUI(bool executing)
        {
            m_testPropertyGrid.Enabled = !executing;

            m_fileMenuItem.Enabled = !executing;
            m_editMenuItem.Enabled = !executing;
            m_helpMenuItem.Enabled = !executing;
            m_toolsMenuItem.Enabled = !executing;

            m_suiteResetMenuItem.Enabled = !executing;
            m_suiteExecuteMenuItem.Text = !executing ? "&Execute" : "&Continue";
            m_suiteStopExecuteMenuItem.Enabled = executing;

            m_executeToolStripButton.Text = !executing ? "Execute" : "Continue";
            m_executeToolStripButton.Enabled = !executing;

            m_stopToolStripButton.Enabled = executing;
            m_openToolStripButton.Enabled = !executing;
            m_newToolStripButton.Enabled = !executing;
            m_saveToolStripButton.Enabled = !executing;
            m_resetToolStripButton.Enabled = !executing;

            this.UseWaitCursor = executing;
        }

        private void onTestWarning(TestWarning testWarning)
        { }

        private void onTestCheck(TestCheck testCheck)
        { }

        private void onTestTrace(string virtualUser, string traceMessage)
        {
            m_traceViewer.AppendText($"{traceMessage} ({virtualUser})\n\n");
            m_traceViewer.ScrollToCaret();
        }

        private void onBeginningTestScriptObjectExecution(TestScriptObject testScriptObject, TestScriptObjectBeginExecutionArgs args)
        {
            if (testScriptObject is TestCase)
            {
                incrementLabelCounter(this.m_inprocessStatusBarLabel, m_inprogressLabelFormat);
                decrementLabelCounter(m_totalAvailableStatusBarLabel, m_totalLabelFormat);
            }
        }

        private void onTestScriptObjectExecutionComplete(TestScriptObject testScriptObject, TestScriptResult testScriptObjectResult)
        {
            TestTreeNode node = m_testTreeView.FindNode(testScriptObject);

            m_traceViewer.AppendText(string.Format("{0} \"{1}\" execution complete ({2}).\n\n",
                node.GetTestScriptObjectType(), node.TestScriptObject.Title, testScriptObjectResult.VirtualUser));

            if (testScriptObjectResult is TestCaseResult)
            {
                decrementLabelCounter(m_inprocessStatusBarLabel, m_inprogressLabelFormat);

                switch (testScriptObjectResult.TestVerdict)
                {
                    case TestVerdict.Pass:
                        incrementLabelCounter(m_passedStatusBarLabel, m_passedLabelFormat);
                        break;
                    case TestVerdict.Fail:
                        incrementLabelCounter(m_failedStatusBarLabel, m_failededLabelFormat);
                        break;
                    case TestVerdict.Error:
                        incrementLabelCounter(m_erroredStatusBarLabel, m_erroredLabelFormat);
                        break;
                    case TestVerdict.Inconclusive:
                        incrementLabelCounter(m_inconclusiveStatusBarLabel, m_inconclusiveLabelFormat);
                        break;
                    case TestVerdict.DidNotExecute:
                        incrementLabelCounter(m_didnotexecuteStatusBarLabel, m_didnotexecuteLabelFormat);
                        break;
                    default:
                        break;
                }

                updateStatusBarTooltips();
            }
        }

        private void decrementLabelCounter(ToolStripStatusLabel label, string toolTipFormat)
        {
            int counter = (int)label.Tag;
            counter--;
            label.Tag = counter;
            label.Text = counter.ToString();
        }

        private void incrementLabelCounter(ToolStripStatusLabel label, string toolTipFormat)
        {
            int counter = (int)label.Tag;
            counter++;
            label.Tag = counter;
            label.Text = counter.ToString();
        }

        const string m_totalLabelFormat = "Test cases pending execution {0}";
        const string m_inprogressLabelFormat = "Test cases currently in execution {0}";
        const string m_passedLabelFormat = "Test cases that have passed {0}";
        const string m_failededLabelFormat = "Test cases that have failed {0}";
        const string m_erroredLabelFormat = "Test cases that have errored during execution {0}";
        const string m_inconclusiveLabelFormat = "Test cases that have inconclusive results {0}";
        const string m_didnotexecuteLabelFormat = "Test cases that did not execute {0}";

        private void updateStatusBarTooltips()
        {
            int available = (int)m_totalAvailableStatusBarLabel.Tag;
            int inprocess = (int)m_inprocessStatusBarLabel.Tag;
            int passed = (int)m_passedStatusBarLabel.Tag;
            int failed = (int)m_failedStatusBarLabel.Tag;
            int errored = (int)m_erroredStatusBarLabel.Tag;
            int inconclusive = (int)m_inconclusiveStatusBarLabel.Tag;
            int didnotexecute = (int)m_didnotexecuteStatusBarLabel.Tag;

            int total = available + passed + failed + errored + inconclusive + didnotexecute;

            m_inprocessStatusBarLabel.ToolTipText = string.Format(m_inprogressLabelFormat, getPercentageString(inprocess, total));
            m_totalAvailableStatusBarLabel.ToolTipText = string.Format(m_totalLabelFormat, getPercentageString(available, total));
            m_passedStatusBarLabel.ToolTipText = string.Format(m_passedLabelFormat, getPercentageString(passed, total));
            m_failedStatusBarLabel.ToolTipText = string.Format(m_failededLabelFormat, getPercentageString(failed, total));
            m_erroredStatusBarLabel.ToolTipText = string.Format(m_erroredLabelFormat, getPercentageString(errored, total));
            m_inconclusiveStatusBarLabel.ToolTipText = string.Format(m_inconclusiveLabelFormat, getPercentageString(inconclusive, total));
            m_didnotexecuteStatusBarLabel.ToolTipText = string.Format(m_didnotexecuteLabelFormat, getPercentageString(didnotexecute, total));
        }

        private string getPercentageString(int numerator, int denominator)
        {
            string percentageString = null;

            if (denominator != 0)
            {
                percentageString = string.Format("({0:0.0}%)", denominator != 0 ? ((decimal)numerator / (decimal)denominator) * 100 : 0);
            }

            return percentageString;
        }

        #endregion
    }
}
