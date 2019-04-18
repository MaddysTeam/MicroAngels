using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

    public class IdentityApiResource
    {

        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<IdentityApiSecret> Secrets { get; set; }
        public List<IdentityApiScope> Scopes { get; set; }
        public List<IdentityApiResourceClaim> UserClaims { get; set; }

    }

}
