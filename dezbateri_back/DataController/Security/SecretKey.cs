using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataController.Security
{
    public static class SecretKey
    {
        private static readonly String _secretKey = "test";

        public static string GetSecretKey()
        {
            return _secretKey;
        }
    }
}