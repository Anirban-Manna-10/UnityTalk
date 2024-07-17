using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnityTalk.Models;

namespace UnityTalk.viewModel
{
    public class GroupView
    {
        public GroupTable Group { get; set; }
        public AnnouncementTable AnnounceModel { get; set; }
        public MeetingTable MeetingModel { get; set; }
        public IEnumerable<MessageTable> MessageTabl { get; set; }
        public IEnumerable<AnnouncementTable> AnnouncementTabl { get; set; }
        public IEnumerable<GroupMemberTable> GroupMemberTabl { get; set; }
        public IEnumerable<MeetingTable> MeetingTabl { get; set; }
        public IEnumerable<AnnouncementTable> WhiteBoardTbl { get; set; }
        public FileTable File { get; set; }
        public IEnumerable<FileTable> FileTbl { get; set; }
    }
}