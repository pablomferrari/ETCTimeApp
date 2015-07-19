using System;
using ETCTimeApp.DL.SQLite;

namespace ETCTimeApp.BL.Contracts {
	/// <summary>
	/// Business entity base class. Provides the ID property.
	/// </summary>
	public abstract class BusinessEntityBase : IBusinessEntity {
		public BusinessEntityBase ()
		{
		}
		
		/// <summary>
		/// Gets or sets the Database ID.
		/// </summary>
		[PrimaryKey, AutoIncrement]
        public int Id { get; set; }
	}
}

