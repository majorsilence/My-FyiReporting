using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlViewer
{
	public partial class RdlViewer
	{
		#region Windows Form Designer generated code

private ToolTip _vScrollToolTip;
private PageDrawing _DrawPanel;
private Button _RunButton;
private PictureBox _WarningButton;
private ScrollableControl _ParameterPanel;
private RdlViewerFind _FindCtl;

		
		#endregion
private HScrollBar _hScroll;
private VScrollBar _vScroll;


private void InitializeComponent()
{
    this._RunButton = new System.Windows.Forms.Button();
    this._hScroll = new System.Windows.Forms.HScrollBar();
    this._vScroll = new System.Windows.Forms.VScrollBar();
    this._DrawPanel = new fyiReporting.RdlViewer.PageDrawing();
    this.SuspendLayout();
    // 
    // _RunButton
    // 
    this._RunButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
    this._RunButton.Location = new System.Drawing.Point(454, 14);
    this._RunButton.Name = "_RunButton";
    this._RunButton.Size = new System.Drawing.Size(90, 23);
    this._RunButton.TabIndex = 1;
    this._RunButton.Text = "Run Report";
    this._RunButton.UseVisualStyleBackColor = true;
    this._RunButton.Click += new System.EventHandler(this.ParametersViewClick);
    // 
    // _hScroll
    // 
    this._hScroll.Location = new System.Drawing.Point(242, 306);
    this._hScroll.Name = "_hScroll";
    this._hScroll.Size = new System.Drawing.Size(80, 17);
    this._hScroll.TabIndex = 2;
    this._hScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HorizontalScroll);
    // 
    // _vScroll
    // 
    this._vScroll.Enabled = false;
    this._vScroll.Location = new System.Drawing.Point(541, 142);
    this._vScroll.Name = "_vScroll";
    this._vScroll.Size = new System.Drawing.Size(17, 80);
    this._vScroll.TabIndex = 3;
    this._vScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.VerticalScroll);
    // 
    // _DrawPanel
    // 
    this._DrawPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
    this._DrawPanel.Location = new System.Drawing.Point(95, 51);
    this._DrawPanel.Name = "_DrawPanel";
    this._DrawPanel.Size = new System.Drawing.Size(353, 224);
    this._DrawPanel.TabIndex = 0;
    this._DrawPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawPanelPaint);
    this._DrawPanel.Resize += new System.EventHandler(this.DrawPanelResize);
    this._DrawPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DrawPanelKeyDown);
    // 
    // RdlViewer
    // 
    this.Controls.Add(this._vScroll);
    this.Controls.Add(this._hScroll);
    this.Controls.Add(this._RunButton);
    this.Controls.Add(this._DrawPanel);
    this.Name = "RdlViewer";
    this.Size = new System.Drawing.Size(558, 323);
    this.ResumeLayout(false);

}
		
	}
}
