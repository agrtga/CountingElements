using System;

namespace CountingElements
{
    public static class ArgCheck
    {
        public static void IsNullOrEmpty(string value, string paramName)
        {
            if (value == null) {
                throw new ArgumentNullException(paramName);
            }

            if (value.Length == 0) {
                throw new ArgumentException("The argument cannot be an empty string.", paramName);
            }
        }
    }
}
