using DatabaseAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository
{
    public class RoundStateRepository
    {
        public void Add(RoundState roundState)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    context.RoundStates.Add(roundState);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new RoundStateException("Add", ex);
            }
        }

        public List<RoundState> GetAll()
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var roundStates = context.RoundStates;
                    if (roundStates.ToList().Count == 0)
                    {
                        throw new RoundStateException("Table is empty");
                    }
                    return roundStates.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new RoundStateException("Add", ex);
            }
        }

        public RoundState GetById(int roundStateDebateId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var roundStateFound = context.RoundStates.FirstOrDefault(roundState => roundState.debate_id == roundStateDebateId);
                    if (roundStateFound == null)
                    {
                        throw new RoundStateException("No roundState found");
                    }
                    return roundStateFound;
                }
            }
            catch (Exception ex)
            {
                throw new RoundStateException("Find", ex);
            }
        }

        public void Update(RoundState roundState)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var roundStateUpdated = context.RoundStates.FirstOrDefault(roundStateObj => roundStateObj.debate_id == roundState.debate_id);
                    if (roundStateUpdated == null)
                    {
                        throw new RoundStateException("No roundState found");
                    }
                    roundStateUpdated.next_round = roundState.next_round;
                    roundStateUpdated.time_to_next = roundState.time_to_next;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new RoundStateException("Update", ex);
            }
        }

        public void Delete(int roundStateId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteRoundState = context.RoundStates.FirstOrDefault(roundState => roundState.debate_id == roundStateId);
                    if (deleteRoundState == null)
                    {
                        throw new RoundStateException("RoundState not found to delete");
                    }
                    context.RoundStates.Remove(deleteRoundState);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new RoundStateException("Delete", ex);
            }
        }
    }
}
