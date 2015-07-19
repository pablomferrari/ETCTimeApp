using System;

namespace ETCTimeApp.BL
{
	public class JobHistory
	{
		private string _Description;
		private string _Task;
		private DateTime _Start;
		private DateTime _Stop;
		//		private bool _isBillable;
		//sync property
		private bool _isSynched;


		public JobHistory(string description, string task, bool isBillable)
		{
			this._Description = description;
			this._Task = task;
			//			this._isBillable = isBillable;
			this._isSynched = false;
			this.Start ();
		}

		public void Description(string value)
		{
			this._Description = value;
		}

		public string Description()
		{
			return this._Description;
		}

		public void Task(string value)
		{
			this._Task = value;
		}

		public String Task()
		{
			return this._Task;
		}

		public DateTime StartedAt()
		{
			return this._Start;
		}

		public DateTime StoppedAt()
		{
			return this._Stop;
		}

		public void Start()
		{
			this._Start = DateTime.Now;
		}

		public void Stop()
		{
			this._Stop = DateTime.Now;
		}

		public void IsSynched(bool value)
		{
			this._isSynched = value;
		}

		public bool IsSynched()
		{
			return this._isSynched;
		}

		//		public void IsBillable(bool value)
		//		{
		//			this._isBillable = value;
		//		}
		//
		//		public bool IsBillable()
		//		{
		//			return this._isBillable;
		//		}


	}
}

