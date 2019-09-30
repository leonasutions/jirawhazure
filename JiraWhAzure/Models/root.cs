using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraWhAzure.Models
{
    public class root
    {
        public string webhookEvent { get; set; }
        public string Issue_event_type_name { get; set; }
        public CommentEntity Comment { set; get; }
        public IssueEntity Issue { set; get; }
        public string Body { set; get; }
        public string Timestamp { set; get; }

        public Creator user { get; set; }

    }
}
