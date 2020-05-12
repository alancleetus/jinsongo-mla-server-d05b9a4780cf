using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlaWebApi.Models
{
    using System;
    using System.Collections.Generic;

    public class Encrypted_post
    {
        public long postId { get; set; }
        public string message { get; set; }
        public string messagekey { get; set; }
        public string encryptedGroupKey { get; set; }
        public string userName { get; set; }
        public System.DateTime timestamp { get; set; }
        public string groupName { get; set; }

    }
}