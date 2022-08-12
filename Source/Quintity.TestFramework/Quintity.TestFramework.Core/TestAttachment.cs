using System;
using System.Threading;

namespace Quintity.TestFramework.Core
{
    public class TestAttachment
    {
        #region Event and delegate definitions

        // Attach TestAttachment 

        public delegate void TestAttachmentAttachEventHandler(string virtualUser, TestAttachment testAttachment);
        public static event TestAttachmentAttachEventHandler OnTestAttachmentAttach;
        internal static void FireTestAttachmentAttachEvent(TestAttachment testAttachment) => 
            OnTestAttachmentAttach?.Invoke(Thread.CurrentThread.Name, testAttachment);

        // Detach TestAttachment
        public delegate void TestAttachmentDetachEventHandler(string virtualUser, string key);
        public static event TestAttachmentDetachEventHandler OnTestAttachmentDetach;
        internal static void FireTestAttachmentDetachEvent(string key) => 
            OnTestAttachmentDetach?.Invoke(Thread.CurrentThread.Name, key);

        #endregion

        #region Properties

        public string Key { get; private set; }
        public Uri Value { get; private set; }

        #endregion

        #region Constructors
        public TestAttachment(string key, Uri value)
        {
            Key = key;
            Value = value;
        }

        #endregion

        public static void Attach(string key, Uri value)
        {
            var testAttachment = new TestAttachment(key, value);

            FireTestAttachmentAttachEvent(testAttachment);
        }

        public static void Detach(string key)
        {
            FireTestAttachmentDetachEvent(key);
        }

        public override string ToString()
        {
            return $"Key:  {Key}, Value: {Value.ToString()}";
        }
    }
}
