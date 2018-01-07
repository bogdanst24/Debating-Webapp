using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository.OpinionRepos
{
    public class OpinionRepository
    {
        public Opinion Add(Opinion opinion)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    Opinion op = context.Opinions.Add(opinion);
                    context.SaveChanges();
                    return op;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("810", ex);
            }
        }

        public List<Opinion> GetAll()
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var opinions = context.Opinions;
                    if (opinions.ToList().Count == 0)
                    {
                        check = true;
                    }
                    return opinions.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("811", ex);
            }
            finally
            {
                if (check) throw new Exception("812");
            }
        }

        public Opinion GetById(int opinionId)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var opinionFound = context.Opinions.FirstOrDefault(opinion => opinion.Id == opinionId);
                    if (opinionFound == null)
                    {
                        check = true;
                    }
                    return opinionFound;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("814", ex);
            }
            finally
            {
                if (check) throw new Exception("812");
            }
        }

        public List<Opinion> GetAllOfUsername(string email)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    List<Opinion> opinionFound = context.Opinions.Where(opinion => opinion.User_email == email).ToList();
                    if (opinionFound == null)
                    {
                        check = true;
                    }
                    return opinionFound;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("814", ex);
            }
            finally
            {
                if (check) throw new Exception("812");
            }
        }

        public void Update(Opinion opinion)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var opinionUpdated = context.Opinions.FirstOrDefault(opinionObj => opinionObj.Id == opinion.Id);
                    if (opinionUpdated == null)
                    {
                        check = true;
                    }
                    opinionUpdated.Subject = opinion.Subject;
                    opinionUpdated.Date_created = opinion.Date_created;
                    opinionUpdated.User_email = opinion.User_email;
                    opinionUpdated.Picture_url = opinion.Picture_url;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("815", ex);
            }
            finally
            {
                if (check) throw new Exception("813");
            }
        }

        public void Delete(int opinionId)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteOpinion = context.Opinions.FirstOrDefault(opinion => opinion.Id == opinionId);
                    if (deleteOpinion == null)
                    {
                        check = true;
                    }
                    context.Opinions.Remove(deleteOpinion);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("816", ex);
            }
            finally
            {
                if (check) throw new Exception("813");
            }
        }
    }
}
