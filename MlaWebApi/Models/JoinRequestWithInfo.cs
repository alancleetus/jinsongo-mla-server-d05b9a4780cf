using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlaWebApi.Models
{
    public class JoinRequestWithInfo
    {
        public long groupId { get; set; }
        public long userId { get; set; }

        public string groupName { get; set; }
        public string userName { get; set; }


    }
}