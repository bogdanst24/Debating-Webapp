using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
/*
 *      120 - no header
 *      121 - wrong token
 *      122 - wrong role
 */
namespace DataController.Security
{
    public static class AuthorizationH
    {

        public static void Verify(HttpRequestMessage Request, UserTypes role, Boolean itself, String email)
        {

            var headers = Request.Headers;
            if (!headers.Contains("auth_token"))
                throw new AuthorizationException("120");
            else
            {
            
                var token = headers.GetValues("auth_token").First();
                if (!AuthToken.VerifyToken(token))
                    throw new AuthorizationException("121");
                if (!AuthToken.CheckRole(token, role))
                    throw new AuthorizationException("122");
                if (!AuthToken.CheckExpiration(token))
                    throw new AuthorizationException("123");

                if (itself && role==UserTypes.User)
                {
                    if(email == null)
                        throw new AuthorizationException("124");
                    if (!AuthToken.CheckOperationOnItselfOnly(token, email))
                        throw new AuthorizationException("124");
                }
            }

        }
    }
}