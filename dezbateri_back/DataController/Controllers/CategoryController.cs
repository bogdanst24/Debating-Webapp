using DatabaseAccess;
using DatabaseAccess.Repository;
using DataController.Security;
using DataController.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataController.Controllers
{
    public class CategoryController : ApiController
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController()
        {
            _categoryRepository = new CategoryRepository();
        }

        [HttpGet]
        public IHttpActionResult GetAllCategories()
        {

            try
            {
                List<Category> categories = _categoryRepository.GetAll();
                var resultCategories =
                   categories.Select(category => new Category
                   {
                       id = category.id,
                       name = category.name
                   });
                return Ok(resultCategories);
            }
            catch (Exception ex)
            {
                return Ok(new ErrorMessage(int.Parse(ex.Message)));
            }

        }

        [HttpGet]
        public IHttpActionResult GetById(int param)
        {
            try
            {
                AuthorizationH.Verify(Request, UserTypes.User, false, null);
                try
                {
                    Category category = _categoryRepository.GetById(param);
                    Category responseCateg = new Category
                    {
                        id = category.id,
                        name = category.name,
                        CategoryDebates = null
                    };
                    return Ok(responseCateg);
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
        public IHttpActionResult Add([FromBody] dynamic categBody)
        {
            try
            {
                if (categBody == null)
                    return Ok(new ErrorMessage(706));

                AuthorizationH.Verify(Request, UserTypes.Administrator, false, null);
                try
                {
                    try
                    {
                        Category category = new Category
                        {
                            id = 0,
                            name = categBody.name,
                            CategoryDebates = null
                        };
                        try
                        {
                            _categoryRepository.Add(category);
                            return Ok(new SuccessMessage(400));
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
        public IHttpActionResult Update([FromBody] dynamic categBody)
        {
            try
            {
                if (categBody == null)
                    return Ok(new ErrorMessage(706));

                AuthorizationH.Verify(Request, UserTypes.Administrator, false, null);
                try
                {
                    try
                    {
                        Category category = new Category
                        {
                            id = categBody.id,
                            name = categBody.name,
                            CategoryDebates = null
                        };
                        try
                        {
                            _categoryRepository.Update(category);
                            return Ok(new SuccessMessage(400));
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
        public IHttpActionResult Delete([FromBody] dynamic categBody)
        {
            try
            {
                if (categBody == null)
                    return Ok(new ErrorMessage(701));
                if (categBody.id == null)
                    return Ok(new ErrorMessage(701));


                AuthorizationH.Verify(Request, UserTypes.Administrator, false, null);
                try
                {
                    _categoryRepository.Delete(int.Parse(categBody.id.ToString()));
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
