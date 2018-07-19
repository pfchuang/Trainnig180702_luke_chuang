using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BMS.Service
{
    public class CodeService : ICodeService
    {
        private BMS.Dao.ICodeDao codeDao { get; set; }

        public List<SelectListItem> GetAllBookName()
        {
            return codeDao.GetAllBookName();
        }

        public List<SelectListItem> GetAllBookKeeper()
        {
            return codeDao.GetAllBookKeeper();
        }

        public List<SelectListItem> GetAllBookStatus()
        {
            return codeDao.GetAllBookStatus();
        }

        public List<SelectListItem> GetAllBookClass()
        {
            return codeDao.GetAllBookClass();
        }
    }
}
