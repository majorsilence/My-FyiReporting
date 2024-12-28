using SkiaSharp;


namespace Majorsilence.Drawing.Imaging
{
    // Extension method for converting ImageFormat to SkiaSharp's image encode format
    public static class ImageFormatExtensions
    {
        public static SKEncodedImageFormat ToSkImageEncodeFormat(this ImageFormat format)
        {
            return format switch
            {
                ImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
                ImageFormat.Png => SKEncodedImageFormat.Png,
                ImageFormat.Bmp => SKEncodedImageFormat.Bmp,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
