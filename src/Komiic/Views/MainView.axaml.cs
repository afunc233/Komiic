using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Messages;

namespace Komiic.Views;

public partial class MainView : UserControl, IRecipient<OpenDialogMessage<bool>>, IRecipient<CloseDialogMessage<bool>>,
    IRecipient<OpenNotificationMessage>
{
    private IManagedNotificationManager? _notificationManager;

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
        _notificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this));
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    public void Receive(OpenDialogMessage<bool> message)
    {
        message.Reply(Dispatcher.UIThread.InvokeAsync(async () =>
        {
            bool? result = await MainDialogHost.Show<bool>(message.DialogContent);
            return result as bool? ?? false;
        }));
    }

    public void Receive(CloseDialogMessage<bool> message)
    {
        MainDialogHost.Close(message.DialogContent, message.Result);
    }

    public void Receive(OpenNotificationMessage message)
    {
        _notificationManager?.Show(message.Content);
    }
}