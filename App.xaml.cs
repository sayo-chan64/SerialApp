using System.Windows;
using SerialApp.ViewModels; // ViewModelの名前空間を追加

namespace SerialApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var connectWindow = new ConnectWindow();

        bool? result = connectWindow.ShowDialog();

        if (result == true)
        {
            // ViewModelから確定したデータを取得
            var vm = connectWindow.ViewModel;

            // 画面表示用の文字から純粋なポート名を取り出す
            string portName = vm.ParsePortName(vm.SelectedPort);
            int baudRate = int.Parse(vm.SelectedBaudRate);

            var mainWindow = new MainWindow();

            // ここも本来はMainViewModelを作るべきですが、
            // 段階的に進めるため一旦メソッド渡しのままとします
            mainWindow.InitializeSerialPort(portName, baudRate);
            mainWindow.Show();
        }
        else
        {
            this.Shutdown();
        }
    }
}