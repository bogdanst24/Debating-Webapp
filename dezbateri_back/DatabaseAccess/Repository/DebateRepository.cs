using DatabaseAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository
{
    public class DebateRepository
    {
        public void Add(DebateInfo debate)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    context.DebateInfoes.Add(debate);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DebateException("Add", ex);
            }
        }

        public List<DebateInfo> GetAll()
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var debates = context.DebateInfoes;
                    if (debates.ToList().Count == 0)
                    {
                        throw new DebateException("Table is empty");
                    }
                    return debates.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DebateException("Add", ex);
            }
        }

        public DebateInfo GetById(int debateId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var debateFound = context.DebateInfoes.FirstOrDefault(debate => debate.debate_id == debateId);
                    if (debateFound == null)
                    {
                        throw new DebateException("No debate found");
                    }
                    return debateFound;
                }
            }
            catch (Exception ex)
            {
                throw new DebateException("Find", ex);
            }
        }

        public void Update(DebateInfo debate)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var debateUpdated = context.DebateInfoes.FirstOrDefault(debateObj => debateObj.debate_id == debate.debate_id);
                    if (debateUpdated == null)
                    {
                        throw new DebateException("No debate found");
                    }
                    debateUpdated.subject = debate.subject;
                    debateUpdated.date_created = debate.date_created;
                    debateUpdated.state = debate.state;
                    debateUpdated.description = debate.description;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DebateException("Update", ex);
            }
        }

        public void Delete(int debateId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteDebate = context.DebateInfoes.FirstOrDefault(debate => debate.debate_id == debateId);
                    if (deleteDebate == null)
                    {
                        throw new DebateException("Debate not found to delete");
                    }
                    context.DebateInfoes.Remove(deleteDebate);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DebateException("Delete", ex);
            }
        }
    }
}
