using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTF = Quintity.TestFramework.Core;

namespace $safeprojectname$
{
    public class TestListener : QTF.ITestExecutionListener
    {
        public void TestExecutionBegin(Dictionary<string, object> listenerProperties, QTF.PreExecutionEventArgs eventArgs)
        {
            // Enter runtime event handler code here
        }

        public void TestSuiteExecutionBegin(QTF.TestSuite testSuite)
        {
            // Enter runtime event handler code here
        }

        public void TestPreprocessorExecutionBegin(QTF.TestSuite testSuite, QTF.TestProcessBeginEventArgs eventArgs)
        {
            // Enter runtime event handler code here
        }

        public void TestPreprocessorExecutionComplete(QTF.TestSuite testSuite, QTF.TestProcessCompleteEventArgs eventArgs)
        {
            // Enter runtime event handler code here
        }

        public void TestCaseExecutionBegin(QTF.TestCase testCase)
        {
            // Enter runtime event handler code here
        }

        public void TestStepExecutionBegin(QTF.TestStep testStep)
        {
            // Enter runtime event handler code here
        }

        public void TestTrace(string traceMessage)
        {
            // Enter runtime event handler code here
        }
        
        public void TestCheck(QTF.TestCheck testCheck)
        {
            // Enter runtime event handler code here
        }

        public void TestStepExecutionComplete(QTF.TestStep testStep, QTF.TestExecutionCompleteEventArgs eventArgs)
        {
            // Enter runtime event handler code here
        }

        public void TestCaseExecutionComplete(QTF.TestCase testCase, QTF.TestExecutionCompleteEventArgs eventArgs)
        {
            // Enter runtime event handler code here
        }

        public void TestPostprocessorExecutionBegin(QTF.TestSuite testSuite, QTF.TestProcessBeginEventArgs eventArgs)
        {
            // Enter runtime event handler code here
        }

        public void TestPostprocessorExecutionComplete(QTF.TestSuite testSuite, QTF.TestProcessCompleteEventArgs eventArgs)
        {
            // Enter runtime event handler code here
        }

        public void TestSuiteExecutionComplete(QTF.TestSuite testSuite, QTF.TestExecutionCompleteEventArgs eventArgs)
        {
            // Enter runtime event handler code here
        }

        public void TestExecutionComplete(QTF.PostExecutionEventArgs eventArgs)
        {
            // Enter runtime event handler code here
        }

        public void TestExecutionStopRequest(QTF.TestScriptObject testScriptObject, QTF.TestExecutionStopRequestArgs eventArgs)
        {
            // Enter runtime event handler code here
        }

        public void Dispose()
        {
            // Enter runtime event handler code here
        }
    }
}
