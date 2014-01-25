using AgileCalendarExample.CalendarDomainLogic;
using AgileCalendarExample.Models.DomainModels;
using AgileCalendarExample.Models.ViewModels.Agile;
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
            return View();
        }

        public ActionResult DisplayReleaseCycle()
        {
            //Create an agile release cycle object schedule from an example xml-file
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Server.MapPath("~/AgileCalendarExample.xml"));

            ReleaseCycleModel releaseCycle = ReleaseCycleModel.GetRecord(xmlDocument.OuterXml);
            return View(releaseCycle);
        }

        public ActionResult EditReleaseCycle()
        {
            //Create an agile release cycle object schedule from an example xml-file
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Server.MapPath("~/AgileCalendarExample.xml"));

            ReleaseCycleModel releaseCycle = ReleaseCycleModel.GetRecord(xmlDocument.OuterXml);
            return View(releaseCycle);
        }

        [HttpPost]
        public JsonResult Save(ReleaseCycleModel releaseCycleModel)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(releaseCycleModel.ToString());
            xmlDocument.Save(Server.MapPath("~/AgileCalendarExample.xml"));

            return Json("Ok");
        }

        [ChildActionOnly]
        public ActionResult AgileCalendar(ReleaseCycleModel model)
        {
            AgileDateFactory factory = new AgileDateFactory(model.Normolize());
            CalendarBuilder builder = new CalendarBuilder(DayOfWeek.Monday, factory);

            return PartialView("Calendar", builder.Build());
        }
    }
}
