using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Majorsilence.Reporting.RdlDesign
{
    public partial class ColorPickerPopup : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		ColorPicker _ColorPicker;
private Label lStatus;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
            this.lStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lStatus
            // 
            this.lStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lStatus.Location = new System.Drawing.Point(0, 174);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(233, 13);
            this.lStatus.TabIndex = 0;
            this.lStatus.Text = "Color";
            // 
            // ColorPickerPopup
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(233, 187);
            this.ControlBox = false;
            this.Controls.Add(this.lStatus);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorPickerPopup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.lbColors_Hide);
            this.Load += new System.EventHandler(this.ColorPickerPopup_Load); 
            this.Shown += new System.EventHandler(this.ColorPickerPopup_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ColorPickerPopup_KeyPress);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ColorPickerPopup_MouseMove);
            this.Leave += new System.EventHandler(this.lbColors_Hide);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ColorPickerPopup_MouseDown);
            this.ResumeLayout(false);

		}
		#endregion

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
	}
}
