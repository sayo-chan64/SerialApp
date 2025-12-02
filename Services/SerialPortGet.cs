using System.Management; // NuGet: System.Management が必要
using SerialApp.Models;

namespace SerialApp.Services;

public static class SerialPortGet
{
    /// <summary>
    /// 利用可能なCOMポートの詳細情報を取得します。
    /// </summary>
    public static List<SerialPortInfo> AvailablePorts()
    {
        List<SerialPortInfo> portList = [];

        try
        {
            // WMIを使ってデバイス一覧からCOMポートを含むものを検索
            // Win32_PnPEntity はプラグアンドプレイデバイスを表します
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%(COM%)'");

            foreach (var item in searcher.Get())
            {
                // デバイス名を取得 (例: "USB Serial Port (COM3)")
                string? name = item["Name"]?.ToString();

                if (name != null && name.Contains("(COM"))
                {
                    // "COMx" の部分を抽出するロジック
                    int start = name.LastIndexOf("(COM");
                    int end = name.LastIndexOf(')');

                    if (start >= 0 && end > start)
                    {
                        // カッコの中身 "COM3" を取り出す
                        // start + 1 で "(" の次から開始
                        // end - start - 1 で ")" の手前まで
                        string comPort = name.Substring(start + 1, end - start - 1);

                        // 表示用には元の詳細な名前を使うと分かりやすい
                        // 例: PortName="COM3", DisplayName="USB Serial Port (COM3)"
                        portList.Add(new SerialPortInfo(comPort, name));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ポート検出エラー: {ex.Message}");
        }

        // ポート名順にソートして返す
        return [.. portList.OrderBy(p => p.PortName)];
    }
}