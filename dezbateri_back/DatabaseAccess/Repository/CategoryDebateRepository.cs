using DatabaseAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository
{
    public class CategoryDebateRepository
    {
        public void Add(CategoryDebate categoryDebate)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    context.CategoryDebates.Add(categoryDebate);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new CategoryDebateException("Add", ex);
            }
        }

        public List<CategoryDebate> GetAll()
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var categoryDebates = context.CategoryDebates;
                    if (categoryDebates.ToList().Count == 0)
                    {
                        throw new CategoryDebateException("Table is empty");
                    }
                    return categoryDebates.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new CategoryDebateException("Add", ex);
            }
        }

      

        public void Update(CategoryDebate categoryDebate)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var categoryDebateUpdated = context.CategoryDebates.FirstOrDefault(categoryDebateObj => categoryDebateObj.debate_id == categoryDebate.debate_id);
                    if (categoryDebateUpdated == null)
                    {
                        throw new CategoryDebateException("No categoryDebate found");
                    }
                    categoryDebateUpdated.category_id = categoryDebate.category_id;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new CategoryDebateException("Update", ex);
            }
        }

        public void Delete(int categoryDebateId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteCategoryDebate = context.CategoryDebates.FirstOrDefault(categoryDebate => categoryDebate.debate_id == categoryDebateId);
                    if (deleteCategoryDebate == null)
                    {
                        throw new CategoryDebateException("CategoryDebate not found to delete");
                    }
                    context.CategoryDebates.Remove(deleteCategoryDebate);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new CategoryDebateException("Delete", ex);
            }
        }
    }
}
