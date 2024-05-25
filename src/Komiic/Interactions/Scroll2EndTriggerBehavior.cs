using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Mvvm.Input;

namespace Komiic.Interactions;

public class Scroll2EndTriggerBehavior : Behavior
{
    /// <summary>
    /// Identifies the <seealso cref="Distance2End"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<double> Distance2EndProperty =
        AvaloniaProperty.Register<Scroll2EndTriggerBehavior, double>(nameof(Distance2End), 200d);

    /// <summary>
    /// Gets or sets the bound object that  <see cref="Scroll2EndTriggerBehavior"/> will listen to. This is an avalonia property.
    /// </summary>
    public double Distance2End
    {
        get => GetValue(Distance2EndProperty);
        set => SetValue(Distance2EndProperty, value);
    }

    /// <summary>
    /// Identifies the <seealso cref="LoadMoreDataCmd"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<ICommand?> LoadMoreDataCmdProperty =
        AvaloniaProperty.Register<Scroll2EndTriggerBehavior, ICommand?>(nameof(LoadMoreDataCmd));

    /// <summary>
    /// Gets or sets the bound object that  <see cref="Scroll2EndTriggerBehavior"/> will listen to. This is an avalonia property.
    /// </summary>
    public ICommand? LoadMoreDataCmd
    {
        get => GetValue(LoadMoreDataCmdProperty);
        set => SetValue(LoadMoreDataCmdProperty, value);
    }

    private bool _attached;

    protected override void OnAttachedToVisualTree()
    {
        base.OnAttachedToVisualTree();
        if (AssociatedObject is not Interactive control) return;
        _attached = true;
        control.AddHandler(ScrollViewer.ScrollChangedEvent, ScrollViewerOnScrollChanged);
    }

    protected override void OnDetachedFromVisualTree()
    {
        base.OnDetachedFromVisualTree();
        if (AssociatedObject is not Interactive control) return;
        _attached = false;

        control.RemoveHandler(ScrollViewer.ScrollChangedEvent, ScrollViewerOnScrollChanged);
    }

    private ScrollViewer? GetScrollViewer(object? sender)
    {
        if (sender is ScrollViewer scrollViewer)
        {
            return scrollViewer;
        }

        if (AssociatedObject is TemplatedControl templatedControl)
        {
            return templatedControl.GetTemplateChildren().OfType<ScrollViewer>().FirstOrDefault();
        }

        return default;
    }

    private async void ScrollViewerOnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        var scrollViewer = GetScrollViewer(sender);

        if (scrollViewer is null)
        {
            return;
        }
        
        var extent = scrollViewer.Extent;
        var offset = scrollViewer.Offset;
        if (extent.Height - scrollViewer.Viewport.Height < 0.5d)
        {
            // Layout 那一次，调用 Command
            await CallCommand(scrollViewer, e);
        }
        else if (extent.Height - (offset.Y + scrollViewer.Viewport.Height) < Distance2End)
        {
            await CallCommand(scrollViewer, e);
        }
    }

    private async Task CallCommand(ScrollViewer scrollViewer, object e)
    {
        while (true)
        {
            if (!_attached)
            {
                break;
            }

            if (LoadMoreDataCmd is IAsyncRelayCommand asyncRelayCommand)
            {
                if (asyncRelayCommand.IsRunning)
                {
                    return;
                }

                if (asyncRelayCommand.CanExecute(e))
                {
                    await asyncRelayCommand.ExecuteAsync(e);
                    var extent = scrollViewer.Extent;
                    if (extent.Height - scrollViewer.Viewport.Height < 0.5d)
                    {
                        continue;
                    }
                }
            }
            else
            {
                if (LoadMoreDataCmd?.CanExecute(e) ?? false)
                {
                    LoadMoreDataCmd?.Execute(e);
                }
            }
            break;
        }
    }
}