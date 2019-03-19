using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Quintity.TestFramework.Core
{
    internal class TestAutomationDefinitionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Displays test parameters dialog for edit from properties window.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            PropertyGrid grid = provider.GetType().GetProperty("OwnerGrid").GetGetMethod().Invoke(provider, null) as PropertyGrid;
            TestSuite testSuite = grid.SelectedObject as TestSuite;
            TestAutomationDefinition definition = null;

            if (context.Instance is TestPreprocessor)
            {
                TestScriptObjectEditorDialog dlg = new TestScriptObjectEditorDialog(null, testSuite.TestPreprocessor);
                DialogResult result = dlg.ShowDialog();
                definition = testSuite.TestPreprocessor.TestAutomationDefinition;
            }
            else if (context.Instance is TestPostprocessor)
            {
                TestScriptObjectEditorDialog dlg = new TestScriptObjectEditorDialog(null, testSuite.TestPostprocessor);
                DialogResult result = dlg.ShowDialog();
                definition = testSuite.TestPostprocessor.TestAutomationDefinition;
            }

            return definition;
        }
    }
}
