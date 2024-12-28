namespace Majorsilence.Drawing.Imaging
{
    // Simulate ImageCodecInfo (simplified)
    public class ImageCodecInfo
    {
        public Guid Guid { get; set; }
        public string MimeType { get; set; }
        public ImageFormat Format { get; set; }
        public string FormatDescription
        {
            get
            {
                return Format switch
                {
                    ImageFormat.Bmp => "BMP",
                    ImageFormat.Jpeg => "JPEG",
                    ImageFormat.Png => "PNG",
                    ImageFormat.Gif => "GIF",
                    ImageFormat.Tiff => "TIFF",
                    _ => "Unknown"
                };
            }
        }

        public static ImageCodecInfo[] GetImageEncoders()
        {
            return new[]
            {
                    new ImageCodecInfo { MimeType = "image/bmp", Format = ImageFormat.Bmp },
                    new ImageCodecInfo { MimeType = "image/jpeg", Format = ImageFormat.Jpeg },
                    new ImageCodecInfo { MimeType = "image/png", Format = ImageFormat.Png },
                    new ImageCodecInfo { MimeType = "image/gif", Format = ImageFormat.Gif },
                    new ImageCodecInfo { MimeType = "image/tiff", Format = ImageFormat.Tiff }
                };
        }
    }



}
