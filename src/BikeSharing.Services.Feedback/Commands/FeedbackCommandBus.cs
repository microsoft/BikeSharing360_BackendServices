using Autofac;
using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Feedback.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.StdHost.Commands
{
    public class FeedbackCommandBus : CommandBus<FeedbackDbContext>
    {
        public FeedbackCommandBus(ILifetimeScope scope) : base(scope)
        {
        }
    }
}
