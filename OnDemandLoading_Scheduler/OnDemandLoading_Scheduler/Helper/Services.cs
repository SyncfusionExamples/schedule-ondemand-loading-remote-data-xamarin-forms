using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandLoading_Scheduler
{
    public class WebAPIService
    {
        private HttpClient client; 
        private string WebAPIUrl { get; set; }

        public WebAPIService()
        {
            client = new HttpClient();
        }

        /// <summary>
        /// Asynchronously fetching the data from web API service.
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<Appointment>> RefreshDataAsync()
        {
            WebAPIUrl = "https://js.syncfusion.com/demos/ejservices/api/Schedule/LoadData";
            var uri = new Uri(WebAPIUrl);
            try
            {
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ObservableCollection<Appointment>>(content); 
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}
