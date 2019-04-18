using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

    public class IdentityClientSecret: IdentitySecret
    {
        public IdentityClient Client { get; set; }
    }

}
