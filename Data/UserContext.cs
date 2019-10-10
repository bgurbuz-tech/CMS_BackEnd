using MongoDB.Driver;
using Microsoft.Extensions.Options;
using CMS_App.Model;

namespace CMS_App.Data
{
    public class UserContext
    {
        private readonly IMongoDatabase _database = null;

        public UserContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
