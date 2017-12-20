using DatabaseAccess;
using DatabaseAccess.Repository;
using DataController.Services;
using DataController.Utility;
using DataController.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataController.Controllers
{
    public class UserController : ApiController
    {
        private readonly UserRepository _userRepository;
        private readonly UserServices _userServices;

        public UserController()
        {
            _userRepository = new UserRepository();
            _userServices = new UserServices();
        }

        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            try
            {
                AuthorizationH.Verify(Request, UserTypes.User, false, null);
                try
                {
                    List<User> users = _userRepository.GetAll();
                    return Ok(users);
                }
                catch (Exception ex)
                {
                    return Ok(new ErrorMessage(int.Parse(ex.Message)));
                }
            }
            catch (AuthorizationException ex)
            {
                return Ok(new ErrorMessage(int.Parse(ex.Message)));
            }
        }

        [HttpGet]
        public IHttpActionResult GetByUsername(string param)
        {
            try
            {
                if (param == null)
                    return Ok(new ErrorMessage(701));


                AuthorizationH.Verify(Request, UserTypes.User, false, null);
                try
                {
                    User user = _userRepository.GetByUsername(param);
                    user.Password = "";
                    return Ok(user);
                }
                catch (Exception ex)
                {
                    return Ok(new ErrorMessage(int.Parse(ex.Message)));
                }
            }
            catch (AuthorizationException ex)
            {
                return Ok(new ErrorMessage(int.Parse(ex.Message)));
            }
        }

        [HttpPost]
        public IHttpActionResult GetByEmail([FromBody] dynamic body)
        {
            try
            {
                if (body == null)
                    return Ok(new ErrorMessage(701));
                if (body.email == "")
                    return Ok(new ErrorMessage(701));


                AuthorizationH.Verify(Request, UserTypes.User, false, null);
                try
                {
                    User user = _userRepository.GetByEmail(body.email.ToString());
                    user.Password = "";
                    return Ok(user);
                }
                catch (Exception ex)
                {
                    return Ok(new ErrorMessage(int.Parse(ex.Message)));
                }
            }
            catch (AuthorizationException ex)
            {
                return Ok(new ErrorMessage(int.Parse(ex.Message)));
            }
        }

        [HttpPost]
        public IHttpActionResult Add([FromBody] dynamic userBody)
        {
            try
            {
                if (userBody == null)
                    return Ok(new ErrorMessage(706));

                AuthorizationH.Verify(Request, UserTypes.Administrator, false, null);
                try
                {
                    try
                    {
                        User user = new User();
                        user.Username = userBody.username;
                        user.Email = userBody.email;
                        user.Birthdate = userBody.birthdate;
                        user.Password = userBody.password;
                        user.Type = userBody.type;
                        try
                        {
                            if (_userServices.CheckIfDupplicate(user)) { 
                                 _userRepository.Add(user);
                                return Ok(new SuccessMessage(400));
                            } 
                            return Ok(new ErrorMessage(807));
                        }
                        catch (Exception ex)
                        {
                            return Ok(new ErrorMessage(int.Parse(ex.Message)));
                        }
                    }
                    catch
                    {
                        return Ok(new ErrorMessage(706));
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new ErrorMessage(int.Parse(ex.Message)));
                }
            }
            catch (AuthorizationException ex)
            {
                return Ok(new ErrorMessage(int.Parse(ex.Message)));
            }
        }

        [HttpPost]
        public IHttpActionResult Update([FromBody] dynamic userBody)
        {
            try
            {
                if (userBody == null)
                    return Ok(new ErrorMessage(706));

                AuthorizationH.Verify(Request, UserTypes.User, true, userBody.email.ToString());
                try
                {
                    try
                    {
                        User user = new User();
                        user.Username = userBody.username;
                        user.Email = userBody.email;
                        user.Birthdate = userBody.birthdate;
                        user.Password = userBody.password;
                        user.Type = userBody.type;
                        try
                        {
                            if (_userServices.CheckIfDupplicateUsername(user))
                            {
                                _userRepository.Update(user);
                                return Ok(new SuccessMessage(400));
                            }
                            return Ok(new ErrorMessage(807));
                        }
                        catch (Exception ex)
                        {
                            return Ok(new ErrorMessage(int.Parse(ex.Message)));
                        }
                    }
                    catch
                    {
                        return Ok(new ErrorMessage(706));
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new ErrorMessage(int.Parse(ex.Message)));
                }
            }
            catch (AuthorizationException ex)
            {
                return Ok(new ErrorMessage(int.Parse(ex.Message)));
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromBody] dynamic userBody)
        {
            try
            {
                if (userBody == null)
                    return Ok(new ErrorMessage(701));
                if(userBody.email == null)
                    return Ok(new ErrorMessage(701));


                AuthorizationH.Verify(Request, UserTypes.Administrator, false, null);
                try
                {
                    _userRepository.Delete(userBody.email.ToString());
                    return Ok(new SuccessMessage(400));
                }
                catch (Exception ex)
                {
                    return Ok(new ErrorMessage(int.Parse(ex.Message)));
                }
            }
            catch (AuthorizationException ex)
            {
                return Ok(new ErrorMessage(int.Parse(ex.Message)));
            }
        }
    }
}
