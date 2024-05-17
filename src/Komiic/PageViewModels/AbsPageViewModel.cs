using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Komiic.ViewModels;
using Microsoft.Extensions.Logging;

namespace Komiic.PageViewModels;

public abstract partial class AbsPageViewModel(ILogger logger) : ViewModelBase, IPageViewModel
{
    private ILogger Logger => logger;

    public abstract NavBarType NavBarType { get; }
    public abstract string Title { get; }

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private bool _isDataError;

    [RelayCommand]
    public virtual Task LoadData()
    {
        return NavigatedTo();
    }
    
    public async Task NavigatedTo()
    {
        await SafeLoadData(async ()=>await OnNavigatedTo());
    }

    protected async Task SafeLoadData(Func<Task> execute)
    {
        try
        {
            IsDataError = false;
            IsLoading = true;
            await execute.Invoke();
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

    public async Task NavigatedFrom()
    {
        await OnNavigatedFrom();
    }

    protected virtual Task OnNavigatedFrom()
    {
        return Task.CompletedTask;
    }

}