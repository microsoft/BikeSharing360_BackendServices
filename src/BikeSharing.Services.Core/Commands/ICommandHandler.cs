using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Core.Commands
{
    public interface ICommandHandler { }

    public interface ICommandHandler<in TCommand> : ICommandHandler
        where TCommand : ICommand
    {
        CommandHandlerResult Handle(TCommand command);
    }
}
