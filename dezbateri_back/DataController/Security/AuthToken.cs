using DatabaseAccess;
using DatabaseAccess.Repository;
using Jose;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DataController.Security
{
    public static class AuthToken
    {

        public static string CreateJwt(String username, String email, UserTypes role, long ttlMillis)
        {

            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (role.ToString() == null)
                throw new ArgumentNullException(nameof(role));

            var currentTime = GetNistTime();
            var expiration = currentTime + ttlMillis;

            var payload = new Dictionary<string, string>()
            {
                {"iss", username },
                {"sub", email },
                {"iat", currentTime.ToString() },
                {"exp", expiration.ToString() },
                {"role", role.ToString() }
            };
            var secretKey = SecretKey.GetSecretKey();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var token = Jose.JWT.Encode(payload, key, JwsAlgorithm.HS256);

            return token;
        }

        internal static bool VerifyToken(String token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            var key = Encoding.ASCII.GetBytes(SecretKey.GetSecretKey());

            try
            {
                Jose.JWT.Decode(token, key, JwsAlgorithm.HS256);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static IDictionary<string, object> Decode(string token)
        {
            var payloadJson = Jose.JWT.Payload<IDictionary<string, object>>(token);
            return payloadJson;
        }

        internal static bool CheckRole(string token, UserTypes typeRequested)
        {
            var decodedToken = Decode(token);
            var tokenRole = decodedToken["role"].ToString();

            var userRepository = new UserRepository();
            User user = userRepository.GetByUsername(decodedToken["iss"].ToString());
            var userRole = user.Type;

            if (userRole == UserTypes.Administrator.ToString())
                return true;

            if (userRole != tokenRole)
                return false;

            if (tokenRole == typeRequested.ToString())
                return true;

            return false;
        }

        internal static bool CheckExpiration(string token)
        {
            var decodedToken = Decode(token);
            var expireTime = long.Parse(decodedToken["exp"].ToString());
            var curentTime = GetNistTime();

            if(curentTime < expireTime)
                return true;
            return false;
        }

        internal static bool CheckOperationOnItselfOnly(string token, string email)
        {
            var decodedToken = Decode(token);
            var tokenEmail = decodedToken["sub"].ToString();
            if (tokenEmail != email)
                return false;
            return true;
        }

        public static long GetNistTime()
        {
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
            var response = myHttpWebRequest.GetResponse();
            string todaysDates = response.Headers["date"];
            var date =  DateTime.ParseExact(todaysDates,
                                       "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                       CultureInfo.InvariantCulture.DateTimeFormat,
                                       DateTimeStyles.AssumeUniversal);
            return (long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}