using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Exceptions
{

    [Serializable]
    public class CommentaryException : Exception
    {

        public CommentaryException()
        {
        }

        public CommentaryException(string message)
            : base(message)
        {
        }

        public CommentaryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
