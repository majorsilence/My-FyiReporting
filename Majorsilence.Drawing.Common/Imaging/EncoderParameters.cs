namespace Majorsilence.Drawing.Imaging
{
    // Simulates EncoderParameters in System.Drawing.Common
    public class EncoderParameters
    {
        private List<EncoderParameter> _parameters;

        // Constructor
        public EncoderParameters(int count)
        {
            _parameters = new List<EncoderParameter>(new EncoderParameter[count]);
        }

        // Add a parameter to the list
        public void Add(EncoderParameter parameter)
        {
            _parameters.Add(parameter);
        }

        // Retrieve the list of parameters
        public List<EncoderParameter> GetParameters() => _parameters;

        // Indexer to access parameters by index
        public EncoderParameter[] Param
        {
            get => _parameters.ToArray();
            set
            {
                if (value.Length != _parameters.Count)
                    throw new ArgumentException("Array length must match the number of parameters.");
                _parameters = new List<EncoderParameter>(value);
            }
        }
    }

}
