﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Quintity.TestFramework.Core
{
    public static class TestProperties
    {
        #region Class data members

        static private TestPropertyCollection _testPropertyCollection;
        public static TestPropertyCollection TestPropertyCollection
        { get { return _testPropertyCollection; } }

        static private string _testPropertiesFile;
        public static string TestPropertiesFile
        {
            get { return _testPropertiesFile; }
            set
            {
                _testPropertiesFile = value;

                FireTestPropertiesFileChangedEvent(_testPropertiesFile);
            }
        }

        #endregion

        #region Class events

        // Breakpoint state changed
        public delegate void TestPropertiesInitializedHandler();
        public static event TestPropertiesInitializedHandler OnTestPropertiesInitialized;

        private static void FireTestPropertiesInitializedEvent()
        {
            OnTestPropertiesInitialized?.Invoke();
        }

        public delegate void TestPropertiesFileChangedHandler(string newFileName);
        public static event TestPropertiesFileChangedHandler OnTestPropertiesFileChangedHandler;

        private static void FireTestPropertiesFileChangedEvent(string newFileName)
        {
            OnTestPropertiesFileChangedHandler?.Invoke(newFileName);
        }


        #endregion

        #region Class properties

        public static string TestHome
        { get { return ExpandString(GetPropertyValueAsString("TestHome")); } }

        public static string TestAssemblies
        { get { return ExpandString(GetPropertyValueAsString("TestAssemblies")); } }

        public static string TestSuites
        { get { return ExpandString(GetPropertyValueAsString("TestSuites")); } }

        public static string TestData
        { get { return ExpandString(GetPropertyValueAsString("TestData")); } }

        public static string TestOutput
        { get { return ExpandString(GetPropertyValueAsString("TestOutput")); } }

        public static string TestConfigs
        { get { return ExpandString(GetPropertyValueAsString("TestConfigs")); } }

        public static string TestResults
        { get { return ExpandString(GetPropertyValueAsString("TestResults")); } }

        public static string TestGolds
        { get { return ExpandString(GetPropertyValueAsString("TestGolds")); } }

        public static TestSuite CurrentTestSuite
        { get { return GetPropertyValue("CurrentTestSuite") as TestSuite; } }

        public static TestCase CurrentTestCase
        { get { return GetPropertyValue("CurrentTestCase") as TestCase; } }

        public static TestStep CurrentTestStep
        { get { return GetPropertyValue("CurrentTestStep") as TestStep; } }

        public static string TestRunId
        { get { return ExpandString(GetPropertyValueAsString("TestRunId")); } }

        //public static int VirtualUsers
        //{ get { return GetPropertyValueAsInt("VirtualUsers"); } }

        //public static TimeSpan LaunchDuration
        //{ get { return GetPropertyValueAsTimeSpan("LaunchDuration"); } }

        #endregion

        #region Class public methods

        public static void Initialize()
        {
            _testPropertyCollection = new TestPropertyCollection();

            addSystemProperties();

            TestPropertiesFile = null;

            FireTestPropertiesInitializedEvent();
        }

        public static void Initialize(string filePath)
        {
            Initialize(filePath, new List<Type>());
        }

        public static void Initialize(string filePath, List<Type> knownTypes)
        {
            TestAssert.IsFalse(string.IsNullOrEmpty(filePath), "The file path cannot be a null or empty value.");

            Initialize();

            TestPropertiesFile = filePath;

            if (knownTypes == null)
            {
                knownTypes = new List<Type>();
            }

            var tempCollection = deserializeFromFile(filePath, knownTypes);

            foreach (TestProperty property in tempCollection)
            {
                if (property is TestSystemProperty)
                {
                    SetPropertyValue(property.Name, property.Value);
                }
                else
                {
                    AddProperty(property);
                }
            }

            FireTestPropertiesInitializedEvent();
        }

        public static List<TestPropertyOverride> GetTestProperityOverrides(string targetEnvironments)
        {
            var environments = targetEnvironments.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            //Hashtable testPropertyOverrides = new Hashtable();
            var testPropertyOverrides = new List<TestPropertyOverride>();

            foreach (string environment in environments)
            {
                string section = $@"TestEnvironments/{environment.Trim()}/testPropertyOverrides";

                var newTestPropertyOverrides = (Hashtable)ConfigurationManager.GetSection(section);

                if (null != newTestPropertyOverrides)
                {
                    //testPropertyOverrides = AddToTestPropertyOverrides(environment, newTestPropertyOverrides, testPropertyOverrides);
                    AddToTestPropertyOverrides(environment, newTestPropertyOverrides, testPropertyOverrides);
                }
            }

            return testPropertyOverrides;
        }

        public static List<TestPropertyOverride> AddToTestPropertyOverrides(
            string environment, Hashtable newTestPropertyOverrides, List<TestPropertyOverride> testPropertyOverrides)
        {
            foreach (DictionaryEntry newTestPropertyOverride in newTestPropertyOverrides)
            {
                var valueElements = parseOverrideValue2(newTestPropertyOverride);

                var testPropertyOverride = new TestPropertyOverride()
                {
                    Environment = environment,
                    Name = newTestPropertyOverride.Key as string
                };

                if (valueElements is Array)
                {
                    testPropertyOverride.Value = valueElements[0];
                    testPropertyOverride.Description = valueElements.Length > 1 ? valueElements[1] : null;
                }

                testPropertyOverrides.Add(testPropertyOverride);
            }

            return testPropertyOverrides;
        }

        public static List<TestPropertyOverride> ApplyTestPropertyOverrides(List<TestPropertyOverride> testPropertyOverrides)
        {
            List<TestPropertyOverride> unusedOverrides = new List<TestPropertyOverride>();

            if (!(testPropertyOverrides is null))
            {
                foreach (var testPropertyOverride in testPropertyOverrides)
                {
                    var property = TestProperties.GetProperty(testPropertyOverride.Name);

                    if (property != null)
                    {
                        // Preserve original value and description
                        property.OverriddenValue = property.Value;
                        property.OverriddenDescription = property.Description;
                        property.Overridden = true;

                        // Capture test property override
                        property.TestPropertyOverride = testPropertyOverride;

                        // Set property members to overridden values.
                        property.Value = testPropertyOverride.Value;
                        property.Description = testPropertyOverride.Description;
                    }
                    else
                    {
                        unusedOverrides.Add(testPropertyOverride);
                    }
                }
            }

            return unusedOverrides;
        }

        public static void ReapplyTestPropertyOverrides()
        {
            var overridden = _testPropertyCollection.FindAll(x => x.TestPropertyOverride != null);

            if (!(overridden is null))
            {
                foreach (var testProperty in overridden)
                {
                    testProperty.Overridden = true;
                    testProperty.Value = testProperty.TestPropertyOverride.Value;
                    testProperty.Description = testProperty.TestPropertyOverride.Description;
                }
            }
        }

        public static void RemoveTestPropertyOverrides()
        {
            var overridden = _testPropertyCollection.FindAll(x => x.Overridden == true);

            if (!(overridden is null))
            {
                foreach(var testProperty in overridden)
                {
                    testProperty.Overridden = false;
                    testProperty.Value = testProperty.OverriddenValue;
                    testProperty.Description = testProperty.OverriddenDescription;
                }
            }
        }

        private static dynamic parseOverrideValue2(DictionaryEntry testPropertyOverride)
        {
            // In case value is not string, some other object.
            dynamic valueElements = testPropertyOverride.Value;

            // If it is a string, parse out elements.
            if (testPropertyOverride.Value is string)
            {
                // Cast to string and parse on description delimiter.
                valueElements = ((string)testPropertyOverride.Value).Split(new char[] { '|' });

                // If second string element, new override description
                var newDescription = valueElements.Length > 1 ? valueElements[1] : string.Empty;
            }

            return valueElements;
        }

        /// <summary>
        /// Parses out and updates passed test property with new value object and possible description from test override entry.
        /// An override value, if a string, can contain a new test override description (delimited by '|').
        /// </summary>
        /// <param name="testProperty"></param>
        /// <param name="testPropertyOverride"></param>
        private static void parseOverrideValue(TestProperty testProperty, DictionaryEntry testPropertyOverride)
        {
            // In case value is not string, some other object.
            dynamic valueElements = testPropertyOverride.Value;

            // If it is a string, parse out elements.
            if (testPropertyOverride.Value is string)
            {
                // Cast to string and parse on description delimiter.
                valueElements = ((string)testPropertyOverride.Value).Split(new char[] { '|' });
                // First element is always the string value.
                testProperty.Value = valueElements[0];

                // If second string element, new override description
                var newDescription = valueElements.Length > 1 ? valueElements[1] : string.Empty;
                testProperty.Description = $"{newDescription}";
            }
        }

        public static void AddProperty(TestProperty testProperty)
        {
            TestUtils.IsNotNull(testProperty, new ArgumentNullException("TestProperty", "The TestProperty argument cannont be a null value."));
            TestUtils.IsFalse(string.IsNullOrEmpty(testProperty.Name), new TestPropertyNameException("The test property name cannot be a null or empty string."));

            _testPropertyCollection.Add(testProperty);
        }

        public static bool RemoveProperty(string name)
        {
            bool success = false;

            TestProperty testProperty = GetProperty(name);

            if (null != testProperty)
            {
                if (isTestSystemProperty(testProperty))
                {
                    throw new TestSystemPropertyChangeException(
                        string.Format("\"{0}\" is a system property and cannot be removed.", name));
                }

                success = _testPropertyCollection.Remove(testProperty);
            }

            return success;
        }

        public static void SetPropertyValue(string name, object value)
        {
            TestProperty testProperty = GetProperty(name);

            if (null != testProperty)
            {
                testProperty.Value = value;
            }
            else
            {
                AddProperty(new TestProperty(name, value));
            }

            string message;

            if (!TestUtils.IsSerializable(value, out message))
            {
                TestWarning.Fail("The test property for key \"{0}\" is not serializable.\r\n{1}", name, message);
            }
        }

        public static string GetPropertyValueAsString(string name, bool expand = false)
        {
            return GetPropertyValueAsString(name, expand, null);
        }

        public static string GetPropertyValueAsString(string name, bool expand, string @default)
        {
            var @value = GetPropertyValue(name, @default) as string;

            return expand ? ExpandString(@value) : @value;
        }

        public static TimeSpan GetPropertyValueAsTimeSpan(string name)
        {
            return GetPropertyValueAsTimeSpan(name, new TimeSpan());
        }

        public static TimeSpan GetPropertyValueAsTimeSpan(string name, TimeSpan @default)
        {
            var @value = GetPropertyValue(name, @default);

            if (@value is int)
            {
                return (TimeSpan)@value;
            }
            else
            {
                return @default;
            }
        }

        public static int GetPropertyValueAsInt(string name)
        {
            return GetPropertyValueAsInt(name, 0);
        }

        public static int GetPropertyValueAsInt(string name, int @default)
        {
            var @value = GetPropertyValue(name, @default);

            if (@value is int)
            {
                return (int)@value;
            }
            else
            {
                return @default;
            }
        }

        public static long GetPropertyValueAsLong(string name)
        {
            return GetPropertyValueAsInt(name, 0);
        }

        public static long GetPropertyValueAsLong(string name, int @default)
        {
            var @value = GetPropertyValue(name, @default);

            if (@value is long)
            {
                return (long)@value;
            }
            else
            {
                return @default;
            }
        }

        public static bool GetPropertyValueAsBool(string name, bool @default)
        {
            var @value = GetPropertyValue(name, @default);

            if (@value is string)
            {
                bool result;

                return bool.TryParse((string)@value, out result) == true ? result : @default;
            }
            else
            {
                return @default;
            }
        }

        public static object GetPropertyValue(string name)
        {
            return GetPropertyValue(name, "86529bef-0704-4f55-9df4-45e299e57f9e");
        }

        public static object GetPropertyValue(string name, object @default)
        {
            object value = null;

            TestProperty testProperty = GetProperty(name);

            if (null != testProperty)
            {
                value = testProperty.Value;
            }
            else
            {
                if (!@default.Equals("86529bef-0704-4f55-9df4-45e299e57f9e"))
                {
                    value = @default;
                }
                else
                {
                    throw new TestPropertyNotFoundException(string.Format("A test property by the name \"{0}\" was not found.", name));
                }
            }

            return value;
        }

        public static TestProperty GetProperty(string name)
        {
            TestUtils.IsNotNull(_testPropertyCollection,
                new TestPropertiesNotInitializedException("The TestProperties collection must be initialized before use."));
            TestUtils.IsFalse(string.IsNullOrEmpty(name), new TestPropertyNameException("The test property name cannot be a null or empty string."));

            TestProperty testProperty;

            switch (name.ToUpper())
            {
                case "NULL":
                    testProperty = new TestSystemProperty("NULL", "Null value", null, true);
                    break;
                case "EMPTYSTRING":
                    testProperty = new TestSystemProperty("EMPTYSTRING", "Empty string value", string.Empty, true);
                    break;
                case "SPACE":
                    testProperty = new TestSystemProperty("SPACE", "Space character value", " ", true);
                    break;
                case "NOW":
                    testProperty = new TestSystemProperty("NOW", "Current date time value", DateTime.Now, true);
                    break;
                case "TODAY":
                    testProperty = new TestSystemProperty("TODAY", "Current date value", DateTime.Today, true);
                    break;
                default:
                    _testPropertyCollection.TryGetValue(name, out testProperty);
                    break;
            }

            return testProperty;
        }

        /// <summary>
        /// Searches the target string replacing substrings with matching test property name.
        /// The replacement sequence is left to right for possible matches
        /// </summary>
        /// <param name="target">String to search</param>
        /// <param name="propertyNames">Possible replacement property call outs.</param>
        /// <returns>Fixed up string with macros or original if no matches</returns>
        public static string FixupString(string target, params string[] propertyNames)
        {
            string fixedup = target;

            if (!string.IsNullOrEmpty(fixedup))
            {
                foreach (string propertyName in propertyNames)
                {
                    fixedup = FixupString(fixedup, propertyName);
                }
            }

            return fixedup;
        }

        /// <summary>
        /// Searches a string for each ocurrence of a string and replaces with named property.
        /// </summary>
        /// <param name="target">Target string to fixup.</param>
        /// <param name="searchValue">String to search for and replace if found.</param>
        /// <param name="propertyName">Name of replacement property (without brackets).</param>
        /// <returns></returns>
        public static string FixupString(string target, string propertyName)
        {
            string fixedup = target;  // Preserve original string.

            // Get fixup property
            string propertyValue = Core.TestProperties.GetPropertyValueAsString(propertyName);

            int index = -1;

            // Iterate through string replacing with property 
            if (target != null && propertyValue != null)
            {
                propertyValue = TestProperties.ExpandString(propertyValue);

                do
                {
                    index = fixedup.IndexOf(propertyValue, StringComparison.OrdinalIgnoreCase);

                    if (-1 != index)
                    {
                        target = target.Remove(index, propertyValue.Length);
                        fixedup = target.Insert(index, string.Format("[{0}]", propertyName));
                    }

                } while (-1 != index);
            }

            return fixedup;
        }

        public static void Save()
        {
            Save(TestPropertiesFile, null);
        }

        public static void Save(string filePath)
        {
            Save(filePath, null);
        }

        public static void Save(string filePath, List<Type> knownTypes)
        {
            TestAssert.IsNotNull(_testPropertyCollection, "The TestProperties collection must be initialized before use.");

            // Want to exclude system properties that are read only from serialization.
            TestPropertyCollection tempCollection = new TestPropertyCollection();

            foreach (TestProperty property in _testPropertyCollection)
            {
                tempCollection.Add(property);
            }

            if (knownTypes == null)
            {
                knownTypes = new List<Type>();
                knownTypes.Add(typeof(TestProperty));
                knownTypes.Add(typeof(TestSystemProperty));
            }

            serializeToFile(tempCollection, knownTypes, filePath);
        }

        public static bool HasTestPropertyOverrides()
        {
            var overrides = _testPropertyCollection.FindAll(x => x.TestPropertyOverride != null);
            return overrides != null && overrides.Count > 0 ? true : false;
        }

        public static string ExpandString(string source)
        {
            return ExpandString(source, false);
        }

        /// <summary>
        /// Iteratively processes a string all property callouts with actual property values.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ExpandString(string source, bool ignoreEscaped)
        {
            TestAssert.IsNotNull(_testPropertyCollection, "The TestProperties collection must be initialized before use.");
            TestAssert.IsFalse(source is null, "The target string key cannot be a null or empty value.");

            string leftEscapeToken = "&@#!~?^";
            string rightEscapeToken = "!@*&%+";

            string target = source;
            bool macroExpanded = false;

            TestProperty testProperty;

            if (source.ToUpper() == "[NULL]")
            {
                target = null;
            }
            else if (source.ToUpper() == "[EMPTYSTRING]")
            {
                target = "";
            }
            else
            {
                string macro = StripMacro(source, ignoreEscaped);

                if (null != macro)
                {
                    if (_testPropertyCollection.TryGetValue(macro, out testProperty))
                    {
                        if (testProperty.Value is string)
                        {
                            target = target.Replace(string.Format("[{0}]", macro), (string)testProperty.Value);
                            macroExpanded = true;
                        }
                    }
                }

                if (macroExpanded)
                {
                    target = ExpandString(target, ignoreEscaped);
                }

                target = target.Replace(leftEscapeToken, "[");
                target = target.Replace(rightEscapeToken, "]");
            }

            return target;
        }

        new public static string ToString()
        {
            return _testPropertyCollection.ToString();
        }

        public static string StripMacro(string source, bool ignoreEscape = false)
        {
            string macro = null;

            string leftEscapeToken = "&@#!~?^";
            string rightEscapeToken = "!@*&%+";

            string target = source;
            int startPos = 0;
            int endPos = 0;

            // Tokenize escaped brackets.
            if (!ignoreEscape)
            {
                target = target.Replace(@"\[", leftEscapeToken);
                target = target.Replace(@"\]", rightEscapeToken);
            }

            startPos = target.IndexOf('[', startPos);

            if (-1 != startPos)
            {
                endPos = target.IndexOf(']', startPos++);

                if (-1 != endPos)
                {
                    macro = target.Substring(startPos, endPos - startPos);
                }
            }

            return macro;
        }

        #endregion

        #region Class internal methods

        internal static bool isSerializable(object value, out string message, List<Type> knownTypes = null)
        {
            bool serializable = false;

            try
            {
                System.Runtime.Serialization.DataContractSerializer serializer =
                    new System.Runtime.Serialization.DataContractSerializer(value.GetType(), knownTypes);

                // Create a memory stream and write to it.
                MemoryStream memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, value);

                // Just to verify
                memoryStream.Flush();
                memoryStream.Position = 0;

                StreamReader reader = new StreamReader(memoryStream);
                message = reader.ReadToEnd();

                serializable = true;
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return serializable;
        }

        /// <summary>
        /// Writes a test property collection to a valid XMLWriter for subsequent serialization
        /// </summary>
        /// <param name="xmlWriter"></param>
        /// <param name="testProperties"></param>
        internal static void Write(XmlWriter xmlWriter, TestPropertyCollection testProperties)
        {
            xmlWriter.WriteStartElement("TestProperties");

            foreach (var testProperty in testProperties)
            {
                xmlWriter.WriteStartElement("TestProperties");

                xmlWriter.WriteElementString("Name", testProperty.Name);
                xmlWriter.WriteElementString("Description", testProperty.Description);
                xmlWriter.WriteElementString("Value", testProperty.Value.ToString());
                xmlWriter.WriteElementString("Active", testProperty.Active.ToString());

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
        }

        //internal static void Read(XPathNavigator nav)
        //{
        //    XPathNodeIterator iterator = nav.Select("TestProperty");

        //    string key, val;

        //    while (iterator.MoveNext())
        //    {
        //        key = iterator.Current.GetAttribute("key", "");
        //        val = iterator.Current.GetAttribute("value", "");

        //        if (key.Length != 0 && val.Length != 0)
        //        {
        //            this.m_properties.Add(key, val);
        //        }
        //    }
        //}

        public static void SetTestPropertyCollection(TestPropertyCollection testPropertyCollection)
        {
            _testPropertyCollection = testPropertyCollection;
        }

        #endregion

        #region Class private methods

        private static bool isTestSystemProperty(TestProperty testProperty)
        {
            return testProperty is TestSystemProperty;
        }

        private static void addSystemProperties()
        {
            string path = System.IO.Path.GetDirectoryName(
               System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            Uri uri = new Uri(path);

            addPropertyIfDoesNotExist("TestHome", "Default test home location", uri.LocalPath);
            addPropertyIfDoesNotExist("TestProperties", "Default test properties location", "[TESTHOME]\\TestProperties");
            addPropertyIfDoesNotExist("TestSuites", "Default test suites location", "[TESTHOME]\\TestSuites");
            addPropertyIfDoesNotExist("TestAssemblies", "Default test assemblies location", "[TESTHOME]\\TestAssemblies");
            addPropertyIfDoesNotExist("TestData", "Default test data location", "[TESTHOME]\\TestData");
            addPropertyIfDoesNotExist("TestOutput", "Default test output location.", "[TESTHOME]\\TestOutput");
            addPropertyIfDoesNotExist("TestResults", "Default test results location.", "[TESTHOME]\\TestResults");
            addPropertyIfDoesNotExist("TestGolds", "Default test golds location.", "[TESTHOME]\\TestGolds");
            addPropertyIfDoesNotExist(
                "TestConfigs", "Default test configuration files (e.g., test listener configs, test profile configs, etc.)", "[TESTHOME]\\TestConfigs");
            //addPropertyIfDoesNotExist(
            //   "VirtualUsers", "The number of performance virtual user instances.", 1);
            //addPropertyIfDoesNotExist(
            //   "LaunchDuration", "The time duration in which to launch the specified number of virtual users.", new TimeSpan(0, 0, 0, 0, 0));
        }

        private static void addPropertyIfDoesNotExist(string name, string description, object value)
        {
            TestProperty testProperty;

            if (!_testPropertyCollection.TryGetValue(name, out testProperty))
            {
                _testPropertyCollection.Add(new TestSystemProperty(name, description, value, true));
            }
        }

        private static void serializeToFile(TestPropertyCollection testProperties, List<Type> knownTypes, string filePath)
        {
            try
            {
                //TODO Remove?
                //knownTypes.Add(typeof(TestListenerCollection));

                DataContractSerializer serializer = new DataContractSerializer(typeof(TestPropertyCollection), knownTypes);

                var settings = new XmlWriterSettings() { Indent = true };

                using (var writer = XmlWriter.Create(filePath, settings))
                {
                    serializer.WriteObject(writer, testProperties);
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }

        private static TestPropertyCollection deserializeFromFile(string filePath, List<Type> knownTypes)
        {
            FileStream reader = null;
            TestPropertyCollection testProperties = null;

            try
            {
                //TODO:  05/25/2018, remove?
                //knownTypes.Add(typeof(TestListenerCollection));
                knownTypes.Add(typeof(TestProperty));
                knownTypes.Add(typeof(TestSystemProperty));

                // Create DataContractSerializer.
                DataContractSerializer serializer =
                    new System.Runtime.Serialization.DataContractSerializer(typeof(TestPropertyCollection), knownTypes);

                // Create a file stream to read into.
                reader = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                // Read into object.
                testProperties = serializer.ReadObject(reader) as TestPropertyCollection;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    // Close file.
                    reader.Close();
                }
            }

            return testProperties;
        }

        #endregion
    }
}
