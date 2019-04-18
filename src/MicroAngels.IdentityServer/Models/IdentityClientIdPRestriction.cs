using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

    public class IdentityClientIdPRestriction
    {
        public int Id { get; set; }
        public string Provider { get; set; }
        public IdentityClient Client { get; set; }
    }

}
