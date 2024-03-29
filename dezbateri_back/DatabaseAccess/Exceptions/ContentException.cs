﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Exceptions
{
    [Serializable]
    public class ContentException:Exception
    {
        public ContentException()
        {
        }

        public ContentException(string message)
            : base(message)
        {
        }

        public ContentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
