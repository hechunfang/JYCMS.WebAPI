using System;
using System.Collections.Generic;
using System.Text;

namespace YYCMS.Model.Model
{
     public class UserInfo
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int RoleID { get; set; }


        public DateTime CreateTime { get; set; }

    }
}
