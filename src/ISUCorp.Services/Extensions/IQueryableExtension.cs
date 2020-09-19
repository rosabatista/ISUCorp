using ISUCorp.Core.Domain;
using ISUCorp.Core.Kernel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ISUCorp.Services.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> ApplyOrder<T>(
            this IQueryable<T> source, string orderByQueryString) where T : BaseEntity
        {
            if (!source.Any())
            {
                return source;
            }

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return source.OrderByDescending(e => e.AddedAt);
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var param in orderParams)
            {
                if (!string.IsNullOrWhiteSpace(param))
                {
                    var propertyFromQueryName = param.Split(" ")[0];
                    var objectProperty = propertyInfos.FirstOrDefault(
                        pi => pi.Name.Equals(propertyFromQueryName,
                        StringComparison.InvariantCultureIgnoreCase));

                    if (objectProperty != null)
                    {
                        var lambda = ToLambda<T>(propertyFromQueryName);
                        source = param.EndsWith(" desc")
                            ? source.OrderByDescending(lambda)
                            : source.OrderBy(lambda);
                    }
                }
            }

            return source;
        }

        public static IQueryable<Reservation> ApplyOrderToReservation(
            this IQueryable<Reservation> source, string orderByQueryString)
        {
            if (!source.Any())
            {
                return source;
            }

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return source.OrderByDescending(e => e.AddedAt);
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Reservation).GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (var param in orderParams)
            {
                if (!string.IsNullOrWhiteSpace(param))
                {
                    var propertyFromQueryName = param.Split(" ")[0];
                    var objectProperty = propertyInfos.FirstOrDefault(
                        pi => pi.Name.Equals(propertyFromQueryName,
                        StringComparison.InvariantCultureIgnoreCase));

                    if (objectProperty != null)
                    {
                        var lambda = ToLambda<Reservation>(propertyFromQueryName);
                        source = param.EndsWith(" desc")
                            ? source.OrderByDescending(lambda)
                            : source.OrderBy(lambda);
                    }
                    else if (param.Contains("name"))
                    {
                        source
                            .Include(e => e.Contact)
                            .Include(e => e.Place);

                        source = param.EndsWith(" desc")
                            ? source
                                .OrderByDescending(e => e.Contact.Name)
                                .ThenByDescending(e => e.Place.Name)
                            : source
                                .OrderBy(e => e.Contact.Name)
                                .ThenBy(e => e.Place.Name);
                    }
                }
            }

            return source;
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}
