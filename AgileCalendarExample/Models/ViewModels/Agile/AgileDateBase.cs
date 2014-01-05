using System;

namespace AgileCalendarExample.Models.ViewModels.Agile
{
    /// <summary>
    /// Base model for all agile items to be displayed by a common view
    /// </summary>
    public class AgileDateBase : CalendarDateBase
    {
        public AgileItemsEnum AgileItem { get; set; }

        public String Name { get; set; }

        public String Title { get; set; }

        public String Color { get; set; }
    }
}