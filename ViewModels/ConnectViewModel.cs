using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SerialApp.Models;   // 追加
using SerialApp.Services; // 追加
using System.Collections.ObjectModel;
using System.IO.Ports;
using Wpf.Ui.Controls;
using System.Threading.Tasks;

namespace SerialApp.ViewModels;

public partial class ConnectViewModel : ObservableObject
{
    public event Action<bool>? RequestClose;
    public event Action<string, string>? ShowAlert;

    // 選択されたポート情報そのものを保持するように変更
    [ObservableProperty]
    private SerialPortInfo? _selectedPortInfo;

    [ObservableProperty]
    private string _selectedBaudRate = "19200";

    // 文字列ではなくクラスのリストに変更
    public ObservableCollection<SerialPortInfo> PortList { get; } = [];

    public ObservableCollection<string> BaudRateList { get; } =
    [
        "9600", "19200", "38400", "57600", "115200"
    ];

    public ConnectViewModel()
    {
        LoadPorts();
    }

    public void LoadPorts()
    {
        PortList.Clear();

        // 画像のプログラム（Services.SerialPortGet）を使用して取得
        var ports = SerialPortGet.AvailablePorts();

        foreach (var port in ports)
        {
            PortList.Add(port);
        }

        if (PortList.Count > 0)
        {
            SelectedPortInfo = PortList[0];
        }
    }

    [RelayCommand]
    private async Task Connect()
    {
        // Infoオブジェクトがnullでないかチェック
        if (SelectedPortInfo == null)
        {
            await DialogService.ShowAsync("警告", "シリアルポートが選択されていません。", SymbolRegular.Warning24);
            return;
        }

        // COM番号を取り出す (例: "COM3")
        string portName = SelectedPortInfo.PortName;

        if (!int.TryParse(SelectedBaudRate, out int baudRate))
        {
            baudRate = 19200;
        }

        try
        {
            using (var serial = new SerialPort(portName, baudRate))
            {
                serial.Open();
                serial.Close();
            }

            RequestClose?.Invoke(true);
        }
        catch (Exception ex)
        {
            await DialogService.ShowAsync("接続エラー", $"ポートに接続できませんでした。\n{ex.Message}", SymbolRegular.ErrorCircle24);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}