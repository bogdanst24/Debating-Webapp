using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository.ArgumentRepos
{
    public class ArgumentRepository
    {
        public void Add(Argument argument)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    context.Arguments.Add(argument);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("810", ex);
            }
        }

        public List<Argument> GetAll()
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var arguments = context.Arguments;
                    if (arguments.ToList().Count == 0)
                    {
                        check = true;
                    }
                    return arguments.ToList();
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

        public Argument GetById(int argumentId)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var argumentFound = context.Arguments.FirstOrDefault(argument => argument.Id == argumentId);
                    if (argumentFound == null)
                    {
                        check = true;
                    }
                    return argumentFound;
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

        public List<Argument> GetAllOfOpinion(int opinion_id)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    List<Argument> argumentFound = context.Arguments.Where(argument => argument.Opinion_id == opinion_id).ToList();
                    if (argumentFound == null)
                    {
                        check = true;
                    }
                    return argumentFound;
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

        public void Update(Argument argument)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var argumentUpdated = context.Arguments.FirstOrDefault(argumentObj => argumentObj.Id == argument.Id);
                    if (argumentUpdated == null)
                    {
                        check = true;
                    }
                    argumentUpdated.Side = argument.Side;
                    argumentUpdated.Date_created = argument.Date_created;
                    argumentUpdated.Content = argument.Content;

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

        public void Delete(int argumentId)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteArgument = context.Arguments.FirstOrDefault(argument => argument.Id == argumentId);
                    if (deleteArgument == null)
                    {
                        check = true;
                    }
                    context.Arguments.Remove(deleteArgument);
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
