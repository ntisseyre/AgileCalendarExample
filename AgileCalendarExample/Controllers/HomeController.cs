using AgileCalendarExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace AgileCalendarExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Create a weekly schedule from an example xml-file
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Server.MapPath("~/WeeklyScheduleExample.xml"));

            ReleaseCycle releaseCycle = ReleaseCycle.GetRecord(xmlDocument.OuterXml);
            return View(releaseCycle);
        }

    }
}
