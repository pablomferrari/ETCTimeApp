using ETCTimeApp.BL;
using System.Collections.Generic;
using ETCTimeApp.DAL;

namespace ETCTimeApp
{
	public static class JobManager
	{
		static JobManager ()
		{
		}

		public static Job GetJob(int id)
		{
			return ETCDbRepository.GetJob(id);
		}
			
		public static int GetTopJobId ()
		{
			return ETCDbRepository.GetTopJob ();
		}
		public static List<Job> GetJobs ()
		{
			return new List<Job>(ETCDbRepository.GetJobs());
		}

		public static int SaveJob (Job item)
		{
			return ETCDbRepository.SaveJob(item);
		}

		public static int DeleteJob(int id)
		{
			return ETCDbRepository.DeleteJob (id);
		}
	}
}

