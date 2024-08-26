namespace Komiic.Messages;

public class CloseDialogMessage<T>(object dialogContent, T result)
{
    public object DialogContent { get; } = dialogContent;

    public T Result { get; } = result;
}