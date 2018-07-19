using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Model
{
    public class BookUpdateArg : BookAddArg
    {
        /// <summary>
        /// 書籍ID
        /// </summary>
        public string BookId { get; set; }

        /// <summary>
        /// 借閱狀態
        /// </summary>
        [DisplayName("借閱狀態")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookStatus { get; set; }

        /// <summary>
        /// 借閱人
        /// </summary>
        [DisplayName("借閱人")]
        public string BookKeeper { get; set; }
    }
}
