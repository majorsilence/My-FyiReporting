using Gtk;

namespace Majorsilence.Reporting.RdlGtk3
{
    public partial class ParameterPrompt : Dialog
    {
        public ParameterPrompt()
        {
            Build();
        }


        public string Parameters
        {
            get => TextBoxParameters.Text;
            set => TextBoxParameters.Text = value;
        }
    }
}