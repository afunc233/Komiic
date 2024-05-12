using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Komiic.Views;

public partial class MainAppSplashContent : UserControl
{
    private readonly Task[]? _tasks;

    public MainAppSplashContent() : this(Task.CompletedTask)
    {
    }

    public MainAppSplashContent(params Task[]? tasks)
    {
        InitializeComponent();
        _tasks = tasks;
    }

    public async Task RunTasks(CancellationToken cancellationToken)
    {
        if (_tasks == null)
        {
            return;
        }

        await Task.WhenAll(_tasks);
    }

    public int MinimumShowTime => 1 * 1000;
}