namespace MicroAngels.IdentityServer.Models
{

    internal class IdentityClientRedirectUri
    {

        public int Id { get; set; }
        public string RedirectUri { get; set; }
        public IdentityClient Client { get; set; }
    }

}