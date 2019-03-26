using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotGateway.Configurations
{

    public class MysqlFileConfigurationRepository : IFileConfigurationRepository
    {

        public Task<Response<FileConfiguration>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Response> Set(FileConfiguration fileConfiguration)
        {
            throw new NotImplementedException();
        }

    }

}
