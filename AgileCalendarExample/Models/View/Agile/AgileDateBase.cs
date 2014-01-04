using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgileCalendarExample.Models.View.Agile
{
    public abstract class AgileDateBase : CalendarDateBase
    {
        public String Name { get; set; }

        public String Title { get; set; }
    }
}