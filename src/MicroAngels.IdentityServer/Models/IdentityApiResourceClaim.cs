namespace MicroAngels.IdentityServer.Models
{

    public class IdentityApiResourceClaim: IdentityUserClaim
    {
        public IdentityApiResource ApiResource { get; set; }
    }

}