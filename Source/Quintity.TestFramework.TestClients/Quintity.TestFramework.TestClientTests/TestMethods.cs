using System;
using Quintity.TestFramework.Core;


namespace Quintity.TestFramework.TestClientTests
{
    [TestClass]
    public class TestMethods : TestClassBase
    {
        #region TestMethods

        [TestMethod("This is a simple test", "This is the description")]
        public TestVerdict SimpleTest(bool throwException)
        {
            try
            {
                Setup();

                if (throwException)
                {
                    throw new Exception("This is a big deal.");
                }
            }
            catch ( Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }
            finally
            {
                Teardown();
            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict ScratchTest1()
        {
            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict SetBreakPoint(string systemId)
        {
           // TestBreakPoints.AddBreakPoint(new Guid(systemId));
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


        #endregion

        #region Private and protected methods

        protected override void Setup()
        {
            base.Setup();
        }

        protected override void Teardown()
        {
            base.Teardown();
        }

        #endregion
    }
}
