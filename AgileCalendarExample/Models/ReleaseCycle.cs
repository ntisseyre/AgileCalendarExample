using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgileCalendarExample.Models
{
    public class ReleaseCycle : AgileItemBase
    {
        public IList<AgileItemBase> Items { get; set; } 
    }
}