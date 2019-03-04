using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestClientTests
{
    [TestClass]
    public class ExtentReportsTests : TestClassBase
    {
        public static ExtentReportsTests extentReports;
        public static ExtentTest extentTest;

        [TestMethod]
        public TestVerdict RunReport()
        {
            try
            {
                StartReport();
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }

            return TestVerdict;
        }

        public void StartReport()
        {
            // start reporters
            var htmlReporter = new ExtentHtmlReporter($"{TestProperties.TestResults}\\bob.html");

            // create ExtentReports and attach reporter(s)
            var extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

            // creates a test 
            var test = extent.CreateTest("MyFirstTest", "Sample1 description");

            // log(Status, details)
            test.Log(AventStack.ExtentReports.Status.Info, "This step shows usage of log(status, details)");

            // info(details)
            test.Info("This step shows usage of info(details)");

            // log with snapshot
            test.Fail("details",
                MediaEntityBuilder.CreateScreenCaptureFromPath(@"c:\temp\screenshot.png").Build());

            // test with snapshot
            test.AddScreenCaptureFromPath(@"c:\temp\screenshot.png");

            test = extent.CreateTest("MySecondTest", "Sample2 description");
            test.AssignAuthor(new string[] { "jmothers" });
            var test2 = test.CreateNode("Bob", "FirstNode");
            test2.CreateNode("Jim", "Nested node");

            // calling flush writes everything to the log file
            extent.Flush();
        }
    }
}
