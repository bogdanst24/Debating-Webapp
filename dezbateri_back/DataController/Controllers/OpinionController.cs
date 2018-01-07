using DatabaseAccess;
using DataController.Models;
using DataController.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataController.Controllers
{
    public class OpinionController : ApiController
    {
        private readonly OpinionServices _opinionServices;

        public OpinionController()
        {
            _opinionServices = new OpinionServices();
        }

        [HttpGet]
        public IHttpActionResult GetAllOpinions()
        {
            List<OpinionModel> opinions = _opinionServices.GetAllOpinions();
            return Ok(opinions);
        }

        [HttpGet]
        public IHttpActionResult GetOpinion(string param)
        {

            OpinionModel opinionModel = _opinionServices.GetOpinion(param);
            return Ok(opinionModel);
        }

        [HttpPost]
        public IHttpActionResult AddOpinion(dynamic newOpinion)
        {
            _opinionServices.AddOpinion(newOpinion);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeleteOpinion(dynamic body)
        {
            _opinionServices.deleteOpinion(body.opinion_id.ToString());
            return Ok();
        }


        [HttpPost]
        public IHttpActionResult AddArgument(dynamic body)
        {
            _opinionServices.AddArgument(body);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult CastVote(dynamic body)
        {
            _opinionServices.CastVote(body.OpinionId.ToString(), body.User_email.ToString(), body.Side.ToString());
            return Ok();
        }

      
    }
}
