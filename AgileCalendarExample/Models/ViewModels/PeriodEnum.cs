using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgileCalendarExample.Models.ViewModels
{
    /// <summary>
    /// This enum is used to instruct the view if it is a beginning of a new week/month, or an end
    /// </summary>
    public enum PeriodEnum
    {
        /// <summary>
        /// Current week/month
        /// </summary>
        Current,

        /// <summary>
        /// New week/month starts
        /// </summary>
        Start,

        /// <summary>
        /// Current week/month ends
        /// </summary>
        End
    }
}