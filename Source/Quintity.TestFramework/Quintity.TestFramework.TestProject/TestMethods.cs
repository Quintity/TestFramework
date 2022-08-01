using System;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestProject
{
    [TestClass]
    public class TestMethods : TestClassBase
    {

        /*
         * <TestParameter>
                <DisplayName>stringParam</DisplayName>
                <Name>stringParam</Name>
                <TypeAsString>System.String</TypeAsString>
                <ValueAsString>[TestHome]</ValueAsString>
              </TestParameter>
              <TestParameter>
                <DisplayName>boolParam</DisplayName>
                <Name>boolParam</Name>
                <TypeAsString>System.Boolean</TypeAsString>
                <ValueAsString>False</ValueAsString>
              </TestParameter>
              <TestParameter>
                <DisplayName>intParam</DisplayName>
                <Name>intParam</Name>
                <TypeAsString>System.Int32</TypeAsString>
                <ValueAsString>0</ValueAsString>
              </TestParameter>
         */
        [TestMethod]
        public TestVerdict ScratchTest1(
            [TestParameter("String parameter", description: "This is the string parameter description", required: true)]
            string stringParam,
            [TestParameter("Boolean parameter", required: true)]
            bool boolParam,
            [TestParameter("Integer parameter", required: true)]
            int intParam1,
            [TestParameter("Integer parameter", required: true)]
            int? intParam2)
        {
            try
            {
                TestTrace.Trace($"Test run ID:  {TestRunId}");
                var testRunId = TestRunId;

                TestCheck.IsTrue("Verify whatcamacallit is true", true);
                var testCheck = TestCheck.IsFalse("Verify whatcamacallit is false", true);

                TestVerdict = TestVerdict.Fail;

                TestMessage = "This is the message.";

                TestAttachment.Attach("bob", "value");
                TestAttachment.Detach("bob");

                TestWarning.IsTrue(false, "The sky is falling");

                //TestAssert.IsTrue(false, "TestAssert message here");
                //throw new Exception("This is the exception."); 
            }
            catch (Exception exp)
            {
                int i = 1;
                throw;
            }
            finally
            {
                //var verdict = GetCurrentTestVerdict();

                int i = 1;
            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict SetBreakPoint()
        {
            //TestBreakPoints.InsertBreakPoint(new Guid("A9C54F2F-B399-4B0D-9797-0FDAC2C09876"));
            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict ScratchTest2()
        {
            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict ScratchTest3()
        {
            return TestVerdict;
        }
    }
}
