using System;
using System.Collections.Generic;
using ETCTimeApp.BL;
using ETCTimeApp.DAL;

namespace ETCTimeApp
{
	public static class TimeManager
	{
		static TimeManager ()
		{
		}

		public static List<TimeEntry> GetTimeEntries ()
		{
			return ETCDbRepository.GetAllTimeEntries ();
		}

		public static TimeEntry GetActiveTimeEntry ()
		{
			return ETCDbRepository.GetActiveTimeEntry ();
		}

		public static List<TimeEntry> GetCompletedBillableEntries ()
		{
			return ETCDbRepository.GetCompletedBillableEntries ();
		}

		public static List<TimeEntry> GetCompletedNonBillableEntries ()
		{
			return ETCDbRepository.GetCompletedNonBillableEntries ();
		}

		public static List<TimeEntry> GetUnSynchedTimeeEntries ()
		{
			return ETCDbRepository.GetUnSynchedTimeeEntries ();
		}

		public static int StartTimeEntry (TimeEntry item)
		{
			return ETCDbRepository.SaveTimeEntry (item);
		}

		public static int StopTimeEntry (TimeEntry item)
		{
			return ETCDbRepository.SaveTimeEntry (item);
		}

		public static int SaveTimeEntry (TimeEntry item)
		{
			return ETCDbRepository.SaveTimeEntry (item);
		}

		public static void DeleteTimeEntry (TimeEntry item)
		{
			ETCDbRepository.DeleteTimeEntry (item);
		}

		public static bool IsEntryActive ()
		{
			return ETCDbRepository.GetActiveTimeEntry () != null;
		}

		public static void GoToBreak (DateTime stop)
		{
			ETCDbRepository.GoToBreak (stop);
		}
	}
}

