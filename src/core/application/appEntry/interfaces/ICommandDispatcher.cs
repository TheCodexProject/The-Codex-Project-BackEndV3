using OperationResult;

namespace application.appEntry.interfaces;

public interface ICommandDispatcher
{
    Task<Result> DispatchAsync<TCommand>(TCommand command);
}