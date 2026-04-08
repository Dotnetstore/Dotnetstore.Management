using Avalonia.Controls;

namespace Dotnetstore.Management.UI.GUI.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        EmailTextBox.Focus();
    }
}
