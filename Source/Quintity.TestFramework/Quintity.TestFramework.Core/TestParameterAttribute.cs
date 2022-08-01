/*
 * Copyright 2002 - 2007 Quintity, LLC.  All Rights Reserved.
 * Use is subject to license terms.
 * 
*/
using System;

namespace Quintity.TestFramework.Core
{
    /// <summary>
    /// Test parameter attribute optional for test method parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class TestParameterAttribute : TestObjectAttribute
    {
        #region Data members

        private object m_defaultValue;
        private bool m_required;

        #endregion fields.

        #region Constructors.

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TestParameterAttribute()
        {
        }
        
        public TestParameterAttribute(string alias, string description = null, object defaultValue = null, bool required = false, object tag = null)
            : base(alias, description, tag)
        {
            m_defaultValue = defaultValue;
            m_required = required;
        }

        #endregion constructors.

        #region TestParameterAttribute properties.

        /// <summary>
        /// Returns class's default value;
        /// </summary>
        public object DefaultValue => m_defaultValue;
        public bool Required => m_required;


        #endregion properties.
    }
}
