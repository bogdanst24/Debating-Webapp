using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Exceptions
{
  
        [Serializable]
        public class UserVerificationException : Exception
        {
            public UserVerificationException()
            {
            }

            public UserVerificationException(string message) : base(message)
            {
            }

            public UserVerificationException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected UserVerificationException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    
}
