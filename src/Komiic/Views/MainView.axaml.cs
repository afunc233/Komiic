using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Messages;

namespace Komiic.Views;

public partial class MainView : UserControl, IRecipient<OpenDialogMessage<bool>>, IRecipient<CloseDialogMessage<bool>>,
    IRecipient<OpenNotificationMessage>
{
    private IManagedNotificationManager? _notificationManager;

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

        CaptionButtons.Detach();
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        _notificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this));
        WeakReferenceMessenger.Default.RegisterAll(this);
        if (TopLevel.GetTopLevel(this) is Window window)
        {
            CaptionButtons.Attach(window);
        }
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


    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // TODO 在 ubuntu 下 BeginMoveDrag 导致 DoubleTapped 触发有些问题 考虑自己移动 而不是 调用 BeginMoveDrag
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            if (TopLevel.GetTopLevel(this) is Window window)
            {
                window.BeginMoveDrag(e);
            }
        }
    }

    private void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window window)
        {
            switch (window.WindowState)
            {
                case WindowState.Normal:
                    window.WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    window.WindowState = WindowState.Normal;
                    break;
                case WindowState.FullScreen:
                    window.WindowState = WindowState.Normal;
                    break;
                case WindowState.Minimized:
                default:
                    break;
            }
        }
    }
}