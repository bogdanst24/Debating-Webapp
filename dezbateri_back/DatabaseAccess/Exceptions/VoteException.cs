using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Exceptions
{
    [Serializable]
    public class VoteException : Exception
    {
        public VoteException()
        {
        }

        public VoteException(string message) : base(message)
        {
        }

        public VoteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VoteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
