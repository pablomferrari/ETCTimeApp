using System;
using Xamarin.Forms;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using ETCTimeApp.BL;

namespace ETCTimeApp
{
	public class EntriesTab : ContentPage
	{
		public TimeEntry[] entries { get; set; }
		public EntriesTab (TimeEntry[] e)
		{
			entries = e;
			const int lastMonth = 30;
			var now = DateTime.Now;
			var title = new Label {
				Text = "Your Recent Entries",
				HorizontalOptions = LayoutOptions.Center,
				FontAttributes = FontAttributes.Bold, FontSize = 24
			};
			var entryLabel = new Label ();
			var sb = new StringBuilder ();

			var entryText = new List<string> ();
			for (int i = 0; i < entries.Count(); i++) {
				var s =  string.Format("{0} {1} from: {2} to: {3} {4}", 
					entries [i].Description, entries [i].Code,	entries [i].Start,
					entries [i].Stop, entries [i].isSynched == 1 ? " Synched" : " Not Synched");
				entryText.Add (s);
			}
			var listview = new ListView {
				RowHeight = 65
			};
			listview.ItemsSource = entryText.ToArray ();;

			Content = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.White,
				Children = { title, listview }
			};
		}
	}
}

