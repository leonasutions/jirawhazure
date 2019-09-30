using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraWhAzure.Models
{
    public class CommentEntity
    {
        public int TrimbleCommentId { set; get; }
        public int CommentId { set; get; }
        public string body { get; set; }
        public string created { get; set; }
        public DateTime updated { get; set; }
        public Creator author { get; set; }
    }
}
