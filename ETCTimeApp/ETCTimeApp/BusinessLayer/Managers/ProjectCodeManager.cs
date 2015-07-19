using System;
using ETCTimeApp.BL;
using System.Collections.Generic;
using ETCTimeApp.DAL;

namespace ETCTimeApp
{
	public static class ProjectCodeManager
	{
		static ProjectCodeManager ()
		{
		}

		public static ProjectCode GetProjectCode (int id)
		{
			return ETCDbRepository.GetProjectCode (id);
		}


		public static ProjectCode GetProjectCodeByCode (string code_id)
		{
			return ETCDbRepository.GetProjectCodeByCode (code_id);
		}

		public static List<ProjectCode> GetProjectCodes ()
		{
			return new List<ProjectCode> (ETCDbRepository.GetProjectCodes ());
		}

		public static List<ProjectCode> GetBillableCodes ()
		{
			return new List<ProjectCode> (ETCDbRepository.GetBillableCodes ());
		}

		public static List<ProjectCode> GetNonBillableCodes ()
		{
			return new List<ProjectCode> (ETCDbRepository.GetNonBillableCodes ());
		}


		public static int SaveProjectCode (ProjectCode item)
		{
			return ETCDbRepository.SaveProjectCode (item);
		}

		public static int DeleteProjectCode (int id)
		{
			return ETCDbRepository.DeleteProjectCode (id);
		}

		public static int DeleteProjectCode (string code)
		{
			return ETCDbRepository.DeleteProjectCode (code);
		}
	}
}

