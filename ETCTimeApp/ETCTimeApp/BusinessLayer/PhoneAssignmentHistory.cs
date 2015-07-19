using System;
using System.ComponentModel;
using ETCTimeApp.BL.Contracts;
using ETCTimeApp.DL.SQLite;

namespace ETCTimeApp.BL
{
	public class PhoneAssignmentHistory : IBusinessEntity
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string employee { get; set; }

		[MaxLength (15)]
		public string device_id { get; set; }

		public DateTime StartDate { get; set; }


	}
}
