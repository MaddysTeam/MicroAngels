using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroAngels.Gateway.Ocelot
{

    public class MysqlFileConfigurationRepository : IFileConfigurationRepository
    {
        //get configuration from db

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
