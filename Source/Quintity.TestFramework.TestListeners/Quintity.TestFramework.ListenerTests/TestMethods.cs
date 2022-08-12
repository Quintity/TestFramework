using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.ListenerTests
{
    [TestClass]
    public class TestMethods : TestClassBase
    {
        #region Data members

        #endregion

        #region Constructors

        public TestMethods()
        { }

        #endregion

        #region Test methods

        [TestMethod]
        public TestVerdict MyTestMethod(
            [TestParameter("String parameter", "Enter string parameter", "Default value")]
            string param1,
            [TestParameter("Int parameter", "Enter integer parameter", 0)]
            int param2,
            [TestParameter("Boolean parameter", "Enter boolean parameter", true)]
            bool param3)
        {
            try
            {
                Setup();

                TestTrace.Trace("This is a sample TestTrace message.");

                TestAssert.IsTrue(param3, "This is a sample assertion.");

                TestCheck.IsTrue("Test of param3", param3, "This is a sample TestCheck.");

                TestMessage += "This is the returned test message.";
                TestVerdict = TestVerdict.Pass;
            }
            catch (TestAssertFailedException e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Fail;
            }
            catch (Exception e)
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
        public TestVerdict ADOScratch()
        {
            try
            {
                var url = "https://dev.azure.com/jmothershead";
                var personalAccessToken = "6zdzztjjru5fcegqmj7oftu4pmndnrfs3oa2w4l27jm2zrfvxiwq";

                VssBasicCredential vbc = default(VssBasicCredential);
                //VssCredentials cred = default(VssCredentials);

                vbc = new VssBasicCredential(string.Empty, personalAccessToken);

                VssConnection connection = new VssConnection(new Uri(url), new VssBasicCredential(string.Empty, personalAccessToken));

                //using (TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri(url), vbc))
                //{
                //    tfs.Authenticate();

                //    var workItemStore = tfs.GetService<WorkItemStore>();
                //}
            }
            catch (Exception e)
            { }
            finally
            {

            }

            return TestVerdict.Pass;
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
