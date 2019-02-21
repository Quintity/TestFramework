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
        private static List<TestScriptObject> _breakPoints;
        private static TestScriptObject _currentTestScriptObject = null;
        public static bool BreakStepMode { get; set; }

        #region Class events

        public delegate void TestBreakPointEnterHandler(TestScriptObject testScriptObject, TestBreakPointArgs args);
        public static event TestBreakPointEnterHandler OnTestBreakPointEnter;

        private static void FireTestBreakPointEnterEvent(TestScriptObject testScriptObject, TestBreakPointArgs args)
        {
            OnTestBreakPointEnter?.Invoke(testScriptObject, args);
        }

        public delegate void TestBreakPointExitHandler(TestScriptObject testScriptObject, TestBreakPointArgs args);
        public static event TestBreakPointExitHandler OnTestBreakPointExit;

        private static void FireTestBreakPointExitEvent(TestScriptObject testScriptObject, TestBreakPointArgs args)
        {
            OnTestBreakPointExit?.Invoke(testScriptObject, args);
        }

        #endregion

        #region public methods

        /// <summary>
        /// Inserts a breakpoint for the test script object.
        /// </summary>
        /// <param name="testScriptObject">Test script object</param>
        public static void InsertBreakPoint(TestScriptObject testScriptObject)
        {
            _breakPoints = _breakPoints ?? new List<TestScriptObject>();

            if (_breakPoints is null)
            {
                _breakPoints = new List<TestScriptObject>();
            }

            _breakPoints.Add(testScriptObject);
        }

        /// <summary>
        /// Deletes an existing breakpoint from the test script object.
        /// </summary>
        /// <param name="testScriptObject">Test script object</param>
        public static void DeleteBreakPoint(TestScriptObject testScriptObject)
        {
            _breakPoints.Remove(testScriptObject);
        }

        /// <summary>
        /// Removes the test script object's breakpoint.
        /// </summary>
        /// <param name="testScriptObject"></param>
        public static void ClearBreakPoint(TestScriptObject testScriptObject) => _breakPoints.Remove(testScriptObject);

        /// <summary>
        /// Clears all breakpoints.
        /// </summary>
        public static void ClearAllBreakPoints() => _breakPoints = new List<TestScriptObject>();

        /// <summary>
        /// Checks if the object has a breakpoint set.
        /// </summary>
        /// <param name="testScriptObject"></param>
        /// <returns>true if set, otherwise false.</returns>
        public static bool HasBreakPointSet(TestScriptObject testScriptObject)
        {
            return _breakPoints is null ? false : _breakPoints.Find(x => x.SystemID.Equals(testScriptObject.SystemID)) is null ? false : true;
        }

        /// <summary>
        /// Enters execution break at test script object.
        /// </summary>
        /// <param name="testScriptObject"></param>
        public static void EnterBreakPoint(TestScriptObject testScriptObject)
        {
            _currentTestScriptObject = testScriptObject;
            FireTestBreakPointEnterEvent(_currentTestScriptObject, new TestBreakPointArgs());
            ResetEvent.WaitOne();
        }

        /// <summary>
        /// Continues script execution from current break location.
        /// </summary>
        public static void ContinueExecution()
        {
            ResetEvent.Set();
            ResetEvent.Reset();
            FireTestBreakPointExitEvent(_currentTestScriptObject, new TestBreakPointArgs());
        }

        #endregion
    }
}
