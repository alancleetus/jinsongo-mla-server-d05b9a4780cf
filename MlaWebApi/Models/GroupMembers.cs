using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlaWebApi.Models
{
    public partial class GroupMembers
    {
        public string userId { get; set; }
        public string groupId { get; set; }
        public string userName { get; set; }
        public string groupName { get; set; }

    }
}