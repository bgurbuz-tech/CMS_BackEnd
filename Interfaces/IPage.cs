using CMS_App.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS_App.Interfaces
{
    public interface IPage
    {
        Page GetIndex();
        Page GetAboutUs();
        Page GetContact();
        IEnumerable<Page> GetAllPages();
        bool UpdatePage(Page item);
    }
}
