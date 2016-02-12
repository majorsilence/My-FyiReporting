using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EncryptionProvider.Properties;

namespace EncryptionProvider
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 200;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterScreen;
            Label textLabel = new Label() {Left = 50, Top = 20, Width = 400, Height = 110, Text = text};
            TextBox textBox = new TextBox() {Left = 50, Top = 100, Width = 400};
            Button confirmation = new Button()
            {
                Text = Resources.Prompt_ShowDialog_OK,
                Left = 350,
                Width = 100,
                Top = 120
            };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog();
            return textBox.Text;
        }
    }
}
