using DatabaseAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository
{
    public class UserVerificationRepository
    {
        public UserVerification Add(UserVerification UserVerification)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    UserVerification comm = context.UserVerifications.Add(UserVerification);
                    context.SaveChanges();
                    return comm;
                }
            }
            catch (Exception ex)
            {
                throw new UserVerificationException("Add", ex);
            }
        }

        public List<UserVerification> GetAll()
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var UserVerifications = context.UserVerifications;
                    if (UserVerifications.ToList().Count == 0)
                    {
                        throw new UserVerificationException("Table is empty");
                    }
                    return UserVerifications.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new UserVerificationException("Add", ex);
            }
        }

        public UserVerification GetByEmail(string email)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var UserVerificationFound = context.UserVerifications.FirstOrDefault(UserVerification => UserVerification.UserEmail == email);
                    if (UserVerificationFound == null)
                    {
                        throw new UserVerificationException("No UserVerification found");
                    }
                    return UserVerificationFound;
                }
            }
            catch (Exception ex)
            {
                throw new UserVerificationException("Find", ex);
            }
        }

        public void Update(UserVerification UserVerification)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var UserVerificationUpdated = context.UserVerifications.FirstOrDefault(UserVerificationObj => UserVerificationObj.UserEmail == UserVerification.UserEmail);
                    if (UserVerificationUpdated == null)
                    {
                        throw new UserVerificationException("No UserVerification found");
                    }
                    UserVerificationUpdated.Code = UserVerification.Code;
                    UserVerificationUpdated.Verified= UserVerification.Verified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserVerificationException("Update", ex);
            }
        }

        public void Delete(string email)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteUserVerification = context.UserVerifications.FirstOrDefault(UserVerification => UserVerification.UserEmail == email);
                    if (deleteUserVerification == null)
                    {
                        throw new UserVerificationException("UserVerification not found to delete");
                    }
                    context.UserVerifications.Remove(deleteUserVerification);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserVerificationException("Delete", ex);
            }
        }
    }
}
