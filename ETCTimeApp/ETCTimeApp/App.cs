using Xamarin.Forms;
using ETCTimeApp.VM;
using Xamarin.Forms.Platform.Android;
using System.Linq;
using Android.Content;
using System.Threading;
using ETCTimeApp.Droid.Services.LocationServices;

namespace ETCTimeApp
{
	public class App : Application
	{
		public App (FormsApplicationActivity a)
		{	
			if (!ProjectCodeManager.GetProjectCodes ().Any () || !JobManager.GetJobs ().Any ()) {
				MainPage = new LoadingTab (a);
				//return load;
			}	
			var activeEntry = TimeManager.GetActiveTimeEntry ();
			if (activeEntry != null) {
				new Thread (() => Forms.Context.StartService (new Intent (a, typeof(StartLocationTimer)))).Start ();
			}
			MainPage = new NavigationPage (new MainContent (a));
		}
	}
}

