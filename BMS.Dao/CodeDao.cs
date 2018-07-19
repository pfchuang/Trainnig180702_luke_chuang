using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BMS.Dao
{
    public class CodeDao : ICodeDao
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return BMS.Common.ConfigTool.GetDBConnectionString("DBConn");
        }

        public List<SelectListItem> GetAllBookName()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                string sql = @"SELECT DISTINCT BOOK_NAME AS Item
                               FROM BOOK_DATA";
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
                conn.Close();
            }

            return this.MapBookName(dt);
        }

        /// <summary>
        /// 取得所有借閱人資料進List
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllBookKeeper()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT DISTINCT USER_ENAME AS Ename, USER_CNAME AS Cname, USER_ID AS Id
                                    FROM MEMBER_M";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
                conn.Close();
            }

            return this.MapKeeperName(dt);
        }

        /// <summary>
        /// 取得所有借閱狀態資料進List
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllBookStatus()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT DISTINCT CODE_NAME AS Item, CODE_ID AS Id
                                    FROM BOOK_CODE
                                    WHERE CODE_TYPE = 'BOOK_STATUS'";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
                conn.Close();
            }

            return this.MapBookData(dt);
        }

        /// <summary>
        /// 取得所有圖書類別資料進List
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllBookClass()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT DISTINCT BOOK_CLASS_NAME AS Item, BOOK_CLASS_ID AS Id
                                    FROM BOOK_CLASS";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
                conn.Close();
            }

            return this.MapBookData(dt);
        }

        /// <summary>
        /// Map書籍資料進List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapBookData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["Item"].ToString(),
                    Value = row["Id"].ToString()
                });
            }

            return result;
        }

        /// <summary>
        /// Map借閱人資料進List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapKeeperName(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["Ename"].ToString() + '(' + row["Cname"] + ')',
                    Value = row["Id"].ToString()
                });
            }

            return result;
        }

        private List<SelectListItem> MapBookName(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["Item"].ToString()
                });
            }

            return result;
        }
    }
}
