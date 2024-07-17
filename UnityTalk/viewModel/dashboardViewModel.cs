using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnityTalk.Models;

namespace UnityTalk.viewModel
{
    public class dashboardViewModel
    {
        public UserTable User { get; set; }
        public List<GroupTable> Groups { get; set; }
        public List<EbookTable> Ebooks { get; set; }
        public List<ToDoTable> ToDos { get; set; }   
    }
}