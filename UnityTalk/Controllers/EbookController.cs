using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;

namespace UnityTalk.Controllers
{
    public class EbookController : Controller
    {
        UnityTalkEntities8 dbE = new UnityTalkEntities8();
        // GET: Ebook
        public ActionResult viewEbook()
        {

            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            else
            {
                int Id = (int)Session["Id"];
                List<EbookTable> books = dbE.EbookTables.ToList();
                books.Reverse();

                return View(books);
            }
        }
         
        [HttpPost]
        public ActionResult viewEbook(string search, string searchBy)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            if (string.IsNullOrEmpty(searchBy))
            {
                searchBy = "top";
            }
            List<EbookTable> grps = new List<EbookTable>();
            if (searchBy == "top")
            {
                grps = dbE.EbookTables.Where(a => a.EbookName.Contains(search)).ToList();
                grps.Reverse();
                ViewBag.SearchBy = "Newest First";
            }
            else if (searchBy == "bottom")
            {
                grps = dbE.EbookTables.Where(a => a.EbookName.Contains(search)).ToList();
                ViewBag.SearchBy = "Newest Last";
            }
            else if (searchBy == "AZ")
            {
                grps = dbE.EbookTables.Where(a => a.EbookName.Contains(search)).ToList();
                grps = grps.OrderBy(g => g.EbookName).ToList();
                ViewBag.SearchBy = "A - Z";
            }
            else if (searchBy == "ZA")
            {
                grps = dbE.EbookTables.Where(a => a.EbookName.Contains(search)).ToList();
                grps = grps.OrderBy(g => g.EbookName).ToList();
                grps.Reverse();
                ViewBag.SearchBy = "A - Z";
            }

            return View(grps);

        }

        public ActionResult ebookDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Can not Find the Page , Please Report!" });
            }
            else 
            {
                EbookTable book = dbE.EbookTables.Find(id);
                if (book == null)
                {
                    TempData["Error"] = "Not Available, The book is removed by the Admin.";
                    return RedirectToAction("viewEbook", "Ebook");
                }
                List<EbookTable> bookList = dbE.EbookTables.Take(5).ToList();
                ViewBag.bookList = bookList;
                return View(book);
            }
        }



    }
}
