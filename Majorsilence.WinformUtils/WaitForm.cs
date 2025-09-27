namespace Majorsilence.WinformUtils
{
    internal class WaitForm : Form
    {
        private DateTime Started;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblTimeTaken;
        private System.Threading.Timer timer1;
        public delegate bool CheckStopWaitDialog();

        public WaitForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            InitializeComponent();
            this.Opacity = 0.50;
            this.BackColor = Color.Black;

            Started = DateTime.Now;
            timer1 = new System.Threading.Timer(_ =>
            {
                try
                {
                    this.Invoke(async ()=>
                    {
                        await timer1_Tick(null, null);
                    });
                }
                catch { }

            }, null, 0, 50);

            this.SizeChanged += WaitForm_SizeChanged;
            this.Move += WaitForm_Move;
        }

        
        private void PlaceControls()
        {
            progressBar1.Location = new Point((this.Width - this.progressBar1.Width) / 2,
                (this.Height - this.progressBar1.Height) / 2);
            lblTimeTaken.Location = new Point((this.Width - this.lblTimeTaken.Width) / 2 + 20,
                (this.Height - this.lblTimeTaken.Height) / 2 + 50);
        }
        
        private void WaitForm_Move(object? sender, EventArgs e)
        {
            PlaceControls();
        }

        private void WaitForm_SizeChanged(object? sender, EventArgs e)
        {
            PlaceControls();
        }

        private async Task timer1_Tick(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(Strings));
            var time = DateTime.Now - Started;
            if (time.TotalMinutes < 1)
                lblTimeTaken.Text = string.Format("{0} {1}", time.Seconds, resources.GetString("WaitForm_Seconds"));
            else
                lblTimeTaken.Text = string.Format("{0} {1} {2} {3}", (int)time.TotalMinutes,
                    resources.GetString("WaitForm_Minutes"),
                    time.Seconds, resources.GetString("WaitForm_Seconds"));
            
            Application.DoEvents();
        }

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            timer1?.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.DoubleBuffered = true;
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(Strings));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblTimeTaken = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.UseWaitCursor = true;
            this.progressBar1.Location = new Point((this.Width - this.progressBar1.Width) / 2,
                (this.Height - this.progressBar1.Height) / 2);

            this.lblTimeTaken.Name = "lblTimeTaken";
            this.lblTimeTaken.ForeColor = Color.White;
            this.lblTimeTaken.UseWaitCursor = true;
            this.lblTimeTaken.Location = new Point((this.Width - this.lblTimeTaken.Width) / 2 + 20,
                (this.Height - this.lblTimeTaken.Height) / 2 + 50);

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.lblTimeTaken);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}