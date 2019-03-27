using System;

namespace Castle.Infrastructure
{
    internal static class ArgumentGuard
    {
        public static void NotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Argument must not be null or empty", name);
        }

        public static void NotNull(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }
    }
}
