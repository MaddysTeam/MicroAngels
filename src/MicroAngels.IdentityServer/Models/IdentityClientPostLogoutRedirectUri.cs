namespace MicroAngels.IdentityServer.Models
{
    public class IdentityClientPostLogoutRedirectUri
    {
        public int Id { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public IdentityClient Client { get; set; }
    }
}