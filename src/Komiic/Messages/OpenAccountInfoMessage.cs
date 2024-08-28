using Komiic.Core.Contracts.Models;

namespace Komiic.Messages;

public record OpenAccountInfoMessage(Account? AccountData, ImageLimit ImageLimit);