using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Exceptions
{
    [Serializable]
    public class CategoryException : Exception
    {

        public CategoryException()
        {
        }

        public CategoryException(string message)
            : base(message)
        {
        }

        public CategoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
