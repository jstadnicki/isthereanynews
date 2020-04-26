using System;

namespace Itan.Functions.Workers
{
    public static class Ensure
    {
        public static void NotNull(object element, string elementName)
        {
            if (null == element)
            {
                throw new ArgumentNullException(elementName);
            }
        }
    }
}