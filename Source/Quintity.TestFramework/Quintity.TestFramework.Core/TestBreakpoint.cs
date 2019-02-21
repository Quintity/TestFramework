using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private Guid _testScriptObjectID;
        public Guid TestScriptObjectID
        {
            get { return _testScriptObjectID; }
            set { _testScriptObjectID = value; }
        }

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
            _testScriptObjectID = testScriptObjectId;
            _state = State.Enabled;
        }

        public TestBreakpoint()
        { }

        #endregion
    }
}
