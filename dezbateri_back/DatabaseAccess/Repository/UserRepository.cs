using DatabaseAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository
{
    public class UserRepository
    {
        public void Add(User user)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserException("800", ex);
            }
        }

        public List<User> GetAll()
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var users = context.Users;
                    if (users.ToList().Count == 0)
                    {
                        check = true;
                    }
                    return users.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new UserException("801", ex);
            }
            finally { 
                if(check) throw new UserException("802");
            }
        }

        public User GetByUsername(string userUsername)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var userFound = context.Users.FirstOrDefault(user => user.Username == userUsername);
                    if (userFound == null)
                    {
                        check = true;
                    }
                    return userFound;
                }
            }
            catch (Exception ex)
            {
                throw new UserException("804", ex);
            }
            finally
            {
                if(check) throw new UserException("802");
            }
        }

        public User GetByEmail(string userEmail)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var userFound = context.Users.FirstOrDefault(user => user.Email == userEmail);
                    if (userFound == null)
                    {
                        check = true;
                    }
                    return userFound;
                }
            }
            catch (Exception ex)
            {
                throw new UserException("804", ex);
            }
            finally
            {
                if(check) throw new UserException("803");
            }
        }

        public void Update(User user)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var userUpdated = context.Users.FirstOrDefault(userObj => userObj.Email == user.Email);
                    if (userUpdated == null)
                    {
                        check = true;
                    }
                    userUpdated.Birthdate = user.Birthdate;
                    userUpdated.Password = user.Password;
                    userUpdated.Type = user.Type;
                    userUpdated.Username = user.Username;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserException("805", ex);
            }
            finally
            {
                if(check) throw new UserException("803");
            }
        }

        public void Delete(string userEmail)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteUser = context.Users.FirstOrDefault(user => user.Email == userEmail);
                    if (deleteUser == null)
                    {
                        check = true;
                    }
                    context.Users.Remove(deleteUser);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserException("806", ex);
            }
            finally
            {
                if(check) throw new UserException("803");
            }
        }
    }
}
