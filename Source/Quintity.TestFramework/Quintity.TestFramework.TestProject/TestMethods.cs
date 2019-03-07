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
            var testRunId = TestProperties.GetPropertyValueAsString("TestRunId");
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
