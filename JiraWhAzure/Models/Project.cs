using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JiraWhAzure.Models
{
    public class Project
    {
        public int ProjectId { set; get; }
        public string Key { set; get; }

        public string ProjectTypeKey { set; get; }

        public int TrimbleProjectId { set; get; }

        public int AvatarId { set; get; }

        public string Lead { set; get; }

        public int LeadAccountId { set; get; }

        public string Name { set; get; }

        public int NotificationScheme { set; get; }

        public int PermissionScheme { set; get; }
    }
}
