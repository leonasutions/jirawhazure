using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JiraWhAzure.Models
{
    public class Topic
    {
        public string QGuid { get; set; }

        public string ProjectId { get; set; }

        public string Id { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        public string Title { get; set; }


        public string CreationDate { get; set; }

    }
}
