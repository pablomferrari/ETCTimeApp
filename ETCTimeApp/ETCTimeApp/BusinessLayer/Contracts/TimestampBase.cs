using System;

namespace ETCTimeApp.BL.Contracts
{
    public abstract class TimestampBase : VersionBase
    {
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public TimestampBase()
        {
            Created = DateTime.UtcNow;
        }
    }
}
