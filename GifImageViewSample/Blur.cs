using System;
using Android.Support.V8.Renderscript;
using Android.Graphics;
using Android.Content;

namespace GifImageViewSample
{
    public class Blur
    {
        const float BLUR_RADIUS = 25f;

        RenderScript rs;
        ScriptIntrinsicBlur script;
        Allocation input;
        Allocation output;
        bool configured;
        Bitmap tmp;
        int[] pixels;


        public static Blur NewInstance(Context context)
        {
            return new Blur(context);
        }

        Blur(Context context)
        {
            rs = RenderScript.Create(context);
        }

        public Bitmap BlurImage(Bitmap image)
        {
            if (image == null)
                return null;

            image = RGB565toARGB888(image);
            if (!configured)
            {
                input = Allocation.CreateFromBitmap(rs, image);
                output = Allocation.CreateTyped(rs, input.Type);
                script = ScriptIntrinsicBlur.Create(rs, Element.U8_4(rs));
                script.SetRadius(BLUR_RADIUS);
                configured = true;
            }
            else
            {
                input.CopyFrom(image);
            }

            script.SetInput(input);
            script.ForEach(output);
            output.CopyTo(image);

            return image;
        }

        Bitmap RGB565toARGB888(Bitmap img)
        {
            int numPixels = img.Width * img.Height;

            //Create a Bitmap of the appropriate format.
            if (tmp == null)
            {
                tmp = Bitmap.CreateBitmap(img.Width, img.Height, Bitmap.Config.Argb8888);
                pixels = new int[numPixels];
            }

            //Get JPEG pixels.  Each int is the color values for one pixel.
            img.GetPixels(pixels, 0, img.Width, 0, 0, img.Width, img.Height);

            //Set RGB pixels.
            tmp.SetPixels(pixels, 0, tmp.Width, 0, 0, tmp.Width, tmp.Height);

            return tmp;
        }
    }
}

