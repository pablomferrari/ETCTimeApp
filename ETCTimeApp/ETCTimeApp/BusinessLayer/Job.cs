using ETCTimeApp.BL.Contracts;
using ETCTimeApp.DL.SQLite;

namespace ETCTimeApp.BL
{
    public class Job : IBusinessEntity
    {
		public Job()
		{
		}
		[PrimaryKey, AutoIncrement]
        public int Id { get; set; }
		public int job_id { get; set;}
        public string facility_address { get; set; }
    }
}
