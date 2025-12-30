namespace EF.Core.Repositories.Internal.Base
{
    internal abstract class WrapperReadOnlyRepositoryBase<T, TSource>(TSource source) : WrapperReadOnlyRepositoryBase<T, TSource, T>(source)
        where TSource : IInternalReadOnlyRepository<T>
    {
    }

    internal abstract class WrapperReadOnlyRepositoryBase<T, TSource, TResult>(TSource source) : ReadOnlyRepositoryBase<TResult>(source.Transaction)
        where TSource : IInternalReadOnlyRepository<T>
    {
        protected readonly TSource _internalSource = source;
    }
}