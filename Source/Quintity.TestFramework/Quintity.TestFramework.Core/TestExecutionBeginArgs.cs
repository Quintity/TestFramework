using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [KnownType(typeof(TestSuite))]
    [KnownType(typeof(TestCase))]
    [KnownType(typeof(TestStep))]
    [DataContract]
    public class TestExecutionBeginArgs
    {
        #region Class data members

        // Virtual User
        [DataMember]
        private string _virtualUser;

        [IgnoreDataMember]
        public string VirtualUser
        { get { return _virtualUser; } }

        // TestRun Id
        [DataMember]
        private string _testRunId;

        [IgnoreDataMember]
        public string TestRunId
        { get { return _testRunId; } }

        // TestScriptObject
        [DataMember]
        private TestScriptObject _testScriptObject;

        [IgnoreDataMember]
        public TestScriptObject TestScriptObject
        { get { return _testScriptObject; } }

        #endregion

        #region Class constructors

        public TestExecutionBeginArgs(string virtualUser, TestScriptObject testScriptObject)
        {
            _virtualUser = virtualUser;
            _testScriptObject = testScriptObject;
            _testRunId = TestProperties.TestRunId;
        }

        #endregion

        #region Class public methods

        #endregion

        #region Class private methods

        #endregion
    }
}
