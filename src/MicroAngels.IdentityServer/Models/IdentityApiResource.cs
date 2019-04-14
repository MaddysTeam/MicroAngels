using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer
{

    public class IdentityApiResource
    {
        public string Id { get; set; }

        public ICollection<IdentityScope> Scopes { get; set; }
    }

}
