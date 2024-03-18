using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Internal
{
    internal class ContextQueryable<T> : IContextQueryable<T>
    {
        public ContextQueryable(IQueryable<T> source, DbContext context)
        {
            Source = source;
            Context = context;
        }

        public DbContext Context { get; }
        public Type ElementType => Source.ElementType;
        public Expression Expression => Source.Expression;
        public IQueryProvider Provider => Source.Provider;
        private IQueryable<T> Source { get; }

        public IEnumerator<T> GetEnumerator() => Source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}