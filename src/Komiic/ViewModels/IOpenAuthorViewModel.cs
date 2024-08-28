using CommunityToolkit.Mvvm.Input;
using Komiic.Core.Contracts.Models;

namespace Komiic.ViewModels;

public interface IOpenAuthorViewModel
{
    IAsyncRelayCommand<Author> OpenAuthorCommand { get; }
}