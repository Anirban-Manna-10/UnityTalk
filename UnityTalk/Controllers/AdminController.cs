using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;

namespace VirtualMeeting.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        UnityTalkEntities8 ut = new UnityTalkEntities8();
        
        // GET: Admin

        public ActionResult login()
        {
            ViewData["Properjs"] = "https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(AdminTable user)
        {
            ViewData["Properjs"] = "https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js";
            var checkLogin = ut.AdminTables.Where(a => a.Email.Equals(user.Email) && a.Pwd.Equals(user.Pwd)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["fit"] = user.Email.ToString();

                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                ViewBag.Notifi = "Wrong Email or Password";
            }
            return View();
        }

        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Dashboard()
        {
            ViewData["Properjs"] = "https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js";
            if (Session["fit"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "Not Logged in!" });
            }
            return View();
        }

        public ActionResult feedBack()
        {
            if (Session["fit"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            List<FeedBackTable> list = ut.FeedBackTables.ToList();
            list.Reverse();
            return View(list);
        }
    }
}