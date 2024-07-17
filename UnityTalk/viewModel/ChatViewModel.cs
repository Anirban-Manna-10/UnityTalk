using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnityTalk.Models;

namespace UnityTalk.viewModel
{
    public class ChatViewModel
    {
        public GroupTable Group { get; set; }
        public UserTable User { get; set; }
        public GroupMemberTable Member { get; set; }
        public MessageTable Message { get; set; }
        public List<MessageTable> Messages { get; set; }
        public List<GroupMemberTable> Members { get; set; }
    }
}