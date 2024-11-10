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
    ///     Identifies the <seealso cref="FinalText" /> avalonia property.
    /// </summary>
    public static readonly StyledProperty<string?> FinalTextProperty =
        AvaloniaProperty.Register<CountdownBehavior, string?>(nameof(FinalText));

    /// <summary>
    ///     Identifies the <seealso cref="ActionDelay" /> avalonia property.
    /// </summary>
    public static readonly StyledProperty<TimeSpan> ActionDelayProperty =
        AvaloniaProperty.Register<CountdownBehavior, TimeSpan>(nameof(ActionDelay),
            defaultValue: TimeSpan.FromSeconds(1));


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
    ///     Gets or sets the value to be compared with the value of <see cref="CountdownBehavior.FinalText" />. This is an
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

    /// <summary>
    ///     Gets or sets the value to be compared with the value of <see cref="CountdownBehavior.ActionDelay" />. This is an
    ///     avalonia  property.
    /// </summary>
    public TimeSpan ActionDelay
    {
        get => GetValue(ActionDelayProperty);
        set => SetValue(ActionDelayProperty, value);
    }

    static CountdownBehavior()
    {
        InitTimeSpanProperty.Changed.AddClassHandler<CountdownBehavior, TimeSpan?>((countdown, args) =>
        {
            if (args.NewValue.Value is null)
            {
                CountdownBehaviorManager.Instance.RemoveBehavior(countdown);
            }
            else
            {
                CountdownBehaviorManager.Instance.RemoveBehavior(countdown);
                CountdownBehaviorManager.Instance.AddBehavior(countdown);

                countdown.InitTimeSpan = args.NewValue.Value;
                countdown._attachedTime = DateTime.UtcNow;
                if (countdown._textBlock is not null)
                {
                    countdown._textBlock.IsVisible = true;
                }

                countdown.Update();
            }
        });
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
        Dispatcher.UIThread.Invoke(() =>
            {
                _attachedTime ??= DateTime.UtcNow;
                if (_textBlock is null || InitTimeSpan is null)
                {
                    return;
                }

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
                else if (ActionDelay + calcTimeSpan >= TimeSpan.Zero)
                {
                    _textBlock.Text = FinalText ?? "";
                }
                else
                {
                    _textBlock.IsVisible = true;
                    CountdownBehaviorManager.Instance.RemoveBehavior(this);
                    Interaction.ExecuteActions(AssociatedObject, Actions, default);
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
        _timer.Elapsed -= TimerOnElapsed;
        _timer.Elapsed += TimerOnElapsed;
    }

    public static CountdownBehaviorManager Instance { get; } = new();

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        // Update 内 可能调用 Remove 这导致 foreach 出现修改集合的异常
        // 故而这里 执行 ToList  循环本地变量 以解决 可能出现的异常
        var behaviors = _behaviors.ToList();
        foreach (var countdownBehavior in behaviors)
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