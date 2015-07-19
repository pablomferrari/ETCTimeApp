using System;
using Xamarin.Forms;
using ETCTimeApp.BL;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using ETCTimeApp.Droid.Services.LocationServices;
using Android.Content;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ETCTimeApp.Droid.Helpers;

namespace ETCTimeApp.VM
{
	public class BillableTab : ContentPage
	{
		#region variables

		public StartStopLayout buttons;
		public JobCodeLayout JobCode;
		public Label jobAddress;
		public Job currentJob;
		public Label currentJobLabel;
		public HistoryListView history;
		public string imei;
		public ProjectCode code;

		//job data
		public string jobNumber;
		public int selectedCodeId;
		public string selectedCodeDescription;
		public List<ProjectCode> billableCodes;
		public ObservableCollection<WorkHistory> h;


		#endregion

		public BillableTab ()
		{
			imei = DeviceHelper.GetMyIMEI ();

			#region titles
			this.Padding = 5;
			this.Title = "ETC TimeTrack Billable";
			var hLabel = new Label {
				Text = "Your Billable History",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.Black,
				FontFamily = "Helvetica", FontSize = 14
			};
			#endregion

			#region selections
			this.selectedCodeId = -1;
			jobAddress = new Label {
				TextColor = Color.Gray,
				FontSize = 8.5,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};


			billableCodes = ProjectCodeManager.GetBillableCodes ();
			JobCode = new JobCodeLayout (billableCodes);
			JobCode.jobEntry.Completed += FindJob;
			JobCode.codePicker.SelectedIndexChanged += codePickerChanged;

			#endregion

			#region buttons
			//start & stop area
			buttons = new StartStopLayout ();
			buttons.btnStart.Clicked += onStartClicked;
			buttons.btnStop.Clicked += onStopClicked;
			#endregion

			#region active job

			var activeEntry = TimeManager.GetActiveTimeEntry ();
			currentJobLabel = new Label ();
			currentJobLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
			if (activeEntry != null) {
				currentJobLabel.Text = "Active on:" + activeEntry.Description + " " + activeEntry.Code;
				currentJobLabel.TextColor = Color.Green;
			} else {
				currentJobLabel.Text = "Inactive";
				currentJobLabel.TextColor = Color.Red;
			}

			#endregion

			#region history view

			var localEntries = TimeManager.GetCompletedBillableEntries ()
				.Where (x => x.Stop > DateTime.Now.AddHours (-24)).ToList();
			h = new ObservableCollection<WorkHistory>();
			history = new HistoryListView();

			foreach(var e in localEntries)
				h.Add(history.TimeEntryToWorkHistory(e));

			history.ItemsSource = h;
			history.ItemSelected += (sender, e) => {
				if (e.SelectedItem == null)
					return; // don't do anything if we just de-selected the row
				// do something with e.SelectedItem
				WorkHistory t = e.SelectedItem as WorkHistory;
				JobCode.jobEntry.Text = t.Job;
				JobCode.codePicker.SelectedIndex = 
					billableCodes.IndexOf (billableCodes.First (b => b.code_ID == t.Code));			
				((ListView)sender).SelectedItem = null; // de-select the row
			};

			#endregion 


			this.Content = new StackLayout {
				Children = {
					JobCode,
					jobAddress,
					buttons,
					currentJobLabel,
					hLabel,
					history
				}
			};
		}

		#region methods


		void FindJob (object sender, EventArgs e)
		{
			Entry x = (Entry)sender;
			if (string.IsNullOrEmpty (x.Text))
				jobNumber = "0";
			else
				jobNumber = x.Text.Trim ();
			int jobid = Convert.ToInt32 (jobNumber);
			currentJob = JobManager.GetJobs ().FirstOrDefault (j => j.job_id == jobid);
			if (currentJob != null)
				jobAddress.Text = currentJob.facility_address;
			else {
				jobAddress.Text = "Job Not Found!";
				DisplayAlert ("Job Number", string.Format ("Can't find {0}. \nDon't worry. It's not a big deal.", jobid), "OK");
			}
		}

		void codePickerChanged (object sender, EventArgs e)
		{
			Picker x = (Picker)sender;
			selectedCodeId = Convert.ToInt32 (x.SelectedIndex);
			if (selectedCodeId > -1) {
				code = billableCodes.ElementAt (selectedCodeId);
				selectedCodeDescription = code.code_ID;
			}
		}

		void onStartClicked (object sender, EventArgs e)
		{
			//get input number
			jobNumber = JobCode.jobEntry.Text.Trim ();
			if (String.IsNullOrEmpty (jobNumber)) {
				var action = DisplayAlert ("Billable Work", 
					"You need a job number to start billable work", "OK");
				return;
			}
			if (selectedCodeId < 0) {
				var action = DisplayAlert ("Billable Work", 
					"You need to select a code to start billable work", "OK");
				return;
			}
			FindJob (JobCode.jobEntry, new EventArgs ());
			//populate current time entry
			DateTime now = DateTime.Now;
			var activeEntry = TimeManager.GetActiveTimeEntry ();
			if (activeEntry != null) {
				activeEntry.Stop = now;
				TimeManager.SaveTimeEntry (activeEntry);
				h.Add (history.TimeEntryToWorkHistory(activeEntry));
			}
			var newEntry = new TimeEntry { 
				isBillable = 1,
				device_id = imei,
				Description = jobNumber,
				Code = selectedCodeDescription,
				Start = now,
				Stop = null,
				isSynched = 0
			};
			//start job
			TimeManager.SaveTimeEntry (newEntry);

			//switch buttons
			var masterPage = this.Parent as TabbedPage;
			var nbTab = masterPage.Children [1] as NonBillableTab;
			buttons.btnStop.IsEnabled = nbTab.buttons.btnStop.IsEnabled = true;
			buttons.btnStop.BackgroundColor = nbTab.buttons.btnStop.BackgroundColor = Color.Red;

			//let current view visible
			currentJobLabel.Text = "Active on:" + newEntry.Description + " " + newEntry.Code;
			currentJobLabel.TextColor = Color.Green;

			nbTab.currentJobLabel.Text = "Active on:" + newEntry.Description + " " + newEntry.Code;
			nbTab.currentJobLabel.TextColor = Color.Green;

			new Thread (() => Forms.Context.StartService (new Intent (Forms.Context, typeof(StartLocationTimer)))).Start ();
		}

		void onStopClicked (object sender, EventArgs e)
		{
			new Thread (() => Forms.Context.StopService (new Intent (Forms.Context, typeof(StartLocationTimer)))).Start ();
			TimeManager.GoToBreak (DateTime.Now);
			#region switch labels
			//switch buttons
			//nb tab
			var masterPage = this.Parent as TabbedPage;
			var nbTab = masterPage.Children [1] as NonBillableTab;
			nbTab.buttons.btnStop.IsEnabled = buttons.btnStop.IsEnabled = false;
			nbTab.buttons.btnStop.BackgroundColor = buttons.btnStop.BackgroundColor = Color.Gray;
			nbTab.buttons.btnStart.IsEnabled = buttons.btnStart.IsEnabled = true;
			nbTab.buttons.btnStart.BackgroundColor = buttons.btnStart.BackgroundColor = Color.Green;


			currentJobLabel.Text = "Inactive";
			currentJobLabel.TextColor = Color.Red;
			nbTab.currentJobLabel.Text = "Inactive";
			nbTab.currentJobLabel.TextColor = Color.Red;
			#endregion
			//refresh history
			var newEntry = TimeManager.GetCompletedBillableEntries ()
				.OrderByDescending (x => x.Id).FirstOrDefault ();
			h.Add(history.TimeEntryToWorkHistory(newEntry));	
		}
	}
	#endregion
}


