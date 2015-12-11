using MinHangWisdomParkWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinHangWisdomParkWeb.Controllers
{
    /// <summary>
    /// 运行收入管理
    /// </summary>
    public class OperatingIncomeController : BaseController
    {
        #region 页面
        /// <summary>
        /// 卡证发行
        /// </summary>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult Card()
        {
            return View();
        }

        /// <summary>
        /// 广告收入
        /// </summary>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult Advertisement()
        {
            return View();
        }

        /// <summary>
        /// 服务收入
        /// </summary>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult Service()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult kazhenfaxing(string Type, string Title)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
            }
            return View();
        }


        #endregion
    }
}
