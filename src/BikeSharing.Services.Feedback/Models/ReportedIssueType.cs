using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.Models
{
    public enum ReportedIssueType
    {
        Unknown = 0,
        Handlebar,
        Fork,
        Pedals,
        FlatTire,
        Chain,
        Stolen
    }
}
