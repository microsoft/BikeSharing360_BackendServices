using BikeSharing.Services.Core.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Core.Controllers
{
    public abstract class BaseApiController : Controller
    {
        protected ICommandBus CommandBus { get; }

        protected BaseApiController(ICommandBus commandBus)
        {
            CommandBus = commandBus;
        }

        protected IActionResult ProcessCommand<T>(T command) where T : ICommand
        {
            var result = CommandBus.Send(command);
            if (result.IsOk)
            {
                CommandBus.ApplyChanges();
            }

            // Some commands need to return a new command result with additional content
            // after db is hit.
            var delayedResult = CommandBus.GetDelayedCommandResult();

            return delayedResult ?? result;

        }


    }
}
