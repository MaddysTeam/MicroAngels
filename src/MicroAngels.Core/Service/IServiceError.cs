namespace MicroAngels.Core.Service
{

    public interface IServiceError : IError
    {
         string ServiceId { get; }
    }

}
