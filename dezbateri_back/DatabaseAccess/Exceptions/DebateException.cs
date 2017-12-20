using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Exceptions
{
    [Serializable]
    public class DebateException : Exception
    {
        public DebateException()
        {
        }

        public DebateException(string message) : base(message)
        {
        }

        public DebateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DebateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
