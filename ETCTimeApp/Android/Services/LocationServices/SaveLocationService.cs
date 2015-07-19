using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Locations;

using ETCTimeApp.BL;
using Android.Provider;
using Xamarin.Forms;
using ETCTimeApp.Droid.Helpers;

namespace ETCTimeApp.Droid.Services.LocationServices
{
	[Service]
	[IntentFilter (new String[]{ "com.ETCTimeApp.SaveLocationService" })]
	public class SaveLocationService : Service, ILocationListener
	{
		Location _currentLocation;
		LocationManager _locationManager;
		double _latitude;
		double _longitude;

		string imei;

		public override void OnCreate ()
		{
			base.OnCreate ();
			imei = DeviceHelper.GetMyIMEI ();
			InitializeLocationManager ();
		}

		public override void OnDestroy ()
		{
			_locationManager.RemoveUpdates (this);
		}

		public override IBinder OnBind (Intent intent)
		{
			return null;
		}

		public void OnProviderDisabled (string provider)
		{
		}

		public void OnProviderEnabled (string provider)
		{
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
		}

		public void OnLocationChanged (Location location)
		{
			_currentLocation = location;
			_latitude = _currentLocation.Latitude;
			_longitude = _currentLocation.Longitude;

		}

		// This gets called when StartService is called in our App class
		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			//save location here
			if (_currentLocation != null) {
				_latitude = _currentLocation.Latitude;
				_longitude = _currentLocation.Longitude;
			}
			var gpsLocation = new GpsLocation {
				isSynched = 0,
				Imei = imei,
				Latitude = (float)_latitude,
				Longitude = (float)_longitude,
				OccuredAt = DateTime.Now
			};
			GpsLocationManager.SaveLocation (gpsLocation);
			return StartCommandResult.Sticky;
		}

		void InitializeLocationManager ()
		{

			_locationManager = (LocationManager)GetSystemService (LocationService);
			if (!_locationManager.IsProviderEnabled (LocationManager.NetworkProvider)) {
				buildNoGPSAlert ();
				StopSelf ();
			} else {
				if (_locationManager.AllProviders.Contains (LocationManager.NetworkProvider)
					&& _locationManager.IsProviderEnabled (LocationManager.NetworkProvider)) {
					_locationManager.RequestLocationUpdates (LocationManager.NetworkProvider, 2000, 1, this);
				} else {
					if (_locationManager.AllProviders.Contains (LocationManager.GpsProvider)
						&& _locationManager.IsProviderEnabled (LocationManager.GpsProvider)) {
						_locationManager.RequestLocationUpdates (LocationManager.GpsProvider, 2000, 1, this);					
					}
				}
			}
		}

		void buildNoGPSAlert ()
		{
			new Handler ().Post (() => Toast.MakeText (Forms.Context, "Your GPS is disabled while you are performing an activity.\nPlease enable gps.!", ToastLength.Long).Show ());
			try {
				var intent = new Intent (Settings.ActionLocationSourceSettings);
				intent.AddFlags (ActivityFlags.NewTask);
				StartActivity (intent);
			} catch (Exception e) {
				Console.WriteLine (e.Message);
			}
		}
	}
}

