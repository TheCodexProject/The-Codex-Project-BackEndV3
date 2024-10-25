using application.appEntry.commands.user;
using application.appEntry.dispatcher;
using application.appEntry.interfaces;
using application.features.user;
using Microsoft.Extensions.DependencyInjection;

namespace application.extensions;

public static class ApplicationExtensions
{
    public static void RegisterCommandHandlers(this IServiceCollection services)
    {
        // * ------------ *
        // * User Handlers *
        // * ------------ *
        services.AddScoped<ICommandHandler<CreateUserCommand>,CreateUserHandler>();
        services.AddScoped<ICommandHandler<GetUserCommand>, GetUserHandler>();
        services.AddScoped<ICommandHandler<GetAllUsersCommand>, GetAllUsersHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand>,UpdateUserHandler>();
        services.AddScoped<ICommandHandler<DeleteUserCommand>, DeleteUserHandler>();
    }


    public static void RegisterCommandDispatcher(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
    }
}