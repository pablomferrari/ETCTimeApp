using Android.Content;
using Android.Net;
using System.Threading;
using ETCTimeApp.Droid.Helpers;

namespace ETCTimeApp.Droid.Services
{
	[BroadcastReceiver]
	[global::Android.App.IntentFilter (new string[]{ ConnectivityManager.ConnectivityAction }, Priority = (int)IntentFilterPriority.LowPriority)]
	public class ConnectivityReceiverService : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			if (WiFiHelper.IsWiFiAvailable()) {
				//connected to wifi let's do sync
				Intent DbServiceIntent = new Intent ("com.ETCTimeApp.DbSyncService");
				new Thread (() => context.StartService (DbServiceIntent)).Start ();
			}
		}
	}
}

