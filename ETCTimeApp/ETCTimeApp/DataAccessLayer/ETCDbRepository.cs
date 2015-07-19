using System;
using ETCTimeApp.BL;
using ETCTimeApp.DL;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace ETCTimeApp.DAL
{
	public class ETCDbRepository
	{
		ETCDb db = null;
		protected static string dbLocation;
		protected static ETCDbRepository me;

		static ETCDbRepository ()
		{
			me = new ETCDbRepository ();
		}

		protected ETCDbRepository ()
		{
			// set the db location
			dbLocation = DatabaseFilePath;

			// instantiate the database	
			db = new ETCDb (dbLocation);
		}

		public static string DatabaseFilePath {
			get { 
				var sqliteFilename = ETCDb.dbName;

				#if NETFX_CORE
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
				#else

				#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
				#else

				#if __ANDROID__
				// Just use whatever directory SpecialFolder.Personal returns
				string libraryPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				;
				#else
				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
				// (they don't want non-user-generated data in Documents)
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "../Library/"); // Library folder
				#endif
				var path = Path.Combine (libraryPath, sqliteFilename);
				#endif		

				#endif
				return path;	
			}
		}

		#region Job

		public static Job GetJob (int id)
		{
			return me.db.GetItem<Job> (id);
		}

		public static int GetTopJob ()
		{
			int topJob = 0;
			var job = me.db.GetItems<Job> ().OrderByDescending (j => j.Id).FirstOrDefault ();
			if (job != null)
				topJob = job.job_id;
			return topJob;
		}

		public static IEnumerable<Job> GetJobs ()
		{
			return me.db.GetItems<Job> ();
		}

		public static int SaveJob (Job item)
		{
			return me.db.SaveItem<Job> (item);
		}

		public static int DeleteJob (int id)
		{
			return me.db.DeleteItem<Job> (id);
		}

		#endregion

		#region project codes

		public static ProjectCode GetProjectCode (int id)
		{
			return me.db.GetItem<ProjectCode> (id);
		}

		public static ProjectCode GetProjectCodeByCode (string code)
		{
			var codes = me.db.GetItems<ProjectCode> ();

			return codes.Where (c => c.code_ID == code.Trim ()).FirstOrDefault ();
		}

		public static IEnumerable<ProjectCode> GetProjectCodes ()
		{
			return me.db.GetItems<ProjectCode> ();
		}

		public static IEnumerable<ProjectCode> GetBillableCodes ()
		{
			var items = me.db.GetItems<ProjectCode> ();
			return items.Where (i => i.IsBillable == true);
		}

		public static IEnumerable<ProjectCode> GetNonBillableCodes ()
		{
			var items = me.db.GetItems<ProjectCode> ();
			return items.Where (i => i.IsBillable == false);
		}

		public static int SaveProjectCode (ProjectCode item)
		{
			return me.db.SaveItem<ProjectCode> (item);
		}

		public static void AddCode (ProjectCode p)
		{
			throw new NotImplementedException ();
		}

		public static int DeleteProjectCode (int id)
		{
			return me.db.DeleteItem<ProjectCode> (id);
		}

		public static int DeleteProjectCode (string code)
		{
			var c = GetProjectCodeByCode (code);
			return me.db.DeleteItem<ProjectCode> (c.Id);
		}

		#endregion

		#region time history

		public static List<TimeEntry> GetAllTimeEntries ()
		{
			return me.db.GetItems<TimeEntry> ().ToList ();
		}

		public static List<TimeEntry> GetUnSynchedTimeeEntries ()
		{
			var unsynched = me.db.GetItems<TimeEntry> ()
				.Where (t => t.isSynched == 0 && t.Stop.HasValue).ToList ();
			return unsynched;
		}

		public static TimeEntry GetActiveTimeEntry ()
		{
			var lastEntry = me.db.GetItems<TimeEntry> ()
				.OrderByDescending (t => t.Id).Where (t => !t.Stop.HasValue).FirstOrDefault ();
			return lastEntry;
		}

		public static int SaveTimeEntry (TimeEntry item)
		{
			return me.db.SaveItem <TimeEntry> (item);
		}

		public static void DeleteTimeEntry (TimeEntry item)
		{
			me.db.Delete<TimeEntry> (item);
		}

		public static void GoToBreak (DateTime stop)
		{
			var activeEntry = ETCDbRepository.GetActiveTimeEntry ();
			if (activeEntry != null)
				activeEntry.Stop = stop;
			SaveTimeEntry (activeEntry);
		}


		public static List<TimeEntry> GetCompletedBillableEntries ()
		{
			var entries = me.db.GetItems<TimeEntry> ()
				.Where (t => t.Stop.HasValue
			              && t.isBillable == 1)
				.OrderByDescending (t => t.Id).ToList ();
			return entries;
		}

		public static List<TimeEntry> GetCompletedNonBillableEntries ()
		{
			var entries = me.db.GetItems<TimeEntry> ()
				.Where (t => t.Stop.HasValue
			              && t.isBillable == 0)
				.OrderByDescending (t => t.Id).ToList ();
			return entries;
		}

		#endregion

		#region GPS Location

		public static int SaveLocation (GpsLocation item)
		{
			return me.db.SaveItem<GpsLocation> (item);
		}

		public static void DeleteGPS (GpsLocation item)
		{
			me.db.Delete<GpsLocation> (item);
		}

		public static List<GpsLocation> GetUnsynchedLocations ()
		{
			var locations = me.db.GetItems<GpsLocation> ()
				.Where (l => l.isSynched == 0).ToList ();
			return locations;
		}

		public static List<GpsLocation> GetSynchedLocations ()
		{
			var locations = me.db.GetItems<GpsLocation> ()
				.Where (l => l.isSynched == 1).ToList ();
			return locations;
		}

		public static int SynchLocation (GpsLocation item)
		{
			var location = me.db.GetItem<GpsLocation> (item.Id);
			location.isSynched = 1;
			return SaveLocation (location);
		}

		#endregion
	}
}

