namespace MicroAngels.Core.Domain
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        T GetById(string id);
       

        IUnitOfWork UnitOfWork { get; }
    }
}
