using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ETCTimeApp.BL;
using ETCTimeApp.Droid.Helpers;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ETCTimeApp.Android.Helpers
{
    public class DbUploadHelper
    {
        public string baseUrl = ApiHelper.BaseURL();
        public bool entriesDone { get; set; }
        public bool gpsDone { get; set; }
        public string message { get; set; }
        public bool failed { get; set; }

		public async Task UploadAll(List<TimeEntry> timeEntries, List<GpsLocation> gpsLocations)
		{
			UploadTimeEntries (timeEntries);
			UploadLocations (gpsLocations);
				
		}

		public bool UploadFailed(){
			return !entriesDone || !gpsDone;
		}

        public async Task UploadLocations(List<GpsLocation> gpsLocations)
        {		
            
			if (gpsLocations.Any())
            {
				var locations = gpsLocations.ToArray();
				var locationJSON = JsonConvert.SerializeObject (locations);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
						HttpResponseMessage response = 
							client.PostAsync ("GPS/Upload",
								new StringContent (locationJSON, System.Text.Encoding.UTF8, "application/json")).Result;
						if(response.IsSuccessStatusCode)
							gpsDone = true;
                    }
					catch (Exception e){
						message = "upload locations failed " + e.Data;
						gpsDone = false;
					}
                }
            }
        }

        public async Task UploadTimeEntries(List<TimeEntry> timeEntries)
        {           //get all unsynched entries			
			if (timeEntries.Any())
            {
				var entries = timeEntries.ToArray ();
				var entriesJson = JsonConvert.SerializeObject (timeEntries);
                using (var client = new HttpClient())
                {
					client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
						HttpResponseMessage response = 
							client.PostAsync ("Time/Upload",
								new StringContent (entriesJson, System.Text.Encoding.UTF8, "application/json")).Result;
						if(response.IsSuccessStatusCode)
							entriesDone = true;
						else{
							Console.WriteLine(response.ToString());
						}				
							
                    }
					catch (Exception e){
						message = "upload entries failed " + e.Data;
						entriesDone = false;
                    }

                }
            }            
        }

    }
}