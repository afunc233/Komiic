using System;
using Avalonia;
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

    private readonly IMessenger _messenger;

    public MainView()
    {
        InitializeComponent();
        _messenger = WeakReferenceMessenger.Default;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        _notificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this));
        _messenger.RegisterAll(this);

        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel is not null)
        {
            if (topLevel is Window window)
            {
                CaptionButtons.Attach(window);
            }

            topLevel.BackRequested -= TopLevelOnBackRequested;
            topLevel.BackRequested += TopLevelOnBackRequested;
        }

        // 这里奇怪的操作是解决安卓上 SplitView 先相应 BackRequested 事件
        MainSplitView.PaneOpened -= MainSplitViewOnPaneOpened;
        MainSplitView.PaneOpened += MainSplitViewOnPaneOpened;

        MainSplitView.PaneClosed -= MainSplitViewOnPaneClosed;
        MainSplitView.PaneClosed += MainSplitViewOnPaneClosed;
    }


    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _messenger.UnregisterAll(this);
        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel is not null)
        {
            topLevel.BackRequested -= TopLevelOnBackRequested;
        }

        MainSplitView.PaneOpened -= MainSplitViewOnPaneOpened;
        MainSplitView.PaneClosed -= MainSplitViewOnPaneClosed;

        CaptionButtons.Detach();
    }

    private bool IsInOverlayMode(SplitView splitView)
    {
        return splitView.DisplayMode == SplitViewDisplayMode.CompactOverlay ||
               splitView.DisplayMode == SplitViewDisplayMode.Overlay;
    }

    private void MainSplitViewOnPaneOpened(object? sender, RoutedEventArgs e)
    {
        if (!IsInOverlayMode(MainSplitView))
        {
            return;
        }

        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel is not null)
        {
            topLevel.BackRequested -= TopLevelOnBackRequested;
        }
    }

    private void MainSplitViewOnPaneClosed(object? sender, RoutedEventArgs e)
    {
        if (!IsInOverlayMode(MainSplitView))
        {
            return;
        }

        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel is not null)
        {
            topLevel.BackRequested -= TopLevelOnBackRequested;
            topLevel.BackRequested += TopLevelOnBackRequested;
        }
    }

    private void TopLevelOnBackRequested(object? sender, RoutedEventArgs e)
    {
        var handled = false;
        try
        {
            handled = CloseDialog();

            if (!handled)
            {
                handled = _messenger.Send(new BackRequestedMessage());
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
        finally
        {
            e.Handled = handled;
        }
    }

    private bool CloseDialog()
    {
        return MainDialogHost.TryCloseLastDialog();
    }


    public void Receive(OpenDialogMessage<bool> message)
    {
        message.Reply(Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var result = await MainDialogHost.Show<bool?>(message.DialogContent);
            return result ?? false;
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

    private void InputElement_OnKeyDown(object? sender, KeyEventArgs e)
    {
        e.Handled = true;
    }
}