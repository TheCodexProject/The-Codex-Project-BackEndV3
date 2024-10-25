using OperationResult;

namespace application.appEntry.interfaces;

public interface ICommandHandler<in TCommand>
{
    Task<Result> HandleAsync(TCommand command);
}