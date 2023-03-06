using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    abstract public class TestScriptResult : TestArtifact
    {
        #region Class data members

        [DataMember(Order = 1)]
        protected Guid _referenceSystemID;

        [DataMember(Order = 2)]
        protected string _referenceUserId;

        [DataMember(Order = 3)]
        protected string _referenceTitle;

        [DataMember(Order = 4)]
        protected TestVerdict _testVerdict;

        [DataMember(Order = 5)]
        protected string _testMessage;

        [DataMember(Order = 6)]
        protected string _virtualUser;

        [DataMember(Order = 7)]
        protected DateTime _startTime;

        [DataMember(Order = 8)]
        protected DateTime _endTime;

        [DataMember(Order = 9)]
        protected string _testRunId;

        #endregion

        #region Class properties

        [IgnoreDataMember]
        public Guid ReferenceSystemID
        { get { return _referenceSystemID; } }

        [IgnoreDataMember]
        public string ReferenceUserID
        { get { return _referenceUserId; } }

        [IgnoreDataMember]
        public string ReferenceTitle
        { get { return _referenceTitle; } }

        [IgnoreDataMember]
        public TestVerdict TestVerdict
        { get { return _testVerdict; } }

        [IgnoreDataMember]
        public string TestMessage
        { get { return _testMessage; } }

        [IgnoreDataMember]
        public string VirtualUser
        { get { return _virtualUser; } }

        [IgnoreDataMember]
        public DateTime StartTime
        { get { return _startTime; } }

        [IgnoreDataMember]
        public DateTime EndTime
        { get { return _endTime; } }

        [IgnoreDataMember]
        public TimeSpan ElapsedTime
        { get { return _endTime - _startTime; } }

        [IgnoreDataMember]
        public String TestRunId
        { get { return _testRunId; } }

        #endregion

        #region Class constructors

        public TestScriptResult()
            : base()
        {
            _virtualUser = Thread.CurrentThread.Name;
            _testRunId = TestProperties.GetPropertyValueAsString("TestRunId");
        }

        #endregion

        #region Class public methods

        public override string ToString(bool verbose = false)
        {
            return string.Format("TestRun ID: {0}\r\nTest verdict:  {1}\r\nTest message:  {2}\r\n\r\n" +
                "Start time:  {3}\r\nEnd time: {4}\r\nElapsed time:  {5}",
                _testRunId,
                _testVerdict,
                !string.IsNullOrEmpty(_testMessage) ? _testMessage : "None",
                _startTime,
                _endTime,
                _endTime - _startTime);
        }

        #endregion

        #region Class internal methods

        internal void SetReferenceID(Guid referenceID)
        {
            _referenceSystemID = referenceID;
        }

        internal void SetReferenceUserId(string referenceUserID)
        {
            _referenceUserId = referenceUserID;
        }

        internal void SetReferenceTitle(string referenceTitle)
        {
            _referenceTitle = referenceTitle;
        }

        internal void SetTestVerdict(TestVerdict testVerdict)
        {
            _testVerdict = testVerdict;
        }

        internal void SetTestMessage(string testMessage)
        {
            _testMessage += testMessage;
        }

        internal void SetVirtualUser(string virtualUser)
        {
            //_virtualUser = virtualUser;
        }

        internal void SetStartTime(DateTime startTime)
        {
            _startTime = startTime;
        }

        internal void SetEndTime(DateTime endTime)
        {
            _endTime = endTime;
        }

        #endregion

        #region Class private methods

        #endregion
    }
}
