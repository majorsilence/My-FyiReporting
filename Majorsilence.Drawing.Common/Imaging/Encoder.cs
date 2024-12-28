namespace Majorsilence.Drawing.Imaging
{
    // Simulates Encoder in System.Drawing.Common
    public class Encoder
    {
        public static readonly Encoder Quality = new Encoder("Quality");
        public static readonly Encoder Compression = new Encoder("Compression");

        public string ParameterName { get; }

        private Encoder(string parameterName)
        {
            ParameterName = parameterName;
        }
    }



}
