using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Core.Service
{

    public class ServiceHealthCheckOptions
    {
        public TimeSpan IntervalTimeSpan { get; set; }

        public TimeSpan RegisterWaitTimeSpan { get; set; }

        public TimeSpan DeRegisterDelayTimeSpan { get; set; }

        public string HealthCheckHTTP { get; set; }

        public TimeSpan TimeOutTimeSpan { get; set; }
    }

}
