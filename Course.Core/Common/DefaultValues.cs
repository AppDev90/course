using System;

namespace Course.Core.Common
{
    public class DefaultValues
    {
        public static DateTime DateTimeNow(bool Utc = true)
        {
            return Utc ? DateTime.UtcNow : DateTime.Now;
        }
    }
}
