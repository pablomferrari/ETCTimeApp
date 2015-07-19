using System;
using ETCTimeApp.BL;
using ETCTimeApp.DAL;
using System.Collections.Generic;

namespace ETCTimeApp
{
	public static class GpsLocationManager
	{
		static GpsLocationManager ()
		{
		}

		public static int SaveLocation (GpsLocation item)
		{
			return ETCDbRepository.SaveLocation (item);
		}

		public static List<GpsLocation> GetUnSynchedGpsLocations ()
		{
			return ETCDbRepository.GetUnsynchedLocations ();
		}

		public static List<GpsLocation> GetSynchedGpsLocations ()
		{
			return ETCDbRepository.GetSynchedLocations ();
		}

		public static int SynchGpsLocation (GpsLocation item)
		{
			return ETCDbRepository.SynchLocation (item);
		}

		public static void DeleteLocation (GpsLocation item)
		{
			ETCDbRepository.DeleteGPS (item);
		}
	}
}

