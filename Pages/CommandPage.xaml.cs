using System.Windows.Controls;
using System.Windows.Media;

namespace SerialApp.Pages;

public partial class CommandPage : Page
{
    // 自動スクロールを有効にするかどうかのフラグ
    // 初期値はTrue（最初は自動スクロールする）
    private bool _autoScrollSend = true;
    private bool _autoScrollReceive = true;

    public CommandPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// リストボックスのスクロール状態が変わったときに呼ばれます
    /// </summary>
    private void OnLogScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        // イベント発生元がListBoxであることを確認
        if (sender is not ListBox listBox) return;

        // 内部のScrollViewerを取得するためにVisualTreeを辿るヘルパーが必要ですが、
        // ListBox自体がScrollViewerのプロパティを間接的に持っているわけではないので、
        // senderは実際にはListBoxではなく、ListBox内部のScrollViewerからバブルアップしてくるか、
        // XAMLでListBoxに直接アタッチした場合はListBoxのTemplate内部のScrollViewerを探す必要があります。
        
        // 今回のXAMLでは ListBox に対して ScrollViewer.ScrollChanged を設定しています。
        // これは添付イベントとして機能します。
        // e.OriginalSource が ScrollViewer になります。
        
        if (e.OriginalSource is not ScrollViewer scrollViewer) return;

        // 判定対象のフラグを決定（送信ログか受信ログか）
        bool isSendLog = (listBox == SendLogList);
        ref bool autoScrollFlag = ref _autoScrollSend; // 参照渡しでフラグを操作
        if (!isSendLog) autoScrollFlag = ref _autoScrollReceive;

        // --- ロジック ---

        // 1. ユーザーが操作してスクロール位置が変わった場合 (ExtentHeightChange == 0)
        if (e.ExtentHeightChange == 0)
        {
            // 一番下までスクロールされているか判定
            // VerticalOffset(現在位置) + ViewportHeight(表示領域) == ExtentHeight(全体高さ)
            // 誤差を許容するために少し余裕を持たせます (< 1.0)
            if (scrollViewer.VerticalOffset + scrollViewer.ViewportHeight >= scrollViewer.ExtentHeight - 1.0)
            {
                // 一番下に来たら自動スクロール再開
                autoScrollFlag = true;
            }
            else
            {
                // 一番下以外にいるなら自動スクロール停止
                // ただし、データ追加による自動スクロール時はここには来ない(ExtentHeightが変わるから)
                autoScrollFlag = false;
            }
        }

        // 2. データが追加されて全体の高さが増えた場合 (ExtentHeightChange > 0)
        if (e.ExtentHeightChange > 0)
        {
            if (autoScrollFlag)
            {
                // 自動スクロール有効なら一番下へ
                scrollViewer.ScrollToBottom();
            }
        }
    }
}


