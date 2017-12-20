using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Exceptions
{
    [Serializable]
    public class RoundStateException : Exception
    {
        public RoundStateException()
        {
        }

        public RoundStateException(string message) : base(message)
        {
        }

        public RoundStateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RoundStateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
