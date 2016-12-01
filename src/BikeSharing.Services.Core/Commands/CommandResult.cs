using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BikeSharing.Services.Core.Commands
{
    public class CommandResult : ObjectResult
    {

        public static CommandResult Ok = new CommandResult(HttpStatusCode.NoContent);

        public bool IsOk { get; }
        public Exception Error { get; }

        public string Message { get; }
        public object Content { get; }

        public CommandResult(HttpStatusCode code) : this(HttpStatusCode.OK, (object)null)
        {

        }


        public CommandResult(HttpStatusCode code, object content) : base(content)
        {
            IsOk = (int)code < (int)HttpStatusCode.BadRequest;
            StatusCode = (int)code;
            Content = content;
            Message = null;

        }

        public CommandResult(HttpStatusCode code, string message) : this(code, (object)message)
        {
            Content = null;
            Message = message;
        }

        public CommandResult(Exception exception, HttpStatusCode statusCode) : base(exception.Message)
        {
            IsOk = false;
            Error = exception;
            Message = exception.Message;
            StatusCode = (int)statusCode;
            Content = null;
        }

        public static CommandResult NonExistentCommand(string command)
        {
            return new CommandResult(HttpStatusCode.InternalServerError,
                $"Invalid command type {command}");
        }

        public static CommandResult FromValidationErrors(IEnumerable<string> errors)
        {
            var content = new
            {
                Err = errors.ToArray()
            };

            return new CommandResult(HttpStatusCode.BadRequest, content);
        }





    }
}