using SerialApp.ViewModels;
using Wpf.Ui.Controls; // モダンなMessageBox用

namespace SerialApp;

public partial class ConnectWindow : FluentWindow
{
    public ConnectViewModel ViewModel { get; }

    public ConnectWindow()
    {
        InitializeComponent();

        ViewModel = new ConnectViewModel();
        DataContext = ViewModel;

        // 画面を閉じるイベントの購読
        ViewModel.RequestClose += (isSuccess) =>
        {
            DialogResult = isSuccess;
            Close();
        };

        // 警告を表示するイベントの購読
        ViewModel.ShowAlert += async (title, message) =>
        {
            // モダンなMessageBoxを作成
            var msgBox = new MessageBox
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                MaxWidth = 400
            };

            // 表示 (非同期で待機)
            await msgBox.ShowDialogAsync();
        };
    }
}