using Syncfusion.SfBusyIndicator.XForms;
using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace OnDemandLoading_Scheduler
{
    public class SchedulerPageBehavior : Behavior<ContentPage>
    {
        SfSchedule schedule;
        SfBusyIndicator busyIndicator;
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            this.schedule = bindable.Content.FindByName<SfSchedule>("schedule");
            this.busyIndicator = bindable.Content.FindByName<SfBusyIndicator>("busyindicator");
            this.schedule.MoveToDate = new DateTime(2017, 06, 15);
            this.WireEvents();
        }
        private void WireEvents()
        {
            this.schedule.VisibleDatesChangedEvent += OnVisibleDatesChangedEvent;
        }

        private void OnVisibleDatesChangedEvent(object sender, VisibleDatesChangedEventArgs e)
        {
            if (schedule.BindingContext == null)
                return;

            var scheduleViewModel = schedule.BindingContext as SchedulerViewModel;
            scheduleViewModel.BusyIndicator = this.busyIndicator;
            scheduleViewModel.VisibleDates = e.visibleDates;
            scheduleViewModel.UpdateAppointments();
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            base.OnDetachingFrom(bindable);
            this.UnWireEvents();
        }

        private void UnWireEvents()
        {
            this.schedule.VisibleDatesChangedEvent -= OnVisibleDatesChangedEvent;
        }
    }
}
