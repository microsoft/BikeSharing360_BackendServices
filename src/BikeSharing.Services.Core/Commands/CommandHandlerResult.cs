using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BikeSharing.Services.Core.Commands
{
    public class CommandHandlerResult
    {
        public HttpStatusCode StatusCode { get; }
        public object Content { get; private set; }
        public bool HasDelayedContent => _delayedContent != null;

        private Delegate _delayedContent;
        private object _delayedOwner;

        public static CommandHandlerResult Ok = new CommandHandlerResult();



        public CommandHandlerResult() : this(HttpStatusCode.NoContent, null)
        {

        }

        public void ResolveAndUpdateDelayedContent()
        {
            if (_delayedContent != null)
            {
                var content = _delayedContent.DynamicInvoke(_delayedOwner);
                this.Content = content;
                _delayedOwner = null;
                _delayedContent = null;
            }

        }

        public CommandHandlerResult(HttpStatusCode code, object content)
        {
            StatusCode = code;
            Content = content;
            _delayedContent = null;
        }

        public static CommandHandlerResult OkDelayed<T>(T owner, Func<T, object> delayed)
        {
            var result = new CommandHandlerResult(HttpStatusCode.OK, null);

            result._delayedContent = delayed;

            return result;
        }


        public static CommandHandlerResult Error(string error)
        {
            return InvalidResult(HttpStatusCode.BadRequest, error);
        }
        public static CommandHandlerResult NotFound(string error)
        {
            return InvalidResult(HttpStatusCode.NotFound, error);
        }

        private static CommandHandlerResult InvalidResult(HttpStatusCode code, string error)
        {
            return new CommandHandlerResult(code, new { Err = new[] { error } });
        }
    }

}
