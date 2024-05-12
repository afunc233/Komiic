using CommunityToolkit.Mvvm.Input;
using Komiic.Core.Contracts.Model;

namespace Komiic.ViewModels;

public interface IOpenAuthorViewModel
{
    IAsyncRelayCommand<Author> OpenAuthorCommand { get; }
}