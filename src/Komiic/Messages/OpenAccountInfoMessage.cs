using Komiic.Core.Contracts.Model;

namespace Komiic.Messages;

public record OpenAccountInfoMessage(Account? AccountData, ImageLimit ImageLimit);