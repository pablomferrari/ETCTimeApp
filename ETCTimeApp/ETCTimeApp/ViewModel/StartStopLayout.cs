using Xamarin.Forms;

namespace ETCTimeApp.VM
{
	public class StartStopLayout : StackLayout
	{
		public Button btnStart { get; set; }

		public Button btnStop { get; set; }

		public StartStopLayout ()
		{
			btnStart = new Button {
				Text = "Start",
				Font = Font.SystemFontOfSize (NamedSize.Large),
				TextColor = Color.Black,
				BackgroundColor = Color.Green,
				BorderRadius = 10,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			btnStop = new Button {
				Text = "Break",
				Font = Font.SystemFontOfSize (NamedSize.Large),
				TextColor = Color.Black,
				BorderRadius = 10,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			var activeEntry = TimeManager.GetActiveTimeEntry();
			if (activeEntry != null) {
				btnStop.IsEnabled = true;
				btnStop.BackgroundColor = Color.Red;

			} else {
				btnStop.IsEnabled = false;
				btnStop.BackgroundColor = Color.Gray;
			}
			this.HorizontalOptions = LayoutOptions.FillAndExpand;
			this.Orientation = StackOrientation.Horizontal;
			this.Children.Add (btnStart);
			this.Children.Add (btnStop);
		}
	}
}

