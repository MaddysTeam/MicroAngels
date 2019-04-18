using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

    public class IdentityClientScope
    {
        public int Id { get; set; }
        public string Scope { get; set; }
        public IdentityClient Client { get; set; }
    }

}
