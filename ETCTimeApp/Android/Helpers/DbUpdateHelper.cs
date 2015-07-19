using System;
using System.Collections.Generic;
using ETCTimeApp.BL;
using System.Linq;
using System.Threading.Tasks;
using ETCTimeApp.Android.Helpers;

namespace ETCTimeApp.Droid.Helpers
{
	public class DbUpdateHelper
	{
		public void UpdateAll(List<Job> jobs, List<ProjectCode> bcodes, List<ProjectCode> nbcodes, List<TimeEntry> entries, List<GpsLocation> locations)
		{
			AddJobs (jobs);
			AddbCodes (bcodes);
			AddnbCodes (nbcodes);
			UpdateEntries(entries);
			UpdateLocations(locations);
			DeleteOldEntries();
			DeleteOldLocations();
		}

		void UpdateEntries (List<TimeEntry> entries)
		{
			foreach (var entry in entries) {
				entry.isSynched = 1;
				TimeManager.SaveTimeEntry (entry);
			}

		}

		void UpdateLocations (List<GpsLocation> locations)
		{
			foreach (var location in locations) {
				location.isSynched = 1;
				GpsLocationManager.SaveLocation (location);
			}
		}

		public void AddJobs (List<Job> jobs)
		{
			foreach (var j in jobs) {
				JobManager.SaveJob (j);
			}
			var remainingJobs = JobManager.GetJobs ().Take (2000);
			var lowJob = remainingJobs.OrderByDescending (r => r.Id).FirstOrDefault ();
			int lowJobNumber = 0;
			if (lowJob != null)
				lowJobNumber = lowJob.job_id;
			if (lowJobNumber > 0) {
				foreach (var d in JobManager.GetJobs().Where (j => j.job_id < lowJobNumber))
				JobManager.DeleteJob (d.Id);
			}
		}

		public void AddbCodes (List<ProjectCode> bcodes)
		{
			//add new codes 
			foreach (var b in bcodes) {
				if (ProjectCodeManager.GetProjectCodeByCode (b.code_ID) == null)
					ProjectCodeManager.SaveProjectCode (b);
			}
			//delete non used codes
			var allBCodes = ProjectCodeManager.GetBillableCodes ();
			foreach (var d in allBCodes) {
				if (bcodes.Where (b => b.code_ID == d.code_ID) == null)
					ProjectCodeManager.DeleteProjectCode (d.code_ID);
			}
		}

		public void AddnbCodes (List<ProjectCode> nbcodes)
		{
			//add new codes 
			foreach (var b in nbcodes) {
				if (ProjectCodeManager.GetProjectCodeByCode (b.code_ID) == null)
					ProjectCodeManager.SaveProjectCode (b);
			}
			//delete non used codes
			var allBCodes = ProjectCodeManager.GetNonBillableCodes ();
			foreach (var d in allBCodes) {
				if (nbcodes.Where (b => b.code_ID == d.code_ID) == null)
					ProjectCodeManager.DeleteProjectCode (d.code_ID);
			}
		}

		public void DeleteOldLocations ()
		{
			var now = DateTime.Now;
			var oldEntries = GpsLocationManager.GetSynchedGpsLocations ().Where (x => x.isSynched == 1 && x.OccuredAt < now.AddDays (-7));
			foreach (var e in oldEntries) {
				GpsLocationManager.DeleteLocation (e);
			}

		}

		public void DeleteOldEntries ()
		{
			var now = DateTime.Now;
			var oldEntries = TimeManager.GetTimeEntries ().Where (x => x.isSynched == 1 && x.Start < now.AddDays (-15));
			foreach (var e in oldEntries) {
				TimeManager.DeleteTimeEntry (e);
			}
		}

		public async Task ConsolidateEntries()
		{
			var download = new DbDownloadHelper ();
			var upload = new DbUploadHelper ();
			var synchedEntries = download.DownloadEntriesByIMEI (DeviceHelper.GetMyIMEI ());
			var localEntries = TimeManager.GetTimeEntries ()
				.Where(x => x.Start > DateTime.Now.AddDays(-15) && x.isSynched == 1);

			var wrongEntries = localEntries.Except (synchedEntries);

			var count = wrongEntries.Count ();
			var updatedEntries = new List<TimeEntry> ();

			for (int i = 0; i  < 100; i++) {
				var someEntries = wrongEntries.Skip(i * 100).Take (100);
				await upload.UploadTimeEntries (someEntries.ToList());
				updatedEntries.AddRange (someEntries);
			}

			var remainingEntries = wrongEntries.Except (updatedEntries);
			await upload.UploadTimeEntries (remainingEntries.ToList());
		}
	}
}

