using Xamarin.Forms;
using System.Collections.Generic;
using ETCTimeApp.BL;
using System.Linq;

namespace ETCTimeApp.VM
{
	public class JobCodeLayout : StackLayout
	{
		public Entry jobEntry { get; set; }

		public Picker codePicker { get; set; }

		public JobCodeLayout (List<ProjectCode> codes)
		{
			jobEntry = new Entry {
				Placeholder = "Enter job",
				Keyboard = Keyboard.Numeric,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			codePicker = new Picker {
				Title = "Select Code",
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			var billableCodes = codes;
			if (billableCodes.Any ()) {
				foreach (var x in billableCodes) {
					codePicker.Items.Add (x.code_ID);
				}
			} else {
				codePicker.Items.Add ("Default Code");
			}

			this.HorizontalOptions = LayoutOptions.FillAndExpand;
			this.Orientation = StackOrientation.Horizontal;
			this.Children.Add (jobEntry);
			this.Children.Add (codePicker);
		}
	}
}

