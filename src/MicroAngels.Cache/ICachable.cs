namespace MicroAngels.Cache
{
    public interface ICachable
    {
        string CacheKey { get; }
    }

    public class Product : ICachable
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string CacheKey => this.ProductId > 0 ? this.ProductId.ToString() : "";
    }
}
