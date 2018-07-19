using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Model
{
    public class BookSearchResult : BookSearchArg
    {
        /// <summary>
        /// 書籍ID
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// 購書日期
        /// </summary>
        [DisplayName("購書日期")]
        public string BookBoughtDate { get; set; }
    }
}
