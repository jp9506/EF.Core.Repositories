namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class WrapperRepositoryBase<T, TSource> : RepositoryBase<T>
        where T : class
        where TSource : IInternalRepository<T>
    {
        protected readonly TSource _internalSource;

        protected WrapperRepositoryBase(TSource source) : base(source.Transaction)
        {
            _internalSource = source;
        }
    }
}