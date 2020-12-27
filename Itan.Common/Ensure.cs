using System;

namespace Itan.Common
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