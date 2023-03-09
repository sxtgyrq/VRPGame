using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SixLabors.ImageSharp.Drawing.Processing;
using System.Drawing.Imaging;

namespace CommonClass.Img
{
    public class Combine
    {
        public Stream _stream1;
        public string _path2;
        public Combine(Stream stream1, string path2)
        {
            this._stream1 = stream1;
            this._path2 = path2;
            this._stream1.Position = 0;
            //List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();


        }

        public string GetBase64()
        {

            using (Image<Rgba32> img1 = Image.Load<Rgba32>(this._stream1)) // load up source images
            using (Image<Rgba32> img2 = Image.Load<Rgba32>(this._path2))
            using (Image<Rgba32> outputImage = new Image<Rgba32>(1200, 1200)) // create output image of the correct dimensions
            {
                // reduce source images to correct dimensions
                // skip if already correct size
                // if you need to use source images else where use Clone and take the result instead
                img1.Mutate(o => o.Resize(new Size(1200, 1200)));
                img2.Mutate(o => o.Resize(new Size(300, 300)));
                img2.Mutate(o => o.Rotate(RotateMode.Rotate270));

                //outputImage.Mutate(o => o.DrawText()
                // take the 2 source images and draw them onto the image
                outputImage.Mutate(o => o
                    .DrawImage(img1, new Point(0, 0), 1f) // draw the first one top left
                    .DrawImage(img2, new Point(475, 564), 0.8f) // draw the second next to it
                );
                //FontCollection collection = new FontCollection();
                //FontFamily family = collection.add("path/to/font.ttf");
                //Font font = family.CreateFont(12, FontStyle.Italic);
                // outputImage.Mutate(o => o.DrawText("要",new Font( 

                outputImage.Mutate(o => o.Resize(new Size(512, 512)));
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    //IImageFormat format = SixLabors.ImageSharp.Formats.Png.PngFormat;
                    outputImage.Save(stream, new PngEncoder());
                    byte[] imageArray = stream.ToArray();
                    var imgBase64Str = Convert.ToBase64String(imageArray);
                    return imgBase64Str;
                }
            }
        }
    }

    public class CombineFont
    {
        string _path1;

        Stream _stream2;
        public CombineFont(string path1, Stream stream2)
        {
            this._path1 = path1;
            this._stream2 = stream2;

            this._stream2.Position = 0;//需要重置，要不了load时，会报错
            //List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();


        }

        public string GetBase64(MemoryStream stream)
        {
            stream.Position = 0;
            using (Image<Rgba32> img1 = Image.Load<Rgba32>(stream))
            {
                img1.Mutate(o => o.Resize(new Size(512, 512)));

                using (System.IO.MemoryStream outStream = new System.IO.MemoryStream())
                {
                    //IImageFormat format = SixLabors.ImageSharp.Formats.Png.PngFormat;
                    img1.Save(outStream, new PngEncoder());
                    byte[] imageArray = outStream.ToArray();
                    var imgBase64Str = Convert.ToBase64String(imageArray);
                    return imgBase64Str;
                }
            }
        }

        public MemoryStream GetMsWithFront()
        {
            var ms = new MemoryStream();
            using (Image<Rgba32> img1 = Image.Load<Rgba32>(this._path1)) // load up source images
            {
                PngDecoder pd = new SixLabors.ImageSharp.Formats.Png.PngDecoder();
                using (Image<Rgba32> img2 = Image.Load<Rgba32>(this._stream2, pd))
                {
                    using (Image<Rgba32> outputImage = new Image<Rgba32>(1200, 1200)) // create output image of the correct dimensions
                    {
                        // reduce source images to correct dimensions
                        // skip if already correct size
                        // if you need to use source images else where use Clone and take the result instead
                        img1.Mutate(o => o.Resize(new Size(1200, 1200)));
                        img2.Mutate(o => o.Resize(new Size(300, 300)));
                        img2.Mutate(o => o.Rotate(RotateMode.Rotate270));

                        //outputImage.Mutate(o => o.DrawText()
                        // take the 2 source images and draw them onto the image
                        outputImage.Mutate(o => o
                            .DrawImage(img1, new Point(0, 0), 1f) // draw the first one top left
                            .DrawImage(img2, new Point(10, 564), 1f) // draw the second next to it
                        );
                        outputImage.Save(ms, new PngEncoder());
                    }
                }

            }
            return ms;
        }
    }
}
