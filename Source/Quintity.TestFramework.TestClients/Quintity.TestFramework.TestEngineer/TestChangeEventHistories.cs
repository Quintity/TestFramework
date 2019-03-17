using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Quintity.TestFramework.TestEngineer
{
    public class TestChangeEventHistories
    {
        #region Event declarations

        public delegate void TestHistoryUndoEvent(TestChangeEvent e);
        public event TestHistoryUndoEvent OnTestHistoryUndoEvent;

        public delegate void TestHistoryRedoEvent(TestChangeEvent e);
        public event TestHistoryRedoEvent OnTestHistoryRedoEvent;

        #endregion

        #region private members

        private Stack<TestChangeEvent> _undoStack;
        private Stack<TestChangeEvent> _redoStack;

        #endregion

        #region constructors

        public TestChangeEventHistories()
        {
            _undoStack = new Stack<TestChangeEvent>();
            _redoStack = new Stack<TestChangeEvent>();
        }

        #endregion

        #region public methods

        public bool CanUndo()
        {
            return _undoStack.Count > 0 ? true : false;
        }

        public bool CanRedo()
        {
            return _redoStack.Count > 0 ? true : false;
        }

        public void RecordChangeEvent(TestChangeEvent changeEvent)
        {
            Debug.WriteLine("Event recorded:  " + changeEvent.ToString());

            _undoStack.Push(changeEvent);
        }

        public void Undo()
        {
            TestChangeEvent changeEvent = null;

            if (_undoStack.Count > 0)
            {
                changeEvent = _undoStack.Pop();

                Debug.WriteLine("Undo event popped:  " + changeEvent.ToString());

                fireUndoEvent(changeEvent);

                _redoStack.Push(changeEvent);
            }
        }

        public void Redo()
        {
             if (_redoStack.Count > 0)
             {
                 TestChangeEvent changeEvent = _redoStack.Pop();

                 fireRedoEvent(changeEvent);

                 _undoStack.Push(changeEvent);
             }
        }

        // Clears the undo/redo stack.
        public void Reset()
        {
            _redoStack.Clear();
            _undoStack.Clear();
        }

        internal TestChangeEvent PopFromUndoStack()
        {
            return _undoStack.Pop();
        }

        internal TestChangeEvent PopFromRedoStack()
        {
            return _redoStack.Pop();
        }

        #endregion

        /// <summary>
        /// Creates a createTestHistoryUndoEvent for undo entity.
        /// </summary>
        /// <param name="testObject">Entity from undo stack.</param>
        private void fireUndoEvent(TestChangeEvent entity)
        {
            // If client event handler defined...
            OnTestHistoryUndoEvent?.Invoke(entity);
        }

        /// <summary>
        /// Creates a createTestHistoryRedoEvent for undo entity.
        /// </summary>
        /// <param name="testObject">Entity from undo stack.</param>
        private void fireRedoEvent(TestChangeEvent entity)
        {
            // If client event handler defined...
            OnTestHistoryRedoEvent?.Invoke(entity);
        }
    }
}
