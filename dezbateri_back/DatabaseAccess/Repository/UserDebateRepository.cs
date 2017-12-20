using DatabaseAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository
{
    public class UserDebateRepository
    {
        public void Add(UserDebate userDebate)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    context.UserDebates.Add(userDebate);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserDebateException("Add", ex);
            }
        }

        public List<UserDebate> GetAll()
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var userDebates = context.UserDebates;
                    if (userDebates.ToList().Count == 0)
                    {
                        throw new UserDebateException("Table is empty");
                    }
                    return userDebates.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new UserDebateException("Add", ex);
            }
        }

        public UserDebate GetById(int userDebateDebateId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var userDebateFound = context.UserDebates.FirstOrDefault(userDebate => userDebate.debate_id == userDebateDebateId);
                    if (userDebateFound == null)
                    {
                        throw new UserDebateException("No userDebate found");
                    }
                    return userDebateFound;
                }
            }
            catch (Exception ex)
            {
                throw new UserDebateException("Find", ex);
            }
        }

        public void Update(UserDebate userDebate)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var userDebateUpdated = context.UserDebates.FirstOrDefault(userDebateObj => userDebateObj.debate_id == userDebate.debate_id);
                    if (userDebateUpdated == null)
                    {
                        throw new UserDebateException("No userDebate found");
                    }
                    userDebateUpdated.pro_username = userDebate.pro_username;
                    userDebateUpdated.con_username = userDebate.con_username;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserDebateException("Update", ex);
            }
        }

        public void Delete(int userDebateId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteUserDebate = context.UserDebates.FirstOrDefault(userDebate => userDebate.debate_id == userDebateId);
                    if (deleteUserDebate == null)
                    {
                        throw new UserDebateException("UserDebate not found to delete");
                    }
                    context.UserDebates.Remove(deleteUserDebate);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserDebateException("Delete", ex);
            }
        }
    }
}
