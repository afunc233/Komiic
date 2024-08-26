using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Komiic.Interactions;

/// <summary>
///     A behavior that listens for a <see cref="ToggleButton.IsChecked" /> property on its source and executes its actions
///     when that event is fired.
/// </summary>
public class RadioButtonCheckedBehavior : Trigger
{
    /// <summary>
    ///     Identifies the <seealso cref="RoutingStrategies" /> avalonia property.
    /// </summary>
    public static readonly StyledProperty<RoutingStrategies> RoutingStrategiesProperty =
        AvaloniaProperty.Register<RadioButtonCheckedBehavior, RoutingStrategies>(nameof(RoutingStrategies),
            RoutingStrategies.Direct | RoutingStrategies.Bubble);


    /// <summary>
    ///     Identifies the <seealso cref="Value" /> avalonia property.
    /// </summary>
    public static readonly StyledProperty<bool?> ValueProperty =
        AvaloniaProperty.Register<RadioButtonCheckedBehavior, bool?>(nameof(Value));

    /// <summary>
    ///     Gets or sets the routing event <see cref="RoutingStrategies" />. This is an avalonia property.
    /// </summary>
    public RoutingStrategies RoutingStrategies
    {
        get => GetValue(RoutingStrategiesProperty);
        set => SetValue(RoutingStrategiesProperty, value);
    }

    /// <summary>
    ///     Gets or sets the value to be compared with the value of <see cref="ToggleButton.IsChecked" />. This is an avalonia
    ///     property.
    /// </summary>
    public bool? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is not Interactive interactive)
        {
            return;
        }

        interactive.AddHandler(ToggleButton.IsCheckedChangedEvent, RadioButtonOnIsCheckedChanged, RoutingStrategies);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject is Interactive interactive)
        {
            interactive.RemoveHandler(ToggleButton.IsCheckedChangedEvent, RadioButtonOnIsCheckedChanged);
        }
    }

    private void RadioButtonOnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (e.Handled)
        {
            return;
        }

        if (e.RoutedEvent != ToggleButton.IsCheckedChangedEvent)
        {
            return;
        }

        if (sender is not RadioButton radioButton)
        {
            return;
        }

        if (radioButton.IsChecked == Value)
        {
            Interaction.ExecuteActions(AssociatedObject, Actions, e);
        }
    }
}