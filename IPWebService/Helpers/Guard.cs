using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPWebService.Helpers
{
    public static class Guard
    {
        public static class KeyNotFound
        {
            public static T Throw<T>(string key)
                    => throw new KeyNotFoundException($"{key} of {nameof(T)} type hasn't been found");

            public static void Throw(string key)
                    => throw new KeyNotFoundException($"{key} hasn't been found");
        }

        public static class NullArgument
        {
            public static void Throw(string argument)
                    => throw new ArgumentNullException($"{argument} argument is null");
        }
    }
}
