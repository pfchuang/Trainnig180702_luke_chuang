using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Service
{
    public class BookService : IBookService
    {
        private BMS.Dao.IBookDao bookDao { get; set; }

        public List<BMS.Model.BookSearchResult> GetBookByCondition(BMS.Model.BookSearchArg arg)
        {         
            return bookDao.GetBookByCondition(arg);
        }

        public int AddBook(BMS.Model.BookAddArg arg)
        {
            return bookDao.AddBook(arg);
        }

        public void DeleteBookById(string bookId)
        {
            bookDao.DeleteBookById(bookId);
        }

        public string UpdateBook(BMS.Model.BookUpdateArg arg)
        { 
            return bookDao.UpdateBook(arg);
        }

        public BMS.Model.BookUpdateArg GetBookById(string bookId)
        {
            return bookDao.GetBookById(bookId);
        }
    }
}
