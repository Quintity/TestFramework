using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestProperty
    {
        #region Data members

        [DataMember(Order = 0)]
        public string Name
        { get; set; }

        [DataMember(Order = 2)]
        public object Value
        { get; set; }

        [DataMember(Order = 1)]
        public string Description
        { get; set; }

        [DataMember(Order = 3)]
        public bool Active
        { get; set; }

        /// <summary>
        /// Indicates where the value has been overridden by and environmental setting.
        /// </summary>
        [IgnoreDataMember]
        public bool Overridden
        { get; set; }

        /// <summary>
        /// Pre-override value
        /// </summary>
        [IgnoreDataMember]
        public object OverriddenValue
        { get; set; }

        /// <summary>
        /// Pre-override description
        /// </summary>
        [IgnoreDataMember]
        public string OverriddenDescription
        { get; set; }

        #endregion

        #region Constructors

        public TestProperty()
        { }

        public TestProperty(string name, object value)
            : this(name, null, value, true)
        { }

        public TestProperty(string name, string description, object value, bool active)
        {
            Name = name;
            Description = description;
            Value = value;
            Active = active;
        }

        public TestProperty(TestProperty testProperty)
        {
            TestUtils.IsNotNull(testProperty, new ArgumentNullException("TestProperty", "The TestProperty argument cannot be a null value."));

            Name = testProperty.Name;
            Description = testProperty.Description;
            Value = testProperty.Value;
            Active = testProperty.Active;
            Overridden = testProperty.Overridden;
            OverriddenValue = testProperty.OverriddenValue;
            OverriddenDescription = testProperty.OverriddenDescription;
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            string format = "Name:{0}\r\n Description:  {1}\r\n Value:  {2}\r\n Active:  {3}";
            return string.Format(format, Name, Description, Value, Active);
        }

        #endregion
    }
}
