using Majorsilence.Drawing.Imaging;
using SkiaSharp;


namespace Majorsilence.Drawing
{
    // Import the SkiaSharp library

    public class Image : IDisposable
    {
        protected SKBitmap _skBitmap;

        public int Width => _skBitmap.Width;
        public int Height => _skBitmap.Height;

        public Image(SKBitmap bitmap)
        {
            _skBitmap = bitmap;
        }

        public Image(int width, int height)
        {
            _skBitmap = new SKBitmap(width, height);
        }

        // Constructor for loading an image from a file
        public Image(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                _skBitmap = SKBitmap.Decode(stream);
            }
        }

        public Image(MemoryStream ms)
        {
            ms.Position = 0;
            _skBitmap = SKBitmap.Decode(ms);
        }
        
        public static Image FromFile(string path)
        {
            var bitmap = SKBitmap.Decode(path);
            if (bitmap == null)
            {
                throw new FileNotFoundException("The image file could not be found or loaded.", path);
            }
            return new Image(bitmap);
        }

        public void Save(string path, ImageFormat format)
        {
            var skFormat = GetSkFormat(format);

            using var image = SKImage.FromBitmap(_skBitmap);
            using var data = image.Encode(skFormat, 100);
            using var stream = File.OpenWrite(path);
            data.SaveTo(stream);
        }

        public void Save(Stream stream, ImageFormat format)
        {
            var skFormat = GetSkFormat(format);
            using var image = SKImage.FromBitmap(_skBitmap);
            using var data = image.Encode(skFormat, 100);
            data.SaveTo(stream);
        }

        public void Save(Stream stream, ImageFormat format, int quality)
        {
            var skFormat = GetSkFormat(format);
            using var image = SKImage.FromBitmap(_skBitmap);
            using var data = image.Encode(skFormat, quality);
            data.SaveTo(stream);
        }

        public void Save(Stream stream, ImageCodecInfo codec, EncoderParameters encoderParams)
        {
            var skFormat = GetSkFormat(codec.Format);
            using var image = SKImage.FromBitmap(_skBitmap);
            using var data = image.Encode(skFormat, 100);
            data.SaveTo(stream);
        }

        private SKEncodedImageFormat GetSkFormat(ImageFormat format)
        {
            SKEncodedImageFormat skFormat;
            switch (format)
            {
                case ImageFormat.Jpeg:
                    skFormat = SKEncodedImageFormat.Jpeg;
                    break;
                case ImageFormat.Png:
                    skFormat = SKEncodedImageFormat.Png;
                    break;
                case ImageFormat.Gif:
                    skFormat = SKEncodedImageFormat.Gif;
                    break;
                case ImageFormat.Bmp:
                    skFormat = SKEncodedImageFormat.Bmp;
                    break;
                case ImageFormat.Tiff:
                    throw new NotSupportedException("TIFF format is not supported.");
                default:
                    throw new ArgumentException("Unsupported image format.", nameof(format));
            }
            return skFormat;
        }

        public static Image FromStream(Stream stream)
        {
            var bitmap = SKBitmap.Decode(stream);
            if (bitmap == null)
            {
                throw new ArgumentException("The stream does not contain a valid image.", nameof(stream));
            }
            return new Image(bitmap);
        }


        public static implicit operator SKBitmap(Image i)
        {
            return i._skBitmap;
        }

        public static implicit operator Image(SKBitmap i)
        {
            return new Image(i);
        }

        public void Dispose()
        {
           _skBitmap?.Dispose();
        }
    }

}
