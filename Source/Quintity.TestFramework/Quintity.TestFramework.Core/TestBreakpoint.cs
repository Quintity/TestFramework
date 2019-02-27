using System;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    public class TestBreakpoint
    {
        public enum State
        {
            Enabled,
            Disabled
        }

        #region Class properties

        // Properties are written this way for serialization purposes.

        [DataMember(Order = 0)]
        private DateTime _created;
        public DateTime Created
        {
            get { return _created; }
            set {_created = value; }
        }

        [DataMember(Order = 1)]
        private DateTime? _changed;
        public DateTime? Changed
        {
            get { return _changed; }
            set { _changed = value; }
        }

        [DataMember(Order = 2)]
        private Guid _testScriptObjectID;
        public Guid TestScriptObjectID
        {
            get { return _testScriptObjectID; }
            set { _testScriptObjectID = value; }
        }

        [DataMember(Order = 3)]
        private State _state;
        public State CurrentState
        {
            get { return _state; }
            set
            {
                _state = value;
                FireTestBreakpointStateChangedEvent(this, new TestBreakPointArgs());
            }
        }

        #endregion

        #region Class events

        // Breakpoint state changed
        public delegate void TestBreakpointStateChangeHandler(TestBreakpoint testBreakpoint, TestBreakPointArgs args);
        public static event TestBreakpointStateChangeHandler OnTestBreakpointStateChanged;

        private static void FireTestBreakpointStateChangedEvent(TestBreakpoint testBreakpoint, TestBreakPointArgs args)
        {
            OnTestBreakpointStateChanged?.Invoke(testBreakpoint, args);
        }

        #endregion

        #region Constructors

        public TestBreakpoint(TestScriptObject testScriptObject) : this(testScriptObject.SystemID)
        { }

        public TestBreakpoint(Guid testScriptObjectId) : this()
        {
            _created = DateTime.Now;
            _testScriptObjectID = testScriptObjectId;
            _state = State.Enabled;
        }

        public TestBreakpoint()
        { }

        #endregion
    }
}
