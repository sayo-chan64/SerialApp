using System.Windows;
using SerialApp.ViewModels;

namespace SerialApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 【重要】設定画面が閉じただけでアプリが終了しないように、
        // シャットダウンモードを「明示的に終了するまで閉じない」設定に変更します。
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        var connectWindow = new ConnectWindow();
        bool? result = connectWindow.ShowDialog();

        if (result == true)
        {
            var vm = connectWindow.ViewModel;

            // ViewModelのnullチェックをしつつ安全にポート名を取得
            string portName = vm.SelectedPortInfo?.PortName ?? "COM1";
            int baudRate = int.Parse(vm.SelectedBaudRate);

            var mainWindow = new MainWindow();

            // アプリケーションのメインウィンドウとして登録
            MainWindow = mainWindow;

            // 初期化処理
            mainWindow.InitializeSerialPort(portName, baudRate);
            mainWindow.Show();

            // メインウィンドウが表示されたので、
            // 「メインウィンドウが閉じたらアプリを終了する」という設定に戻します。
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
        else
        {
            // キャンセルされた場合は手動でアプリを終了させます
            Shutdown();
        }
    }
}