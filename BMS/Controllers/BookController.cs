using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMS.Controllers
{
    public class BookController : Controller
    {
        private BMS.Service.ICodeService codeService { get; set; }
        private BMS.Service.IBookService bookService { get; set; }

        /// <summary>
        /// 查詢書籍畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                BMS.Common.Logger.Write(BMS.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return View("Error");
            }
        }

        /// <summary>
        /// 新增書籍畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult AddBook()
        {
            return View();
        }

        /// <summary>
        /// 修改書籍畫面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult UpdateBook(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        /// <summary>
        /// 查詢書籍
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult SearchBook(BMS.Model.BookSearchArg arg)
        {
            return Json(bookService.GetBookByCondition(arg));
        }

        /// <summary>
        /// 新增書籍
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult Add(BMS.Model.BookAddArg arg)
        {
            bookService.AddBook(arg);
            return Json("success");
        }

        /// <summary>
        /// 修改書籍
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult Update(BMS.Model.BookUpdateArg arg)
        {
            bookService.UpdateBook(arg);
            return Json("success");
        }

        /// <summary>
        /// 刪除書籍
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteBook(string bookId)
        {
            try
            {
                bookService.DeleteBookById(bookId);
                return Json(true);
            }

            catch (Exception ex)
            {
                return Json(false);
            }
        }

        /// <summary>
        /// 取得待修改書籍
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetUpdateBook(string bookId)
        {
            return Json(bookService.GetBookById(bookId));
        }

        /// <summary>
        /// 取得所有書名
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetAllBookName()
        {
            return Json(codeService.GetAllBookName());
        }

        /// <summary>
        /// 取得所有圖書類別
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetAllBookClass()
        {
            return Json(codeService.GetAllBookClass());
        }

        /// <summary>
        /// 取得所有借閱人
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetAllBookKeeper()
        {
            return Json(codeService.GetAllBookKeeper());
        }

        /// <summary>
        /// 取得所有借閱狀態
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetAllBookStatus()
        {
            return Json(codeService.GetAllBookStatus());
        }
    }
}