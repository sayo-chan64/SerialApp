using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls; // モダンなUI用

namespace SerialApp.Services;

/// <summary>
/// アプリ全体でモダンなメッセージボックスを表示するためのヘルパークラス
/// </summary>
public static class DialogService
{
    /// <summary>
    /// アイコン付きのモダンなメッセージボックスを表示します。
    /// </summary>
    /// <param name="title">ウィンドウのタイトル</param>
    /// <param name="message">表示するメッセージ</param>
    /// <param name="iconSymbol">表示するアイコン (デフォルトは情報アイコン)</param>
    public static async Task ShowAsync(string title, string message, SymbolRegular iconSymbol = SymbolRegular.Info24)
    {
        // 1. アイコンの色を種類によって自動決定
        Brush iconColor = iconSymbol switch; // デフォルト
        {
            SymbolRegular.Warning24 => Brushes.Orange;
            SymbolRegular.ErrorCircle24 => Brushes.Red;
            SymbolRegular.CheckmarkCircle24 => Brushes.LightGreen;
            SymbolRegular.Info24 => Brushes.SkyBlue;
            _ => Brushes.White;
        };

        // 2. レイアウトの構築 (アイコン + テキスト)
        var contentStack = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center
        };

        var icon = new SymbolIcon
        {
            Symbol = iconSymbol,
            FontSize = 32,
            Margin = new Thickness(0, 0, 15, 0),
            Foreground = iconColor
        };

        var textBlock = new Wpf.Ui.Controls.TextBlock
        {
            Text = message,
            VerticalAlignment = VerticalAlignment.Center,
            TextWrapping = TextWrapping.Wrap
        };

        contentStack.Children.Add(icon);
        contentStack.Children.Add(textBlock);

        // 3. モダンなMessageBoxの生成
        var msgBox = new Wpf.Ui.Controls.MessageBox
        {
            Title = title,
            Content = contentStack,
            CloseButtonText = "OK",
            MaxWidth = 500,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };

        // 4. 表示して待機
        await msgBox.ShowDialogAsync();
    }
}