using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.IdentityServer.Models
{

    public class IdentityApiSecret
    {
		public string Description { get; set; }
		public string Value { get; set; }
		public DateTime? Expiration { get; set; }
		public string Type { get; set; }

		public IdentityApiResource ApiResource { get; set; }
    }

}
