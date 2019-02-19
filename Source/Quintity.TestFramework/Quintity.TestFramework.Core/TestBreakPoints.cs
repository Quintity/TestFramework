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

    public static class TestBreakPoints
    {
        private static ManualResetEvent ResetEvent = new ManualResetEvent(false);
        private static List<TestScriptObject> _breakPoints;
        private static TestScriptObject _currentTestScriptObject = null;

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

        public static void InsertBreakPoint(TestScriptObject testScriptObject)
        {
            _breakPoints = _breakPoints ?? new List<TestScriptObject>();

            if (_breakPoints is null)
            {
                _breakPoints = new List<TestScriptObject>();
            }

            _breakPoints.Add(testScriptObject);
        }

        public static void ClearBreakPoint(TestScriptObject testScriptObject)
        {
            _breakPoints.Remove(testScriptObject);
        }

        public static bool HasBreakPoint(TestScriptObject testScriptObject)
        {
            return _breakPoints is null ? false : _breakPoints.Find(x => x.SystemID.Equals(testScriptObject.SystemID)) is null ? false : true;
        }

        public static void EnterBreakPoint(TestScriptObject testScriptObject)
        {
            _currentTestScriptObject = testScriptObject;
            FireTestBreakPointEnterEvent(_currentTestScriptObject, new TestBreakPointArgs());
            ResetEvent.WaitOne();
        }

        public static void ContinueExecution()
        {
            ResetEvent.Set();
            ResetEvent.Reset();
            FireTestBreakPointExitEvent(_currentTestScriptObject, new TestBreakPointArgs());
        }
    }
}
