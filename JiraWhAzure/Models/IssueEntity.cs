using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraWhAzure.Models
{
    public class IssueEntity
    {
        public int IssueId { set; get; }

        public int TrimbleIssueId { set; get; }

        public Project Project { set; get; }

        public string key { get; set; }
        public Fields fields { get; set; }
    }
}
