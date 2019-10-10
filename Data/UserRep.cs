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
    public class UserRep : IUserRep
    {
        private readonly UserContext _context = null;

        public UserRep(IOptions<Settings> settings)
        {
            _context = new UserContext(settings);
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return _context.Users.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //public async Task<User> GetUser(string id)
        //{
        //    try
        //    {
        //        ObjectId internalId = GetInternalId(id);
        //        return await _context.Users
        //                        .Find(user => user.UserId == id || user.InternalId == internalId)
        //                        .FirstOrDefaultAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        // Try to convert the Id to a BSonId value
        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        public bool Authenticate(string usermail, string pass)
        {
            try
            {
                var f = _context.Users.Find(x => x.UserEmail == usermail && x.UserPassword == pass);
                if (f.CountDocuments() == 0) 
                    return false;
                else { 

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetUser(string usermail)
        {
            try
            {
                return  _context.Users.Find(x => x.UserEmail == usermail).FirstOrDefault();
            }
            catch ( Exception ex)
            { throw ex; }
        }

        public bool AddUser(User item)
        {
            try
            {                
                _context.Users.InsertOne(item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveUser(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                DeleteResult actionResult = _context.Users.DeleteOne(
                     Builders<User>.Filter.Eq("InternalId", internalId));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateUser(string id, User item)
        {
            ObjectId objID = GetInternalId(id);
            var filter = Builders<User>.Filter.Eq(s => s.InternalId, objID);
            var update = Builders<User>.Update
                            .Set(s => s.UserPassword, item.UserPassword)
                            .Set(s => s.UserType, item.UserType)
                            .Set(s => s.UserStatus, item.UserStatus)
                            .CurrentDate(s => s.UserModified);

            try
            {
                UpdateResult actionResult = _context.Users.UpdateOne(filter, update);

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
