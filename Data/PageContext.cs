using MongoDB.Driver;
using Microsoft.Extensions.Options;
using CMS_App.Model;

namespace CMS_App.Data
{
    public class PageContext
    {
        private readonly IMongoDatabase _database = null;

        public PageContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Page> Pages => _database.GetCollection<Page>("Pages");

    }
}
