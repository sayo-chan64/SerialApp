using SerialApp.ViewModels;
using Wpf.Ui.Controls;

namespace SerialApp;

public partial class ConnectWindow : FluentWindow
{
    public ConnectViewModel ViewModel { get; }

    public ConnectWindow()
    {
        InitializeComponent();

        ViewModel = new ConnectViewModel();
        this.DataContext = ViewModel;

        ViewModel.RequestClose += (isSuccess) =>
        {
            this.DialogResult = isSuccess;
            this.Close();
        };
    }
}