using System;

namespace fyiReporting.RdlGtkViewer
{
    public partial class ParameterPrompt : Gtk.Dialog
    {
        public ParameterPrompt()
        {
            this.Build();
        }

		
        public string Parameters
        {
            get
            {
                return TextBoxParameters.Text;
            }
            set
            {
                TextBoxParameters.Text = value;
            }
        }
		
    }
	
	
}

