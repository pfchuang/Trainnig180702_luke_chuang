using System.Collections.Generic;
using BMS.Model;

namespace BMS.Dao
{
    public interface IBookDao
    {
        int AddBook(BookAddArg arg);
        void DeleteBookById(string bookId);
        List<BookSearchResult> GetBookByCondition(BookSearchArg arg);
        BookUpdateArg GetBookById(string bookId);
        string UpdateBook(BookUpdateArg arg);
    }
}