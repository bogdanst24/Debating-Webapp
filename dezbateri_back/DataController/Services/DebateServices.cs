using DatabaseAccess;
using DatabaseAccess.Exceptions;
using DatabaseAccess.Repository;
using DataController.Models;
using DataController.Security;
using DataController.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataController.Services
{
    public class DebateServices
    {
        DebateRepository _debateRepository;
        CategoryDebateRepository _categoryDebateRepository;
        CategoryRepository _categoryRepository;
        UserDebateRepository _userDebateRepository;
        RoundStateRepository _roundStateRepository;
        ContentRepository _contentRepository;
        UserRepository _userRepository;
        VoteRepository _voteRepository;
        CommentaryRepository _commentaryRepository;

        private long threeDaysInMillis = 259200000;


        public DebateServices()
        {
            _debateRepository = new DebateRepository();
            _categoryDebateRepository = new CategoryDebateRepository();
            _categoryRepository = new CategoryRepository();
            _userDebateRepository = new UserDebateRepository();
            _roundStateRepository = new RoundStateRepository();
            _contentRepository = new ContentRepository();
            _userRepository = new UserRepository();
            _voteRepository = new VoteRepository();
            _commentaryRepository = new CommentaryRepository();
        }

        internal List<DebateModel> GetAllDebates()
        {
            List<DebateInfo> allDebates = _debateRepository.GetAll();
            List<DebateModel> allModelDebates = new List<DebateModel>();
            foreach(DebateInfo deb in allDebates)
            {
                DebateModel dm = new DebateModel();
                dm.DebateId = deb.debate_id.ToString();
                dm.Subject = deb.subject;
                dm.Date_created = deb.date_created;
                dm.Description = deb.description;
                dm.State = deb.state;

                List<CategoryDebate> categDebList = _categoryDebateRepository.GetAll().FindAll(categ => categ.debate_id.ToString() == dm.DebateId);
                List<String> debateCategories = new List<String>();
                foreach(CategoryDebate cd in categDebList)
                {
                    debateCategories.Add(_categoryRepository.GetById(cd.category_id).name);
                }
                dm.Category = debateCategories;

                UserDebate userDebList = _userDebateRepository.GetById(int.Parse(dm.DebateId));
                dm.Pro_username = userDebList.pro_username;
                dm.Con_username = userDebList.con_username;

                RoundState roundState = _roundStateRepository.GetById(int.Parse(dm.DebateId));
                dm.Next_round = roundState.next_round.ToString();
                dm.Time_to_next = roundState.time_to_next;

                Content debContent = _contentRepository.GetById(int.Parse(dm.DebateId));
                dm.Round_1 = debContent.round_1;
                dm.Round_2 = debContent.round_2;
                dm.Round_3 = debContent.round_3;
                dm.Round_4 = debContent.round_4;
                dm.Round_5 = debContent.round_5;
                dm.Round_6 = debContent.round_6;

                allModelDebates.Add(dm);
            }

            return allModelDebates;
        }

        internal void AddDebate(dynamic newDebate)
        {
            DebateInfo debate = new DebateInfo
            {
                subject = newDebate.Subject,
                date_created = newDebate.Date_created,
                state = newDebate.State,
                description = newDebate.Description,
            };

            dynamic addedDebate = _debateRepository.Add(debate);
            var id = addedDebate.debate_id;

            Content newContent = new Content
            {
                debate_id = id,
                round_1 = newDebate.Round_1,
                round_2 = newDebate.Round_2,
                round_3 = newDebate.Round_3,
                round_4 = newDebate.Round_4,
                round_5 = newDebate.Round_5,
                round_6 = newDebate.Round_6
            };
            _contentRepository.Add(newContent);

            foreach(string categ in newDebate.Category)
            {
                int categ_id = _categoryRepository.GetByName(categ).id;
                _categoryDebateRepository.Add(new CategoryDebate
                {
                    debate_id = id,
                    category_id = categ_id
                });
            }

            _userDebateRepository.Add(new UserDebate
            {
                debate_id = id,
                pro_username = newDebate.Pro_username,
                con_username = newDebate.Con_username
            });

            _roundStateRepository.Add(new RoundState
            {
                debate_id = id,
                next_round = 1,
                time_to_next = (AuthToken.GetNistTime() + threeDaysInMillis).ToString()
            });

        }

      

        internal DebateModel GetDebate(string param, string username)
        {
            DebateModel dm = new DebateModel();

            DebateInfo di = _debateRepository.GetById(int.Parse(param));
            dm.DebateId = di.debate_id.ToString();
            dm.Subject = di.subject;
            dm.Date_created = di.date_created;
            dm.State = di.state;
            dm.Description = di.description;

            List<CategoryDebate> categDebList = _categoryDebateRepository.GetAll().FindAll(categ => categ.debate_id.ToString() == dm.DebateId);
            List<String> debateCategories = new List<String>();
            foreach (CategoryDebate cd in categDebList)
            {
                debateCategories.Add(_categoryRepository.GetById(cd.category_id).name);
            }
            dm.Category = debateCategories;

            UserDebate userDebList = _userDebateRepository.GetById(int.Parse(dm.DebateId));
            dm.Pro_username = userDebList.pro_username;
            dm.Con_username = userDebList.con_username;

            RoundState roundState = _roundStateRepository.GetById(int.Parse(dm.DebateId));
            dm.Next_round = roundState.next_round.ToString();
            dm.Time_to_next = roundState.time_to_next;

            Content debContent = _contentRepository.GetById(int.Parse(dm.DebateId));
            dm.Round_1 = debContent.round_1;
            dm.Round_2 = debContent.round_2;
            dm.Round_3 = debContent.round_3;
            dm.Round_4 = debContent.round_4;
            dm.Round_5 = debContent.round_5;
            dm.Round_6 = debContent.round_6;

            CheckRoundVisible(username, dm);

            return dm;
        }

        private void CheckRoundVisible(string username, DebateModel dm)
        {
            var round = dm.Next_round;
            Boolean isVisitor = (username == "null");
            switch (round)
            {
                case "1":
                    if(username != dm.Pro_username || isVisitor)
                    {
                        dm.Round_1 = "Discursul inca nu a fost publicat";
                    }
                    break;
                case "2":
                    if (username != dm.Con_username || isVisitor)
                    {
                        dm.Round_2 = "Discursul inca nu a fost publicat";
                    }
                    break;
                case "3":
                    if (username != dm.Pro_username || isVisitor)
                    {
                        dm.Round_3 = "Discursul inca nu a fost publicat";
                    }
                    break;
                case "4":
                    if (username != dm.Con_username || isVisitor)
                    {
                        dm.Round_4 = "Discursul inca nu a fost publicat";
                    }
                    break;
                case "5":
                    if (username != dm.Pro_username || isVisitor)
                    {
                        dm.Round_5 = "Discursul inca nu a fost publicat";
                    }
                    break;
                case "6":
                    if (username != dm.Con_username || isVisitor)
                    {
                        dm.Round_6 = "Discursul inca nu a fost publicat";
                    }
                    break;
                default: break;
            }
        }

        internal DebateModel getDebateState(string param)
        {
            throw new NotImplementedException();
        }

        internal void deleteDebate(string id)
        {
            _contentRepository.Delete(int.Parse(id));
            _roundStateRepository.Delete(int.Parse(id));
            _categoryDebateRepository.Delete(int.Parse(id));
            _userDebateRepository.Delete(int.Parse(id));
            _debateRepository.Delete(int.Parse(id));
        }

        internal void JoinDebate(string debate_id, string user_id)
        {
            RoundState rs = _roundStateRepository.GetById(int.Parse(debate_id.ToString()));
            long next_speech = AuthToken.GetNistTime() + threeDaysInMillis;
            rs.time_to_next = next_speech.ToString();
            _roundStateRepository.Update(rs);

            User us = _userRepository.GetByUsername(user_id);
       
            UserDebate ud = _userDebateRepository.GetById(int.Parse(debate_id.ToString()));
            ud.con_username = us.Username;
            _userDebateRepository.Update(ud);

            User pro_us = _userRepository.GetByUsername(ud.pro_username);

            DebateInfo di = _debateRepository.GetById(int.Parse(debate_id.ToString()));
            di.state = "desfasurare";
            _debateRepository.Update(di);

            MailService.SendStartDebateMail(pro_us, us, di);
        }

        internal void updateDebateContent(dynamic body)
        {
            Content content = _contentRepository.GetById(int.Parse(body.debate_id.ToString()));
            content.round_1 = body.round_1;
            content.round_2 = body.round_2;
            content.round_3 = body.round_3;
            content.round_4 = body.round_4;
            content.round_5 = body.round_5;
            content.round_6 = body.round_6;
            _contentRepository.Update(content);
        }

        internal void NextRound(string debate_id)
        {
            RoundState debateState = _roundStateRepository.GetById(int.Parse(debate_id));
            if (debateState != null)
            {
                UserDebate ud = _userDebateRepository.GetById(int.Parse(debate_id.ToString()));
                User pro_username = _userRepository.GetByUsername(ud.pro_username);
                User con_username = _userRepository.GetByUsername(ud.con_username);
                DebateInfo di = _debateRepository.GetById(int.Parse(debate_id));

                if (debateState.next_round == 6)
                {
                    di.state = "incheiat";
                    _debateRepository.Update(di);

                    MailService.SendDoneDebateMail(pro_username, con_username, di);
                }
                else
                {
                    int currentRound = debateState.next_round;
                    debateState.next_round = ++currentRound;

                    long actualTime = AuthToken.GetNistTime();
                    long nextRoundTime = actualTime + threeDaysInMillis;
                    debateState.time_to_next = nextRoundTime.ToString();
                    _roundStateRepository.Update(debateState);

                    MailService.SendNextRoundMail(pro_username, con_username, di, debateState.next_round);
                }
            }
           
        }

        internal void Vote(string debate_id, string voteCasted, string userVoting)
        {
            Vote vote = new DatabaseAccess.Vote();
            vote.debate_id = int.Parse(debate_id);
            vote.user_username = userVoting;
            if (voteCasted == "pro") vote.vote_pro = true;
            else vote.vote_pro = false;

            try
            {
                Vote debateVote = _voteRepository.GetDebateVote(int.Parse(debate_id), userVoting);
                if(debateVote.vote_pro == vote.vote_pro)
                {
                    _voteRepository.Delete(debateVote.vote_id);
                } else
                {
                    debateVote.vote_pro = vote.vote_pro;
                    _voteRepository.Update(debateVote);
                }
            }
            catch (VoteException vex)
            {
                if (vex.Message == "333")
                {
                    _voteRepository.Add(vote);
                }

            }
        }

        internal List<Vote> GetAllVotesOfDebate(string param)
        {
            List<Vote> allVotes = _voteRepository.GetById(int.Parse(param));
            foreach (Vote v in allVotes)
            {
                v.DebateInfo = new DebateInfo();
            }
            return (allVotes);
        }

        internal List<Commentary> GetAllComments(string param)
        {
            List<Commentary> allComs = _commentaryRepository.GetAllByDebateId(int.Parse(param));
            return allComs;

        }

        internal Commentary AddComment(string debate_id, string commentary, string date_created, string user_username)
        {
            Commentary com = new Commentary
            {
                Debate_id = int.Parse(debate_id),
                Comment = commentary,
                Date_created = date_created,
                User_username = user_username
            };
            Commentary newComm = _commentaryRepository.Add(com);
            return newComm;
        }

    }
}