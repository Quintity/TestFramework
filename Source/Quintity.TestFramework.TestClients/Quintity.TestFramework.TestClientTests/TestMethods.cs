﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Quintity.TestFramework.Core;


namespace Quintity.TestFramework.TestClientTests
{
    [TestClass]
    public class TestMethods : TestClassBase
    {
        #region TestMethods

        [TestMethod("This is a simple test", "This is the description")]
        public TestVerdict SimpleTest(bool throwException)
        {
            try
            {
                Setup();

                if (throwException)
                {
                    throw new Exception("This is a big deal.");
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
