using DatabaseAccess;
using DatabaseAccess.Exceptions;
using DatabaseAccess.Repository;
using DataController.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace DataController.Services
{
    public class UserServices
    {

        private UserRepository _userRepository;
        private UserVerificationRepository _userVerificationRepository;

        public UserServices() {
            _userRepository = new UserRepository();
            _userVerificationRepository = new UserVerificationRepository();
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

        internal void VerifyEmail(User user)
        {
            var email = user.Email;

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[12];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var cod_verificare = new String(stringChars);
         
            MailService.SendVerificationMail(email, cod_verificare);

            UserVerification uv = new UserVerification();
            uv.UserEmail = email;
            uv.Code = cod_verificare;
            uv.Verified = false;
            _userVerificationRepository.Add(uv);
        }

        internal void ResendVerifyEmail(String email)
        {
            UserVerification uv = _userVerificationRepository.GetByEmail(email);
            MailService.SendVerificationMail(email, uv.Code);
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

        internal Boolean CheckCode(String email, String code)
        {
            UserVerification uv =_userVerificationRepository.GetByEmail(email);
            if(uv.Code == code)
            {
                uv.Verified = true;
                _userVerificationRepository.Update(uv);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void CheckIfActivated(string type, string id)
        {
            UserVerification uv = new UserVerification();
            if (type == "email")
            {
                uv = _userVerificationRepository.GetByEmail(id);
            } else
            {
                User user = _userRepository.GetByUsername(id);
                uv = _userVerificationRepository.GetByEmail(user.Email);
            }
            if (!uv.Verified)
            {
                throw new Exception("133");
            }
        }
    }
}