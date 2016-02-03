using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taoqi._code
{
    
        public class UserInfo
        {
            public string Domainname { get; set; }
            public string Fullname { get; set; }
            public string Internalemailaddress { get; set; }
            public Guid Id { get; set; }
            public string MainTelephone { get; set; }
            public DateTime ValidCodeTime { get; set; }
            public string ValidCode { get; set; }
            public Guid LineManagerId { get; set; }

        }

    
    
}