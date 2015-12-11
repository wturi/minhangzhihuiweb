using MinHangWisdomParkWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinHangWisdomParkWeb.Controllers
{
    /// <summary>
    /// 数据查询分析
    /// </summary>
    public class DataQueryController : BaseController
    {
        #region 页面

        /// <summary>
        /// 营收
        /// </summary>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult Revenue()
        {
            return View();
        }

        #endregion
    }
}
