using System;

namespace ETCTimeApp.BL.Contracts
{
    public static class Extensions
    {
        public static string ToSGuid(this Guid g)
        {
            return Convert.ToBase64String(g.ToByteArray()).Replace('/', '_').Replace('+', '-').Substring(0, 22);
        }
    }
}

