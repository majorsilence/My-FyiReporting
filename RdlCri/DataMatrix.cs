namespace Majorsilence.Reporting.Cri
{
    public class DataMatrix : ZxingBarcodes
    {
        public DataMatrix() : base(35.91f, 65.91f) // Optimal width at mag 1
        {
            format = ZXing.BarcodeFormat.DATA_MATRIX;
        }       
    }
}