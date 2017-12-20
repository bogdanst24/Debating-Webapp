using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DataController.Security
{
    [Serializable]
    public class UserRightsException : Exception
    {
        public UserRightsException()
        {
        }

        public UserRightsException(string message) : base(message)
        {
        }

        public UserRightsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserRightsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}