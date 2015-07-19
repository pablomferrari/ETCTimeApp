using Android.Content;
using Xamarin.Forms;
using Android.Telephony;

namespace ETCTimeApp.Droid.Helpers
{
	public static class DeviceHelper
	{
		public static string GetMyIMEI ()
		{
			string IMEI = "";
			var phone = (TelephonyManager)Forms.Context.GetSystemService (Context.TelephonyService); 
			if (phone != null)
			if (phone.DeviceId != null)
				IMEI = phone.DeviceId;
			return IMEI;
		}
	}
}

