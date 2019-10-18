using CMS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_App.Interfaces
{
    public interface IPostRep
    {
        bool AddPost(Post item);
        IEnumerable<Post> GetPosts();
        Post GetPost(string id);
        bool UpdatePost(string id,Post item);
        bool ArchievePost(string id);
    }
}
