using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataController.Utility
{
    public class SuccessMessage
    {
        public int SuccessCode { get; set; }

        public SuccessMessage(int code)
        {
            SuccessCode = code;
        }

        public SuccessMessage()
        {
        }
    }
}