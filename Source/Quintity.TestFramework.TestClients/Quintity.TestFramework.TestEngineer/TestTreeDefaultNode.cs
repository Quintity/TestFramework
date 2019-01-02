using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Quintity.TestFramework.TestEngineer
{
    internal class TestTreeDefaultNode : TreeNode
    {
        public TestTreeDefaultNode(string text)
        {
            this.NodeFont = new Font("Microsoft Sans Serif", 8, FontStyle.Italic);
            this.ImageIndex = 999;
            this.Text = text;
        }
    }
}
