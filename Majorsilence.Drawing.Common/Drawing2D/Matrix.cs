using SkiaSharp;


namespace Majorsilence.Drawing.Drawing2D
{
    public class Matrix
    {
        private SKMatrix _matrix;

        public Matrix()
        {
            _matrix = SKMatrix.MakeIdentity();
        }

        public void Translate(float dx, float dy)
        {
            _matrix = SKMatrix.Concat(_matrix, SKMatrix.CreateTranslation(dx, dy));
        }

        public void Scale(float scaleX, float scaleY)
        {
            _matrix = SKMatrix.Concat(_matrix, SKMatrix.CreateScale(scaleX, scaleY));
        }

        public void Rotate(float angle)
        {
            _matrix = SKMatrix.Concat(_matrix, SKMatrix.CreateRotationDegrees(angle));
        }

        public SKMatrix ToSKMatrix() => _matrix;
    }

}
