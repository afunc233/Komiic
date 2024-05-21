using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Komiic.Messages;

public class OpenDialogMessage<T>(object dialogContent) : AsyncRequestMessage<T>
{
    public object DialogContent { get; } = dialogContent;
}