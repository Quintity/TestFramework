using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Quintity.TestFramework.Core
{

    [CollectionDataContract
     (Name = "TestAttachments", ItemName = "TestName", KeyName = "DataKey", ValueName = "DataValue")]
    public class TestAttachmentCollection : Dictionary<string, object>
    {
        #region Constructors

        internal TestAttachmentCollection()
            : base()
        { }

        internal TestAttachmentCollection(TestAttachmentCollection original)
            : base(original)
        { }

        #endregion

        public void Attach(string key, object value)
        {
            TestWarning.IsTrue(IsSerializable(value), 
                string.Format("The test data object associated with key \"{0}\", is not serializable.", key));

            base.Add(key, value);
        }

        public bool Detach(string key)
        {
            return base.Remove(key);
        }

        public bool IsSerializable(object obj)
        {
            Type t = obj.GetType();

            return Attribute.IsDefined(t, typeof(DataContractAttribute)) || t.IsSerializable;
        }

        public override string ToString()
        {
            string @value = null;

            if (this.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Test Data:");

                foreach (KeyValuePair<string, object> keyvaluepair in this)
                {
                    sb.AppendLine(string.Format("  Key:  {0}, Value: {1}", keyvaluepair.Key, keyvaluepair.Value.ToString()));
                }

                sb.AppendLine("  Count:  " + this.Count.ToString());
                @value = sb.ToString();
            }
            else
            {
                @value = "Test Data:  None";
            }

            return @value;
        }
    }
}
