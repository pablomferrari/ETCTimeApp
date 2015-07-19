using System;
using ETCTimeApp.BL.Contracts;
using ETCTimeApp.DL.SQLite;


namespace TrackTimeGPS.BL
{
    public class Exception2 : GuidBase
    {
		public override Guid ID { get; set; }

        public const string Url = "exceptions";

        public string StackTrace { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        #if !__ANDROID__
        [Ignore]
        #endif
        public Exception2 InnerException { get; set; }

        public Guid? InnerExceptionId { get; set; }

        [Obsolete("User code should use .ctor(Exception)")]
        public Exception2() { }

        public Exception2(Exception ex)
        {
            if (ex.InnerException != null && ex.InnerException != ex)
            {
                InnerException = new Exception2(ex.InnerException);
                InnerExceptionId = InnerException.ID;
            }
            ID = Guid.NewGuid();
            StackTrace = ex.StackTrace;
            Message = ex.Message;
            Source = ex.Source;
        }
    }
}
