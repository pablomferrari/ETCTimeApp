using Android.App;
using Android.Content.PM;
using Android.OS;

using Xamarin.Forms.Platform.Android;


namespace ETCTimeApp.Droid
{
	[Activity (Label = "ETC TimeApp 2.4", MainLauncher = true
		, Theme = "@android:style/Theme.Holo.Light"
		, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

	public class MainActivity : FormsApplicationActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			Xamarin.Forms.Forms.Init (this, savedInstanceState);
			LoadApplication (new App(this));
		}
	}
}

