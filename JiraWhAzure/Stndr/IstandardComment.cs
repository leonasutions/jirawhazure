using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraWhAzure.Stndr
{
    public class IstandardComment
    {
        public string MessageOrigin { set; get; }
        public string Action { get; set; }
        public string Author { get; set; }
        public string Created { get; set; }
        public string Content { get; set; }
        public string Updated { get; set; }
    }
}
