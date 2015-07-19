using Android.Content;
using Android.Net;
using Xamarin.Forms;

namespace ETCTimeApp.Droid.Helpers
{
	public static class WiFiHelper
	{
		public static bool IsWiFiAvailable ()
		{
			var cm = (ConnectivityManager)Forms.Context.GetSystemService (Context.ConnectivityService);
			var mobileState = cm.GetNetworkInfo (ConnectivityType.Wifi).GetState ();
			return mobileState == NetworkInfo.State.Connected;
		}
	}
}

