using DatabaseAccess;
using DatabaseAccess.Exceptions;
using DatabaseAccess.Repository;
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
    public class LoginController : ApiController
    {
        private readonly UserRepository _userRepository;
        private readonly UserServices _userServices;

        public LoginController()
        {
            _userRepository = new UserRepository();
            _userServices = new UserServices();
        }

        [HttpPost]
        public IHttpActionResult Register([FromBody] dynamic UserReg)
        {
            try
            {
                if (UserReg.username != "")
                {
                    if (UserReg.password != "")
                    {
                        if (UserReg.email != "")
                        {
                            if (UserReg.birthdate != "")
                            {
                                try
                                {
                                    User user = new User();
                                    user.Username = UserReg.username;
                                    user.Email = UserReg.email;
                                    user.Password = UserReg.password;
                                    user.Birthdate = UserReg.birthdate;
                                    user.Type = UserTypes.User.ToString();
                                    try
                                    {
                                        _userServices.RegisterUser(user);
                                        _userRepository.Add(user);
                                        _userServices.VerifyEmail(user);
                                        return Ok(new SuccessMessage());
                                    }
                                    catch (Exception ex)
                                    {
                                        return Ok(new ErrorMessage(int.Parse(ex.Message)));
                                    }


                                }
                                catch
                                {
                                    return Ok(new ErrorMessage(800));
                                }
                            }
                            else
                            {
                                return Ok(new ErrorMessage(703));
                            }
                        }
                        else
                        {
                            return Ok(new ErrorMessage(701));
                        }
                    }
                    else
                    {
                        return Ok(new ErrorMessage(700));
                    }
                }
                else
                {
                    return Ok(new ErrorMessage(701));
                }
            }
            catch
            {
                return Ok(new ErrorMessage(101));
            }

        }

        [HttpPost]
        public IHttpActionResult Login([FromBody] dynamic UserLog)
        {
            try
            {
                User user = new User();
                if (UserLog.password.ToString() != "")
                {
                    var password = UserLog.password.ToString();

                    if (UserLog.email.ToString() != "")
                    {
                        var email = UserLog.email.ToString();
                        try
                        {
                            try { 
                                _userServices.CheckIfActivated("email", email.ToString());
                            }
                            catch(Exception ex)
                            {
                                return Ok(new ErrorMessage(int.Parse(ex.Message)));
                            }
                            user = _userRepository.GetByEmail(email);
                            var username = user.Username;
                            if (user.Password == password)
                            {
                                var token = AuthToken.CreateJwt(username, email, UserTypesClass.Convert(user.Type), 450000000);
                                return Ok(token);
                            }
                        }
                        catch (UserException ex)
                        {
                            return Ok(new ErrorMessage(int.Parse(ex.Message)));
                        }
                    }
                    else if (UserLog.username.ToString() != "")
                    {
                        var username = UserLog.username.ToString();
                        try
                        {
                            _userServices.CheckIfActivated("username", username.ToString());
                            user = _userRepository.GetByUsername(username);
                            if (user.Password == password)
                            {
                                var token = AuthToken.CreateJwt(username.ToString(), user.Email, UserTypesClass.Convert(user.Type), 450000000);
                                return Ok(token);
                            }
                        }
                        catch (UserException ex)
                        {
                            return Ok(new ErrorMessage(int.Parse(ex.Message)));
                        }
                    }
                    else
                    {
                        return Ok(new ErrorMessage(701));
                    }
                }
                else
                {
                    return Ok(new ErrorMessage(703));
                }

                return Ok(new ErrorMessage(702));
            }
            catch
            {
                return Ok(new ErrorMessage(101));
            }

        }

        [HttpPost]
        public IHttpActionResult CheckCode([FromBody] dynamic body)
        {
            var response = false;
            if (body.email == null || body.code == null)
                return Ok(response);
            response = _userServices.CheckCode(body.email.ToString(), body.code.ToString());
            return Ok(response);
        }

        [HttpPost]
        public IHttpActionResult ResendCode([FromBody] dynamic email)
        {
            _userServices.ResendVerifyEmail(email.ToString());
            return Ok();
        }
    }
}
