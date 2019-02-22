using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class TestBreakPointArgs
    {
    }

    public static class TestBreakpoints
    {
        private static ManualResetEvent ResetEvent = new ManualResetEvent(false);
        private static List<TestBreakpoint> _breakPoints;
        private static TestBreakpoint _currentBreakpoint = null;
        public static bool StepOverMode { get; set; }

        #region Class events

        // Breakpoint inserted
        public delegate void TestBreakpointInsertedHandler(TestBreakpoint testBreakpoint, TestBreakPointArgs args);
        public static event TestBreakpointInsertedHandler OnTestBreakpointInserted;

        private static void FireTestBreakpointInsertedEvent(TestBreakpoint testBreakpoint, TestBreakPointArgs args)
        {
            OnTestBreakpointInserted?.Invoke(testBreakpoint, args);
        }

        // Breakpoint deleted
        public delegate void TestBreakpointDeletedHandler(TestBreakpoint testBreakpoint, TestBreakPointArgs args);
        public static event TestBreakpointDeletedHandler OnTestBreakpointDeleted;

        private static void FireTestBreakpointDeletedEvent(TestBreakpoint testBreakpoint, TestBreakPointArgs args)
        {
            OnTestBreakpointDeleted?.Invoke(testBreakpoint, args);
        }

        // Breakpoint entered
        public delegate void TestBreakpointEnterHandler(TestBreakpoint testBreakpoint, TestBreakPointArgs args);
        public static event TestBreakpointEnterHandler OnTestBreakpointEnter;

        private static void FireTestBreakpointEnterEvent(TestBreakpoint testBreakpoint, TestBreakPointArgs args)
        {
            OnTestBreakpointEnter?.Invoke(testBreakpoint, args);
        }

        // Breakpoint exited
        public delegate void TestBreakpointExitHandler(TestBreakpoint testBreakpoint, TestBreakPointArgs args);
        public static event TestBreakpointExitHandler OnTestBreakpointExit;

        private static void FireTestBreakpointExitEvent(TestBreakpoint testBreakpoint, TestBreakPointArgs args)
        {
            OnTestBreakpointExit?.Invoke(testBreakpoint, args);
        }

        #endregion

        #region public methods

        /// <summary>
        /// Inserts a breakpoint for the test script object.
        /// </summary>
        /// <param name="testScriptObject">Test script object</param>
        public static TestBreakpoint InsertBreakpoint(TestScriptObject testScriptObject)
        {
            _breakPoints = _breakPoints ?? new List<TestBreakpoint>();

            if (_breakPoints is null)
            {
                _breakPoints = new List<TestBreakpoint>();
            }

            var breakpoint = new TestBreakpoint(testScriptObject);
            _breakPoints.Add(breakpoint);

            FireTestBreakpointInsertedEvent(breakpoint, new TestBreakPointArgs());

            return breakpoint;
        }

        /// <summary>
        /// Deletes an existing breakpoint from the test script object.
        /// </summary>
        /// <param name="testScriptObject">Test script object</param>
        public static void DeleteBreakpoint(TestScriptObject testScriptObject)
        {
            var breakpoint = _breakPoints.Find(x => x.TestScriptObjectID.Equals(testScriptObject.SystemID));

            if (null != breakpoint)
            {
                DeleteBreakpoint(breakpoint);
            }
        }

        /// <summary>
        /// Clears list of breakpoints
        /// </summary>
        /// <param name="breakpoints">List of breakpoints to remove.</param>
        public static void DeleteBreakpoints(List<TestBreakpoint> breakpoints)
        {
            foreach (var breakpoint in breakpoints)
            {
                DeleteBreakpoint(breakpoint);
            }
        }

        public static void DeleteBreakpoint(TestBreakpoint breakpoint)
        {
            if (breakpoint != null)
            {
                _breakPoints.Remove(breakpoint);
                FireTestBreakpointDeletedEvent(breakpoint, new TestBreakPointArgs());
            }
        }

        public static void ChangeBreakpointState(TestBreakpoint breakpoint, TestBreakpoint.State newState)
        {
            breakpoint.CurrentState = newState;
        }

        public static void ChangeBreakpointStates(List<TestBreakpoint> breakpoints, TestBreakpoint.State newState)
        {
            foreach (var breakpoint in breakpoints)
            {
                breakpoint.CurrentState = newState;
            }
        }

        /// <summary>
        /// Checks if the object has a breakpoint set.
        /// </summary>
        /// <param name="testScriptObject"></param>
        /// <returns>true if set, otherwise false.</returns>
        public static bool HasBreakpoint(TestScriptObject testScriptObject)
        {
            _breakPoints = _breakPoints ?? new List<TestBreakpoint>();

            return _breakPoints is null ? false : _breakPoints.Find(x => x.TestScriptObjectID.Equals(testScriptObject.SystemID)) is null ? false : true;
        }

        /// <summary>
        /// Checks if the object has an enabled breakpoint set.
        /// </summary>
        /// <param name="testScriptObject"></param>
        /// <returns>true if set, otherwise false.</returns>
        public static bool HasEnableBreakpoint(TestScriptObject testScriptObject)
        {
            _breakPoints = _breakPoints ?? new List<TestBreakpoint>();

            return _breakPoints is null ? false :
                _breakPoints.Find(x => (x.TestScriptObjectID.Equals(testScriptObject.SystemID) &&
                    x.CurrentState == TestBreakpoint.State.Enabled)) is null ? false : true;
        }

        public static TestBreakpoint GetBreakpoint(TestScriptObject testScriptObject)
        {
            return GetBreakpoint(testScriptObject.SystemID);
        }

        /// <summary>
        /// Gets the TestBreakpoint object associated with the TestScriptObject. 
        /// </summary>
        /// <param name="testScriptObjectId">The TestScriptObject requested.</param>
        /// <returns>The associated TestScriptObject or null if no associated breakpoint</returns>
        public static TestBreakpoint GetBreakpoint(Guid testScriptObjectId)
        {
            _breakPoints = _breakPoints ?? new List<TestBreakpoint>();

            return _breakPoints.Find(x => x.TestScriptObjectID.Equals(testScriptObjectId));
        }

        /// <summary>
        /// Enters execution break at test script object.
        /// </summary>
        /// <param name="testScriptObject"></param>
        public static void EnterBreakpoint(TestScriptObject testScriptObject)
        {
            EnterBreakpoint(testScriptObject.SystemID);
        }

        public static void EnterBreakpoint(Guid testScriptObjectId)
        {
            _breakPoints = _breakPoints ?? new List<TestBreakpoint>();

            var testBreakpoint = _breakPoints.Find(x => x.TestScriptObjectID.Equals(testScriptObjectId));

            if (testBreakpoint != null)
            {
                _currentBreakpoint = testBreakpoint;
                FireTestBreakpointEnterEvent(testBreakpoint, new TestBreakPointArgs());
                ResetEvent.WaitOne();
            }
        }

        /// <summary>
        /// Places thread into wait state.
        /// </summary>
        public static void EnterStepOverMode()
        {
            ResetEvent.WaitOne();
        }

        /// <summary>
        /// Allow thread to proceed while in step over mode.
        /// </summary>
        public static void StepOverExecution()
        {
            StepOverMode = true;
            ResetEvent.Set();
            ResetEvent.Reset();
        }

        /// <summary>
        /// Continues script execution from current break location.
        /// </summary>
        public static void ContinueExecution()
        {
            StepOverMode = false;
            ResetEvent.Set();
            ResetEvent.Reset();
            FireTestBreakpointExitEvent(_currentBreakpoint, new TestBreakPointArgs());
        }

        #endregion
    }
}
