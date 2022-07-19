using System;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestProject
{
    [TestClass]
    public class TestMethods : TestClassBase
    {
        [TestMethod]
        public TestVerdict ScratchTest1()
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
