using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinHangWisdomParkWeb
{
    /// <summary>
    /// 枚举数据类
    /// </summary>
    public class Enumerations
    {
        /// <summary>
        /// 业务申请状态
        /// </summary>
        public enum Business
        {
            未通过 = 0,
            审核中 = 1,
            已通过 = 2,
        };
    }
}