using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestEngineer
{
    internal class TestPropertiesEditor : UITypeEditor
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
            IWindowsFormsEditorService wfservice = provider.GetService(typeof(IWindowsFormsEditorService))
                             as IWindowsFormsEditorService;
            
            PropertyGrid grid = provider.GetType().GetProperty("OwnerGrid").GetGetMethod().Invoke(provider, null) as PropertyGrid;            
            TestPropertyCollection testProperties = value as TestPropertyCollection;

            TestPropertiesEditorDialog dialog = new TestPropertiesEditorDialog((TestPropertyCollection)value);
            DialogResult result = wfservice.ShowDialog(dialog);

            if (result == DialogResult.OK)
            {
                testProperties = dialog.TestProperties;
            }

            return testProperties;
        }
    }
}
