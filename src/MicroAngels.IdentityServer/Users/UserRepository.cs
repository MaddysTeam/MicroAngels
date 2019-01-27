using MicroAngels.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer
{

    public class UserRepository : IRepository<User>
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public User GetById(string id)
        {
            throw new NotImplementedException();
        }
    }

}
