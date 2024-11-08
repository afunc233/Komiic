using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;

namespace Komiic.Interactions;

public class CountdownBehavior : Trigger
{
    /// <summary>
    ///     Identifies the <seealso cref="InitTimeSpan" /> avalonia property.
    /// </summary>
    public static readonly StyledProperty<TimeSpan?> InitTimeSpanProperty =
        AvaloniaProperty.Register<CountdownBehavior, TimeSpan?>(nameof(InitTimeSpan));


    /// <summary>
    ///     Identifies the <seealso cref="TimeFormat" /> avalonia property.
    /// </summary>
    public static readonly StyledProperty<string> TimeFormatProperty =
        AvaloniaProperty.Register<CountdownBehavior, string>(nameof(TimeFormat), @"hh\:mm\:ss");

    /// <summary>
    ///     Identifies the <seealso cref="TextFormat" /> avalonia property.
    /// </summary>
    public static readonly StyledProperty<string> TextFormatProperty =
        AvaloniaProperty.Register<CountdownBehavior, string>(nameof(TextFormat), @"{0}");

    /// <summary>
    ///     Identifies the <seealso cref="TextFormat" /> avalonia property.
    /// </summary>
    public static readonly StyledProperty<string?> FinalTextProperty =
        AvaloniaProperty.Register<CountdownBehavior, string?>(nameof(FinalText));


    private DateTime? _attachedTime;

    private TextBlock? _textBlock;


    /// <summary>
    ///     Gets or sets the value to be compared with the value of <see cref="CountdownBehavior.InitTimeSpan" />. This is an
    ///     avalonia  property.
    /// </summary>
    public TimeSpan? InitTimeSpan
    {
        get => GetValue(InitTimeSpanProperty);
        set => SetValue(InitTimeSpanProperty, value);
    }

    /// <summary>
    ///     Gets or sets the value to be compared with the value of <see cref="CountdownBehavior.TimeFormat" />. This is an
    ///     avalonia  property.
    /// </summary>
    public string TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    /// <summary>
    ///     Gets or sets the value to be compared with the value of <see cref="CountdownBehavior.TextFormat" />. This is an
    ///     avalonia  property.
    /// </summary>
    public string? FinalText
    {
        get => GetValue(FinalTextProperty);
        set => SetValue(FinalTextProperty, value);
    }

    /// <summary>
    ///     Gets or sets the value to be compared with the value of <see cref="CountdownBehavior.TextFormat" />. This is an
    ///     avalonia  property.
    /// </summary>
    public string TextFormat
    {
        get => GetValue(TextFormatProperty);
        set => SetValue(TextFormatProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is not TextBlock textBlock)
        {
            return;
        }

        _attachedTime = DateTime.UtcNow;
        _textBlock = textBlock;
        CountdownBehaviorManager.Instance.AddBehavior(this);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        CountdownBehaviorManager.Instance.RemoveBehavior(this);
        _textBlock = null;
    }

    public void Update()
    {
        _attachedTime ??= DateTime.UtcNow;

        if (_textBlock is null)
        {
            return;
        }

        Dispatcher.UIThread.Invoke(() =>
            {
                var calcTimeSpan = InitTimeSpan - (DateTime.UtcNow - _attachedTime.Value);

                if (calcTimeSpan is { Ticks: > 0 })
                {
                    string calcTimeSpanStr;
                    try
                    {
                        calcTimeSpanStr = calcTimeSpan.Value.ToString(TimeFormat);
                    }
                    catch (Exception)
                    {
                        calcTimeSpanStr =
                            calcTimeSpan.Value.ToString(TimeFormatProperty.GetDefaultValue(typeof(string)));
                    }

                    _textBlock.Text = string.Format(TextFormat, calcTimeSpanStr);
                }
                else
                {
                    _textBlock.Text = FinalText ?? "";
                }
            },
            DispatcherPriority.Background);
    }
}

internal class CountdownBehaviorManager
{
    private readonly List<CountdownBehavior> _behaviors = new();
    private readonly Timer _timer;

    private CountdownBehaviorManager()
    {
        _timer = new Timer(TimeSpan.FromSeconds(1));
        _timer.Elapsed += TimerOnElapsed;
    }

    public static CountdownBehaviorManager Instance { get; } = new();

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        foreach (var countdownBehavior in _behaviors)
        {
            countdownBehavior.Update();
        }
    }

    internal void AddBehavior(CountdownBehavior behavior)
    {
        _behaviors.Add(behavior);
        if (_timer is not { Enabled: true })
        {
            _timer.Start();
        }
    }

    internal void RemoveBehavior(CountdownBehavior behavior)
    {
        _behaviors.Remove(behavior);
        if (!_behaviors.Any())
        {
            _timer.Stop();
        }
    }
}