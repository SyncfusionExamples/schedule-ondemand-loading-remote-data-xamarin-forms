# How to load appointments on-demand via web services in Xamarin Forms scheduler

When developing Xamarin applications for Android, iOS, and UWP, the most prevalent requirement is to access data from web services. Syncfusion Xamarin.Forms scheduler control provides all the common scheduling functionalities, which allows user to load and bind appointments on demand from web services as custom appointments using the mapping technique. 

In this blog post, we will discuss how to load appointments on-demand via web services in Xamarin Forms scheduler. If you are new to the scheduler control, please read the Getting Started article in the scheduler documentation before proceeding.

## Creating a web API service 
Web services are the server-side applications that are meant to serve data or logic to various client applications. REST and SOAP are the widely used industry standard web service architecture. Use the following reference to create an ASP.NET Core web API service and host it for public access. For demo purposes, we are going to use the following hosted service.

## Creating a model class
Create a model class Appointment that contains the similar data structure in Web API service containing the appointment subject, time and other related information.

    /// <summary>   
    /// Represents custom data properties.   
    /// </summary> 
    public class Appointment
    {
        /// <summary>
        ///  Gets or sets the subject of the appointment. 
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///  Gets or sets the id of the appointment. 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///  Gets or sets the start time of the appointment. 
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///  Gets or sets the end time of the appointment. 
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        ///  Gets or sets a boolean property indicates whether the appointment is all day. 
        /// </summary>
        public bool AllDay { get; set; }

        /// <summary>
        ///  Gets or sets the recurrence rule of the appointment. 
        /// </summary>
        public string RecurrenceRule { get; set; }

        /// <summary>
        ///  Gets or sets the color of the appointment. 
        /// </summary>
        public Color Color { get; set; }
    }

## Fetching data from Web API service
In Xamarin, HttpClient provides a base class to send HTTP requests and receive HTTP responses from a web service identified by a URI. Create a helper class WebAPIService with asynchronous method RefreshDataAsync and consume the API service URI. Use GetAsync on the base URL to retrieve the appointments data using HttpClient. Use the C# await option to consume the value easily. Pass the returned object into JsonConvert.DeserializeObject to convert the JSON data into a collection of appointment and return the data to the service caller.

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

## Binding remote data in Scheduler
The Schedule Appointment is an MVVM-friendly feature with complete data-binding support. This allows you to bind the data fetched from the web API service in order to load and manage appointments in the Scheduler control. Create a view model SchedulerViewModel with asynchronous method GetData to invoke the service call and store the received data in the proper collection.

    public class SchedulerViewModel : INotifyPropertyChanged
    {
        private WebAPIService webAPIService;
        private ObservableCollection<Appointment> appointments;
        private ObservableCollection<Appointment> webData;
        private List<Color> colorCollection;
        private List<DateTime> visibleDates;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the visible dates in scheduler. 
        /// </summary>
        public List<DateTime> VisibleDates
        {
            get
            {
                return visibleDates;
            }
            set
            {
                this.visibleDates = value;
                RaisepropertyChanged("VisibleDates");
            }
        }

        /// <summary>
        /// Gets or sets the data fetched from web API service. 
        /// </summary>
        public ObservableCollection<Appointment> WebData
        {
            get
            {
                return webData;
            }
            set
            {
                webData = value;
                this.UpdateAppointments();
                RaisepropertyChanged("WebData");
            }
        }

        /// <summary>
        ///  Gets or sets the appointments. 
        /// </summary>
        public ObservableCollection<Appointment> Appointments
        {
            get
            {
                return this.appointments;
            }
            set
            {
                this.appointments = value;
                RaisepropertyChanged("Appointments");
            }
        }

        public SchedulerViewModel()
        {
            this.webAPIService = new WebAPIService();
            this.Appointments = new ObservableCollection<Appointment>();
            this.InitializeEventColor();
            this.GetDataFromWebAPI();
        }

        private async void GetDataFromWebAPI()
        {
            this.WebData = await webAPIService.RefreshDataAsync();

            var random = new Random();
            foreach (var scheduleEvent in this.WebData)
            {
                //// random color added for web appointments
                scheduleEvent.Color = this.colorCollection[random.Next(9)];
            }
        }

        private void InitializeEventColor()
        {
            this.colorCollection = new List<Color>();
            this.colorCollection.Add(Color.FromHex("#FF339933"));
            this.colorCollection.Add(Color.FromHex("#FF00ABA9"));
            this.colorCollection.Add(Color.FromHex("#FFE671B8"));
            this.colorCollection.Add(Color.FromHex("#FF1BA1E2"));
            this.colorCollection.Add(Color.FromHex("#FFD80073"));
            this.colorCollection.Add(Color.FromHex("#FFA2C139"));
            this.colorCollection.Add(Color.FromHex("#FFA2C139"));
            this.colorCollection.Add(Color.FromHex("#FFD80073"));
            this.colorCollection.Add(Color.FromHex("#FF339933"));
            this.colorCollection.Add(Color.FromHex("#FFE671B8"));
            this.colorCollection.Add(Color.FromHex("#FF00ABA9"));
        }

        private void RaisepropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

You can bind custom data with the schedule component using mapping technique. Map the properties of the custom appointment with the equivalent properties of ScheduleAppointmentMapping class. Now, set the SchedulerViewModel to the BindingContext of scheduler to bind SchedulerViewModel properties to scheduler and assign the received appointment collection as DataSource for the scheduler.


<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:syncfusion="clr-namespace:Syncfusion.SfSchedule.XForms;assembly=Syncfusion.SfSchedule.XForms"
             xmlns:local="clr-namespace:OnDemandLoading_Scheduler"
             x:Class="OnDemandLoading_Scheduler.SchedulerPage">
    <ContentPage.Content>
        <syncfusion:SfSchedule x:Name="schedule" 
                               ScheduleView = "MonthView" 
                               DataSource = "{Binding Appointments}" >

            <syncfusion:SfSchedule.AppointmentMapping>
                <syncfusion:ScheduleAppointmentMapping 
                         SubjectMapping="Subject"
                         StartTimeMapping="StartTime"
                         EndTimeMapping="EndTime"
                         IsAllDayMapping="AllDay"
                         ColorMapping="Color"
                         RecurrenceRuleMapping="RecurrenceRule"/>
            </syncfusion:SfSchedule.AppointmentMapping>

            <syncfusion:SfSchedule.MonthViewSettings>
                <syncfusion:MonthViewSettings AppointmentDisplayMode="Appointment" />
            </syncfusion:SfSchedule.MonthViewSettings>

            <syncfusion:SfSchedule.BindingContext>
                <local:SchedulerViewModel />
            </syncfusion:SfSchedule.BindingContext>

        </syncfusion:SfSchedule>
    </ContentPage.Content>

    <ContentPage.Behaviors>
        <local:SchedulerPageBehavior/>
    </ContentPage.Behaviors>

</ContentPage>

## Loading appointments on-demand
On initial load, you can update the load the filtered appointments in visible dates range for all the scheduler views. Also, you can update the appointments in scheduler when data is asynchronously changed in web.

        /// <summary>
        /// Gets or sets the data fetched from web API service. 
        /// </summary>
        public ObservableCollection<Appointment> WebData
        {
            get
            {
                return webData;
            }
            set
            {
                webData = value;
                this.UpdateAppointments();
                RaisepropertyChanged("WebData");
            }
        }

               …

        /// <summary>
        /// Updates the appointment collection property to load appointments on demand.
        /// </summary>
        public void UpdateAppointments()
        {
            if (this.visibleDates == null)
                return;

            if (this.webData == null || this.webData.Count == 0)
                return;

            var appointments = new ObservableCollection<Appointment>();

            foreach (Appointment App in this.webData)
            {
                if ((this.visibleDates.First() <= App.StartTime.Date && this.visibleDates.Last() >= App.StartTime.Date)  ||
                    (this.visibleDates.First() <= App.EndTime.Date && this.visibleDates.Last() >= App.EndTime.Date))
                {
                    appointments.Add(App);
                }
            }

            this.Appointments = appointments;
        }


On view swiping, you can update the filtered appointments in visible dates range by using VisibleDatesChangedEvent of scheduler control.

        this.schedule.VisibleDatesChangedEvent += OnVisibleDatesChangedEvent;

                …

        private void OnVisibleDatesChangedEvent(object sender, VisibleDatesChangedEventArgs e)
        {
            if (schedule.BindingContext == null)
                return;

            var scheduleViewModel = schedule.BindingContext as SchedulerViewModel;
            scheduleViewModel.VisibleDates = e.visibleDates;
            scheduleViewModel.UpdateAppointments();
        }

Now, scheduler control is configured with an application to load appointments on-demand via web API service. Just running the sample with the previous steps will render a scheduler with appointments.

## Conclusion
In this blog post, we’ve discussed about loading appointments on-demand via web services in Xamarin Forms scheduler. You can also check out our project samples in this GitHub repository. Feel free to try out this sample and share your feedback or questions in the comments section. You can also contact us through our support forum, Direct-Trac, or feedback portal. We are happy to assist you.


## References
https://www.syncfusion.com/blogs/post/consume-asp-net-core-web-api-in-xamarin.aspx
https://www.c-sharpcorner.com/article/how-to-fetch-data-from-web-api-using-xamarin-forms/
https://www.stacktips.com/tutorials/xamarin/consuming-rest-web-service-in-xamarin-android
http://bsubramanyamraju.blogspot.com/2017/04/xamarinforms-consuming-rest-webserivce_17.html


