using System;

namespace AgileCalendarExample.Models.ViewModels.Agile
{
    /// <summary>
    /// Base model for all agile items to be displayed by a common view
    /// </summary>
    public class AgileDateBase : CalendarDateBase
    {
        /// <summary>
        /// Name of the item to be displayed in the calendar
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Title of the item to be displayed in the calendar.
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Color of the cell in the calendar
        /// </summary>
        public String Color { get; set; }
    }
}