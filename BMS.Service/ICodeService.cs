using System.Collections.Generic;
using System.Web.Mvc;

namespace BMS.Service
{
    public interface ICodeService
    {
        List<SelectListItem> GetAllBookClass();
        List<SelectListItem> GetAllBookKeeper();
        List<SelectListItem> GetAllBookName();
        List<SelectListItem> GetAllBookStatus();
    }
}