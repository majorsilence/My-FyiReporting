using Majorsilence.Drawing.Imaging;
using SkiaSharp;


namespace Majorsilence.Drawing
{
    // Extension method to handle quality parameter for encoding
    public static class SkiaImageExtensions
    {
        // Converts EncoderParameters to SkiaSharp encoding options
        public static SKEncodedImageFormat ToSkImageEncodeOptions(this EncoderParameters encoderParams, ImageFormat format)
        {
            // Default Skia options
            var encodeOptions = new SKEncodedImageFormat();

            foreach (var parameter in encoderParams.GetParameters())
            {
                if (parameter.Encoder == Encoder.Quality)
                {
                    var quality = (int)parameter.Value;

                    //// SkiaSharp supports quality for JPEG encoding
                    //if (format == ImageFormat.Jpeg)
                    //{
                    //    encodeOptions = quality;
                    //}
                    //else
                    //{
                    //    throw new NotSupportedException("Quality parameter is only supported for JPEG encoding in SkiaSharp.");
                    //}
                }
                else
                {
                    throw new NotSupportedException($"Encoder parameter {parameter.Encoder.ParameterName} is not supported.");
                }
            }

            return encodeOptions;
        }
    }



}
