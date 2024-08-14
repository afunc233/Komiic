using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Controls;
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
#if DEBUG                                                                                      
        RendererDiagnostics.DebugOverlays = Avalonia.Rendering.RendererDebugOverlays.None |
                                            Avalonia.Rendering.RendererDebugOverlays.Fps |
                                            Avalonia.Rendering.RendererDebugOverlays.LayoutTimeGraph |
                                            Avalonia.Rendering.RendererDebugOverlays.RenderTimeGraph;
#endif       
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
        _splashCts = new();
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
                new()
                {
                    Cue = new(0d),
                    Setters =
                    {
                        new Setter(OpacityProperty, 1d)
                    }
                },
                new()
                {
                    Cue = new(1d),
                    Setters =
                    {
                        new Setter(OpacityProperty, 0d),
                    },
                    KeySpline = new(0, 0, 0, 1)
                }
            }
        };

        var aniCP = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(167),
            Children =
            {
                new()
                {
                    Cue = new(0d),
                    Setters =
                    {
                        new Setter(OpacityProperty, 0d)
                    }
                },
                new()
                {
                    Cue = new(1d),
                    Setters =
                    {
                        new Setter(OpacityProperty, 1d),
                    },
                    KeySpline = new(0, 0, 0, 1)
                }
            }
        };
        MainView.IsVisible = true;
        await Task.WhenAll(aniSplash.RunAsync(SplashHost), aniCP.RunAsync(MainView));

        PseudoClasses.Set(":splashOpen", false);
    }


    protected override void OnClosed(EventArgs e)
    {
        _splashCts?.Cancel();
        _splashCts?.Dispose();
        _splashCts = null;

        base.OnClosed(e);
    }

}