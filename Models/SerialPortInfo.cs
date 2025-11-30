namespace SerialApp.Models;

/// <summary>
/// シリアルポートの情報を保持するクラス
/// </summary>
public class SerialPortInfo(string portName, string displayName)
{
    public string PortName { get; } = portName;
    public string DisplayName { get; } = displayName;
}