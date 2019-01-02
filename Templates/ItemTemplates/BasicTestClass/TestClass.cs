using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTF = Quintity.TestFramework.Core;

namespace $rootnamespace$
{
    [QTF.TestClass]
    public class $safeitemrootname$ : QTF.TestClass
    {
        #region Data members

        #endregion

        #region Constructors

        public $safeitemname$()
        { }

        #endregion

        #region Test methods

        [QTF.TestMethod]
        public QTF.TestResult MyTestMethod(
            [QTF.TestParameter("String parameter", "Enter string parameter", "Default value")]
            string param1,
            [QTF.TestParameter("Int parameter", "Enter integer parameter", 0)]
            int param2,
            [QTF.TestParameter("Boolean parameter", "Enter boolean parameter", true)]
            bool param3)
        {
            try
            {
                Setup();

                QTF.TestTrace.Trace("This is a sample TestTrace message.");

                QTF.TestAssert.IsTrue(param3, "This is a sample assertion.");

                QTF.TestCheck.IsTrue("Test of param3", param3, "This is a sample TestCheck.");

                TestMessage += "This is the returned test message.";
                TestResult = QTF.TestResult.Pass;
            }
            catch (QTF.TestAssertFailedException e)
            {
                TestMessage += e.ToString();
                TestResult = QTF.TestResult.Fail;
            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestResult = QTF.TestResult.Error;
            }
            finally
            {
                Teardown();
            }

            return TestResult;
        }

        #endregion

        #region Private methods

        #endregion

        #region Protected methods

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
