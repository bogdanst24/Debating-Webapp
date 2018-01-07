using DatabaseAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repository
{
    public class ContentRepository
    {
        public void Add(Content content)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    context.Contents.Add(content);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ContentException("Add", ex);
            }
        }

        public List<Content> GetAll()
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var contents = context.Contents;
                    if (contents.ToList().Count == 0)
                    {
                        throw new ContentException("Table is empty");
                    }
                    return contents.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new ContentException("Add", ex);
            }
        }

        public Content GetById(int contentDebateId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var contentFound = context.Contents.FirstOrDefault(content => content.debate_id == contentDebateId);
                    if (contentFound == null)
                    {
                        throw new ContentException("No content found");
                    }
                    return contentFound;
                }
            }
            catch (Exception ex)
            {
                throw new ContentException("Find", ex);
            }
        }

        public void Update(Content content)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var contentUpdated = context.Contents.FirstOrDefault(contentObj => contentObj.debate_id == content.debate_id);
                    if (contentUpdated == null)
                    {
                        throw new ContentException("No content found");
                    }
                    contentUpdated.round_1 = content.round_1;
                    contentUpdated.round_2 = content.round_2;
                    contentUpdated.round_3 = content.round_3;
                    contentUpdated.round_4 = content.round_4;
                    contentUpdated.round_5 = content.round_5;
                    contentUpdated.round_6 = content.round_6;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ContentException("Update", ex);
            }
        }

        public void Delete(int contentId)
        {
            try
            {
                using (var context = new dezbateriEntities())
                {
                    var deleteContent = context.Contents.FirstOrDefault(content => content.debate_id == contentId);
                    if (deleteContent == null)
                    {
                        throw new ContentException("Content not found to delete");
                    }
                    context.Contents.Remove(deleteContent);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ContentException("Delete", ex);
            }
        }
    }
}
