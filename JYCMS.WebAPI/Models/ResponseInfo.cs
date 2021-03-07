using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JYCMS.WebAPI.Models
{
    public class ResponseInfo
    {
        public int status { get; set; }

        public Data data { get; set; }

        public string message { get; set; } 
    }

    public class Data 
    { 
       public int code { get; set; }

       public string data { get; set; } 
    }

}
