using DatabaseAccess;
using DataController.Models;
using DataController.Security;
using DataController.Services;
using DataController.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataController.Controllers
{
    public class DebateController : ApiController
    {
        private readonly DebateServices _debateServices;

        public DebateController()
        {
            _debateServices = new DebateServices();
        }

        [HttpGet]
        public IHttpActionResult GetAllDebates()
        {
            List<DebateModel> debateModel = _debateServices.GetAllDebates();
            return Ok(debateModel);
        }

        [HttpGet]
        public IHttpActionResult GetDebate(string param)
        {
            try
            {
                try { 
                    string username = AuthToken.getIss(Request.Headers.GetValues("token").First());
                    DebateModel debateModel = _debateServices.GetDebate(param, username);
                    return Ok(debateModel);
                }
                catch { 
                    DebateModel debateModel = _debateServices.GetDebate(param, "null");
                    return Ok(debateModel);
                }
            }
            catch (AuthorizationException ex)
            {
                return Ok(new ErrorMessage(int.Parse(ex.Message)));
            }
        }

        [HttpGet]
        public IHttpActionResult GetDebateVotes(string param)
        {
            List<DatabaseAccess.Vote> allVotes = _debateServices.GetAllVotesOfDebate(param);
            return Ok(allVotes);
        }


        [HttpGet]
        public IHttpActionResult GetDebateState(string param)
        {
            DebateModel debateModel = _debateServices.getDebateState(param);
            return Ok(debateModel);
        }

        [HttpPost]
        public IHttpActionResult AddDebate(dynamic newDebate)
        {

            _debateServices.AddDebate(newDebate);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeleteDebate(dynamic body)
        {
            _debateServices.deleteDebate(body.debate_id.ToString());
            return Ok();
        }


        [HttpPost]
        public IHttpActionResult JoinDebate(dynamic body)
        {
            String debate_id = (body.debate_id).ToString();
            String user_id = (body.user_id).ToString();
            _debateServices.JoinDebate(debate_id, user_id);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult UpdateDebateContent(dynamic body)
        {
            _debateServices.updateDebateContent(body);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult GoToNextRound(dynamic body)
        {
            _debateServices.NextRound(body.debate_id.ToString());
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Vote(dynamic body)
        {
            String debate_id = body.debate_id;
            String voteCasted = body.voteCasted;
            String userVoting = body.userVoting;
            _debateServices.Vote(debate_id, voteCasted, userVoting);
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetAllComments(string param)
        {
            List<Commentary> allComs = _debateServices.GetAllComments(param);
            return Ok(allComs);
        }

        [HttpPost]
        public IHttpActionResult AddComment(dynamic body)
        {
            String debate_id = body.Debate_id;
            String commentary = body.Comment;
            String date_created = body.Date_created;
            String user_username = body.User_username;
            Commentary comm = _debateServices.AddComment(debate_id, commentary, date_created, user_username);
            return Ok(comm);
        }

      
    }
}
