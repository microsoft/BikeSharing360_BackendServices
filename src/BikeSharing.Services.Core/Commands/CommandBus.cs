using Autofac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Security;

namespace BikeSharing.Services.Core.Commands
{
    public abstract class CommandBus<TDb> : ICommandBus
        where TDb : DbContext
    {
        private const string SCOPE_NAME = "CommandHandlers";
        private readonly ILifetimeScope _scope;
        private CommandHandlerResult _innerOkResult;

        public CommandBus(ILifetimeScope scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }
            _scope = scope;
        }

        public CommandResult GetDelayedCommandResult()
        {
            if (_innerOkResult == null || !_innerOkResult.HasDelayedContent)
            {
                return null;
            }

            _innerOkResult.ResolveAndUpdateDelayedContent();
            return GetCommandResultFromCommandHandlerResult(_innerOkResult);


        }

        public CommandResult Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            try
            {
                var innerHandler = (ICommandHandler<TCommand>)
                    _scope.ResolveOptional(typeof(ICommandHandler<TCommand>));
                
                if (innerHandler != null)
                {
                    command.Validate();

                    if (command.IsValid)
                    {
                        _innerOkResult = innerHandler.Handle(command);
                        var commandResult = GetCommandResultFromCommandHandlerResult(_innerOkResult);
                        return commandResult;
                    }
                    else
                    {
                        return CommandResult.FromValidationErrors(command.ValidationErrorMessges);
                        
                    }
                }

                return CommandResult.NonExistentCommand(typeof(TCommand).Name);
            }
            catch (SecurityException ex)
            {
                return new CommandResult(ex, HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                return new CommandResult(ex, HttpStatusCode.InternalServerError);
            }
        }

        private CommandResult GetCommandResultFromCommandHandlerResult(CommandHandlerResult result)
        {
            if (result == CommandHandlerResult.Ok)
            {
                return CommandResult.Ok;
            }

            var content = result.Content;
            return  new CommandResult(result.StatusCode, content);
        }

  

        public void ApplyChanges()
        {
            _scope.Resolve<TDb>().SaveChanges();
        }
    }
}
