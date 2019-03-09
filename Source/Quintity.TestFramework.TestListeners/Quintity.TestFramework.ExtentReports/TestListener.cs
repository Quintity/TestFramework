using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Core.Support;
using Quintity.TestFramework.Runtime;


namespace Quintity.TestFramework.ExtentReports
{
    public class TestListener : Runtime.TestListener
    {
        #region Data members

        private Dictionary<Guid, ExtentTest> _extentTests = new Dictionary<Guid, ExtentTest>();


        private AventStack.ExtentReports.ExtentReports _extent;

        #endregion
        #region Constructors

        public TestListener(Dictionary<string, string> args)
        { }

        #endregion

        public override void OnTestExecutionBegin(TestExecutor testExecutor, TestExecutionBeginArgs args)
        {
            // start reporters
            var htmlReporter = new ExtentHtmlReporter(@"C:\DevProjects\Quintity\Repos\TestFramework\Source\Quintity.TestFramework.TestListeners\Quintity.TestFramework.ListenerTests\TestResults\bob.html");

            // create ExtentReports and attach reporter(s)
            _extent = new AventStack.ExtentReports.ExtentReports();
            _extent.AttachReporter(htmlReporter);
            _extent.AddSystemInfo("sysname", "sysInfo");

            //// creates a test 
            //var test = _extent.CreateTest("MyFirstTest", "Sample1 description");

            //// log(Status, details)
            //test.Log(AventStack.ExtentReports.Status.Info, "This step shows usage of log(status, details)");

            //// info(details)
            //test.Info("This step shows usage of info(details)");

            //// log with snapshot
            //test.Fail("details",
            //    MediaEntityBuilder.CreateScreenCaptureFromPath(@"c:\temp\screenshot.png").Build());

            //// test with snapshot
            //test.AddScreenCaptureFromPath(@"c:\temp\screenshot.png");

            //test = _extent.CreateTest("MySecondTest", "Sample2 description");
            //test.AssignAuthor(new string[] { "jmothers" });
            //var test2 = test.CreateNode("Bob", "FirstNode");
            //test2.CreateNode("Jim", "Nested node");
        }

        public override void OnTestExecutionComplete(TestExecutor testExecutor, TestExecutionCompleteArgs args)
        {
            _extent.Flush();
        }

        public override void OnTestSuiteExecutionBegin(TestSuite testSuite, TestSuiteBeginExecutionArgs args)
        {
            ExtentTest extentTest = null;

            var description = $"Test suite:  {testSuite.Title}";

            //If parent is empty, must be root test suite.
            if (testSuite.ParentID.Equals(Guid.Empty))
            {
                extentTest = _extent.CreateTest(testSuite.Title);
            }
            else
            {
                extentTest = getExtentTest(testSuite.ParentID).CreateNode(testSuite.Title);
            }
            var bob = extentTest.Model;

            addToDictionary(testSuite.SystemID, extentTest);
        }


        public override void OnTestCaseExecutionBegin(TestCase testCase, TestCaseBeginExecutionArgs args)
        {
            var testExtent = getExtentTest(testCase.ParentID).CreateNode($"Test case: - {testCase.Title}");

            addToDictionary(testCase.SystemID, testExtent);
        }

        public override void OnTestCaseExecutionComplete(TestCase testCase, TestCaseResult testCaseResult)
        {
            //var testWait = new TestWait<Guid>(testCase.SystemID, "message text", TimeSpan.FromSeconds(30));

            //var spud = testWait.Until<ExtentTest>((x) => dosomething(testCase.SystemID));

            //var spud = testWait.Until<ExtentTest>((x)=>
            //{
            //    _extentTests.TryGetValue(x, out ExtentTest extentTest);

            //    return extentTest;
            //});

            //var dork = testWait(x => x.)
            //var dork = sam.Until<ExtentTest>(testCase.SystemID)=> dosomething(x);

            formatResult(getExtentTest(testCase.SystemID), testCase, testCaseResult);
        }

        /// <summary>
        /// Gets the TestScriptObject's matching ExtentTest from dictionary.
        /// </summary>
        /// <param name="testScriptObject">System ID of object who's key to search on.</param>
        /// <returns>Objects matching TestExtent object (previously created)</returns>
        private ExtentTest getExtentTest(Guid systemId)
        {
            ExtentTest extentTest = null;
            var message = $"Did not find matching TestExtent object for system Id:  {systemId}";

            var testWait = new TestWait<Guid>(systemId, message, TimeSpan.FromSeconds(2));

            var spud = testWait.Until<ExtentTest>((guid) =>
            {
                _extentTests.TryGetValue(guid, out extentTest);

                return extentTest;
            });

            return extentTest;
        }

        private ExtentTest dosomething(Guid guid)
        {
            _extentTests.TryGetValue(guid, out ExtentTest extentTest);

            return extentTest;
        }

        //private void dosomething(Guid)
        //{ }

        //private void bob()
        //{
        //    var testWait = new TestWait<Session>(session, "Vendor request files not downloaded.", TimeSpan.FromSeconds(60));

        //    downLoadedFiles = testWait.Until<List<string>>((x) =>
        //    {
        //        // Download file to target folder.
        //        var result = session.GetFiles(remotePath, targetFolder + '\\', false, transferOptions);

        //        result.Check();

        //        return result.Transfers.Count > 0 ? getDestinationFiles(result.Transfers) : null;
        //    });
        //}

        private void formatResult(ExtentTest extentTest, TestScriptObject testScriptObject, TestScriptResult testScriptResult)
        {
            if (testScriptResult.TestVerdict == TestVerdict.Pass)
            {
                extentTest.Pass(testScriptResult.TestMessage);
            }
            else if (testScriptResult.TestVerdict == TestVerdict.Fail)
            {
                extentTest.Fail(testScriptResult.TestMessage);
            }
            else if (testScriptResult.TestVerdict == TestVerdict.Error)
            {
                extentTest.Error(testScriptResult.TestMessage);
            }
        }

        private object dictionaryLock = new object();

        private void addToDictionary(Guid systemId, ExtentTest testExtent)
        {
            lock (dictionaryLock)
            {
                _extentTests.Add(systemId, testExtent);
            }

            Thread.Sleep(1000);
        }

        public override void OnTestStepExecutionBegin(TestStep testStep, TestStepBeginExecutionArgs args)
        {
            var testExtent = getExtentTest(testStep.ParentID).CreateNode($"Step: - {testStep.Title}");

            //var testExtent = getExtentTest(testStep.SystemID)[testStep.ParentID].CreateNode($"Step - {testStep.Title}");
            addToDictionary(testStep.SystemID, testExtent);
        }

        public override void OnTestStepExecutionComplete(TestStep testStep, TestStepResult testStepResult)
        {
            formatResult(getExtentTest(testStep.SystemID), testStep, testStepResult);
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

        public override void OnTestSuiteExecutionComplete(TestSuite testSuite, TestSuiteResult testSuiteResult)
        {
            formatResult(getExtentTest(testSuite.SystemID), testSuite, testSuiteResult);
        }

        [NoOperation]
        public override void OnTestCheck(TestCheck testCheck) { }

        [NoOperation]
        public override void OnTestMetric(string virtualUser, TestMetricEventArgs args) { }

        [NoOperation]
        public override void OnTestTrace(string virtualUser, string traceMessage) { }

        [NoOperation]
        public override void OnTestWarning(TestWarning testWarning) { }
    }
}
