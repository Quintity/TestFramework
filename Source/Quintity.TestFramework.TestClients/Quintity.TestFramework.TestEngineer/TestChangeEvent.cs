using System;
using System.Collections.Generic;
using System.Text;
using Quintity.TestFramework.Core;

namespace Quintity.TestFramework.TestEngineer
{
    public class TestChangeEvent
    {
        private TestTreeNode _changeObject;
        public dynamic ChangeObject => _changeObject;

        private ChangeType _changeType;
        public ChangeType ChangeType => _changeType;

        private dynamic[] _changeValues;
        public dynamic[] ChangeValues => _changeValues;

        public TestChangeEvent(ChangeType changeType, dynamic changeObject, params dynamic[] changeValues)
        {
            _changeType = changeType;
            _changeObject = changeObject;
            _changeValues = changeValues;
        }

        public override string ToString()
        {
            return $"{_changeType}{_changeObject}";
        }

    }
}
