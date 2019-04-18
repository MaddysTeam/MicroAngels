namespace MicroAngels.IdentityServer.Models
{

    public class IdentityClaim:IdentityUserClaim
    {
        public IdentityResource IdentityResource { get; set; }
    }

}