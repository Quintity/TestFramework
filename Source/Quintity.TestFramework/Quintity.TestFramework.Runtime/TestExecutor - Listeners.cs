using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Quintity.TestFramework.Core;
using System;

namespace Quintity.TestFramework.Runtime
{
    public partial class TestExecutor
    {
        #region Partial class data members

        private Dictionary<string, RuntimeStatus> _virtualUserRuntimeState;

        internal struct ParamsStruct
        {
            public TestListenerDescriptor TestListenerDescriptor
            { get; set; }

            public string Method
            { get; set; }

            public object[] Parameters
            { get; set; }
        }

        #endregion

        #region Partial class private methods

        internal class TestListenerEventParams
        {
            private string _virtualUser;
            public string VirtualUser
            { get { return _virtualUser; } }

            private string _method;
            public string Method
            { get { return _method; } }

            private object[] _parameters;
            public object[] Parameters
            { get { return _parameters; } }

            public TestListenerDescriptor TestListenerDescriptor
            { get; set; }

            public TestListenerEventParams(string virtualUser, string method, params object[] parameters)
            {
                _virtualUser = virtualUser;
                _method = method;
                _parameters = parameters;
            }
        }

        private class RuntimeStatus
        {
            public Queue<TestListenerEventParams> ListenerEventQueue { get; set; }

            public RuntimeStatus()
            {
                ListenerEventQueue = new Queue<TestListenerEventParams>();
            }
        }

        private void registerRuntimeEventHandlers()
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
            TestMetric.OnTestMetric += TestMetric_OnTestMetric;
            TestAttachment.OnTestAttachmentAttach += TestAttachment_OnAddTestAttachment;
            TestAttachment.OnTestAttachmentDetach += TestAttachment_OnDetachTestAttachment;
        }

        private void unregisterRuntimeEventHandlers()
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
            TestMetric.OnTestMetric -= TestMetric_OnTestMetric;
        }

        #region Runtime event handlers

        /// <summary>
        /// OnTestExecutionFinalized event handler
        /// </summary>
        private void TestExecutor_OnTestExecutionFinalizedEvent()
        {
            // Signal/release main execution thread.
            _resetEvent.Set();

            Debug.WriteLine("Unregistering runtime event handlers.");
            unregisterRuntimeEventHandlers();

            // Remove currently executing TestScriptObjects from TestProperties (no long meaningful).
            TestProperties.RemoveProperty("CurrentTestSuite");
            TestProperties.RemoveProperty("CurrentTestCase");
            TestProperties.RemoveProperty("CurrentTestStep");

            _logEvent.Info(message: "Test execution finalized.");
        }

        private void TestExecutor_OnExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            _logEvent.Info(message: $"Beginning test execution ({args.TestRunId})");
            _listenerEventsClient?.OnTestExecutionBegin(args);
        }

        private void TestExecutor_OnExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            _logEvent.Info(message: $"Test execution complete ({args.TestRunId}), elapsed time:  {args.ElapsedTime}");
            _listenerEventsClient?.OnTestExecutionComplete(args);

            Thread.Sleep(3000);

            var available = _listenerEventsClient?.ServiceAvailability();

            _virtualUserRuntimeState.Remove(args.VirtualUser);

            if (_virtualUserRuntimeState.Count <= 0)
            {
                finalizeTestExecution();
            }
        }
        
        private void finalizeTestExecution()
        {
            //Debug.WriteLine("Unregistering runtime event handlers.");
            unregisterRuntimeEventHandlers();

            // Remove currently executing TestScriptObjects from TestProperties (no longer meaningful).
            TestProperties.RemoveProperty("CurrentTestSuite");
            TestProperties.RemoveProperty("CurrentTestCase");
            TestProperties.RemoveProperty("CurrentTestStep");
            TestProperties.RemoveProperty("TestRunId");

            fireTestExecutionFinalizedEvent();

            // Signal/release main execution thread.
            _resetEvent.Set();
        }

        private void TestSuite_OnExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            TestProperties.SetPropertyValue("CurrentTestSuite", testSuite);

            _logEvent.Info(message: $"Beginning test suite execution \"{testSuite.Title}\"");
            _listenerEventsClient?.OnTestSuiteExecutionBegin(testSuite, args);
        }

        private void TestSuite_OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        {
            _listenerEventsClient?.OnTestPreprocessorBegin(testSuite, args);
        }
        
        private void TestSuite_OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        {
            _listenerEventsClient?.OnTestPreprocessorComplete(testSuite, testProcessorResult);
        }

        private void TestSuite_OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        {
            _listenerEventsClient?.OnTestPostprocessorBegin(testSuite, args);
        }

        private void TestSuite_OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        {
            _listenerEventsClient?.OnTestPostprocessorComplete(testSuite, testProcessorResult);
        }

        private void TestSuite_OnExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            _logEvent.Info(message: $"Test suite execution \"{testSuite.Title}\" completed ({testSuiteResult.TestVerdict}).");
            _listenerEventsClient?.OnTestSuiteExecutionComplete((TestSuite)testSuite, testSuiteResult);
        }

        private void TestCase_OnExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args)
        {
            TestProperties.SetPropertyValue("CurrentTestCase", testCase);

            _logEvent.Info(message: $"Beginning test case execution \"{testCase.Title}\"");
            _listenerEventsClient?.OnTestCaseExecutionBegin(testCase, args);
        }

        private void TestCase_OnExecutionComplete(TestCase testCase, TestScriptResult testCaseResult)
        {
            _logEvent.Info(message: $"Test case execution \"{testCase.Title}\" completed ({testCaseResult.TestVerdict}).");
            _listenerEventsClient?.OnTestCaseExecutionComplete((TestCase)testCase, (TestCaseResult)testCaseResult);
        }

        private void TestStep_OnExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            TestProperties.SetPropertyValue("CurrentTestStep", testStep);

            _logEvent.Info(message: $"Beginning test step execution \"{testStep.Title}\"");
            _listenerEventsClient?.OnTestStepExecutionBegin((TestStep)testStep, args);
        }

        private void TestStep_OnExecutionComplete(TestStep testStep, TestStepResult testStepResult)
        {
            var logMsg = testStepResult.TestVerdict == TestVerdict.Pass ? string.Empty : 
                ':' + Environment.NewLine + testStepResult.ToString(true);

            _logEvent.Info(message: $"Test step execution \"{testStep.Title}\" completed ({testStepResult.TestVerdict}) {logMsg}");
            _listenerEventsClient?.OnTestStepExecutionComplete((TestStep)testStep, (TestStepResult)testStepResult);
        }

        private void TestCheck_OnTestCheck(TestCheck testCheck)
        {
            _logEvent.Info($"Test check event: {Environment.NewLine + testCheck.ToString()}");
            _listenerEventsClient?.OnTestCheck(testCheck);
        }

        private void TestWarning_OnTestWarning(string virtualUser, TestWarning testWarning)
        {
            _logEvent.Info($"Test warning event: {Environment.NewLine + testWarning.ToString()}");
            _listenerEventsClient?.OnTestWarning(testWarning);
        }

        private void TestTrace_OnTestTrace(string virtualUser, string traceMessage)
        {
            _logEvent.Info(message: $"TestTrace:  {traceMessage}");
            _listenerEventsClient?.OnTestTrace(virtualUser, traceMessage);
        }

        private void TestMetric_OnTestMetric(string virtualUser, TestMetricEventArgs args)
        {
            _logEvent.Info($"TestMetric event {args.ToString()}");
            _listenerEventsClient?.OnTestMetric(virtualUser, args);
        }

        private void TestAttachment_OnDetachTestAttachment(string virtualUser, string key)
        {
            _logEvent.Info(message: $"TestAttachment {key} detached");
            //_listenerEventsClient?.OnTestAttachmentDetach(virtualUser, key);
        }

        private void TestAttachment_OnAddTestAttachment(string virtualUser, TestAttachment testAttachment)
        {
            _logEvent.Info(message: $"TestAttachment attached:  {testAttachment.ToString()}");
            //_listenerEventsClient?.OnTestAttachmentAttach(virtualUser, testAttachment);


        }

        #endregion

        #endregion
    }
}
