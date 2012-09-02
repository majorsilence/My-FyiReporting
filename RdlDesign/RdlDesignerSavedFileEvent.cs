using System;
using System.Collections.Generic;
using System.Text;

namespace fyiReporting.RdlDesign
{
    public delegate void RdlDesignerSavedFileEventHandler(object sender, RdlDesignerSavedFileEvent e);

    public class RdlDesignerSavedFileEvent : System.EventArgs
    {
        private Uri _filePath;

        public RdlDesignerSavedFileEvent(Uri filePath)
        {
            _filePath = filePath;
        }

        public Uri FilePath
        {
            get { return _filePath; }
        }

    }
}
