using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;

namespace UnityTalk.Controllers
{
    public class FeedBackController : Controller
    {
        UnityTalkEntities8 dbF = new UnityTalkEntities8();

        [HttpGet]
        public ActionResult feedback()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            UserTable user = dbF.UserTables.Find(Session["Id"]);
            if (user == null)
            {
                TempData["Error"] = "Someting is Wrong !!";
            }

            FeedBackTable fb = dbF.FeedBackTables.FirstOrDefault(a => a.UserId == user.Uid);

            if (fb == null)
            {
                return View();
            }
            else
            {
                return View(fb);
            }
        }
        [HttpPost]
        public ActionResult feedback(FeedBackTable fb)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            UserTable user = dbF.UserTables.Find(Session["Id"]);
            if (user == null)
            {
                TempData["Error"] = "Someting is Wrong !!";
            }

            FeedBackTable userfb = dbF.FeedBackTables.FirstOrDefault(a => a.UserId == user.Uid);

            if (userfb == null)
            {
                if(fb.Rating == 0)
                {
                    TempData["Error"] = "Please Give atleast 1 star to rate!!";
                    ModelState.Clear();
                    return RedirectToAction("feedback");
                }
                else
                {
                    FeedBackTable newfb = new FeedBackTable();
                    newfb.Rating = fb.Rating;
                    newfb.Date = DateTime.Today;
                    newfb.cont = fb.cont;
                    newfb.UserId = user.Uid;

                    dbF.FeedBackTables.Add(newfb);
                    dbF.SaveChanges();
                    TempData["Success"] = "Thank You For Your Valuable Feedback!!";
                    return RedirectToAction("feedback");
                }
            }
            else
            {

                userfb.Date = DateTime.Today;
                userfb.Rating = fb.Rating;
                userfb.cont = fb.cont;
                dbF.Entry(userfb).State = System.Data.Entity.EntityState.Modified;
                dbF.SaveChanges();
                TempData["Success"] = "Thank You For Update Your Valuable Feedback!!";
                return RedirectToAction("feedback");
            }
        }
    }
}