using Avalonia.Controls;

namespace Komiic.Views;

public partial class MainView : UserControl
{
    /// <summary>
    /// 绑定用
    /// </summary>
    public SplitView MainSplitView => SplitView;

    public MainView()
    {
        InitializeComponent();
    }
}