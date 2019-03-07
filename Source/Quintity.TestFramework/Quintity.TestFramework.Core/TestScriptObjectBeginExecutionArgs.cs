using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestScriptObjectBeginExecutionArgs
    {
        #region Class data members

        [DataMember]
        private string _virtualUser;

        [DataMember]
        private string _testRunId;

        #endregion

        #region Class properties

        [IgnoreDataMember]
        public string VirtualUser
        { get { return _virtualUser; } }

        [IgnoreDataMember]
        public string TestRunId
        { get { return _testRunId; } }

        #endregion

        #region Class constructors

        public TestScriptObjectBeginExecutionArgs(string virtualUser)
        {
            _virtualUser = virtualUser;
            _testRunId = TestProperties.TestRunId;
        }

        #endregion

        #region Class public methods

        #endregion

        #region Class private methods

        #endregion
    }
}
