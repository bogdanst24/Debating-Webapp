using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataController.Models;
using DatabaseAccess.Repository.OpinionRepos;
using DatabaseAccess.Repository.ArgumentRepos;
using DatabaseAccess;
using DatabaseAccess.Repository;
using DatabaseAccess.Repository.OpinionVoteRepos;
using DatabaseAccess.Exceptions;

namespace DataController.Services
{
    public class OpinionServices
    {
        OpinionRepository _opinionRepository;
        ArgumentRepository _argumentRepository;
        CategoryOpinionRepository _categoryOpinionRepository;
        CategoryRepository _categoryRepository;
        OpinionVoteRepository _voteOpinionRepository;
        UserRepository _userRepository;

        public OpinionServices()
        {
            _opinionRepository = new OpinionRepository();
            _argumentRepository = new ArgumentRepository();
            _categoryOpinionRepository = new CategoryOpinionRepository();
            _categoryRepository = new CategoryRepository();
            _voteOpinionRepository = new OpinionVoteRepository();
            _userRepository = new UserRepository();
        }

        internal List<OpinionModel> GetAllOpinions()
        {
            List<OpinionModel> allOpinions = new List<OpinionModel>();

            List<Opinion> opinions = _opinionRepository.GetAll();
            foreach(Opinion op in opinions)
            {
                OpinionModel om = new OpinionModel();
                om.OpinionId = op.Id.ToString();
                om.Subject = op.Subject;
                om.Date_created = op.Date_created;
                om.User_email = op.User_email;
                om.Picture_url = op.Picture_url;

                om.User_username = _userRepository.GetByEmail(op.User_email).Username.ToString();

                List<String> categList = new List<String>();
                List<CategoryOpinion> categories = _categoryOpinionRepository.GetAllOfOpinion(op.Id);
                foreach(CategoryOpinion co in categories)
                {
                    Category categ = _categoryRepository.GetById(co.Categ_id);
                    categList.Add(categ.name);
                }
                om.Category = categList;

                List<Argument> allOpinionArguments = _argumentRepository.GetAllOfOpinion(op.Id);
                foreach (Argument ar in allOpinionArguments)
                {
                    ar.Opinion = new Opinion();
                    ar.User = new User();
                    ar.User.Email = ar.User_email;
                    ar.User.Username = _userRepository.GetByEmail(ar.User_email).Username;
                }
                om.Arguments = allOpinionArguments;
                om.Pro_votes = "0";
                om.Con_votes = "0";
                
                allOpinions.Add(om);
 
            }

            return allOpinions;
        }

        internal OpinionModel GetOpinion(string op_id)
        {
            Opinion op = _opinionRepository.GetById(int.Parse(op_id));

            OpinionModel om = new OpinionModel();
            om.OpinionId = op.Id.ToString();
            om.Subject = op.Subject;
            om.Date_created = op.Date_created;
            om.User_email = op.User_email;
            om.Picture_url = op.Picture_url;
            om.User_username = _userRepository.GetByEmail(op.User_email).Username.ToString();

            List<String> categList = new List<String>();
            List<CategoryOpinion> categories = _categoryOpinionRepository.GetAllOfOpinion(op.Id);
            foreach (CategoryOpinion co in categories)
            {
                Category categ = _categoryRepository.GetById(co.Categ_id);
                categList.Add(categ.name);
            }
            om.Category = categList;

            List<Argument> allOpinionArguments = _argumentRepository.GetAllOfOpinion(op.Id);
            foreach(Argument ar in allOpinionArguments)
            {
                ar.Opinion = new Opinion();
                ar.User = new User();
                ar.User.Email = ar.User_email;
                ar.User.Username = _userRepository.GetByEmail(ar.User_email).Username;
            }
            om.Arguments = allOpinionArguments;

            try { 
                List<OpinionVote> votes = _voteOpinionRepository.GetAll();
                int pro = 0, con = 0;
                foreach (OpinionVote ov in votes)
                {
                    if (ov.Opinion_id == int.Parse(op_id))
                    {
                        if (ov.Vote_pro)
                            pro++;
                        else
                            con++;
                    }
                }
                om.Pro_votes = pro.ToString();
                om.Con_votes = con.ToString();
            }
            catch 
            {
                om.Pro_votes = "0";
                om.Con_votes = "0";
            }
          

            return om;
        }

        internal void AddOpinion(dynamic newOpinion)
        {
            User user= _userRepository.GetByUsername(newOpinion.User_email.ToString());
            Opinion opinion= new Opinion
            {
                Subject = newOpinion.Subject,
                Date_created = newOpinion.Date_created,
                User_email = user.Email,
                Picture_url = newOpinion.Picture_url
            };

            Opinion addedOpinion = _opinionRepository.Add(opinion);
            var id = addedOpinion.Id;

            foreach (string categ in newOpinion.Category)
            {
                int categ_id = _categoryRepository.GetByName(categ).id;
                _categoryOpinionRepository.Add(new CategoryOpinion
                {
                    Opinion_id = id,
                    Categ_id = categ_id
                });
            }
        }

        internal void deleteOpinion(string id)
        {
            int op_id = int.Parse(id);
            List<Argument> args = _argumentRepository.GetAllOfOpinion(op_id);
            if(args != null)
            {
                foreach (Argument ar in args)
                    _argumentRepository.Delete(ar.Id);
            }

            List<CategoryOpinion> co =_categoryOpinionRepository.GetAllOfOpinion(op_id);
            if(co != null)
            {
                foreach (CategoryOpinion c in co)
                    _categoryOpinionRepository.Delete(c.Id);
            }

            _opinionRepository.Delete(op_id);
           
        }


        internal void AddArgument(dynamic body)
        {
            var username = body.User_email;
            User user = _userRepository.GetByUsername(username.ToString());
            Argument ag = new Argument
            {
                Side = body.Syde,
                Content = body.Content.ToString(),
                Date_created = body.Date_Created.ToString(),
                Opinion_id = int.Parse(body.Opinion_id.ToString()),
                User_email = user.Email
            };
            _argumentRepository.Add(ag);
        }

        internal void CastVote(String OpinionId, String User_email, String Side)
        {
            OpinionVote vote = new OpinionVote();
            vote.Opinion_id = int.Parse(OpinionId);
            vote.User_email = User_email;
            if (Side == "pro") vote.Vote_pro = true;
            else vote.Vote_pro = false;

            try
            {
                OpinionVote opinionVote = _voteOpinionRepository.GetVoteOfUserOnOpinion(int.Parse(OpinionId), User_email);
                if (opinionVote.Vote_pro == vote.Vote_pro)
                {
                    _voteOpinionRepository.Delete(opinionVote.Id);
                }
                else
                {
                    opinionVote.Vote_pro = vote.Vote_pro;
                    _voteOpinionRepository.Update(opinionVote);
                }
            }
            catch (VoteException vex)
            {
                if (vex.Message == "333")
                {
                    _voteOpinionRepository.Add(vote);
                }

            }
        }
    }
}