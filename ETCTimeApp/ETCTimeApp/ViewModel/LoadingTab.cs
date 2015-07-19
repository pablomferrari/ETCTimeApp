using Xamarin.Forms;
using Android.Content;
using Xamarin.Forms.Platform.Android;
using System.Threading;
using ETCTimeApp.Droid.Helpers;

namespace ETCTimeApp.VM
{
	public class LoadingTab : ContentPage
	{

		// you need a view model to bind to
		private ActivityPageViewModel ViewModel { get; set; }

		public LoadingTab (FormsApplicationActivity act)
		{
			ViewModel = new ActivityPageViewModel ();
			BindingContext = ViewModel;

			var label = new Label {
				Text = "\tInstalling app. Please wait.\n\tIt may take a few minutes...",
				HorizontalOptions = LayoutOptions.Center,
				FontSize = 18, FontAttributes = FontAttributes.Bold
			};

			if (!WiFiHelper.IsWiFiAvailable()) {
				DisplayAlert ("Sync", "It looks like you do not have internet connection", "OK");
				return;
			}
			//connected to wifi let's do sync
			Intent DbServiceIntent = new Intent ("com.ETCTimeApp.DbSyncService");
			new Thread (() => act.StartService (DbServiceIntent)).Start ();

			// here's your activity indicator, it's bound to the IsBusy property of the BaseViewModel
			// those bindings are on both the visibility property as well as the IsRunning property
			var activityIndicator = new ActivityIndicator {
				Color = Color.Black,
			};
			activityIndicator.SetBinding (VisualElement.IsVisibleProperty, "IsBusy");
			activityIndicator.SetBinding (ActivityIndicator.IsRunningProperty, "IsBusy");

			// return the layout that includes all the above elements
			Content = new StackLayout {
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				BackgroundColor = Color.White,
				Children = { label, activityIndicator }
			};
		}
	}
}

