using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

    public class IdentityClientGrantType
    {
        public int Id { get; set; }
        public string GrantType { get; set; }
        public IdentityClient Client { get; set; }
    }

}
