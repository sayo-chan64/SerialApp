using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO.Ports;

namespace SerialApp.ViewModels;

public partial class ConnectViewModel : ObservableObject
{
    // 画面を閉じるイベント (true=成功, false=キャンセル)
    public event Action<bool>? RequestClose;

    // 警告を表示するためのイベント (タイトル, メッセージ)
    public event Action<string, string>? ShowAlert;

    [ObservableProperty]
    private string _selectedPort = string.Empty;

    [ObservableProperty]
    private string _selectedBaudRate = "19200";

    public ObservableCollection<string> PortList { get; } = new();

    public ObservableCollection<string> BaudRateList { get; } = new()
    {
        "9600", "19200", "38400", "57600", "115200"
    };

    public ConnectViewModel()
    {
        LoadPorts();
    }

    public void LoadPorts()
    {
        PortList.Clear();
        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
        {
            PortList.Add($"({port}) {port}");
        }

        if (PortList.Count > 0)
        {
            SelectedPort = PortList[0];
        }
    }

    [RelayCommand]
    private void Connect()
    {
        // バリデーション
        if (string.IsNullOrEmpty(SelectedPort))
        {
            // 直接MessageBoxを出さず、Viewに依頼する
            ShowAlert?.Invoke("警告", "シリアルポートが選択されていません。");
            return;
        }

        string portName = ParsePortName(SelectedPort);

        if (!int.TryParse(SelectedBaudRate, out int baudRate))
        {
            baudRate = 19200;
        }

        // 接続テスト
        try
        {
            using (var serial = new SerialPort(portName, baudRate))
            {
                serial.Open();
                serial.Close();
            }

            // 成功
            RequestClose?.Invoke(true);
        }
        catch (Exception ex)
        {
            // エラー表示もViewに依頼
            ShowAlert?.Invoke("接続エラー", $"ポートの解放に失敗しました。\n{ex.Message}");
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }

    public string ParsePortName(string displayText)
    {
        if (displayText.Contains('(') && displayText.Contains(')'))
        {
            string temp = displayText.Substring(displayText.IndexOf('(') + 1);
            return temp.Substring(0, temp.IndexOf(')'));
        }
        return displayText;
    }
}