using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAccess.Exceptions;

namespace DatabaseAccess.Repository
{
    public class CommentaryRepository
    {
        public Commentary Add(Commentary Commentary)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    Commentary comm = context.Commentaries.Add(Commentary);
                    context.SaveChanges();
                    return comm;
                }
            }
            catch (Exception ex)
            {
                throw new CommentaryException("Add", ex);
            }
        }

        public List<Commentary> GetAll()
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var Commentaries = context.Commentaries;
                    if (Commentaries.ToList().Count == 0)
                    {
                        throw new CommentaryException("Table is empty");
                    }
                    return Commentaries.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new CommentaryException("Add", ex);
            }
        }

        public List<Commentary> GetAllByDebateId(int CommentaryDebateId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var CommentaryFound = context.Commentaries.Where(Commentary => Commentary.Debate_id == CommentaryDebateId).ToList();
                    if (CommentaryFound == null)
                    {
                        throw new CommentaryException("No Commentary found");
                    }
                    return CommentaryFound;
                }
            }
            catch (Exception ex)
            {
                throw new CommentaryException("Find", ex);
            }
        }

        public Commentary GetById(int commId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var CommentaryFound = context.Commentaries.FirstOrDefault(Commentary => Commentary.Id == commId);
                    if (CommentaryFound == null)
                    {
                        throw new CommentaryException("No Commentary found");
                    }
                    return CommentaryFound;
                }
            }
            catch (Exception ex)
            {
                throw new CommentaryException("Find", ex);
            }
        }

        public void Update(Commentary Commentary)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var CommentaryUpdated = context.Commentaries.FirstOrDefault(CommentaryObj => CommentaryObj.Id == Commentary.Id);
                    if (CommentaryUpdated == null)
                    {
                        throw new CommentaryException("No Commentary found");
                    }
                    CommentaryUpdated.Comment = Commentary.Comment;
                    CommentaryUpdated.Date_created = Commentary.Date_created;
                    CommentaryUpdated.User_username = Commentary.User_username;
                    CommentaryUpdated.Debate_id = Commentary.Debate_id;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new CommentaryException("Update", ex);
            }
        }

        public void Delete(int CommentaryId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteCommentary = context.Commentaries.FirstOrDefault(Commentary => Commentary.Id == CommentaryId);
                    if (deleteCommentary == null)
                    {
                        throw new CommentaryException("Commentary not found to delete");
                    }
                    context.Commentaries.Remove(deleteCommentary);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new CommentaryException("Delete", ex);
            }
        }
    }
}
