using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ETCTimeApp.Android.Helpers;
using ETCTimeApp.BL;
using Newtonsoft.Json;

namespace ETCTimeApp.Droid.Helpers
{
	public class DbDownloadHelper
	{
	    public string baseUrl = ApiHelper.BaseURL();


		public bool jobsDone { get; set; }

		public bool nbCodesDone { get; set; }

		public bool bCodesDone { get; set; }


		public string message { get; set; }

		public List<Job> jobs { get; set; }

		public List<ProjectCode> nbCodes { get; set; }

		public List<ProjectCode> bCodes { get; set; }

        public List<TimeEntry> SynchedEntries { get; set; } 

		public string Version { get; set; }

		public DbDownloadHelper ()
		{
			message = "Sync completed!";
			jobsDone = nbCodesDone = bCodesDone = false;
		}

		public bool DownloadFailed()
		{
			return !jobsDone || !nbCodesDone || !bCodesDone;
		}

		public async Task GetData ()
		{
			try {
				await Task.Run (() => DownloadJobs ())
					.ContinueWith (task => DownloadBillableCodes ())
					.ContinueWith (task => DownloadNonBillableCodes ())
					.ContinueWith (task => DownloadVersion ());

			} catch (Exception e) {
			} 
		}

		public async Task DownloadJobs ()
		{
			var downloadedJobs = new List<Job> ();
			using (var client = new HttpClient ()) {
				client.BaseAddress = new Uri (baseUrl);
				client.DefaultRequestHeaders.Accept.Clear ();
				client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));

				try {
					var response = client.GetAsync ("jobs").Result;
					if (response.IsSuccessStatusCode) {
						string responseContent = response.Content.ReadAsStringAsync ().Result;
						downloadedJobs = JsonConvert.DeserializeObject<List<Job>> (responseContent);
						jobsDone = true;
					} else {
						jobsDone = false;
						message = "Something went wrong while downloading jobs.";
					}
				} catch (Exception e) {}
			}
			jobs = downloadedJobs;
		}

		public async Task DownloadBillableCodes ()
		{
			var downloadedBCodes = new List<ProjectCode> ();
			using (var client = new HttpClient ()) {
				client.BaseAddress = new Uri (baseUrl);
				client.DefaultRequestHeaders.Accept.Clear ();
				client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));

				try {
					var response = client.GetAsync ("billableCodes").Result;
					if (response.IsSuccessStatusCode) {
						string responseContent = response.Content.ReadAsStringAsync ().Result;
						downloadedBCodes = JsonConvert.DeserializeObject<List<ProjectCode>> (responseContent);
						bCodesDone = true;
					} else {
						bCodesDone = false;
						message = "Something went wrong while downloading billable codes.";
					}
				} catch (Exception e) {			
				}
			}
			bCodes = downloadedBCodes;
		}

		public async Task DownloadNonBillableCodes ()
		{
			var downloadedNBCodes = new List<ProjectCode> ();
			using (var client = new HttpClient ()) {
				client.BaseAddress = new Uri (baseUrl);
				client.DefaultRequestHeaders.Accept.Clear ();
				client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));

				try {
					var response = await client.GetAsync ("nonbillableCodes");
					if (response.IsSuccessStatusCode) {
						string responseContent = response.Content.ReadAsStringAsync ().Result;
						downloadedNBCodes = JsonConvert.DeserializeObject<List<ProjectCode>> (responseContent);
						nbCodesDone = true;
					} else {
						nbCodesDone = false;
						message = "Something went wrong while downloading non-billable codes.";
					}
				} catch (Exception e) {					
				}
			}
			nbCodes = downloadedNBCodes;
		}

	    public List<TimeEntry> DownloadEntriesByIMEI(string imei)
	    {
            var synchedEntries = new List<TimeEntry>();
            var myIMEI = imei;
	        using (var client = new HttpClient())
	        {
	            client.BaseAddress = new Uri(baseUrl);
	            client.DefaultRequestHeaders.Accept.Clear();
	            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

	            try
	            {
	                var response = client.GetAsync("time/" + myIMEI).Result;
                    string responseContent = response.Content.ReadAsStringAsync().Result;
					synchedEntries = JsonConvert.DeserializeObject<List<TimeEntry>>(responseContent);
	            }
	            catch (Exception e){
	                
	            }
				return synchedEntries;
	        }
	    }

		public void DownloadVersion ()
		{
			WebRequest request = WebRequest.Create ("http://api.2etc.com/version");

			// Get the response.
			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();

			// Get the stream containing content returned by the server.
			Stream dataStream = response.GetResponseStream ();
			// Open the stream using a StreamReader for easy access.
			StreamReader reader = new StreamReader (dataStream);
			// Read the content.
			string responseFromServer = reader.ReadToEnd ();
			// Display the content.
			Version = responseFromServer;
			// Cleanup the streams and the response.
			reader.Close ();
			dataStream.Close ();
			response.Close ();
		}




	}
}

