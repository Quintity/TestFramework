using System;
using System.Collections.Generic;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;

namespace Quintity.TestFramework.TestListener.MicrosoftDevOps
{
    public partial class Class1 : Runtime.TestListener
    {
        #region Data members

        private Dictionary<string, string> _arguments = null;

        #endregion

        #region Constructors

        public Class1(Dictionary<string, string> args)
        {
            args.Add("PAT", "6zdzztjjru5fcegqmj7oftu4pmndnrfs3oa2w4l27jm2zrfvxiwq");
            args.Add("DevOpsUri", "");


            _arguments = args;
        }

        #endregion

        #region Event handlers

        public override void OnTestExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
           
        }

        public override void OnTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            throw new NotImplementedException();
        }

        public override void OnTestAttachmentAttach(string virtualUser, TestAttachment testAttachment)
        {
            throw new NotImplementedException();
        }

        public override void OnTestAttachmentDetach(string virtualUser, string key)
        {
            throw new NotImplementedException();
        }

        public override void OnTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args)
        {
            throw new NotImplementedException();
        }

        public override void OnTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult)
        {
            throw new NotImplementedException();
        }

        public override void OnTestCheck(TestCheck testCheck)
        {
            throw new NotImplementedException();
        }

        public override void OnTestMetric(string virtualUser, TestMetricEventArgs args)
        {
            throw new NotImplementedException();
        }

        public override void OnTestPostprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        {
            throw new NotImplementedException();
        }

        public override void OnTestPostprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        {
            throw new NotImplementedException();
        }

        public override void OnTestPreprocessorBegin(TestSuite testSuite, TestProcessorBeginExecutionArgs args)
        {
            throw new NotImplementedException();
        }

        public override void OnTestPreprocessorComplete(TestSuite testSuite, TestProcessorResult testProcessorResult)
        {
            throw new NotImplementedException();
        }

        public override void OnTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            throw new NotImplementedException();
        }

        public override void OnTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult)
        {
            throw new NotImplementedException();
        }

        public override void OnTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            throw new NotImplementedException();
        }

        public override void OnTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            throw new NotImplementedException();
        }

        public override void OnTestTrace(string virtualUser, string traceMessage)
        {
            throw new NotImplementedException();
        }

        public override void OnTestWarning(TestWarning testWarning)
        {
            throw new NotImplementedException();
        }

        #endregion  Event Handlers
    }
}
