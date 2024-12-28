namespace Majorsilence.Drawing.Imaging
{
    // Simulates EncoderParameter in System.Drawing.Common
    public class EncoderParameter
    {
        public Encoder Encoder { get; }
        public object Value { get; }

        public EncoderParameter(Encoder encoder, object value)
        {
            Encoder = encoder ?? throw new ArgumentNullException(nameof(encoder));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }



}
