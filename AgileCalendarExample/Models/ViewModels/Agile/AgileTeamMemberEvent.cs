using System;

namespace AgileCalendarExample.Models.ViewModels.Agile
{
    /// <summary>
    /// View's model to display team memmber's event
    /// </summary>
    public class AgileTeamMemberEvent
    {
        /// <summary>
        /// Name of the team member's event
        /// </summary>
        public String Event { get; set; }

        /// <summary>
        /// Team member's icon
        /// </summary>
        public String Icon { get; set; }
    }
}