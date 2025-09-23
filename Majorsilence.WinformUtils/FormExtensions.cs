using System.ComponentModel;

namespace Majorsilence.WinformUtils;

public static class FormExtensions
{
    private static bool isOpen = false;

    public static void ShowWaiter(this UserControl parent)
    {
        if (parent.FindForm() is Form form)
        {
            form.ShowWaiter();
        }
    }

    public static void ShowWaiter(this ContainerControl parent)
    {
        if (isOpen) return;

        isOpen = true;
        var waitForm = new WaitForm
        {
            StartPosition = FormStartPosition.Manual,
            Size = parent.Size,
            Width = parent.Width,
            Height = parent.Height,
            ShowInTaskbar = false,
            Location = parent.PointToScreen(System.Drawing.Point.Empty)
        };
        if (parent is Form)
        {
            waitForm.Owner = parent as Form;
        }
        parent.SizeChanged += Parent_SizeChanged;
        if (parent is Form form)
        {
            form.Closed += Parent_Closed;
        }

        parent.Move += Parent_Move;
        waitForm.Show();
        waitForm.BringToFront();
        waitForm.Refresh();
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

    public static void HideWaiter(this UserControl parent)
    {
        if (parent.FindForm() is Form form)
        {
            // find the owner of type WaitForm
            form.HideWaiter();
        }
    }

    public static void HideWaiter(this ContainerControl parent)
    {
        WaitForm waitForm = null;
        if (parent is Form form)
        {
            waitForm = form.OwnedForms.OfType<WaitForm>().FirstOrDefault();
        }

        if (waitForm == null)
        {
            // Search application wide
            waitForm = Application.OpenForms.OfType<WaitForm>().FirstOrDefault();
        }

        if (waitForm != null)
        {
            parent.SizeChanged -= Parent_SizeChanged;
            if (parent is Form form2)
            {
                form2.Closed -= Parent_Closed;
            }

            parent.Move -= Parent_Move;
            waitForm.Close();
            waitForm.Dispose();
        }

        isOpen = false;
    }
}