using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestEngineer
{
    public class TestTreeChangedEventArgs
    {
        #region Data members

        public TestTreeNode TestTreeNode
        { get; set; }

        public ChangeType NodeAction
        { get; set; }

        #endregion

        #region Constructors

        public TestTreeChangedEventArgs(TestTreeNode testTreeNode, ChangeType nodeAction)
        {
            TestTreeNode = testTreeNode;
            NodeAction = nodeAction;
        }

        #endregion
    }
}
