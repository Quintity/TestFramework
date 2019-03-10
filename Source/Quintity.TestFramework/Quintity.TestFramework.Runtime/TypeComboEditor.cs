using System;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Runtime
{
    public class TypeComboEditor : System.Drawing.Design.UITypeEditor
    {
        private IWindowsFormsEditorService edSvc;

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            try
            {
                edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (edSvc != null)
                {
                    ListBox listbox = new ListBox();

                    listbox.SelectedIndexChanged += bx_SelectedIndexChanged;

                    var listener = context.Instance as TestListenerDescriptor;

                    if (!string.IsNullOrEmpty(listener.Assembly))
                    {
                        Assembly assembly = Assembly.LoadFrom(TestProperties.ExpandString(listener.Assembly));

                        Type[] types = assembly.GetTypes();

                        foreach (Type type in types)
                        {
                            if (TestListener.IsTestListenerType(type))
                            {
                                listbox.Items.Add(type.FullName);
                            }
                        }

                        edSvc.DropDownControl(listbox);
                    }

                    return (listbox.SelectedItem != null ? ((string)(listbox.SelectedItem)) : value);
                }
            }
            catch (Exception e)
            {
                if (e is System.Reflection.ReflectionTypeLoadException)
                {
                    var typeLoadException = e as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                }

                //TestListenersEditorDialog.DisplayErrorMessage(e.Message);
            }

            return value;
        }

        void bx_SelectedIndexChanged(object sender, EventArgs e)
        {
            edSvc.CloseDropDown();
        }
    }
}
