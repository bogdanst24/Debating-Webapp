using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Exceptions
{
    [Serializable]
    public class CategoryDebateException : Exception
    {
        public CategoryDebateException()
        {
        }

        public CategoryDebateException(string message) : base(message)
        {
        }

        public CategoryDebateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CategoryDebateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
