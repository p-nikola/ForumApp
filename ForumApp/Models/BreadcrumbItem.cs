using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForumApp.Models
{
    public class BreadcrumbItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; } // Indicates whether the item is the current page
    }

}