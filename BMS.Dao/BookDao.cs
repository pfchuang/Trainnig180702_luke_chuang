using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Dao
{
    public class BookDao : IBookDao
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return BMS.Common.ConfigTool.GetDBConnectionString("DBConn");
        }

        /// <summary>
        /// 依照條件取得書籍資料
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public List<BMS.Model.BookSearchResult> GetBookByCondition(BMS.Model.BookSearchArg arg)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString())){
            
                
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Parameters.Add(new SqlParameter("@BookName", arg.BookName ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@BookClass", arg.BookClass ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", arg.BookKeeper ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@BookStatus", arg.BookStatus ?? string.Empty));
				cmd.CommandText = "SELECT BOOK_DATA.BOOK_ID AS BookId,
                                  BOOK_CLASS.BOOK_CLASS_NAME AS BookClass,
                                  BOOK_DATA.BOOK_NAME AS BookName,
                                  CONVERT(varchar(10), BOOK_DATA.BOOK_BOUGHT_DATE, 111) AS BookBoughtDate,
                                  BOOK_CODE.CODE_NAME AS BookStatus,
                                  CONCAT(MEMBER_M.USER_ENAME, '(', MEMBER_M.USER_CNAME, ')') AS BookKeeper 
                           FROM BOOK_DATA
	                         INNER JOIN BOOK_CLASS
                               ON BOOK_DATA.BOOK_CLASS_ID = BOOK_CLASS.BOOK_CLASS_ID
                             INNER JOIN BOOK_CODE 
                               ON BOOK_DATA.BOOK_STATUS = BOOK_CODE.CODE_ID 
	                         LEFT JOIN MEMBER_M 
                               ON BOOK_DATA.BOOK_KEEPER = MEMBER_M.USER_ID
                           WHERE (UPPER(BOOK_DATA.BOOK_NAME) LIKE UPPER('%' + @BookName + '%') OR @BookName = '') AND
                                 (BOOK_DATA.BOOK_CLASS_ID = @BookClass OR @BookClass = '') AND
                                 (BOOK_DATA.BOOK_KEEPER = @BookKeeper OR @BookKeeper = '') AND
                                 (BOOK_DATA.BOOK_STATUS = @BookStatus OR @BookStatus = '') AND
                                  BOOK_DATA.BOOK_BOUGHT_DATE < GETDATE()
                           ORDER BY BookBoughtDate DESC";
                
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                conn.Open();
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookDataToList(dt);
        }

        /// <summary>
        /// 新增書籍
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public int AddBook(BMS.Model.BookAddArg arg)
        {
            string sql = @"INSERT INTO BOOK_DATA
                         (
                             BOOK_NAME, BOOK_AUTHOR, BOOK_PUBLISHER,
                             BOOK_BOUGHT_DATE, BOOK_NOTE, BOOK_CLASS_ID, BOOK_STATUS,
                             BOOK_KEEPER, CREATE_DATE, CREATE_USER
                         )
                           VALUES
                         (
                             @BookName, @BookAuthor, @BookPublisher,
                             @BookBoughtDate, @BookNote, @BookClassID, 'A',
                             '', GETDATE(), '3008'
                         )
                         SELECT SCOPE_IDENTITY()";
            int bookId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookName", arg.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookAuthor", arg.BookAuthor));
                cmd.Parameters.Add(new SqlParameter("@BookPublisher", arg.BookPublisher));
                cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", arg.BookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@BookNote", arg.BookNote));
                cmd.Parameters.Add(new SqlParameter("@BookClassID", arg.BookClass));
                bookId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return bookId;
        }

        /// <summary>
        /// 刪除書籍
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteBookById(string bookId)
        {
            try
            {
                string sql = "Delete FROM BOOK_DATA Where BOOK_ID=@BookId";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// 修改書籍
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public string UpdateBook(BMS.Model.BookUpdateArg arg)
        {
            string sql = @"UPDATE BOOK_DATA SET
                             BOOK_NAME = @BookName, BOOK_AUTHOR = @BookAuthor, BOOK_PUBLISHER = @BookPublisher,
                             BOOK_BOUGHT_DATE = @BookBoughtDate, BOOK_NOTE = @BookNote, BOOK_CLASS_ID = @BookClassId,
                             BOOK_STATUS = @BookStatus, BOOK_KEEPER = @BookKeeper, MODIFY_DATE = GETDATE(), MODIFY_USER = '3008'
                          WHERE BOOK_ID = @BookId";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", arg.BookId));
                cmd.Parameters.Add(new SqlParameter("@BookName", arg.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookAuthor", arg.BookAuthor));
                cmd.Parameters.Add(new SqlParameter("@BookPublisher", arg.BookPublisher));
                cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", arg.BookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@BookNote", arg.BookNote));
                cmd.Parameters.Add(new SqlParameter("@BookClassId", arg.BookClass));
                cmd.Parameters.Add(new SqlParameter("@BookStatus", arg.BookStatus));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", arg.BookKeeper ?? string.Empty));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return arg.BookId;
        }

        /// <summary>
        /// 依照書籍ID取得書籍資料
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public BMS.Model.BookUpdateArg GetBookById(string bookId)
        {
            DataTable dt = new DataTable();
            string sql = @"Select BOOK_ID, BOOK_NAME, BOOK_AUTHOR, BOOK_PUBLISHER, BOOK_NOTE,
                                  CONVERT(varchar(10), BOOK_BOUGHT_DATE, 111) AS BOOK_BOUGHT_DATE,
                                  BOOK_CLASS_ID, BOOK_STATUS, BOOK_KEEPER
                           From BOOK_DATA
                           Where BOOK_ID = @BookId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookToModel(dt);
        }

        /// <summary>
        /// Map書籍資料進Model
        /// </summary>
        /// <param name="bookItem"></param>
        /// <returns></returns>
        private BMS.Model.BookUpdateArg MapBookToModel(DataTable bookItem)
        {
            BMS.Model.BookUpdateArg result = new BMS.Model.BookUpdateArg();
            result.BookId = bookItem.Rows[0]["BOOK_ID"].ToString();
            result.BookName = bookItem.Rows[0]["BOOK_NAME"].ToString();
            result.BookAuthor = bookItem.Rows[0]["BOOK_AUTHOR"].ToString();
            result.BookPublisher = bookItem.Rows[0]["BOOK_PUBLISHER"].ToString();
            result.BookNote = bookItem.Rows[0]["BOOK_NOTE"].ToString();
            result.BookBoughtDate = bookItem.Rows[0]["BOOK_BOUGHT_DATE"].ToString();
            result.BookClass = bookItem.Rows[0]["BOOK_CLASS_ID"].ToString();
            result.BookStatus = bookItem.Rows[0]["BOOK_STATUS"].ToString();
            result.BookKeeper = bookItem.Rows[0]["BOOK_KEEPER"].ToString();

            return result;
        }

        /// <summary>
        /// Map資料進List
        /// </summary>
        /// <param name="bookData"></param>
        /// <returns></returns>
        private List<BMS.Model.BookSearchResult> MapBookDataToList(DataTable bookData)
        {
            List<BMS.Model.BookSearchResult> result = new List<BMS.Model.BookSearchResult>();
            foreach (DataRow row in bookData.Rows)
            {
                result.Add(new BMS.Model.BookSearchResult()
                {
                    BookId = (int)row["BookId"],
                    BookClass = row["BookClass"].ToString(),
                    BookName = row["BookName"].ToString(),
                    BookBoughtDate = row["BookBoughtDate"].ToString(),
                    BookStatus = row["BookStatus"].ToString(),
                    BookKeeper = row["BookKeeper"].ToString() == "()" ? null : row["BookKeeper"].ToString(),
                });
            }
            return result;
        }
    }
}
