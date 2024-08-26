using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Metadata;

namespace Komiic.Controls;

[TemplatePart(PART_Border, typeof(Border))]
[PseudoClasses(pcCloseOnClickAway)]
public class Dialog : TemplatedControl
{
    // ReSharper disable once InconsistentNaming
    private const string PART_Border = nameof(PART_Border);

    // ReSharper disable once InconsistentNaming
    private const string pcCloseOnClickAway = ":closeOnClickAway";

    public static readonly StyledProperty<object> ContentProperty =
        AvaloniaProperty.Register<Dialog, object>(nameof(Content));

    public static readonly StyledProperty<bool> CloseOnClickAwayProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(CloseOnClickAway));

    private Border? _border;
    private TaskCompletionSource<object?>? _taskCompletionSource;

    static Dialog()
    {
    }

    internal Dialog()
    {
    }

    [Content]
    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public bool CloseOnClickAway
    {
        get => GetValue(CloseOnClickAwayProperty);
        set => SetValue(CloseOnClickAwayProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (e.NameScope.Find<Border>(PART_Border) is { } border)
        {
            _border = border;
            PointerPressed += CloseOnClickAwayHandler;
        }

        UpdatePseudoClasses();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        UpdatePseudoClasses();
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(pcCloseOnClickAway, CloseOnClickAway);
    }


    private void CloseOnClickAwayHandler(object? sender, PointerPressedEventArgs e)
    {
        if (CloseOnClickAway && e.Source is Border border && border == _border)
        {
            Close(default);
        }
    }

    public async Task<object?> Show()
    {
        _taskCompletionSource = new TaskCompletionSource<object?>();

        return await _taskCompletionSource.Task;
    }

    public void Close(object? messageResult)
    {
        _taskCompletionSource?.TrySetResult(messageResult);
    }
}