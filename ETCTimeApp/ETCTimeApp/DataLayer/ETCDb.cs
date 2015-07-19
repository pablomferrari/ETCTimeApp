using System.Collections.Generic;
using System.Linq;
using ETCTimeApp.DL.SQLite;
using ETCTimeApp.BL.Contracts;
using ETCTimeApp.BL;

namespace ETCTimeApp.DL
{
	/// <summary>
	/// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the Task DB.
	/// It contains methods for retrieval and persistance as well as db creation, all based on the 
	/// underlying ORM.
	/// </summary>
	public class ETCDb : SQLiteConnection
	{
		static object locker = new object ();
		public static string dbName = "ETCDb.db3";
		/// <summary>
		/// Initializes a new instance of the <see cref="Survey.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public ETCDb (string path) : base (path)
		{
			// create the tables
			CreateTable<Job> ();
			CreateTable<ProjectCode>();
			CreateTable<TimeEntry> ();
			CreateTable<GpsLocation> ();
		}

		public IEnumerable<T> GetItems<T> () where T : IBusinessEntity, new ()
		{
			lock (locker) {
				return (from i in Table<T> () select i).ToList ();
			}
		}

		public T GetItem<T> (int id) where T : IBusinessEntity, new ()
		{
			lock (locker) {
				return Table<T>().FirstOrDefault(x => x.Id == id);
				// Following throws NotSupportedException - thanks aliegeni
				//return (from i in Table<T> ()
				//        where i.ID == id
				//        select i).FirstOrDefault ();
			}
		}

		public int SaveItem<T> (T item) where T : IBusinessEntity
		{
			lock (locker) {
				if (item.Id != 0) {
					Update (item);
					return item.Id;
				} else {
					return Insert (item);
				}
			}
		}

		public int DeleteItem<T>(int id) where T : IBusinessEntity, new ()
		{
			lock (locker) {
				#if NETFX_CORE
				return Delete(new T() { ID = id });
				#else
				return Delete<T> (new T () { Id = id });
				#endif
			}
		}
	}
}