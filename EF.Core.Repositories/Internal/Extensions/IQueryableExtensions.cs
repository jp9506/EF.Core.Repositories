using EF.Core.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace EF.Core.Repositories.Internal.Extensions
{
    internal static class IQueryableExtensions
    {
        public static async Task<T?> FindAsync<T>(this IContextQueryable<T> source, object key, CancellationToken cancellationToken = default)
        {
            var entityExp = Expression.Parameter(typeof(T), "");
            var expressions = source.Context.CreateExpressions<T>(key, entityExp);
            LambdaExpression lambda = Expression.Lambda(expressions.All(), entityExp);
            var query = source.Provider.CreateQuery<T>(
                Expression.Call(
                    typeof(Queryable), "Where",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Quote(lambda)));
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        private static Expression All(this IEnumerable<Expression> expressions)
        {
            if (!expressions.Any()) return Expression.Constant(true);
            if (expressions.Count() == 1) return expressions.First();
            if (expressions.Count() == 2) return Expression.And(expressions.First(), expressions.Last());
            return expressions
                .Select((x, i) => (x, i))
                .GroupBy(g => g.i % 2)
                .Select(g => g.Select(x => x.x).All()).All();
        }

        private static IEnumerable<Expression> CreateExpressions<T>(this DbContext context, object key, ParameterExpression entityExp)
        {
            var tKey = key.GetType();
            return context.GetKeyProperties<T>()
                .Select(p =>
                {
                    var member = FindPropertyOrField<T>(p.Name);
                    if (member == null) throw new ArgumentException($"Invalid key {p.Name}");
                    var left = member is PropertyInfo
                        ? Expression.Property(entityExp, (PropertyInfo)member)
                        : Expression.Field(entityExp, (FieldInfo)member);
                    var val = tKey.GetProperty(p.Name)!.GetValue(key);
                    var right = Expression.Constant(val);
                    return Expression.Equal(left, right);
                });
        }

        private static MemberInfo? FindPropertyOrField<T>(string memberName)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance;
            foreach (Type t in SelfAndBaseClasses(typeof(T)))
            {
                MemberInfo[] members = t.FindMembers(MemberTypes.Property | MemberTypes.Field,
                    flags, Type.FilterNameIgnoreCase, memberName);
                if (members.Length != 0) return members[0];
            }
            return null;
        }

        private static IEnumerable<Type> SelfAndBaseClasses(Type? type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}