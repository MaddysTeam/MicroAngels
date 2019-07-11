using Consul;
using MicroAngels.Core;
using MicroAngels.Core.Service;
using System.Net;
using System.Threading.Tasks;

namespace MicroAngels.ServiceDiscovery.Consul
{

	public interface IConsulServiceRegister : IServiceRegiter<ConsulService, ConsulServiceResult>
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

        public async Task<ConsulServiceResult> RegistAsync(ConsulService service)
        {
            if (_client.IsNull())
            {
                return new ConsulServiceResult(false,"", "", "");
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

            return new ConsulServiceResult(true);
        }

        public async Task<ConsulServiceResult> DeregisterAsync(ConsulService service)
        {
            var result = await _client.Agent.ServiceDeregister(service.Id);

            return result.StatusCode == HttpStatusCode.OK ? null : new ConsulServiceResult(false,"","","");
        }

        private ConsulClient _client;

    }

}
