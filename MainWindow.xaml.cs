using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using SerialApp.Services; // 追加

namespace SerialApp;

public partial class MainWindow : FluentWindow
{
    private readonly SerialPort _serialPort = new();

    public MainWindow()
    {
        InitializeComponent();

        RootNavigation.Loaded += (s, e) =>
        {
            NavigateTo("Command");
        };
    }

    private void RootNavigation_SelectionChanged(NavigationView sender, RoutedEventArgs e)
    {
        if (sender.SelectedItem is NavigationViewItem item && item.Tag is string tag)
        {
            NavigateTo(tag);
        }
    }

    private void NavigateTo(string tag)
    {
        switch (tag)
        {
            case "Command":
                RootFrame.Navigate(new Pages.CommandPage());
                break;
            case "Data":
                RootFrame.Navigate(new Pages.DataPage());
                break;
        }
    }

    public void InitializeSerialPort(string portName, int baudRate)
    {
        if (_serialPort.IsOpen) _serialPort.Close();

        _serialPort.PortName = portName;
        _serialPort.BaudRate = baudRate;

        this.Title = $"SerialApp - {portName} : {baudRate} bps";
    }

    private async void SettingsItem_Click(object sender, RoutedEventArgs e)
    {
        var settingsWindow = new ConnectWindow();
        bool? result = settingsWindow.ShowDialog();

        if (result == true)
        {
            var vm = settingsWindow.ViewModel;

            // 安全にプロパティ取得
            string portName = vm.SelectedPortInfo?.PortName ?? "COM1";
            int baudRate = int.Parse(vm.SelectedBaudRate);

            InitializeSerialPort(portName, baudRate);

            // 【変更点】DialogServiceを使ってモダンな成功メッセージを表示
            // 緑色のチェックマークアイコンを指定
            await DialogService.ShowAsync(
                "設定更新",
                "シリアルポートの接続設定を更新しました。",
                SymbolRegular.CheckmarkCircle24
            );
        }
    }
}