using System;
using System.ComponentModel;
using ETCTimeApp.BL.Contracts;
using ETCTimeApp.DL.SQLite;


namespace ETCTimeApp.BL
{
	public class TaskHistory : IBusinessEntity
	{
		public const string Url = "taskhistory";

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[MaxLength(10)]
		public string PhoneNumber { get; set; }

		public int JobId { get; set; }

		[MaxLength(5)]
		public string ProjectCode { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime? EndTime { get; set; }
	}
}
