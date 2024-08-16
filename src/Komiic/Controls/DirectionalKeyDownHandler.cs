using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Komiic.Controls;

public static class DirectionalKeyDownHandler
{
    public static readonly AttachedProperty<bool> HandledProperty
        = AvaloniaProperty.RegisterAttached<InputElement, bool>("Handled", typeof(DirectionalKeyDownHandler));

    public static void SetHandled(AvaloniaObject target, bool value) => target.SetValue(HandledProperty, value);

    public static bool GetHandled(AvaloniaObject target) => target.GetValue(HandledProperty);

    static DirectionalKeyDownHandler()
    {
        HandledProperty.Changed.AddClassHandler<InputElement>((inputElement, args) =>
        {
            var handled = GetHandled(inputElement);
            if (handled)
            {
                inputElement.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
            }
            else
            {
                inputElement.RemoveHandler(InputElement.KeyDownEvent, OnKeyDown);
            }
        });
    }

    private static void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key.ToNavigationDirection() is not null)
        {
            e.Handled = true;
        }
    }
}