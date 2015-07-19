using System;
using Xamarin.Forms;
using ETCTimeApp.BL;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using System.Threading;
using ETCTimeApp.Droid.Services.LocationServices;
using ETCTimeApp.Droid.Helpers;
using System.Collections.ObjectModel;

namespace ETCTimeApp.VM
{
	public class NonBillableTab : ContentPage
	{
		#region variables

		public StartStopLayout buttons;
		public List<ProjectCode> nonBillableCodes;
		public JobCodeLayout JobCode;
		public Entry generalEntry;
		public ProjectCode code;
		public int selectedCodeId;
		public string selectedCodeDescription;

		public Label currentJobLabel;
		public HistoryListView history;
		public ObservableCollection<WorkHistory> h;



		public string imei;

		#endregion

		public NonBillableTab ()
		{
			imei = DeviceHelper.GetMyIMEI();

			#region titles
			this.Padding = 10;
			this.Title = "ETC TimeTrack Non-Billable";
			var hLabel = new Label {
				Text = "Your Non Billable History",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				TextColor = Color.Black,
				FontSize = 14,
				FontFamily = "Helvetica"
			};
			#endregion
			#region selections
			this.selectedCodeId = -1;
			generalEntry = new Entry {
				Placeholder = "Enter description..."
			};
			//job & code area
			nonBillableCodes = ProjectCodeManager.GetNonBillableCodes ();
			JobCode = new JobCodeLayout (nonBillableCodes);
			JobCode.jobEntry.IsVisible = false;
			JobCode.codePicker.HorizontalOptions = LayoutOptions.FillAndExpand;
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

			#region history listview
			var localEntries = TimeManager.GetCompletedNonBillableEntries ()
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
				var t = e.SelectedItem as WorkHistory;
				generalEntry.Text = t.Header;
				JobCode.codePicker.SelectedIndex = 
					nonBillableCodes.IndexOf (nonBillableCodes.First (b => b.code_ID == t.Code));
				((ListView)sender).SelectedItem = null; // de-select the row
			};
			#endregion 

			this.Content = new StackLayout {
				Children = {
					generalEntry,
					JobCode,
					buttons,
					currentJobLabel,
					hLabel,
					history
				}
			};	
		}

		void codePickerChanged (object sender, EventArgs e)
		{
			Picker x = (Picker)sender;
			selectedCodeId = Convert.ToInt32 (x.SelectedIndex);
			if (selectedCodeId > -1) {
				code = nonBillableCodes.ElementAt (this.selectedCodeId);
				selectedCodeDescription = code.code_ID;
			}
		}

		void onStartClicked (object sender, EventArgs e)
		{
			//check for null boxes
			if (String.IsNullOrEmpty (generalEntry.Text)) {
				var action = DisplayAlert ("Billable Work", 
					"You need some description to start this entry.", "OK");
				return;
			}
			if (this.selectedCodeId < 0) {
				var action = DisplayAlert ("Billable Work", 
					"You need to select a code to start non billable work", "OK");
				return;
			}


			//populate current time entry
			DateTime now = DateTime.Now;
			var activeEntry = TimeManager.GetActiveTimeEntry ();
			if (activeEntry != null) {
				activeEntry.Stop = now;
				TimeManager.SaveTimeEntry (activeEntry);
				//refreshes history
				h.Add (history.TimeEntryToWorkHistory(activeEntry));
			}
			var newEntry = new TimeEntry { 
				isBillable = 0,
				device_id = imei,
				Description = generalEntry.Text,
				Code = selectedCodeDescription,
				Start = now,
				Stop = null,
				isSynched = 0				
			};

			//start job
			TimeManager.SaveTimeEntry (newEntry);

			//switch buttons
			var masterPage = this.Parent as TabbedPage;
			var bTab = masterPage.Children [0] as BillableTab;
			buttons.btnStop.IsEnabled = bTab.buttons.btnStop.IsEnabled = true;
			buttons.btnStop.BackgroundColor = bTab.buttons.btnStop.BackgroundColor = Color.Red;

			//let current view visible
			currentJobLabel.Text = "Active on:" + newEntry.Description + " " + newEntry.Code;
			currentJobLabel.TextColor = Color.Green;

			bTab.currentJobLabel.Text = "Active on:" + newEntry.Description + " " + newEntry.Code;
			bTab.currentJobLabel.TextColor = Color.Green;

			//start location service
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
			var bTab = masterPage.Children [0] as BillableTab;
			bTab.buttons.btnStop.IsEnabled = buttons.btnStop.IsEnabled = false;
			bTab.buttons.btnStop.BackgroundColor = buttons.btnStop.BackgroundColor = Color.Gray;
			bTab.buttons.btnStart.IsEnabled = buttons.btnStart.IsEnabled = true;
			bTab.buttons.btnStart.BackgroundColor = buttons.btnStart.BackgroundColor = Color.Green;


			currentJobLabel.Text = "Inactive";
			currentJobLabel.TextColor = Color.Red;
			bTab.currentJobLabel.Text = "Inactive";
			bTab.currentJobLabel.TextColor = Color.Red;
			#endregion
			//refresh history
			var newEntry = TimeManager.GetCompletedNonBillableEntries ()
				.OrderByDescending (x => x.Id).FirstOrDefault ();
			h.Add(history.TimeEntryToWorkHistory(newEntry));
		}
	}
}

