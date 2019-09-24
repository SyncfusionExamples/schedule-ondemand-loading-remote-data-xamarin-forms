using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OnDemandLoading_Scheduler
{
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
}
