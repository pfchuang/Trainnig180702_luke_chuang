using System.Collections.Generic;
using BMS.Model;

namespace BMS.Service
{
    public interface IBookService
    {
        int AddBook(BookAddArg arg);
        void DeleteBookById(string bookId);
        List<BookSearchResult> GetBookByCondition(BookSearchArg arg);
        BookUpdateArg GetBookById(string bookId);
        string UpdateBook(BookUpdateArg arg);
    }
}