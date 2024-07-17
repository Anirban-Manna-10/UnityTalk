using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;
using static System.Net.Mime.MediaTypeNames;

namespace UnityTalk.Controllers
{
    public class GroupController : Controller
    {
        UnityTalkEntities8 dbG = new UnityTalkEntities8();
        // GET: Group

        [HttpGet]
        public ActionResult allGroup()
        {

            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            else
            {
                int Id = (int)Session["Id"];
                List<GroupMemberTable> mem = dbG.GroupMemberTables.Where(a => a.UserId == Id).ToList();
                mem.Reverse();
                GroupTable grp = new GroupTable()
                {
                    GroupMemberTables = mem,
                };

                return View(grp);
            }
        }
        [HttpPost]
        public ActionResult allGroup(string search, string searchBy)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't signed in yet!" });
            }
            else
            {
                if (string.IsNullOrEmpty(searchBy))
                {
                    searchBy = "All";
                }
                int Id = (int)Session["Id"];
                List<GroupMemberTable> mem = new List<GroupMemberTable>();
                if (searchBy == "public")
                {
                    mem = dbG.GroupMemberTables.Where(a => a.UserId == Id && a.GroupTable.GrpName.Contains(search) && a.GroupTable.GrpMode == true).ToList();
                }
                else if(searchBy == "All"){
                    mem = dbG.GroupMemberTables.Where(a => a.UserId == Id && a.GroupTable.GrpName.Contains(search)).ToList();
                }
                else if (searchBy == "private")
                {
                    mem = dbG.GroupMemberTables.Where(a => a.UserId == Id && a.GroupTable.GrpName.Contains(search) && a.GroupTable.GrpMode == false).ToList();
                }else if(searchBy == "admin")
                {
                    mem = dbG.GroupMemberTables.Where(a => a.UserId == Id && a.GroupTable.GrpName.Contains(search) && a.GroupTable.UserTable.Uid == Id).ToList();
                }

                GroupTable grp = new GroupTable()
                {
                    GroupMemberTables = mem,
                };

                return View(grp);
            }
        }

        [HttpGet]
        public ActionResult publicGroups()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            else
            {
                int Id = (int)Session["Id"];
                List<GroupTable> grp = dbG.GroupTables.Where(a => a.GrpMode == true ).ToList();

                grp.Reverse();

                return View(grp);
            }
        }

        [HttpPost]
        public ActionResult publicGroups(string search, string searchBy)
        {
                if (string.IsNullOrEmpty(searchBy))
                {
                    searchBy = "top";
                }
                List<GroupTable> grps = new List<GroupTable>();
                if (searchBy == "top")
                {
                    grps = dbG.GroupTables.Where(a =>  a.GrpName.Contains(search) && a.GrpMode == true).ToList();
                    grps.Reverse();
                    ViewBag.SearchBy = "Newest First";
                }
                else if (searchBy == "bottom")
                {
                grps = dbG.GroupTables.Where(a => a.GrpName.Contains(search) && a.GrpMode == true).ToList();
                ViewBag.SearchBy = "Newest Last";
            }
                else if (searchBy == "AZ")
                {
                grps = dbG.GroupTables.Where(a => a.GrpName.Contains(search) && a.GrpMode == true).ToList();
                grps = grps.OrderBy(g => g.GrpName).ToList();
                ViewBag.SearchBy = "A - Z";
            }
                else if (searchBy == "ZA")
                {
                grps = dbG.GroupTables.Where(a => a.GrpName.Contains(search) && a.GrpMode == true).ToList();
                grps = grps.OrderBy(g => g.GrpName).ToList();
                grps.Reverse();
                ViewBag.SearchBy = "A - Z";
            }
                return View(grps);
            
        }

        [HttpGet]
        public ActionResult grupEdit(int? grpId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            if(grpId == null)
            {
                return new HttpStatusCodeResult(400, "Group Id is required");
            }

            int Id = (int)Session["Id"];
            GroupTable grp = dbG.GroupTables.Find(grpId);
            if(grp == null)
            {
                TempData["Error"] = "Can not Find this Group";
                return RedirectToAction("allGroup","Group");
            }
            else
            {
                if(grp.UserTable.Uid == Id)
                {
                    ViewBag.ImgList = dbG.GroupImgTables.ToList();
                    return View(grp);
                }
                else
                {
                    TempData["Error"] = "Only Group Admin can make changes.";
                    return RedirectToAction("grupview","GroupView",new { grpid = grpId });
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult grupEdit(GroupTable grp)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            if(!ModelState.IsValid)
            {
                TempData["Error"] = "Enter Valid Informations";
                return View(grp);
            }
            else
            {

                GroupTable grp1 = dbG.GroupTables.Find(grp.GrpId);
                if (grp1 == null)
                {
                    TempData["Error"] = "Group not found. It might have been deleted.";
                    return View(grp);
                }

                grp1.GrpImgId = grp.GrpImgId;
                bool isEmpty = string.IsNullOrWhiteSpace(grp.GrpName.Trim());
                if (isEmpty)
                {
                    TempData["Error"] = "Give a valid Group Name!!";
                    return RedirectToAction("grupEdit", new { grpId = grp.GrpId });
                }
                grp1.GrpName = grp.GrpName;
                grp1.GrpDescp = grp.GrpDescp;
                grp1.GrpMode = grp1.GrpMode;
                grp1.GrpCode = grp1.GrpCode;

                dbG.Entry(grp1).State = System.Data.Entity.EntityState.Modified;
                dbG.SaveChanges();
                TempData["Success"] = "Group Details Successfully upsdated";
                return RedirectToAction("grupView", "GroupView", new { grpid = grp1.GrpId });
            }

        }


    }
}
