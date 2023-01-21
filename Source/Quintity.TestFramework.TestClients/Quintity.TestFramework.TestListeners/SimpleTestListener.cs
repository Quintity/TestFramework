using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.TestListeners
{
    public class SimpleTestListener : TestListener
    {
        public SimpleTestListener()
        {

        }

        public SimpleTestListener(Dictionary<string, string> args)
        { }


        public override void OnTestAttachmentAttach(string virtualUser, TestAttachment testAttachment)
        {
            
        }

        public override void OnTestAttachmentDetach(string virtualUser, string key)
        {
            
        }

        public override void OnTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args)
        {
            
        }

        public override void OnTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult)
        {
            
        }

        public override void OnTestCheck(TestCheck testCheck)
        {
            
        }

        public override void OnTestExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            int i = 1;
        }

        public override void OnTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            
        }

        public override void OnTestMetric(string virtualUser, TestMetricEventArgs args)
        {
            
        }

        public override void OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        {
            
        }

        public override void OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        {
            
        }

        public override void OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        {
            
        }

        public override void OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        {
            
        }

        public override void OnTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            
        }

        public override void OnTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult)
        {
            
        }

        public override void OnTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            
        }

        public override void OnTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            
        }

        public override void OnTestTrace(string virtualUser, string traceMessage)
        {
            
        }

        public override void OnTestWarning(TestWarning testWarning)
        {
            
        }
    }
}
