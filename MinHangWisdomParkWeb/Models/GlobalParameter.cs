using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinHangWisdomParkWeb
{
    public class GlobalParameter
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public static string UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public static string UserName { get; set; }


        public static int? Actorid { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public static string ZongName { get; set; } = "上海市莘庄工业区西区";

        /// <summary>
        /// logo图片地址
        /// </summary>
        public static string LogoUrl { get; set; } = "~/img/Main/LOGO2.png";




    }


}