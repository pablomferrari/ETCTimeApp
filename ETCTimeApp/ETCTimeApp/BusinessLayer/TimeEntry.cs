using System;
using ETCTimeApp.DL.SQLite;
using ETCTimeApp.BL.Contracts;
using Newtonsoft.Json;

namespace ETCTimeApp.BL
{
	[JsonObject (Title = "entries")]
	public class TimeEntry  : IBusinessEntity
	{
		[JsonIgnore]
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[MaxLength (15)]
		[JsonProperty (PropertyName = "device_id")]
		public string device_id { get; set; }

		[MaxLength (50)]
		[JsonProperty (PropertyName = "Description")]
		public string Description { get; set; }

		[MaxLength (5)]
		[JsonProperty (PropertyName = "Code")]
		public string Code { get; set; }

		[JsonProperty (PropertyName = "Start")]
		public DateTime Start { get; set; }

		[JsonProperty (PropertyName = "Stop")]
		public DateTime? Stop { get; set; }

		[JsonIgnore]
		public int isSynched { get; set; }

		[JsonIgnore]
		public int isBillable { get; set; }
	}
}

