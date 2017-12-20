using DatabaseAccess;
using DatabaseAccess.Exceptions;
using DatabaseAccess.Repository;
using DataController.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataController.Services
{
    public class UserServices
    {
        private UserRepository _userRepository;

        public UserServices() {
            _userRepository = new UserRepository();
        }

        public void RegisterUser(User user)
        {
            Boolean check = true;
            try
            {
                _userRepository.GetByUsername(user.Username);
                check = false;
            } catch(UserException ex)
            {
                if (ex.Message != "802")
                    check = false;
            }
            if(check == false)
            {
                throw new Exception("704");
            }

            check = true;
            try
            {
                _userRepository.GetByEmail(user.Email);
                check = false;
            } catch(UserException ex)
            {
                if (ex.Message != "803")
                    check = false;
            }
            if (check == false)
            {
                throw new Exception("705");
            }

        }

        public Boolean CheckIfDupplicate(User user)
        {
            Boolean check = false;
            try { 
                _userRepository.GetByEmail(user.Email);
            }
            catch(Exception ex)
            {
                if (ex.Message == "803") check = true;
            }

            if (check) { 
                try { 
                    _userRepository.GetByUsername(user.Username);
                }
                catch(Exception ex)
                {
                    if (ex.Message == "802") return true;
                }
            }
            return false;

        }

        public Boolean CheckIfDupplicateUsername(User user)
        {
            User userGot = new User();
            try
            {
                userGot = _userRepository.GetByUsername(user.Username);
                if (user.Email == userGot.Email)
                    return true;
            }
            catch (Exception ex)
            {
                if (ex.Message == "802" )
                {
                    return true;
                }
            }

            return false;

        }
    }
}