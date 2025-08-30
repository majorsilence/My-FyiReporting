using Majorsilence.Drawing.Imaging;
using SkiaSharp;


namespace Majorsilence.Drawing
{
    // Compatibility wrapper for Bitmap
    public class Bitmap : Image, IDisposable
    {

        // Constructor for creating a new bitmap with width and height
        public Bitmap(int width, int height) : base(width, height)
        {
        }

        // Constructor for loading an image from a file
        public Bitmap(string filename) : base(filename)
        {
        }
        
        public Bitmap(MemoryStream ms) : base(ms)
        {
        }

        // Constructor for creating a bitmap from SKBitmap
        public Bitmap(SKBitmap skBitmap) : base(skBitmap)
        {
            _skBitmap = skBitmap;
        }

        // Save method with options (e.g., quality)
        public void Save(string filename, ImageFormat format, EncoderParameters encoderParams)
        {
            // This is a placeholder for implementing quality settings or other encoder options.
            Save(filename, format);  // Basic implementation
        }

        public new void Save(Stream stream, ImageFormat format)
        {
            SKImage.FromBitmap(_skBitmap).Encode(format.ToSkImageEncodeFormat(), 100).SaveTo(stream);
        }

        // Convert bitmap to graphics
        public Graphics GetGraphics()
        {
            return new Graphics(new SKCanvas(_skBitmap));
        }

        // Dispose of the bitmap
        public new void Dispose()
        {
            base.Dispose();
            _skBitmap?.Dispose();
        }

        // Static method to create a bitmap from file
        public static new Bitmap FromFile(string filename)
        {
            return new Bitmap(filename);
        }

        public static implicit operator SKBitmap(Bitmap i)
        {
            return i._skBitmap;
        }

        public static implicit operator Bitmap(SKBitmap i)
        {
            return new Bitmap(i);
        }
    }

}
