using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace Quintity.TestFramework.Core
{
    public static class TestBreakpoints
    {
        [DataMember]
        private static List<TestBreakpoint> _breakpoints;
        private static ManualResetEvent ResetEvent = new ManualResetEvent(false);

        private static TestBreakpoint _currentBreakpoint = null;
        public static TestBreakpoint CurrentBreakpoint
        { get { return _currentBreakpoint; } }

        public static bool StepOverMode { get; set; }

        /// <summary>
        /// Specifies the file where the breakpoint collection is written to
        /// and read from.
        /// </summary>
        public static string SerializationFile { get; set; }

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
            _breakpoints = _breakpoints ?? new List<TestBreakpoint>();

            var breakpoint = new TestBreakpoint(testScriptObject);
            _breakpoints.Add(breakpoint);

            WriteToFile();

            FireTestBreakpointInsertedEvent(breakpoint, new TestBreakPointArgs());

            return breakpoint;
        }

        /// <summary>
        /// Deletes an existing breakpoint from the test script object.
        /// </summary>
        /// <param name="testScriptObject">Test script object</param>
        public static void DeleteBreakpoint(TestScriptObject testScriptObject)
        {
            var breakpoint = _breakpoints.Find(x => x.TestScriptObjectID.Equals(testScriptObject.SystemID));

            if (null != breakpoint)
            {
                DeleteBreakpoint(breakpoint);
            }
        }

        public static void DeleteBreakpoint(TestBreakpoint breakpoint)
        {
            if (breakpoint != null)
            {
                _breakpoints.Remove(breakpoint);
                FireTestBreakpointDeletedEvent(breakpoint, new TestBreakPointArgs());
                WriteToFile();
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
                _breakpoints.Remove(breakpoint);
                FireTestBreakpointDeletedEvent(breakpoint, new TestBreakPointArgs());
            }

            WriteToFile();
        }

        public static void ChangeBreakpointState(TestBreakpoint breakpoint, TestBreakpoint.State newState)
        {
            breakpoint.CurrentState = newState;
            breakpoint.Changed = DateTime.Now;
            WriteToFile();
        }

        public static void ChangeBreakpointStates(List<TestBreakpoint> breakpoints, TestBreakpoint.State newState)
        {
            foreach (var breakpoint in breakpoints)
            {
                breakpoint.CurrentState = newState;
                breakpoint.Changed = DateTime.Now;
            }

            WriteToFile();
        }

        /// <summary>
        /// Checks if the object has a breakpoint set.
        /// </summary>
        /// <param name="testScriptObject"></param>
        /// <returns>true if set, otherwise false.</returns>
        public static bool HasBreakpoint(TestScriptObject testScriptObject)
        {
            _breakpoints = _breakpoints ?? new List<TestBreakpoint>();

            return _breakpoints is null ? false : _breakpoints.Find(x => x.TestScriptObjectID.Equals(testScriptObject.SystemID)) is null ? false : true;
        }

        /// <summary>
        /// Checks if the object has an enabled breakpoint set.
        /// </summary>
        /// <param name="testScriptObject"></param>
        /// <returns>true if set, otherwise false.</returns>
        public static bool HasEnableBreakpoint(TestScriptObject testScriptObject)
        {
            _breakpoints = _breakpoints ?? new List<TestBreakpoint>();

            return _breakpoints is null ? false :
                _breakpoints.Find(x => (x.TestScriptObjectID.Equals(testScriptObject.SystemID) &&
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
            _breakpoints = _breakpoints ?? new List<TestBreakpoint>();

            return _breakpoints.Find(x => x.TestScriptObjectID.Equals(testScriptObjectId));
        }

        public static List<TestBreakpoint> GetBreakpoints()
        {
            _breakpoints = _breakpoints ?? new List<TestBreakpoint>();

            return _breakpoints;
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
            _breakpoints = _breakpoints ?? new List<TestBreakpoint>();

            var testBreakpoint = _breakpoints.Find(x => x.TestScriptObjectID.Equals(testScriptObjectId));

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

            _currentBreakpoint = null;
        }

        private static void WriteToFile()
        {
            if (!string.IsNullOrEmpty(SerializationFile))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<TestBreakpoint>));

                var settings = new XmlWriterSettings() { Indent = true };

                using (var writer = XmlWriter.Create(SerializationFile, settings))
                {
                    serializer.WriteObject(writer, _breakpoints);
                }
            }
        }

        public static List<TestBreakpoint> ReadFromFile()
        {
            FileStream reader = null;
            List<TestBreakpoint> breakpoints = new List<TestBreakpoint>();

            if (!string.IsNullOrEmpty(SerializationFile))
            {
                try
                {
                    // Create DataContractSerializer.
                    DataContractSerializer serializer =
                        new System.Runtime.Serialization.DataContractSerializer(typeof(List<TestBreakpoint>));

                    // Create a file stream to read into.
                    reader = new FileStream(SerializationFile, FileMode.Open, FileAccess.Read);

                    // Read into object.
                    _breakpoints = serializer.ReadObject(reader) as List<TestBreakpoint>;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (reader != null)
                    {
                        // Close file.
                        reader.Close();
                    }
                }
            }

            return breakpoints;
        }

        #endregion
    }
}
