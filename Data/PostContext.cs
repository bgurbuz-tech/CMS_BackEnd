using MongoDB.Driver;
using Microsoft.Extensions.Options;
using CMS_App.Model;

namespace CMS_App.Data
{
    public class PostContext
    {
        private readonly IMongoDatabase _database = null;

        public PostContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Posts");
    }
}
