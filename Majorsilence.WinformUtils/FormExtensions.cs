namespace Majorsilence.WinformUtils;

public static class FormExtensions
{
    public static void ShowWaiter(this Form parent)
    {
        var waitForm = new WaitForm
        {
            FormBorderStyle = FormBorderStyle.None,
            StartPosition = FormStartPosition.CenterParent,
            BackColor = Color.Black,
            Opacity = 0.5,
            Width = parent.Width,
            Height = parent.Height,
            ShowInTaskbar = false
        };
        parent.SizeChanged += Parent_SizeChanged;
        parent.Closed += Parent_Closed;
        parent.Move += Parent_Move;
        waitForm.Show(parent);
        waitForm.BringToFront();
        Application.DoEvents();
    }

    private static void Parent_SizeChanged(object? sender, EventArgs e)
    {
        if (sender is Form parent)
        {
            var waitForm = parent.OwnedForms.OfType<WaitForm>().FirstOrDefault();
            if (waitForm != null)
            {
                waitForm.Width = parent.Width;
                waitForm.Height = parent.Height;
            }
        }
    }

    private static void Parent_Closed(object? sender, EventArgs e)
    {
        if (sender is Form parent)
        {
            var waitForm = parent.OwnedForms.OfType<WaitForm>().FirstOrDefault();
            waitForm?.Close();
        }
    }

    private static void Parent_Move(object? sender, EventArgs e)
    {
        if (sender is Form parent)
        {
            var waitForm = parent.OwnedForms.OfType<WaitForm>().FirstOrDefault();
            if (waitForm != null)
            {
                waitForm.Location = parent.Location;
            }
        }
    }

    public static void HideWaiter(this Form parent)
    {
        var waitForm = parent.OwnedForms.OfType<WaitForm>().FirstOrDefault();
        if (waitForm != null)
        {
            parent.SizeChanged -= Parent_SizeChanged;
            parent.Closed -= Parent_Closed;
            parent.Move -= Parent_Move;
            waitForm.Close();
            waitForm.Dispose();
        }
    }
}