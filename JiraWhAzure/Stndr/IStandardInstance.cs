using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraWhAzure.Stndr
{
    public class IStandardInstance
    {
        public string MessageOrigin { get; set; }
        public string Action { get; set; }
        public string Created { get; set; }
        public string Author { get; set; }
        public string Assignee { get; set; }
        public string ModifiedUser { get; set; }
        public string Summary { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string[] Labels { get; set; }
    }
}
