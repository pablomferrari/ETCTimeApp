using System;
using System.ComponentModel;
using ETCTimeApp.BL.Contracts;
using ETCTimeApp.DL.SQLite;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace ETCTimeApp.BL
{
	[JsonObject (Title = "gpsLocations")]
	public class GpsLocation : IBusinessEntity
	{
		[JsonIgnore]
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[MaxLength (15)]
		[JsonProperty (PropertyName = "device_imei")]
		public string Imei { get; set; }

		[JsonProperty (PropertyName = "latitude")]
		public float Latitude { get; set; }

		[JsonProperty (PropertyName = "longitude")]
		public float Longitude { get; set; }

		[JsonProperty (PropertyName = "occured_at")]
		public DateTime OccuredAt { get; set; }

		[JsonIgnore]
		public int isSynched { get; set; }
	}
}
