using System;
using System.ComponentModel;
using ETCTimeApp.BL.Contracts;
using ETCTimeApp.DL.SQLite;

namespace ETCTimeApp.BL
{
	public class ProjectCode : IBusinessEntity
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string code_ID { get; set; }

		public bool IsBillable { get; set; }
	}
}
