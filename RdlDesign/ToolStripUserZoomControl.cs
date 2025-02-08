using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Majorsilence.Reporting.RdlDesign
{
    //Declare a class that inherits from ToolStripControlHost.
    public class ToolStripUserZoomControl : ToolStripControlHost
    {
        // Call the base constructor passing in a MonthCalendar instance.
        public ToolStripUserZoomControl() : base(new UserZoomControl()) { }

        public UserZoomControl ZoomControl
        {
            get
            {
                return Control as UserZoomControl;
            }
        }

        [Browsable(true)]
        public event EventHandler<UserZoomControl.CambiaValori> ZoomChanged;


        // Subscribe and unsubscribe the control events you wish to expose.
        protected override void OnSubscribeControlEvents(Control c)
        {
            // Call the base so the base events are connected.
            base.OnSubscribeControlEvents(c);

            UserZoomControl zoomControl = (UserZoomControl)c;
            zoomControl.ZoomChanged += new EventHandler<UserZoomControl.CambiaValori>(ZoomControl1_ValueChanged);
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            // Call the base method so the basic events are unsubscribed.
            base.OnUnsubscribeControlEvents(c);

            UserZoomControl zoomControl = (UserZoomControl)c;
            zoomControl.ZoomChanged -= new EventHandler<UserZoomControl.CambiaValori>(ZoomControl1_ValueChanged);
        }


        // Raise the DateChanged event.
        private void ZoomControl1_ValueChanged(object sender, UserZoomControl.CambiaValori e)
        {
            if (this.ZoomChanged != null)
                this.ZoomChanged(this, e);
        }

    }
}
