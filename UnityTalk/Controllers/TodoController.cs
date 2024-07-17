using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;
using UnityTalk.viewModel;

namespace UnityTalk.Controllers
{
    public class TodoController : Controller
    {
        private UnityTalkEntities8 dbt = new UnityTalkEntities8();
        // GET: Todo
        public ActionResult todo()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            int usrId = (int)Session["Id"];
            List<ToDoTable> items = dbt.ToDoTables.Where(t => t.Uid == usrId).ToList();
            items.Reverse();
            //UserTable user = dbt.UserTables.Find(usrId);

            ViewBag.todoList = items;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult todo(ToDoTable item)
        {

            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            if (ModelState.IsValid)
            {

                int usrId = (int)Session["Id"];

                item.isDone = false;
                item.Uid = usrId;

                dbt.ToDoTables.Add(item);
                dbt.SaveChanges();
                ModelState.Clear();

                //now packing the new list and retuen
                List<ToDoTable> items = dbt.ToDoTables.Where(t => t.Uid == usrId).ToList();
                items.Reverse();
                ViewBag.todoList = items;
                return View();
            }
            return View();
        }


        public ActionResult removeTodo(int? itemId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            // removing TODO item from database
            ToDoTable item = dbt.ToDoTables.Find(itemId);
            if (item != null) {
                dbt.ToDoTables.Remove(item);
                dbt.SaveChanges();
            }
            
            //Packing the new Updated List for TOdolist

            int usrId = (int)Session["Id"];
            List<ToDoTable> items = dbt.ToDoTables.Where(t => t.Uid == usrId).ToList();
            items.Reverse();
            ViewBag.todoList = items;
            return RedirectToAction("todo", "Todo");
        }

        public ActionResult doneTodo(int? itemId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            // removing TODO item from database
            ToDoTable item = dbt.ToDoTables.Find(itemId);
            if (item != null)
            {
                if(item.isDone)
                {
                    item.isDone = false;
                }
                else
                {
                    item.isDone=true;
                }
                dbt.SaveChanges();
            }

            //Packing the new Updated List for TOdolist

            int usrId = (int)Session["Id"];
            List<ToDoTable> items = dbt.ToDoTables.Where(t => t.Uid == usrId).ToList();

            ViewBag.todoList = items;
            return RedirectToAction("todo", "Todo");
        }


        public ActionResult editTodo(int itemId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            //packet todolist
            int usrId = (int)Session["Id"];
            List<ToDoTable> items = dbt.ToDoTables.Where(t => t.Uid == usrId).ToList();
            items.Reverse();
            ViewBag.todoList = items;

            //finding and sending the sended list
            ToDoTable item = dbt.ToDoTables.Find(itemId);
            if (item != null)
            {
                return View(item);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editTodo(ToDoTable upditem)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            //packet todolist
            int usrId = (int)Session["Id"];
            List<ToDoTable> items = dbt.ToDoTables.Where(t => t.Uid == usrId).ToList();
            items.Reverse();
            ViewBag.todoList = items;

            //finding and sending the sended list
            ToDoTable item = dbt.ToDoTables.Find(upditem.ToDoId);
            if (item != null)
            {
                item.Cont = upditem.Cont;
                item.isDone = item.isDone;
                item.Uid = usrId;

                dbt.Entry(item).State = System.Data.Entity.EntityState.Modified;
                dbt.SaveChanges();
                return RedirectToAction("todo","Todo");
            }

            return View(upditem);
        }

    }
}