using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Oceanus.Utilities
{
    public class ImageUtilities
    {
        private static readonly EncoderParameters encoderParameters;
        private static readonly ImageCodecInfo jpgEncoder;

        private const double MinRatio = 1d;
        private const double MaxRatio = 1.8;
        private const double TargetRatio = 1.48;
        private const long ThumbJpegQuality = 90;

        static ImageUtilities()
        {
            // set up encoding options
            jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            Encoder myEncoder = Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            encoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, ThumbJpegQuality);
            encoderParameters.Param[0] = myEncoderParameter;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static Image GetCroppedImage(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;

            double ratio;

            if (!IsAspectRatioValid(image, out ratio))
            {
                throw new Exception("Image aspect ratio does not work");
            }

            // calculate the new rectangle - crop based on AR.

            double ratioDelta = Math.Abs(TargetRatio - ratio);
            int pixelDeltaPerSide;

            Rectangle? cropRectangle = null;

            // crop top and bottom
            if (ratio < TargetRatio)
            {
                pixelDeltaPerSide = (int)(height * ratioDelta / 2d);
                cropRectangle = new Rectangle(0, pixelDeltaPerSide, width, height - pixelDeltaPerSide);
            }

            // crop left and right
            else if (ratio > TargetRatio)
            {
                pixelDeltaPerSide = (int)(width * ratioDelta / 2d);
                cropRectangle = new Rectangle(pixelDeltaPerSide, 0, width - pixelDeltaPerSide, height);
            }

            if (cropRectangle != null)
            {
                Bitmap bmpCrop = image.Clone(cropRectangle.Value, image.PixelFormat);

                return bmpCrop;
            }

            return image;
        }

        public static bool IsAspectRatioValid(Stream s)
        {
            bool isValid;
            using (Image img = Image.FromStream(s))
            {
                double ratio;
                isValid = IsAspectRatioValid(img, out ratio);
            }

            return isValid;
        }

        public static bool IsAspectRatioValid(Image image, out double ratio)
        {
            int width = image.Width;
            int height = image.Height;

            // get the aspect ratio of the major axis (> 1)
            double r = width > height ? (double)width / (double)height : (double)height / (double)width;
            ratio = r;
            if (r < MinRatio || r > MaxRatio)
            {
                return false;
            }

            return true;
        }

        public static void SaveImage(string targetFile, Image image)
        {
            image.Save(targetFile, jpgEncoder, encoderParameters);
        }

        public static bool IsJpeg(Image img)
        {
            return img.RawFormat.Equals(ImageFormat.Jpeg);
        }
    }
}