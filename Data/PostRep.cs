using CMS_App.Interfaces;
using CMS_App.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_App.Data
{
    public class PostRep : IPostRep
    {
        private readonly PostContext _context = null;

        public PostRep(IOptions<Settings> settings)
        {
            _context = new PostContext(settings);
        }

        public bool AddPost(Post item)
        {
            try
            {
                item.PostState = "Draft";
                _context.Posts.InsertOne(item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ArchievePost(string id)
        {
            ObjectId objectId = GetInternalId(id);
            var filter = Builders<Post>.Filter.Eq(s => s.InternalId, objectId);
            var update = Builders<Post>.Update.Set(s => s.PostState, "Archived");
            try
            {
                UpdateResult actionResult = _context.Posts.UpdateOne(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        public Post GetPost(string id)
        {
            try
            {
                ObjectId objectId = GetInternalId(id);
                return _context.Posts.Find(x => x.InternalId == objectId).FirstOrDefault();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IEnumerable<Post> GetPosts()
        {
            try
            {
                return _context.Posts.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdatePost(string id, Post item)
        {
            ObjectId objectId = GetInternalId(id);
            var filter = Builders<Post>.Filter.Eq(s => s.InternalId, objectId);
            var update = Builders<Post>.Update
                                       .Set(s => s.PostState, item.PostState)
                                       .Set(s => s.PostContentBrief, item.PostContentBrief)
                                       .Set(s => s.PostContentExtended, item.PostContentExtended)
                                       .Set(s => s.PostImage, item.PostImage)
                                       .Set(s => s.PostModifiedDate, DateTime.Now);
            try
            {
                UpdateResult actionResult = _context.Posts.UpdateOne(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdatePostState(string id, string type)
        {
            ObjectId objectId = GetInternalId(id);
            var filter = Builders<Post>.Filter.Eq(s => s.InternalId, objectId);
            var update =  Builders<Post>.Update
                                       .Set(s => s.PostState, type)
                                       .Set(s => s.PostModifiedDate, DateTime.Now);
            try
            {
                UpdateResult actionResult = _context.Posts.UpdateOne(filter, update);

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
