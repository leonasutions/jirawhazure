using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraWhAzure.Models
{
    public class Fields
    {
        public string Summary { get; set; }
        public string description { get; set; }
        public IssueType issueType { get; set; }
        public Creator creator { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
        public Priority priority { get; set; }
        public Project project { get; set; }
        public Status status { get; set; }
    }
}
