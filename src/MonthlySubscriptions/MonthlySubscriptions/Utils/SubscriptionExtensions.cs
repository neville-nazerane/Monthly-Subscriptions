using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonthlySubscriptions.Utils
{
    static class SubscriptionExtensions
    {

        public static ICollection<T> TakeOff<T>(this ICollection<T> collection, T data)
        {
            collection.Remove(data);
            return collection;
        }

        public static IEnumerable<T> TakeOff<T>(this IEnumerable<T> collection, T data)
        {
            collection.ToList().Remove(data);
            return collection;
        }

    }
}
