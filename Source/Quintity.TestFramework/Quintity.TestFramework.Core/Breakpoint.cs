using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Core
{
    public class Breakpoint
    {
        public enum State
        {
            Enabled,
            Disabled
        }

        public State CurrentState { get; set; }
        public Guid TestScriptObjectID { get; set; }

        #region Constructors

        public Breakpoint()
        { }

        public Breakpoint(TestScriptObject testScriptObject)
        {
            TestScriptObjectID = testScriptObject.SystemID;
            CurrentState = State.Enabled;
        }

        #endregion
    }
}
