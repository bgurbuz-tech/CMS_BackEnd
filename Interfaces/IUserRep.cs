using System.Collections.Generic;
using System.Threading.Tasks;
using CMS_App.Model;
using Microsoft.AspNetCore.Mvc;

namespace CMS_App.Interfaces
{
    public interface IUserRep
    {
        IEnumerable<User> GetAllUsers();

        User GetUser(string userMail);      
        
        // add new user
        bool AddUser(User item);

        // remove a single user
        bool RemoveUser(string userId);

        // update just a single user
        bool UpdateUser(string id, User item);


        bool Authenticate(string usermail, string pass);
    }
}
