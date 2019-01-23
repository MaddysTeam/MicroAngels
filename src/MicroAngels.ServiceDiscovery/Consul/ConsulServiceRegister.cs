using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Consul;
using MicroAngels.Core;
using MicroAngels.Core.Service;

namespace MicroAngels.ServiceDiscovery.Consul
{

    public interface IConsulServiceRegister : IServiceRegiter<ConsulService, ConsulServiceError>
    {

    }

    public class ConsulServiceRegister : IConsulServiceRegister
    {

        public ConsulServiceRegister(ConsulHostConfiguration hostConfiguration)
        {
            _client = new ConsulClient(config =>
            {
                config.Address = hostConfiguration.Address;
                config.Datacenter = hostConfiguration.DataCenter;
                config.Token = hostConfiguration.Token;
                config.WaitTime = hostConfiguration.WaitTime;
            });
        }

        public async Task<ConsulServiceError> RegistAsync(ConsulService service)
        {
            if (_client.IsNull())
            {
                return new ConsulServiceError("", "", "");
            }

            await _client.Agent.ServiceRegister(
                 new AgentServiceRegistration
                 {
                     ID = service.Id,
                     Name = service.Name,
                     Address = service.Host,
                     Port = service.Port,
                     Tags = service.Tags,
                     Check = new AgentServiceCheck {
                         HTTP = service.HealthCheckOptoins.HealthCheckHTTP,
                         Interval = service.HealthCheckOptoins.IntervalTimeSpan,
                         DeregisterCriticalServiceAfter = service.HealthCheckOptoins.DeRegisterDelayTimeSpan
                     }
                 }).ConfigureAwait(false);

            return null;
        }

        public async Task<ConsulServiceError> DeregisterAsync(ConsulService service)
        {
            var result = await _client.Agent.ServiceDeregister(service.Id);

            return result.StatusCode == HttpStatusCode.OK ? null : new ConsulServiceError("","","");
        }

        private ConsulClient _client;

    }

}
