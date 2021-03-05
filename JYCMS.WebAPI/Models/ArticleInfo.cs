using System;
using System.Collections.Generic;
using System.Text;

namespace YYCMS.Model.Model
{
    public class ArticleInfo
    {
        public int ArticleID { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Contents { get; set; }

        public string Images { get; set; }
        public string Keywords { get; set; }

        public string Description { get; set; }

        public int ClassifyID { get; set; }
        public int SiteID { get; set; }

        public int UserID { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime PubDate { get; set; }

        public string Label { get; set; }

        //public virtual ClassifyInfo ClassifyInfo { get; set; }
        //public virtual SiteInfo SiteInfo { get; set; }

        //public virtual UserInfo UserInfo { get; set; }
         

    }
}
