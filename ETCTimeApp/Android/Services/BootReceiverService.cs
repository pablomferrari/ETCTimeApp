using Android.Content;
using Android.Widget;
using ETCTimeApp;

namespace ETCTimeApp.Droid.Services
{
	[BroadcastReceiver]
	[global::Android.App.IntentFilter (new[]{ Intent.ActionBootCompleted })]
	public class BootReceiverService : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			if (intent.Action == Intent.ActionBootCompleted) {

				if (TimeManager.GetActiveTimeEntry () != null) {
					Toast.MakeText (context, "Active task found. Restarting ETC Time App...", ToastLength.Long).Show ();
					Intent applicationIntent = new Intent (context, typeof(MainActivity));
					applicationIntent.AddFlags (ActivityFlags.NewTask);
					context.StartActivity (applicationIntent);
				}
			}
		}
	}
}

