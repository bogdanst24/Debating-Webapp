using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataController.Models
{
    public class OpinionModel
    {
        public String OpinionId { get; set; }
        public String Subject { get; set; }
        public String Date_created { get; set; }
        public String User_email { get; set; }
        public String User_username { get; set; }
        public String Picture_url { get; set; }
        public String Pro_votes { get; set; }
        public String Con_votes { get; set; }
        public List<String> Category  { get; set; }
        public List<Argument> Arguments { get; set; }

    }
}