/*
 * Copyright 2002 - 2007 Quintity, LLC.  All Rights Reserved.
 * Use is subject to license terms.
 * 
*/
using System.Windows.Forms;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestEngineer
{
    public partial class TestPropertiesEditorDialog2 : Form
    {
        public TestPropertiesEditorDialog2()
        {
            InitializeComponent();

            loadTestProperties();
        }

        private void loadTestProperties()
        {
            foreach (TestProperty testProperty in TestProperties.TestPropertyCollection)
            {
                var node = new TreeNode(testProperty.Name);
                node.Tag = testProperty;
                _testPropertiesTreeView.Nodes[0].Nodes.Add(node);
            }
        }

        private void _testPropertiesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (null != e.Node.Tag)
            {
                _testPropertyPropertyGrid.SelectedObject = e.Node.Tag;
            }
        }
    }
}
