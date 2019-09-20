# How to load appointments on-demand via web services in Xamarin Forms scheduler

When developing Xamarin applications for Android, iOS, and UWP, the most prevalent requirement is to access data from web services. Syncfusion Xamarin.Forms scheduler control provides all the common scheduling functionalities, which allows user to bind appointments from web services as custom appointments using the mapping technique. 

In this blog post, we will discuss how to load appointments on-demand via web services in Xamarin Forms scheduler. If you are new to the scheduler control, please read the Getting Started article in the scheduler documentation before proceeding.

## Creating a web API service 
Web services are the server-side applications that are meant to serve data or logic to various client applications. REST and SOAP are the widely used industry standard web service architecture. Use the following reference to create an ASP.NET Core web API service and host it for public access. For demo purposes, we are going to use the following hosted service.

## Creating a model class
Create a model class Appointment that contains the similar data structure in Web API service containing the appointment subject, time and other related information.

    public class Appointment
    {
        public string Subject { get; set; }
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool AllDay { get; set; }
        public string RecurrenceRule { get; set; }
        public Color Color { get; set; }
    }


## Fetching data from Web API service
In Xamarin, HttpClient provides a base class to send HTTP requests and receive HTTP responses from a web service identified by a URI. Create a helper class WebAPIService with asynchronous method RefreshDataAsync and consume the API service URI. Use GetAsync on the base URL to retrieve the appointments data using HttpClient. Use the C# await option to consume the value easily. Pass the returned object into JsonConvert.DeserializeObject to convert the JSON data into a collection of appointment and return the data to the service caller.

    public class WebAPIService
     {
        System.Net.Http.HttpClient client;
        public ObservableCollection<Appointment> Appointments { get; private set; }
        public string WebAPIUrl { get; private set; }
 
        public WebAPIService()
         {
             client = new System.Net.Http.HttpClient();
         }
 
        public async System.Threading.Tasks.Task<ObservableCollection<Appointment>> RefreshDataAsync()
         {
             WebAPIUrl = https://js.syncfusion.com/demos/ejservices/api/Schedule/LoadData;
             var uri = new Uri(WebAPIUrl);
             try
             {
                 var response = await client.GetAsync(uri);
 
                if (response.IsSuccessStatusCode)
                 {
                     var content = await response.Content.ReadAsStringAsync();
                   this.Appointments  = JsonConvert.DeserializeObject<ObservableCollection<Appointment>>(content);
                     return Appointments;
                 }
             }
             catch (Exception ex)
             {
             }
             return null;
         }
     }

## Binding and loading appointments in Scheduler
The Schedule Appointment is an MVVM-friendly feature with complete data-binding support. This allows you to bind the data fetched from the web API service in order to load and manage appointments in the Scheduler control. Create a ViewModel SchedulerViewModel with asynchronous method GetData to invoke the service call and store the received data in the proper collection.

    public class SchedulerViewModel : INotifyPropertyChanged
     {
         WebAPIService webAPIService;
         public event PropertyChangedEventHandler PropertyChanged;
         private ObservableCollection<Appointment> appointments;
 
        public ObservableCollection<Appointment> Appointments
         {
             get
             {
                 return appointments;
             }
             set
             {
                 events = value;
                 RaisepropertyChanged(" Appointments");
             }
         }
 
        public ViewModel()
         {
             webAPIService = new WebAPIService();
             this.Appointments = new ObservableCollection<Appointment>();
             GetDataFromWebAPI();
         }
 
        private async void GetDataFromWebAPI()
         {
             this.Appointments = await webAPIService.RefreshDataAsync();
        }
 
        private void RaisepropertyChanged(string propertyName)
         {
             if (PropertyChanged != null)
                 PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
         }
     }

You can bind custom data with the schedule component using mapping technique. Map the properties of the custom appointment with the equivalent properties of ScheduleAppointmentMapping class. Now, set the SchedulerViewModel to the BindingContext of scheduler to bind SchedulerViewModel properties to scheduler and assign the received appointment collection as DataSource for the scheduler.

            <syncfusion:SfSchedule 
                                    x:Name="schedule" 
                                    ScheduleView = "MonthView" 
                                    DataSource = "{Binding Events}" >
 
                <syncfusion:SfSchedule.AppointmentMapping>
                     <syncfusion:ScheduleAppointmentMapping 
                         SubjectMapping="Subject"
                         StartTimeMapping="StartTime"
                         EndTimeMapping="EndTime"
                         IsAllDayMapping="AllDay"
                         ColorMapping="Color"
                         RecurrenceRuleMapping="RecurrenceRule"/>
                 </syncfusion:SfSchedule.AppointmentMapping>
                 
                 <schedule:SfSchedule.MonthViewSettings>
                     <schedule:MonthViewSettings 
                         AppointmentDisplayMode="Appointment" />
                 </schedule:SfSchedule.MonthViewSettings>
                 
                 <syncfusion:SfSchedule.BindingContext>
                     <local:SchedulerViewModel />
                 </syncfusion:SfSchedule.BindingContext>
                 
             </syncfusion:SfSchedule>

## Conclusion
In this blog post, we’ve discussed about loading appointments on-demand via web services in Xamarin Forms scheduler. You can also check out our project samples in this GitHub repository. Feel free to try out this sample and share your feedback or questions in the comments section. You can also contact us through our support forum, Direct-Trac, or feedback portal. We are happy to assist you.


## References
https://www.syncfusion.com/blogs/post/consume-asp-net-core-web-api-in-xamarin.aspx
https://www.c-sharpcorner.com/article/how-to-fetch-data-from-web-api-using-xamarin-forms/
https://www.stacktips.com/tutorials/xamarin/consuming-rest-web-service-in-xamarin-android
http://bsubramanyamraju.blogspot.com/2017/04/xamarinforms-consuming-rest-webserivce_17.html


