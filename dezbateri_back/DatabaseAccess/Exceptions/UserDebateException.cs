using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Exceptions
{
    [Serializable]
    public class UserDebateException : Exception
    {
        public UserDebateException()
        {
        }

        public UserDebateException(string message) : base(message)
        {
        }

        public UserDebateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserDebateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
