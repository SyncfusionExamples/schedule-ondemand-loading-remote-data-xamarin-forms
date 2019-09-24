using Syncfusion.SfBusyIndicator.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace OnDemandLoading_Scheduler
{
    public class SchedulerViewModel : INotifyPropertyChanged
    {
        private WebAPIService webAPIService;
        private ObservableCollection<Appointment> appointments;
        private ObservableCollection<Appointment> webData;
        private List<Color> colorCollection;
        private List<DateTime> visibleDates;

        public SfBusyIndicator BusyIndicator;
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

            try
            {
                var random = new Random();
                foreach (var scheduleEvent in this.WebData)
                {
                    //// random color added for web appointments
                    scheduleEvent.Color = this.colorCollection[random.Next(9)];
                }
            }
            catch (Exception ex)
            { 
            }
        }

        /// <summary>
        /// Updates the appointment collection property to load appointments on demand.
        /// </summary>
        public async void UpdateAppointments()
        {
            if (this.visibleDates == null)
                return;

            if (this.BusyIndicator != null && this.BusyIndicator.IsBusy)
            {
                await System.Threading.Tasks.Task.Delay(5000);
                this.BusyIndicator.IsBusy = false;
            }

            if (this.webData == null || this.webData.Count == 0)
                return;

            var filteredAppointments = new ObservableCollection<Appointment>();

            foreach (Appointment App in this.webData)
            {
                if ((this.visibleDates.First() <= App.StartTime.Date && this.visibleDates.Last() >= App.StartTime.Date)  ||
                    (this.visibleDates.First() <= App.EndTime.Date && this.visibleDates.Last() >= App.EndTime.Date))
                {
                    filteredAppointments.Add(App);
                }
            }

            this.Appointments = filteredAppointments;
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
}
