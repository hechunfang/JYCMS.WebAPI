using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JYCMS.WebAPI.Models
{
    public class ResponseInfo<T>
    {
        public string status { get; set; }

        public List<Data<T>> data { get; set; }

        public string message { get; set; }

        public string sysMessage { get; set; }
    }

    public class Data<T>
    { 
       public int code { get; set; }

       public List<T> data { get; set; }

        public string sysMessage { get; set; }
    }

}
