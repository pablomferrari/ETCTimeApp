using System;
using Android.App;
using Android.Content;
using Android.OS;
using ETCTimeApp.Android;
using Android.Support.V4.App;
using Xamarin.Forms;

namespace ETCTimeApp.Droid.Services.LocationServices
{
	[Service]
	[IntentFilter (new String[]{ "com.ETCTimeApp.StartLocationTimer" })]
	public class StartLocationTimer : Service
	{
		public AlarmManager alarm;
		public PendingIntent pendingSaveLocationServiceIntent;


		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			var LocationServiceIntent = new Intent ("com.ETCTimeApp.SaveLocationService");
			pendingSaveLocationServiceIntent = PendingIntent.GetService (this, 0, LocationServiceIntent, 0);
			alarm = (AlarmManager)this.BaseContext.GetSystemService (Context.AlarmService);
			//repeat every 10 minutes
			alarm.SetRepeating (AlarmType.RtcWakeup,
				10000, 
				1 * 5000 * 60,
				pendingSaveLocationServiceIntent);

			var pendingIntent = PendingIntent.GetActivity (this, 1, new Intent (this, typeof(MainActivity)), 0);
			var resultString = "Location service is running on background!";

			int ic_small = Resource.Drawable.gps_small;
			var builder = new NotificationCompat.Builder (this)
				.SetAutoCancel (true)
				.SetContentIntent (pendingIntent)
				.SetContentTitle ("ETC Location Notification")
				.SetSmallIcon (ic_small)
				.SetContentText (resultString);

			// start our service foregrounded, that way it won't get cleaned up from memory pressure
			StartForeground ((int)NotificationFlags.ForegroundService, builder.Build ());

			return StartCommandResult.Sticky;
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			alarm.Cancel (pendingSaveLocationServiceIntent);
			pendingSaveLocationServiceIntent.Cancel ();
			Forms.Context.StopService (new Intent (Forms.Context, typeof(SaveLocationService)));
		}

		public override IBinder OnBind (Intent intent)
		{
			return null;
		}

	}
}

