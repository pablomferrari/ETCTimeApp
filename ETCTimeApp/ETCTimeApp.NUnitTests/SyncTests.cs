using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETCTimeApp.Android.Helpers;
using ETCTimeApp.BL;
using ETCTimeApp.Droid.Helpers;

namespace ETCTimeApp.NUnitTests
{
	[TestFixture ()]
	public class SyncTests
	{
        public const string imei = "014009003734790";
        public List<TimeEntry> TimeEntries { get; set; }
        public DbDownloadHelper DbDownload { get; set; }

        public DbUploadHelper DbUpload { get; set; }

	    public SyncTests()
	    {
            DbDownload = new DbDownloadHelper();
            DbUpload = new DbUploadHelper();
	    }

        public List<TimeEntry> CreateThousandEntries()
        {
			var oldDate = DateTime.MinValue;

            List<TimeEntry> result = new List<TimeEntry>();
            for (int i = 0; i < 1000; i++)
            {				
                result.Add(new TimeEntry
                {
                    Code = string.Format("code {0)", i),
                    Description = string.Format("description {0}", i),
                    device_id = imei,
                    isBillable = i / 2,
					Start = oldDate.AddHours(i),
					Stop = oldDate.AddHours(i + 1),
                    isSynched = 0
                });
            }
            return result;
        }
		[Test ()]
        public void GetTimeEntriesByIMEIReturnsSomething()
		{
            var serverEntries = DbDownload.DownloadEntriesByIMEI(imei);
            Assert.IsTrue(serverEntries.Any());
		}

        [Test()]
        public void UpLoadThousandEntriesSucceeds()
        {
            var entries = CreateThousandEntries();
            var task = DbUpload.UploadTimeEntries(entries);
            Assert.IsTrue(task.Status == TaskStatus.RanToCompletion);

        }
	}
}

