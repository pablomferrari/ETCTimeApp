using System;
using Xamarin.Forms;
using Android.Content;
using ETCTimeApp.Droid;
using System.Linq;
using Xamarin.Forms.Platform.Android;
using Android.Views.Animations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ETCTimeApp.VM
{
	// Your BaseViewModel MUST inherit from INotifyProprtyChanged.. it's what makes everything work
	public class BaseViewModel : INotifyPropertyChanged
	{
		// here's your shared IsBusy property
		private bool _isBusy;

		public bool IsBusy {
			get { return _isBusy; }
			set {
				_isBusy = value;
				// again, this is very important
				OnPropertyChanged ();
			}
		}

		// this little bit is how we trigger the PropertyChanged notifier.
		public event PropertyChangedEventHandler PropertyChanged;

		public virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler (this, new PropertyChangedEventArgs (propertyName));
		}
	}


}

