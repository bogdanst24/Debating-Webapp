using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataController.Security
{
    public static class UserTypesClass
    {
        public static UserTypes Convert(string input)
        {
            if (input.ToLower() == "user")
                return UserTypes.User;
            if (input.ToLower() == "administrator")
                return UserTypes.Administrator;
            if (input.ToLower() == "moderator")
                return UserTypes.Moderator;
            return UserTypes.Error;
        }
    }

    

    public enum UserTypes
    {
        Administrator,
        Moderator,
        User,
        Error
    }
}