using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public abstract partial class AbsPageViewModel(ILogger logger) : ViewModelBase, IPageViewModel
{
    [ObservableProperty] private bool _isDataError;

    [ObservableProperty] private bool _isLoading;
    private ILogger Logger => logger;

    public abstract NavBarType NavBarType { get; }
    public abstract string Title { get; }

    private CancellationTokenSource? _cancellationTokenSource;
    protected CancellationToken CancellationToken => _cancellationTokenSource?.Token ?? CancellationToken.None;

    [RelayCommand]
    public virtual Task LoadData()
    {
        return NavigatedTo();
    }

    public async Task NavigatedTo()
    {
        _cancellationTokenSource ??= new CancellationTokenSource();
        await SafeLoadData(async () => await OnNavigatedTo());
    }

    public async Task NavigatedFrom()
    {
        await OnNavigatedFrom();
        if (_cancellationTokenSource is not null)
        {
            await _cancellationTokenSource.CancelAsync();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }

    protected async Task SafeLoadData(Func<Task> execute)
    {
        try
        {
            IsDataError = false;
            IsLoading = true;
            await execute.Invoke();
        }
        catch (TaskCanceledException e)
        {
            IsDataError = true;
            Logger.LogTrace(e, "get data cancel! {e.Message} {e.StackTrace}", e.Message, e.StackTrace);
        }
        catch (OperationCanceledException e)
        {
            IsDataError = true;
            Logger.LogTrace(e, "get data cancel! {e.Message} {e.StackTrace}", e.Message, e.StackTrace);
        }
        catch (Exception e)
        {
            IsDataError = true;
            Logger.LogError(e, "get data error! {e.Message} {e.StackTrace}", e.Message, e.StackTrace);
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected virtual Task OnNavigatedTo()
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnNavigatedFrom()
    {
        return Task.CompletedTask;
    }
}