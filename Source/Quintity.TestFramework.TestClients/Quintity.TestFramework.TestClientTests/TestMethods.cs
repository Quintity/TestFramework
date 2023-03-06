using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Quintity.TestFramework.Core;


namespace Quintity.TestFramework.TestClientTests
{
    [TestClass("P365 Navigation methods", "Sample navigation methods")]
    public class TestMethods : TestClassBase
    {
        #region TestMethods

        [TestMethod]
        public TestVerdict TestPropertiesFixupStringTest()
        {
            try
            {
                var unFixedString = $@"{TestProperties.TestAssemblies}\library.dll";

                var fixedString = TestProperties.FixupString(unFixedString, "TestAssemblies", "TestHome");
                //var fixedString = TestProperties.FixupString(unFixedString, "TestHome", "TestAssemblies");

                TestMessage += $"{Environment.NewLine}UnFixed string: {unFixedString}{Environment.NewLine}Fixedup string: {fixedString}";
                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception exp)
            {
                TestMessage += exp.Message;
                TestVerdict = TestVerdict.Error;
            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict SerializeTestArtificat(
            string filePath)
        {
            try
            {
                var outputFileName = $"{filePath}\\TestStep.xml";
                var testSuite = TestProperties.CurrentTestSuite;
                var testCase = TestProperties.CurrentTestCase;
                var testStep = TestProperties.CurrentTestStep;

                var serializer = new DataContractSerializer(typeof(TestStep));

                var settings = new XmlWriterSettings() { Indent = true };

                using (var writer = XmlWriter.Create(outputFileName, settings))
                {
                    serializer.WriteObject(writer, testStep);
                }

                var reader = new FileStream(outputFileName, FileMode.Open, FileAccess.Read);

                // Read into object.
                var testSuite2 = serializer.ReadObject(reader) as TestSuite;


                // Testcase result
                outputFileName = $"{filePath}\\TestCaseResult.xml";

                var testCaseResult = new TestCaseResult();

                serializer = new DataContractSerializer(typeof(TestCaseResult));

                settings = new XmlWriterSettings() { Indent = true };

                using (var writer = XmlWriter.Create(outputFileName, settings))
                {
                    serializer.WriteObject(writer, testCaseResult);
                }

                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception exp)
            {
                TestMessage += exp.Message;
                TestVerdict = TestVerdict.Error;
            }

            return TestVerdict;
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

        [TestMethod]
        public TestVerdict TestAttachmentTest()
        {
            try
            {
                var uri = new Uri(@"C:\temp");
                TestAttachment.Attach("Important", uri);

                TestMessage = uri.ToString();
                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception exp)
            {
                TestMessage += exp.Message;
                TestVerdict = TestVerdict.Error;
            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict TestCacheTest1()
        {
            try
            {
                TestCache.Stash("TestCacheTest1", 999);

                var item= TestCache.Grab<int>("TestCacheTest1");
               // TestCache.Clear();
                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception exp)
            {
                TestMessage += exp.Message;
                TestVerdict = TestVerdict.Error;
            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict TestCacheTest2()
        {
            try
            {
                var found = TestCache.TryGrabValue<int>("TestCacheTest1", out int @value);
                var item = TestCache.Grab<int>("TestCacheTest1");
                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception exp)
            {
                TestMessage += exp.Message;
                TestVerdict = TestVerdict.Error;
            }

            return TestVerdict;
        }


        [TestMethod]
        public TestVerdict TestParameterCheck(
            [TestParameter(alias: "String param", required: true)]
            string stringParam,
            [TestParameter(alias: "Integer param", required: true)]
            int intParam,
            [TestParameter(alias: "Bool param", required: true)]
            bool boolParam,
            [TestParameter(alias: "Nullable bool param", required: true)]
            bool? boolNullableParam
            )
        {
            try
            {
                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception exp)
            {
                TestMessage += exp.Message;
                TestVerdict = TestVerdict.Error;
            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict DoSomething()
        {
            try
            {
                TestCheck.IsTrue("Checking something important", false, false);

                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception exp)
            {
                TestMessage += exp.Message;
                TestVerdict = TestVerdict.Error;
            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict ChangeType()
        {
            try
            {
                var dork = TestProperties.GetPropertyValue<bool>("BooleanParameter");
                var spud = TestProperties.GetPropertyValue<int>("IntegerParameter");
                changeType<string>("This is a test");
                changeType<string>(null);
                changeType<int>("99");
                changeType<bool>(true);
                changeType<bool>(false);
                changeType<DateTime>(DateTime.Now);
                //changeType<DateTime?>(DateTime.Now);
                changeType<TimeSpan>(TimeSpan.FromSeconds(5000));
                changeType<long>(999999);
                changeType<long>(999999);
                TestVerdict = TestVerdict.Pass;
            }
            catch (Exception exp)
            {
                TestMessage += exp.Message;
                TestVerdict = TestVerdict.Error;
            }

            return TestVerdict;
        }

        private T changeType<T>(object @value)
        {
            var typeofT = typeof(T).Name;
            var spud = (T)Convert.ChangeType(value, typeof(T));
            TestTrace.Trace($"ConvertTo:  '{typeofT}', Value: '{spud}' ({spud?.GetType()})");
            return spud;
        }

        [TestMethod("Parameter passing method", "This is an example of multiple parameter passing")]
        public TestVerdict ParameterPassing(
            [TestParameter("Enter string parameter", "This is an example of a string parameter", "Carpe diem")]
            string stringParam, 
            [TestParameter("Enter a boolean parameter", "This is an example of a boolean parameter", "False")]
            bool boolParam,
            [TestParameter("Enter an integer parameter", "This is an example of an integer parameter", 999)]
            int intParam)
        {
            TestMessage += $"String parameter: {stringParam}, boolean parameter:{boolParam}, int parameter:  {intParam}";
            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict ParalellTest(string filePath)
        {
            try
            {
                // Prepare to setup
                Setup();

                var testSuite = TestSuite.ReadFromFile(filePath);

                var parallelTestScriptObjects = testSuite.TestScriptObjects.ConvertAll<TestCase>(t => t as TestCase)
                    .Where(t => t != null && t.Parallelizable == true);



                var nonParallelTestScriptOjects = testSuite.TestScriptObjects.Except(parallelTestScriptObjects);

                foreach (var testScriptObject in nonParallelTestScriptOjects)
                {
                    if (testScriptObject is TestCase)
                    {
                        // execute test case.
                    }
                    else if (testScriptObject is TestSuite)
                    {
                        // execute test suite.
                    }
                }

            }
            catch (Exception e)
            {
                TestMessage += e.ToString();
                TestVerdict = TestVerdict.Error;
            }
            finally
            {
                Teardown();
            }

            return TestVerdict;
        }

        private TestCase convert(TestScriptObject input)
        {
            if (input is TestCase)
            {
                return input as TestCase;
            }

            return null;
        }

        [TestMethod("This is a simple test", "This is the description")]
        public TestVerdict SimpleTest(bool throwException)
        {
            try
            {
                Setup();
                System.Threading.Thread.Sleep(2000);
                TestMessage += "Really important stuff.";

                //TestCheck.IsTrue("This is a lame test", false);
                //TestAttachments.Add("bob", new Uri(@"c:\temp\stuff"));

                TestAssert.IsTrue(!throwException, "Assert failed at a lame test.");

                //if (throwException)
                //{
                //    throw new Exception("This is a big deal.");
                //}
            }
            //catch (Exception e)
            //{
            //    TestMessage += e.ToString();
            //    TestVerdict = TestVerdict.Error;
            //}
            finally
            {
                Teardown();
            }

            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict ScratchTest1()
        {
            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict SetBreakPoint(string systemId)
        {
            // TestBreakPoints.AddBreakPoint(new Guid(systemId));
            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict ScratchTest2()
        {
            return TestVerdict;
        }

        [TestMethod]
        public TestVerdict ScratchTest3()
        {
            return TestVerdict;
        }


        [TestMethod]
        public TestVerdict MergeGraphics(string backgroundFile, string overlayFile, string mergedImageFile)
        {
            List<string> imageFiles = new List<string>();
            imageFiles.Add(backgroundFile);
            imageFiles.Add(overlayFile);

            //var bitmap = mergeGraphics(imageFiles);

            //bitmap.Save(mergedImageFile);

            var backgroundImage = Image.FromFile(backgroundFile);
            var overlayImage = Image.FromFile(overlayFile);

            var mergedImage = mergeImages(backgroundImage, overlayImage);
            mergedImage.Save(mergedImageFile);

            return TestVerdict;
        }

        private Image mergeImages(Image backgroundImage, Image overlayImage)
        {
            Image mergedImage = backgroundImage;

            if (null != overlayImage)
            {
                Image theOverlay = overlayImage;

                if (PixelFormat.Format32bppArgb != overlayImage.PixelFormat)
                {
                    theOverlay = new Bitmap(overlayImage.Width,
                                            overlayImage.Height,
                                            PixelFormat.Format32bppArgb);
                    using (Graphics graphics = Graphics.FromImage(theOverlay))
                    {
                        graphics.DrawImage(overlayImage,
                                           new Rectangle(0, 0, theOverlay.Width, theOverlay.Height),
                                           new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                                           GraphicsUnit.Pixel);
                    }

                    ((Bitmap)theOverlay).MakeTransparent();
                }

                using (Graphics graphics = Graphics.FromImage(mergedImage))
                {
                    graphics.DrawImage(theOverlay,
                                       new Rectangle(0, 0, mergedImage.Width, mergedImage.Height),
                                       new Rectangle(0, 0, theOverlay.Width, theOverlay.Height),
                                       GraphicsUnit.Pixel);
                }
            }

            return mergedImage;
        }


        public Bitmap mergeGraphics(List<string> imageFiles)
        {
            //read all images into memory
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();

            Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (string image in imageFiles)
                {
                    //create a Bitmap from the file and add it to the list
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);

                    //update the size of the final bitmap
                    width += bitmap.Width;
                    height = bitmap.Height > height ? bitmap.Height : height;

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Black);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (System.Drawing.Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new System.Drawing.Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }

        #endregion

        #region Private and protected methods

        protected override void Setup()
        {
            base.Setup();
        }

        protected override void Teardown()
        {
            base.Teardown();
        }

        #endregion
    }
}
