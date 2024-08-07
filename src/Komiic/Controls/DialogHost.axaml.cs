using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Komiic.Controls;

[TemplatePart(PART_DialogHost, typeof(Panel))]
public class DialogHost : TemplatedControl
{
    // ReSharper disable once InconsistentNaming
    private const string PART_DialogHost = nameof(PART_DialogHost);

    public static readonly StyledProperty<bool> CloseOnClickAwayProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(CloseOnClickAway));

    public bool CloseOnClickAway
    {
        get => GetValue(CloseOnClickAwayProperty);
        set => SetValue(CloseOnClickAwayProperty, value);
    }

    private Panel? _hostPanel;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _hostPanel = e.NameScope.Find<Panel>(PART_DialogHost);
        this.IsHitTestVisible = false;
    }

    public async Task<T?> Show<T>(object messageDialogContent)
    {
        if (_hostPanel != null)
        {
            var lastChildren = _hostPanel.Children.LastOrDefault();

            if (lastChildren != null)
            {
                lastChildren.IsEnabled = false;
            }

            var dialog = new Dialog()
            {
                Content = messageDialogContent,
                CloseOnClickAway = CloseOnClickAway,
            };

            _hostPanel.Children.Add(dialog);

            this.IsHitTestVisible = _hostPanel.Children.Any();

            var result = await dialog.Show();

            CloseInner(messageDialogContent, result, true);
            if (result != null)
            {
                try
                {
                    return (T)result;
                }
                catch
                {
                    // ignored
                }
            }
        }

        return default;
    }

    public void Close<T>(object messageDialogContent, T messageResult)
    {
        CloseInner(messageDialogContent, messageResult);
    }

    private void CloseInner<T>(object messageDialogContent, T messageResult, bool innerClose = false)
    {
        if (_hostPanel != null)
        {
            var dialog = _hostPanel.Children.OfType<Dialog>().FirstOrDefault(it => it.Content == messageDialogContent);

            if (dialog != null)
            {
                _hostPanel.Children.Remove(dialog);
                if (!innerClose)
                {
                    dialog.Close(messageResult);
                }
            }

            var lastChildren = _hostPanel.Children.LastOrDefault();

            if (lastChildren != null)
            {
                lastChildren.IsEnabled = true;
            }

            this.IsHitTestVisible = _hostPanel.Children.Any();
        }
    }
}