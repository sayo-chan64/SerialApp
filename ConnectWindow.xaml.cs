using System.Windows.Controls; // StackPanel, TextBlock用
using System.Windows.Media;    // Brushes用
using SerialApp.ViewModels;
using Wpf.Ui.Controls;         // モダンなMessageBox, SymbolIcon用

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
            // アイコンとメッセージを横並びにするためのパネルを作成
            var contentStack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            // 警告アイコンの設定 (SymbolIconを使用)
            var icon = new SymbolIcon
            {
                Symbol = SymbolRegular.Warning24, // 警告マーク
                FontSize = 32,                    // アイコンサイズを少し大きく
                Margin = new System.Windows.Thickness(0, 0, 15, 0), // テキストとの間隔
                Foreground = Brushes.Orange       // アイコンの色（警告色）
            };

            // テキストの設定
            var textBlock = new Wpf.Ui.Controls.TextBlock
            {
                Text = message,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                TextWrapping = System.Windows.TextWrapping.Wrap
            };

            // パネルに追加
            contentStack.Children.Add(icon);
            contentStack.Children.Add(textBlock);

            // モダンなMessageBoxを作成
            var msgBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = title,
                Content = contentStack, // 文字列ではなくパネルを設定
                CloseButtonText = "OK",
                MaxWidth = 450
            };

            // 表示 (非同期で待機)
            await msgBox.ShowDialogAsync();
        };
    }
}