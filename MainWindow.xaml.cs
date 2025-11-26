using System.IO.Ports;
using System.Windows;
using System.Windows.Controls; // Frame操作用
using Wpf.Ui.Controls;

namespace SerialApp;

public partial class MainWindow : FluentWindow
{
    private SerialPort _serialPort = new SerialPort();

    public MainWindow()
    {
        InitializeComponent();

        // 起動時に最初のページを表示
        RootNavigation.Loaded += (s, e) =>
        {
            NavigateTo("Command");
        };
    }

    // ナビゲーションメニューが切り替わった時の処理
    private void RootNavigation_SelectionChanged(NavigationView sender, RoutedEventArgs e)
    {
        if (sender.SelectedItem is NavigationViewItem item && item.Tag is string tag)
        {
            NavigateTo(tag);
        }
    }

    // ページ切り替えロジック
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

        Title = $"SerialApp - {portName} : {baudRate} bps";
    }

    private void SettingsItem_Click(object sender, RoutedEventArgs e)
    {
        var settingsWindow = new ConnectWindow();
        bool? result = settingsWindow.ShowDialog();

        if (result == true)
        {
            var vm = settingsWindow.ViewModel;
            string portName = vm.ParsePortName(vm.SelectedPort);
            int baudRate = int.Parse(vm.SelectedBaudRate);

            InitializeSerialPort(portName, baudRate);
            // 修正: System.Windows を明示
            System.Windows.MessageBox.Show("接続設定を更新しました。", "情報",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }
}