namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class WrapperRepositoryBase<T, TSource>(TSource source) : RepositoryBase<T>(source.Transaction)
        where T : class
        where TSource : IInternalRepository<T>
    {
        protected readonly TSource _internalSource = source;
    }
}