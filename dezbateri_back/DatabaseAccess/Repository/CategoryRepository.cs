using DatabaseAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository
{
    public class CategoryRepository
    {
        public void Add(Category category)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    context.Categories.Add(category);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new CategoryException("810", ex);
            }
        }

        public List<Category> GetAll()
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var categories = context.Categories;
                    if (categories.ToList().Count == 0)
                    {
                        check = true;
                    }
                    return categories.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new CategoryException("811", ex);
            }
            finally
            {
                if(check) throw new CategoryException("812");
            }
        }

        public Category GetById(int categoryId)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var categoryFound = context.Categories.FirstOrDefault(category => category.id == categoryId);
                    if (categoryFound == null)
                    {
                        check = true;
                    }
                    return categoryFound;
                }
            }
            catch (Exception ex)
            {
                throw new CategoryException("814", ex);
            }
            finally
            {
                if (check) throw new UserException("812");
            }
        }

        public void Update(Category category)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var categoryUpdated = context.Categories.FirstOrDefault(categoryObj => categoryObj.id == category.id);
                    if (categoryUpdated == null)
                    {
                        check = true;
                    }
                    categoryUpdated.name = category.name;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new CategoryException("815", ex);
            }
            finally
            {
                if (check) throw new UserException("813");
            }
        }

        public void Delete(int categoryId)
        {
            Boolean check = false;
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteCategory = context.Categories.FirstOrDefault(category => category.id == categoryId);
                    if (deleteCategory == null)
                    {
                        check = true;
                    }
                    context.Categories.Remove(deleteCategory);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new CategoryException("816", ex);
            }
            finally
            {
                if (check) throw new UserException("813");
            }
        }

    }
}
