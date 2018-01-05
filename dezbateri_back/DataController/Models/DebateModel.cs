using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataController.Models
{
    public class DebateModel
    {
        public String DebateId { get; set; }
        public String Pro_username { get; set; }
        public String Con_username { get; set; }
        public String Subject { get; set; }
        public String Description { get; set; }
        public List<String> Category { get; set; }
        public String Date_created { get; set; }
        public String State { get; set; }
        public String Next_round { get; set; }
        public String Time_to_next { get; set; }
        public String Round_1 { get; set; }
        public String Round_2 { get; set; }
        public String Round_3 { get; set; }
        public String Round_4 { get; set; }
        public String Round_5 { get; set; }
        public String Round_6 { get; set; }


    }
}