using AgileCalendarExample.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgileCalendarExample.Controllers
{
    public class TeamMembersController : Controller
    {
        public ActionResult GetTeamMembers()
        {
            //Making some fake data.
            //Here can be done a call to a Domain Layer to retrieve a real list
            IList<TeamMember> teamMembers = new List<TeamMember>();
            teamMembers.Add(new TeamMember() { Name = "Emilly", Icon = "teamMember5" });
            teamMembers.Add(new TeamMember() { Name = "William", Icon = "teamMember6" });
            teamMembers.Add(new TeamMember() { Name = "Maxic", Icon = "teamMember4" });
            teamMembers.Add(new TeamMember() { Name = "Frank", Icon = "teamMember1" });
            teamMembers.Add(new TeamMember() { Name = "Matilda", Icon = "teamMember2" });
            teamMembers.Add(new TeamMember() { Name = "Natalia", Icon = "teamMember3" });
            teamMembers.OrderBy(item => item.Name);

            return PartialView("TeamMembers", teamMembers);
        }

    }
}
