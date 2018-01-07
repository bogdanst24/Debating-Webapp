using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository.OpinionRepos
{
    public class CategoryOpinionRepository
    {
        public void Add(CategoryOpinion categoryOpinion)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    context.CategoryOpinions.Add(categoryOpinion);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("810", ex);
            }
        }

        public List<CategoryOpinion> GetAll()
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var categoryOpinions = context.CategoryOpinions;
                    if (categoryOpinions.ToList().Count == 0)
                    {
                        check = true;
                    }
                    return categoryOpinions.ToList();
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

        public CategoryOpinion GetById(int categoryOpinionId)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var categoryOpinionFound = context.CategoryOpinions.FirstOrDefault(categoryOpinion => categoryOpinion.Id == categoryOpinionId);
                    if (categoryOpinionFound == null)
                    {
                        check = true;
                    }
                    return categoryOpinionFound;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("814", ex);
            }
            finally
            {
                if (check) throw new Exception("812");
            }
        }

        public List<CategoryOpinion> GetAllOfOpinion(int opinion_id)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    List<CategoryOpinion> categoryOpinionFound = context.CategoryOpinions.Where(categoryOpinion => categoryOpinion.Opinion_id == opinion_id).ToList();
                    if (categoryOpinionFound == null)
                    {
                        check = true;
                    }
                    return categoryOpinionFound;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("814", ex);
            }
            finally
            {
                if (check) throw new Exception("812");
            }
        }

        public void Update(CategoryOpinion categoryOpinion)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var categoryOpinionUpdated = context.CategoryOpinions.FirstOrDefault(categoryOpinionObj => categoryOpinionObj.Id == categoryOpinion.Id);
                    if (categoryOpinionUpdated == null)
                    {
                        check = true;
                    }
                    categoryOpinionUpdated.Categ_id = categoryOpinion.Categ_id;
                    categoryOpinionUpdated.Opinion_id = categoryOpinion.Opinion_id;

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

        public void Delete(int categoryOpinionId)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteCategoryOpinion = context.CategoryOpinions.FirstOrDefault(categoryOpinion => categoryOpinion.Id == categoryOpinionId);
                    if (deleteCategoryOpinion == null)
                    {
                        check = true;
                    }
                    context.CategoryOpinions.Remove(deleteCategoryOpinion);
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
