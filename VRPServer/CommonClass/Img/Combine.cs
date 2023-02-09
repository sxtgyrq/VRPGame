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

namespace CommonClass.Img
{
    public class Combine
    {
        public string _path1, _path2;
        public Combine(string path1, string path2)
        {
            this._path1 = path1;
            this._path2 = path2;
            //List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();


        }

        public string GetBase64()
        {

            using (Image<Rgba32> img1 = Image.Load<Rgba32>(this._path1)) // load up source images
            using (Image<Rgba32> img2 = Image.Load<Rgba32>(this._path2))
            using (Image<Rgba32> outputImage = new Image<Rgba32>(1200, 1200)) // create output image of the correct dimensions
            {
                // reduce source images to correct dimensions
                // skip if already correct size
                // if you need to use source images else where use Clone and take the result instead
                img1.Mutate(o => o.Resize(new Size(1200, 1200)));
                img2.Mutate(o => o.Resize(new Size(300, 300)));
                img2.Mutate(o => o.Rotate(RotateMode.Rotate90));

                //outputImage.Mutate(o => o.DrawText()
                // take the 2 source images and draw them onto the image
                outputImage.Mutate(o => o
                    .DrawImage(img1, new Point(0, 0), 1f) // draw the first one top left
                    .DrawImage(img2, new Point(10, 564), 0.6f) // draw the second next to it
                );
                //FontCollection collection = new FontCollection();
                //FontFamily family = collection.add("path/to/font.ttf");
                //Font font = family.CreateFont(12, FontStyle.Italic);
               // outputImage.Mutate(o => o.DrawText("要",new Font( 
                    
                outputImage.Mutate(o => o.Resize(new Size(256, 256)));
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
}
