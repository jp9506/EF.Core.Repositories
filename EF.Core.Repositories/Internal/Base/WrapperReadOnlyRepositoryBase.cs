namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class WrapperReadOnlyRepositoryBase<T, TSource> : WrapperReadOnlyRepositoryBase<T, TSource, T>
        where TSource : IInternalReadOnlyRepository<T>
    {
        protected WrapperReadOnlyRepositoryBase(TSource source) : base(source)
        {
        }
    }

    internal abstract class WrapperReadOnlyRepositoryBase<T, TSource, TResult> : ReadOnlyRepositoryBase<TResult>
        where TSource : IInternalReadOnlyRepository<T>
    {
        protected readonly TSource _internalSource;

        protected WrapperReadOnlyRepositoryBase(TSource source) : base(source.Factory)
        {
            _internalSource = source;
        }
    }
}