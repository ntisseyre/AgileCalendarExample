using System;
using System.Collections.Generic;

namespace AgileCalendarExample.Models.ViewModels.Agile
{
    /// <summary>
    /// View's model for the vacation date
    /// </summary>
    public class AgileVacation : AgileDateBase
    {
        /// <summary>
        /// List of the icons' names for each team member, who has a vacation on the current date
        /// </summary>
        public IList<String> TeamMembersIcons { get; set; }
    }
}