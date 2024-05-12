using System.Threading.Tasks;

namespace Komiic.Contracts;

public interface IActivationHandler
{
    
    int Order { get; }


    Task HandleAsync();
}
