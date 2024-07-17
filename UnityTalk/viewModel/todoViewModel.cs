using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnityTalk.Models;

namespace UnityTalk.viewModel
{
    public class todoViewModel
    {
        public UserTable User { get; set; }
        public List<ToDoTable> todoItems { get; set; }
        public ToDoTable todoModel { get; set; }
    }
}