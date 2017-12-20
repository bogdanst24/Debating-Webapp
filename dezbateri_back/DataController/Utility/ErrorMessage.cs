using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataController.Utility
{
    public class ErrorMessage
    {
        public int ErrorCode { get; set; }
        
        public ErrorMessage(int error)
        {
            ErrorCode = error;
        }

        public ErrorMessage()
        {
        }
    }

}