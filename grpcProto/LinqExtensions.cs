using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grpcChatProto
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self)
            {
                action(item);
            }
        }

        public static void WaitAll(this IEnumerable<Task> self)
        {
            Task.WaitAll(self.ToArray());
        }
    }
}
