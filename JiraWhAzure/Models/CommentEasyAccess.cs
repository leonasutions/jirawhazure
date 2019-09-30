using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraWhAzure.Models
{
    public class CommentEasyAccess
    {
        public string Id { get; set; }
        public string authorId { get; set; }
        public string ProjectId { get; set; }
        public string verbalStatus { get; set; }

        public string authorName { get; set; }
        public string AuthorEmail { get; set; }
        public string ImportGuid { get; set; }

        public string status { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public string ModifiedDate { get; set; }
        public string priority { get; set; }
    }
}
