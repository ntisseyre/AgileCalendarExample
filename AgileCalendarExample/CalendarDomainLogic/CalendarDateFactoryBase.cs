using AgileCalendarExample.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileCalendarExample.CalendarDomainLogic
{
    /// <summary>
    /// Abstract fabric that creates different models for the calendar
    /// </summary>
    public abstract class CalendarDateFactoryBase
    {
        /// <summary>
        /// Get empty date.
        /// Used to align the beginning/end of a month
        /// </summary>
        /// <returns>Empty date</returns>
        public EmptyDate GetEmptyViewModel()
        {
            return new EmptyDate();
        }

        /// <summary>
        /// Start date for the calendar
        /// </summary>
        public abstract DateTime StartDate { get; }

        /// <summary>
        /// End date for the calendar
        /// </summary>
        public abstract DateTime EndDate { get; }

        /// <summary>
        /// Fabric method that creates a model for the calendar
        /// </summary>
        /// <param name="date">Date in calendar</param>
        /// <returns>Model</returns>
        public abstract CalendarDateBase GetCalendarDate(DateTime date);
    }
}
