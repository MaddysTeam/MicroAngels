namespace MicroAngels.IdentityServer.Models
{
    public class IdentityClientClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public IdentityClient Client { get; set; }
    }
}
