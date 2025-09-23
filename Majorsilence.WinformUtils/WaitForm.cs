namespace Majorsilence.WinformUtils
{
    internal class WaitForm : Form
    {
        private DateTime Started;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTimeTaken;
        private System.Windows.Forms.Timer timer1;

        public delegate bool CheckStopWaitDialog();

        public WaitForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            InitializeComponent();
            this.Opacity = 0.50;
            this.BackColor = Color.Black;

            Started = DateTime.Now;
            timer1.Interval = 1000;
            timer1_Tick(null, null);
            timer1.Start();

            this.SizeChanged += WaitForm_SizeChanged;
            this.Move += WaitForm_Move;
        }

        private void WaitForm_Move(object? sender, EventArgs e)
        {
            progressBar1.Location = new Point((this.Width - this.progressBar1.Width) / 2,
                (this.Height - this.progressBar1.Height) / 2);
            label1.Location = new Point((this.Width - this.label1.Width) / 2,
                (this.Height - this.label1.Height) / 2 - 40);
            label2.Location = new Point((this.Width - this.label2.Width) / 2,
                (this.Height - this.label2.Height) / 2 + 40);
            lblTimeTaken.Location = new Point((this.Width - this.lblTimeTaken.Width) / 2,
                (this.Height - this.lblTimeTaken.Height) / 2 + 70);
        }

        private void WaitForm_SizeChanged(object? sender, EventArgs e)
        {
            progressBar1.Location = new Point((this.Width - this.progressBar1.Width) / 2,
                (this.Height - this.progressBar1.Height) / 2);
            label1.Location = new Point((this.Width - this.label1.Width) / 2,
                (this.Height - this.label1.Height) / 2 - 40);
            label2.Location = new Point((this.Width - this.label2.Width) / 2,
                (this.Height - this.label2.Height) / 2 + 40);
            lblTimeTaken.Location = new Point((this.Width - this.lblTimeTaken.Width) / 2,
                (this.Height - this.lblTimeTaken.Height) / 2 + 70);
        }

        private void timer1_Tick(object sender, EventArgs e)
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
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTimeTaken = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            
            this.label1.Text = resources.GetString("label1");
            this.label1.Name = "label1";
            this.label1.UseWaitCursor = true;
            this.label1.Location = new Point((this.Width - this.label1.Width) / 2,
                (this.Height - this.label1.Height) / 2 - 40);
            
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.UseWaitCursor = true;
            this.progressBar1.Location = new Point((this.Width - this.progressBar1.Width) / 2,
                (this.Height - this.progressBar1.Height) / 2);

            this.label2.Text = resources.GetString("label2");
            this.label2.Name = "label2";
            this.label2.UseWaitCursor = true;
            this.label2.Location = new Point((this.Width - this.label2.Width) / 2,
                (this.Height - this.label2.Height) / 2 + 40);
            
            this.lblTimeTaken.Name = "lblTimeTaken";
            this.lblTimeTaken.UseWaitCursor = true;
            this.lblTimeTaken.Location = new Point((this.Width - this.lblTimeTaken.Width) / 2,
                (this.Height - this.lblTimeTaken.Height) / 2 + 70);

            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.lblTimeTaken);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}