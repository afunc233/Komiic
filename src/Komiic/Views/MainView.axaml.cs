using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;
using Komiic.Messages;

namespace Komiic.Views;

public partial class MainView : UserControl, IRecipient<OpenDialogMessage<bool>>, IRecipient<CloseDialogMessage<bool>>
{
    /// <summary>
    /// 绑定用
    /// </summary>
    public SplitView MainSplitView => SplitView;

    public MainView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object? sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    public void Receive(OpenDialogMessage<bool> message)
    {
        message.Reply(Dispatcher.UIThread.InvokeAsync(async () =>
        {
            object? result = await DialogHost.Show(message.DialogContent, MainDialogHost);
            return result as bool? ?? false;
        }));
    }

    public void Receive(CloseDialogMessage<bool> message)
    {
        if (MainDialogHost.IsOpen && !(MainDialogHost.CurrentSession?.IsEnded ?? false) &&
            message.DialogContent == MainDialogHost.CurrentSession?.Content)
        {
            MainDialogHost.CurrentSession.Close(message.Result);
        }
    }
}