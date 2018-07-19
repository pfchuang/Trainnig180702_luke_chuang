using System.Collections.Generic;
using System.Web.Mvc;

namespace BMS.Dao
{
    public interface ICodeDao
    {
        List<SelectListItem> GetAllBookClass();
        List<SelectListItem> GetAllBookKeeper();
        List<SelectListItem> GetAllBookName();
        List<SelectListItem> GetAllBookStatus();
    }
}