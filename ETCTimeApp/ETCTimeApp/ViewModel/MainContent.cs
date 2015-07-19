using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using System.Threading;
using System.Linq;
using ETCTimeApp.Droid.Services.LocationServices;
using ETCTimeApp.Droid.Helpers;
using ETCTimeApp.Android.Helpers;
using System.Collections.Generic;
using ETCTimeApp.BL;


namespace ETCTimeApp.VM
{
	public class MainContent : TabbedPage
	{
		public ContentPage Billable { get; set; }
		public ContentPage NonBillable { get; set; }
		public ContentPage Entries { get; set; }


		#region sync variables

		private ActivityPageViewModel ViewModel { get; set; }

		public string baseUrl = ApiHelper.BaseURL();

		public string versionNumber { get; set; }

		#endregion

		FormsApplicationActivity act;

		public MainContent (FormsApplicationActivity a)
		{
			act = a;
			Title = "ETC TimeApp version " + act.Resources.GetString (ETCTimeApp.Android.Resource.String.version);
			string imei = DeviceHelper.GetMyIMEI ();

			Billable = new BillableTab ();
			Billable.Title = "Billable Time";
			NonBillable = new NonBillableTab ();
			NonBillable.Title = "Non Billable Time";
			var now = DateTime.Now;
			var entries = TimeManager.GetTimeEntries ().Where (x => x.Stop != null && x.Start > now.AddDays (-15))
				.OrderByDescending(x => x.Start).ToArray();
			Entries = new EntriesTab (entries);
			Entries.Title = "History";
			Children.Add (Billable);
			Children.Add (NonBillable);
			Children.Add (Entries);


			var activeEntry = TimeManager.GetActiveTimeEntry ();
			if (activeEntry != null) {
				var timer = new Intent (a, typeof(StartLocationTimer));
				new Thread (() => Forms.Context.StartService (timer)).Start ();
			}

		
			ViewModel = new ActivityPageViewModel ();
			BindingContext = ViewModel;
			var activityIndicator = new ActivityIndicator {
				Color = Color.Black,
			};
			activityIndicator.SetBinding (ActivityIndicator.IsVisibleProperty, "IsBusy");
			activityIndicator.SetBinding (ActivityIndicator.IsRunningProperty, "IsBusy");


			var menu = new ToolbarItem ();
			menu.Text = "";
			menu.Order = ToolbarItemOrder.Default;

			var hours = new ToolbarItem ();
			hours.Text = "Hours";
			hours.Order = ToolbarItemOrder.Primary;
			hours.Clicked += GetHours; 

			var sync = new ToolbarItem ();
			sync.Text = "Sync";
			sync.Order = ToolbarItemOrder.Primary;
			sync.Clicked += CallSync;

			var consolidate = new ToolbarItem ();
			consolidate.Text = "Consolidate";
			consolidate.Order = ToolbarItemOrder.Primary;
			consolidate.Clicked += CallConsolidate;

			ToolbarItems.Add (menu);
			ToolbarItems.Add (hours);
			ToolbarItems.Add (sync);
			ToolbarItems.Add (consolidate);

			//			#region testing
			//			var mock = new ToolbarItem ();
			//			mock.Text = "Mock Entries";
			//			mock.Order = ToolbarItemOrder.Secondary;
			//			mock.Clicked += MockEntries;
			//
			//			var del = new ToolbarItem ();
			//			del.Text = "Delete Entries";
			//			del.Order = ToolbarItemOrder.Secondary;
			//			del.Clicked += DeleteEntries;
			//			ToolbarItems.Add (mock);
			//			ToolbarItems.Add (del);
			//			#endregion


		}

//		#region testing
//		void DeleteEntries(object sender, EventArgs e)
//		{
//			var entries = TimeManager.GetTimeEntries ();
//			foreach (var t in entries) {
//				TimeManager.DeleteTimeEntry (t);
//			}
//		}
//		void MockEntries(object sender, EventArgs e)
//		{
//			DeleteEntries (sender, e);
//			var oldDate = DateTime.Now.AddYears(-10);
//
//			for (int i = 0; i < 1000; i++)
//			{	
//				var entry = new TimeEntry ();
//				entry.Code = "Test code";
//				entry.Description = "Test Description";
//				entry.device_id = DeviceHelper.GetMyIMEI ();
//				entry.isBillable = i / 2;
//				entry.Start = oldDate.AddHours ((double)i);
//				entry.Stop = oldDate.AddHours ((double)i + 1);
//				entry.isSynched = 1;
//				TimeManager.SaveTimeEntry (entry);
//			}
//		}
//		#endregion
		void CallSync (object sender, EventArgs e)
		{
			if (!WiFiHelper.IsWiFiAvailable()) {
				DisplayAlert ("Sync", "It looks like you do not have internet connection", "OK");
				return;
			}
			//connected to wifi let's do sync
			DisplayAlert ("Sync", "Sync will now run in your background...", "OK");
			Intent DbServiceIntent = new Intent ("com.ETCTimeApp.DbSyncService");
			new Thread (() => Forms.Context.StartService (DbServiceIntent)).Start ();
		}

		void CallConsolidate (object sender, EventArgs e)
		{
			if (!WiFiHelper.IsWiFiAvailable()) {
				DisplayAlert ("Sync", "It looks like you do not have internet connection", "OK");
				return;
			}
			DisplayAlert ("Sync", "Consolidate process running on background...", "OK");
			Consolidate ();

		}

		void Consolidate ()
		{
			var update = new DbUpdateHelper ();
			update.ConsolidateEntries ();

		}

		void GetHours (object sender, EventArgs e)
		{
			var rightnow = DateTime.Now;
			var lastSaturday = DateTime.Now.Date;
			while (lastSaturday.DayOfWeek != DayOfWeek.Saturday)
				lastSaturday = lastSaturday.AddDays (-1);

			TimeSpan total = new TimeSpan ();
			var completedBillable = TimeManager.GetCompletedBillableEntries ().Where(x => x.Start >= lastSaturday
				&& x.Start < rightnow).ToList();
			if (completedBillable.Any ()) {
				foreach (var c in completedBillable) {
					total += c.Stop.Value - c.Start;
				}
			}

			var completednonbillable = TimeManager.GetCompletedNonBillableEntries ().Where(x => x.Start >= lastSaturday
				&& x.Start < rightnow).ToList();;
			if (completednonbillable.Any ()) {
				foreach (var nc in completednonbillable) {
					total += nc.Stop.Value - nc.Start;
				}
			}
			var current = TimeManager.GetActiveTimeEntry ();
			if (current != null)
				total += DateTime.Now - current.Start;
			string strTotal = string.Format ("{0}:{1}",
				(int)total.TotalHours,
				total.Minutes.ToString ().PadLeft (2, '0'));
			DisplayAlert ("Worked This Week", strTotal, "OK");
		}
	}
}

