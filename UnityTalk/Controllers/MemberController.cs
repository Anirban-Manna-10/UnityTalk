using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;

namespace VirtualMeeting.Controllers
{
    public class MemberController : Controller
    {
        UnityTalkEntities8 ut2 = new UnityTalkEntities8();
        // GET: Member
        public ActionResult ViewGroup()
        {
            return View(ut2.GroupTables.ToList());

        }

        public ActionResult ViewMember()
        {
            return View(ut2.GroupMemberTables.ToList());

        }

        public ActionResult Search_Group(string search)
        {
            
            return View(ut2.GroupTables.Where(x => x.GrpName.Contains(search) || search == null).ToList());
        }

        public ActionResult Search_Member(string search)
        {
            return View(ut2.GroupMemberTables.Where(x => x.UserTable.UserName.Contains(search) || search == null).ToList());

        }

    }
}