using System;
using Fac.Service.Infrastructure.MassTransit.Models;

namespace Correspondence.Service.Infrastructure.MassTransit.Models
{
    public class FacDecisionEvent
    {
        public FacCaseDecision FacDecision { get; set; }
    }
}
