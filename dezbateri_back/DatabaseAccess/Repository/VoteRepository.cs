using DatabaseAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository
{
    public class VoteRepository
    {
        public void Add(Vote vote)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    context.Votes.Add(vote);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new VoteException("Add", ex);
            }
        }

        public List<Vote> GetAll()
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var votes = context.Votes;
                    if (votes.ToList().Count == 0)
                    {
                        throw new VoteException("Table is empty");
                    }
                    return votes.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new VoteException("Add", ex);
            }
        }

        public Vote GetById(int voteDebateId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var voteFound = context.Votes.FirstOrDefault(vote => vote.vote_id == voteDebateId);
                    if (voteFound == null)
                    {
                        throw new VoteException("No vote found");
                    }
                    return voteFound;
                }
            }
            catch (Exception ex)
            {
                throw new VoteException("Find", ex);
            }
        }

        public void Update(Vote vote)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var voteUpdated = context.Votes.FirstOrDefault(voteObj => voteObj.vote_id == vote.vote_id);
                    if (voteUpdated == null)
                    {
                        throw new VoteException("No vote found");
                    }
                    voteUpdated.debate_id = vote.debate_id;
                    voteUpdated.user_username = vote.user_username;
                    voteUpdated.vote_pro = voteUpdated.vote_pro;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new VoteException("Update", ex);
            }
        }

        public void Delete(int voteId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteVote = context.Votes.FirstOrDefault(vote => vote.vote_id == voteId);
                    if (deleteVote == null)
                    {
                        throw new VoteException("Vote not found to delete");
                    }
                    context.Votes.Remove(deleteVote);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new VoteException("Delete", ex);
            }
        }
    }
}
