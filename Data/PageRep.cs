using CMS_App.Interfaces;
using CMS_App.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace CMS_App.Data
{
    public class PageRep : IPage
    {
        private readonly PageContext _context = null;

        public PageRep(IOptions<Settings> settings)
        {
            _context = new PageContext(settings);
        }

        public Page GetAboutUs()
        {
            return _context.Pages.Find(x => x.PageName == "AboutUs").FirstOrDefault();
        }

        public Page GetIndex()
        {
            return _context.Pages.Find(x => x.PageName == "Index").FirstOrDefault();
        }

        public Page GetContact()
        {
            return _context.Pages.Find(x => x.PageName == "Contact").FirstOrDefault();
        }

        public IEnumerable<Page> GetAllPages()
        {
            return _context.Pages.Find(_ => true).ToList();
        }

        public bool UpdatePage(Page item)
        {
            var filter = Builders<Page>.Filter.Eq(s => s.PageName, item.PageName);
            var update = Builders<Page>.Update
                            .Set(s => s.PageTitle, item.PageTitle)
                            .Set(s => s.PageContent, item.PageContent)
                            .Set(s => s.Image, item.Image);
            try
            {
                UpdateResult actionResult = _context.Pages.UpdateOne(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
