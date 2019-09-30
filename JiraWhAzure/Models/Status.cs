using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraWhAzure.Models
{
    public class Status
    {
        public string name { get; set; }
        public string description { get; set; }
        public StatusCategory statusCategory { get; set; }
    }
}
