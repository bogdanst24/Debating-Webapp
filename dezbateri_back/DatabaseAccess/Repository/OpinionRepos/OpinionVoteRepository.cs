using DatabaseAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository.OpinionVoteRepos
{
    public class OpinionVoteRepository
    {
        public OpinionVote Add(OpinionVote opinionVote)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    OpinionVote op = context.OpinionVotes.Add(opinionVote);
                    context.SaveChanges();
                    return op;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("810", ex);
            }
        }

        public List<OpinionVote> GetAll()
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var opinionVotes = context.OpinionVotes;
                    if (opinionVotes.ToList().Count == 0)
                    {
                        check = true;
                    }
                    return opinionVotes.ToList();
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

        public OpinionVote GetVoteOfUserOnOpinion(int opinionId, string user_email)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var query = context.OpinionVotes.Where(vote => vote.Opinion_id == opinionId);
                    OpinionVote voteFound = query.FirstOrDefault(vote => vote.User_email == user_email);

                    if (voteFound == null)
                    {
                        throw new VoteException("333");
                    }
                    return voteFound;
                }
            }
            catch (Exception ex)
            {
                throw new VoteException(ex.Message);
            }
           
        }

        public void Update(OpinionVote opinionVote)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var opinionVoteUpdated = context.OpinionVotes.FirstOrDefault(opinionVoteObj => opinionVoteObj.Id == opinionVote.Id);
                    if (opinionVoteUpdated == null)
                    {
                        check = true;
                    }
                    opinionVoteUpdated.Vote_pro = opinionVote.Vote_pro;

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

        public void Delete(int opinionVoteId)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteOpinionVote = context.OpinionVotes.FirstOrDefault(opinionVote => opinionVote.Id == opinionVoteId);
                    if (deleteOpinionVote == null)
                    {
                        check = true;
                    }
                    context.OpinionVotes.Remove(deleteOpinionVote);
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
