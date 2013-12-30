using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgileCalendarExample.Models
{
    public class Sprint : AgileItemBase
    {
        public IList<AgileItemBase> Items { get; set; }
    }
}