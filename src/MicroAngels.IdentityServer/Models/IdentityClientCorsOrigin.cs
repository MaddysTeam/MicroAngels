namespace MicroAngels.IdentityServer.Models
{

    public class IdentityClientCorsOrigin
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public IdentityClient Client { get; set; }
    }

}
