using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace Komiic.Views;

public partial class MainWindow : Window
{
    public MainAppSplashContent? SplashScreen { get; init; }

    private CancellationTokenSource? _splashCts;

    /// <summary>
    /// TODO 在 ubuntu 下  resize 没了？
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        // RendererDiagnostics.DebugOverlays = RendererDebugOverlays.None | RendererDebugOverlays.Fps |
        //                                     RendererDebugOverlays.LayoutTimeGraph |
        //                                     RendererDebugOverlays.RenderTimeGraph;
    }

    protected override async void OnOpened(EventArgs e)
    {
        if (SplashScreen == null)
        {
            return;
        }

        SplashHost.Content = SplashScreen;
        SplashHost.IsVisible = true;

        PseudoClasses.Set(":splashOpen", true);
        var time = DateTime.Now;
        _splashCts = new CancellationTokenSource();
        await SplashScreen.RunTasks(_splashCts.Token);
        _splashCts?.Dispose();
        _splashCts = null;

        var delta = DateTime.Now - time;
        if (delta.TotalMilliseconds < SplashScreen.MinimumShowTime)
        {
            await Task.Delay(Math.Max(1, SplashScreen.MinimumShowTime - (int)delta.TotalMilliseconds));
        }

        await LoadApp();

        SplashHost.Content = null;
        SplashHost.IsVisible = false;

        base.OnOpened(e);
    }

    private async Task LoadApp()
    {
        // Taking this out, it's causing flickering of the content after the splash fade animation
        // Another regression in the animation system for 11.0...
        //using var disp = cp.SetValue(OpacityProperty, 0d, Avalonia.Data.BindingPriority.Animation);

        var aniSplash = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(250),
            FillMode = FillMode.Forward,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0d),
                    Setters =
                    {
                        new Setter(OpacityProperty, 1d)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1d),
                    Setters =
                    {
                        new Setter(OpacityProperty, 0d),
                    },
                    KeySpline = new KeySpline(0, 0, 0, 1)
                }
            }
        };

        var aniCP = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(167),
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0d),
                    Setters =
                    {
                        new Setter(OpacityProperty, 0d)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1d),
                    Setters =
                    {
                        new Setter(OpacityProperty, 1d),
                    },
                    KeySpline = new KeySpline(0, 0, 0, 1)
                }
            }
        };
        MainView.IsVisible = true;
        await Task.WhenAll(aniSplash.RunAsync(SplashHost), aniCP.RunAsync(MainView));

        PseudoClasses.Set(":splashOpen", false);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        CaptionButtons.Attach(this);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        CaptionButtons.Detach();
    }

    protected override void OnClosed(EventArgs e)
    {
        _splashCts?.Cancel();
        _splashCts?.Dispose();
        _splashCts = null;

        base.OnClosed(e);
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // TODO 在 ubuntu 下 BeginMoveDrag 导致 DoubleTapped 触发有些问题 考虑自己移动 而不是 调用 BeginMoveDrag
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }

    private void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        switch (WindowState)
        {
            case WindowState.Normal:
                WindowState = WindowState.Maximized;
                break;
            case WindowState.Maximized:
                WindowState = WindowState.Normal;
                break;
            case WindowState.FullScreen:
                WindowState = WindowState.Normal;
                break;
            case WindowState.Minimized:
            default:
                break;
        }
    }
}