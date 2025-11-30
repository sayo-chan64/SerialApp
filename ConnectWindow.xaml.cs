using SerialApp.Services;   // 追加
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
        DataContext = ViewModel;

        // 画面を閉じるイベントの購読
        ViewModel.RequestClose += (isSuccess) =>
        {
            DialogResult = isSuccess;
            Close();
        };

        // 警告を表示するイベントの購読 (DialogServiceを使って簡略化)
        ViewModel.ShowAlert += async (title, message) =>
        {
            // 警告アイコンを指定して呼び出し
            await DialogService.ShowAsync(title, message, SymbolRegular.Warning24);
        };
    }
}