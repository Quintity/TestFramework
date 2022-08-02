using System;
using System.Collections.Generic;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestProject
{
    [TestClass]
    public class TestMethods : TestClassBase
    {
        [TestMethod]
        public TestVerdict TestCacheTests()
        {
            return TestVerdict.Pass;
        }

        [TestMethod]
        public TestVerdict ScratchTest1(
            [TestParameter("String parameter", description: "This is the string parameter description", required: true)]
            string stringParam,
            [TestParameter("Boolean parameter", required: true)]
            bool boolParam,
            [TestParameter("Integer parameter", required: true)]
            int intParam1,
            [TestParameter("Integer parameter", required: false)]
            int? intParam2)
        {
            try
            {
                TestCache.Stash("Something", new List<string>() 
                { "ItemOne"});

                var thing1 = TestCache.Grab("Something");
                var thing2 = TestCache.Grab<List<string>>("Something");

                try
                {
                    var thing3 = TestCache.Grab("Nothing");
                }
                catch (Exception e)
                {
                    var msg = e.Message;
                }

                try
                {
                    var thing3 = TestCache.Grab<string>("Something");
                }
                catch (Exception e)
                {
                    var msg = e.Message;
                }

                var found1 = TestCache.TryGrabValue("Something", out List<string> value1);
                var found2 = TestCache.TryGrabValue("Nothing", out List<string> value2);


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
