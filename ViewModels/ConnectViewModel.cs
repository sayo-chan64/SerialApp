using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows; // 明示的に使用しますが、MessageBoxButton等で衝突するため注意

namespace SerialApp.ViewModels;

public partial class ConnectViewModel : ObservableObject
{
    public event Action<bool>? RequestClose;

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
        if (string.IsNullOrEmpty(SelectedPort))
        {
            // 修正: System.Windows を明示
            System.Windows.MessageBox.Show("シリアルポートが選択されていません。", "警告",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            return;
        }

        string portName = ParsePortName(SelectedPort);

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
            // 修正: System.Windows を明示
            System.Windows.MessageBox.Show($"ポートの解放に失敗しました。\n{ex.Message}", "接続エラー",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
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