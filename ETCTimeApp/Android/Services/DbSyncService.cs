using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using ETCTimeApp.Android;
using ETCTimeApp.Droid.Helpers;
using ETCTimeApp.Android.Helpers;
using Xamarin.Forms;

namespace ETCTimeApp.Droid.Services
{
	[Service]
	[IntentFilter (new String[]{ "com.ETCTimeApp.DbSyncService" })]
	public class DbSyncService : IntentService
	{
		public bool isRunning;

		public DbSyncService ()
		{
			isRunning = false;
		}

		protected override async void OnHandleIntent (Intent intent)
		{
			var download = new DbDownloadHelper ();
			var upload = new DbUploadHelper ();
			var newEntries = TimeManager.GetUnSynchedTimeeEntries ();
			var newLocations = GpsLocationManager.GetUnSynchedGpsLocations ();
			//getting jobs, codes and version
			await download.GetData();
			await upload.UploadAll (newEntries, newLocations);

			//wait until is over
			var success = !download.DownloadFailed () && !upload.UploadFailed ();
			if (download.bCodes.Any () && download.nbCodes.Any () && download.jobs.Any ()) {
				var update = new DbUpdateHelper ();
				update.UpdateAll (download.jobs, download.bCodes, download.nbCodes, newEntries, newLocations);
			}

			string resultString = success ? "Synch Successfull" : "Synch Unsuccessfull";
			string textString = download.message + " " + upload.message;

			var ic = success ? Resource.Drawable.sync : Resource.Drawable.unsynch;

			var pendingIntent = PendingIntent.GetActivity (this, 0, new Intent (this, typeof(MainActivity)), 0);

			var builder = new NotificationCompat.Builder (this)
				.SetAutoCancel (true)
				.SetContentIntent (pendingIntent)
				.SetContentTitle (resultString)
				.SetSmallIcon (ic)
				.SetContentInfo ("ETC Sync Notification")
				.SetContentText (textString);

			// Finally publish the notification
			var notificationManager = (NotificationManager)GetSystemService (NotificationService);
			notificationManager.Notify (0, builder.Build ());
			Intent applicationIntent = new Intent (Forms.Context, typeof(MainActivity));
			applicationIntent.AddFlags (ActivityFlags.NewTask);
			//Forms.Context.StartActivity (applicationIntent);
		}
	}
}


