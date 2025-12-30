using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EF.Core.Repositories.Internal
{
    internal class ContextQueryable<T>(IQueryable<T> source, DbContext context) : IContextQueryable<T>
    {
        public DbContext Context { get; } = context;
        public Type ElementType => Source.ElementType;
        public Expression Expression => Source.Expression;
        public IQueryProvider Provider => Source.Provider;
        private IQueryable<T> Source { get; } = source;

        public IEnumerator<T> GetEnumerator() => Source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}