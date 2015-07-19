using System;
using Xamarin.Forms;
using Android.Telephony;
using Android.Content;
using Xamarin.Forms.Platform.Android;

namespace ETCTimeApp.VM
{
	public class Login : ContentPage
	{
		private Entry user;
		private Entry password;
		private string phoneNumber;
		private AndroidActivity _act;

		public Login (AndroidActivity a)
		{
			this.Padding = 10;
			_act = a;
			var titleLabel = new Label {
				Text = "Login Page"
			};
			titleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
			titleLabel.Font = Font.SystemFontOfSize (NamedSize.Large);
			TelephonyManager phone = (TelephonyManager)a.GetSystemService (Context.TelephonyService); 
			string name = "Enter your name: ";
			user = new Entry {
				Placeholder = name,
				Keyboard = Keyboard.Text,
				HorizontalOptions = LayoutOptions.CenterAndExpand,

			};

			Button go = new Button {
				Text = "Login",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Gray,
				TextColor = Color.Blue
			};

			go.Clicked += goToContent;
			this.Content = new StackLayout {
				Children = {
					titleLabel,
					user,
					password,
					go
				}
			};

		}

		private async void goToContent (object sender, EventArgs e)
		{
			var username = user.Text;
			var pwd = password.Text;
			bool goOn = true;
			if (pwd != username.Substring (username.Length - 4)) {
				var action = await DisplayAlert ("ETC credentials", 
					             "Your credentials were not recognized.\n " +
					             "Click Continue if you want to disregard this message and contact your admin or Cancel to try again."
				, "Cancel", "Continue");
				if (action)
					goOn = false;
				else
					goOn = true;
			}
			if (goOn) {
				var Billable = new BillableTab (phoneNumber, _act);
				Billable.IsEnabled = false;
				Billable.Title = "Billable Time";
				var NonBillable = new NonBillableTab (phoneNumber, _act);
				NonBillable.IsEnabled = false;
				NonBillable.Title = "Non Billable Time";
				var masterPage = this.Parent as TabbedPage;
				masterPage.Children.Add (Billable);
				masterPage.Children.Add (NonBillable);
				masterPage.Children.Remove (this);
				masterPage.Title = "ETC Time Track";
			}
		}
	}
}


