using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quintity.TestFramework.Core;
using Quintity.TestFramework.Runtime;

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
                var testRunId = TestRunId;

                TestCheck.IsTrue("Is true", true);
                var testCheck = TestCheck.IsFalse("Is false", true);

                TestVerdict = TestVerdict.Fail;

                TestMessage = "This is the message.";

                //TestAssert.IsTrue(false, "TestAssert message here");
                //throw new Exception("This is the exception.");
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
