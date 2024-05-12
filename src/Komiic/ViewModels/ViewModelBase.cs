using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Komiic.ViewModels;

public class ViewModelBase : ObservableObject, IViewModelBase;

public class RecipientViewModelBase(IMessenger messenger) : ObservableRecipient(messenger), IViewModelBase;
