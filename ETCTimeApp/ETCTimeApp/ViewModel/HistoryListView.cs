using System;
using Xamarin.Forms;
using System.Collections.Generic;
using ETCTimeApp.BL;
using ETCTimeApp.VM;
using System.Linq;

namespace ETCTimeApp
{
	public class HistoryListView : ListView
	{
		public string Description { get; set; }
		public int CodeIndex { get; set; }

		public HistoryListView ()
		{
			Description = "";
			CodeIndex = 0;
			ItemTemplate = new DataTemplate (() => {
				var cell = new ImageCell ();
				cell.ImageSource = ImageSource.FromFile ("upload.png");
				cell.SetBinding (TextCell.TextProperty, "Header");
				cell.SetBinding (TextCell.DetailProperty, "Description");
				return cell;
			});
		}

		public WorkHistory TimeEntryToWorkHistory(TimeEntry e)
		{
			return new WorkHistory {
				Header = e.Description,
				Description = "Code: " + e.Code + "\nElapsed Time: " + String.Format ("{0:g}", e.Stop - e.Start),
				Code = e.Code,
				Job = e.Description
			};
		}
	}
}

