using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Komiic.Core.Contracts.Api;
using Komiic.Core.Contracts.Model;
using Komiic.Messages;

namespace Komiic.ViewModels;

public partial class HeaderViewModel(IMessenger messenger, IKomiicAccountApi komiicQueryApi)
    : RecipientViewModelBase(messenger), IRecipient<LoadMangeImageDataMessage>
{
    [ObservableProperty] private ImageLimit _imageLimit = new()
    {
        limit = 300,
        usage = 0,
    };

    public async Task LoadData()
    {
        try
        {
            var getImageLimitData = await komiicQueryApi.GetImageLimit(QueryDataEnum.GetImageLimit.GetQueryData());
            if (getImageLimitData is { Data: not null })
            {
                ImageLimit = getImageLimitData.Data.ImageLimit;
            }
        }
        catch
        {
            // ignored
        }
    }

    private Task? _loadDataTask;

    public async void Receive(LoadMangeImageDataMessage message)
    {
        _loadDataTask ??= LoadData().ContinueWith(_ => { _loadDataTask = null; });
        if (_loadDataTask != null)
        {
            await _loadDataTask;
        }
    }
}