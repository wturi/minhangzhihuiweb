using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinHangWisdomParkWeb.Models
{
    public class ConfirmeMenos
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string ConfirmeMeno { get; set; }
        public string ConfirmeID { get; set; }
        public int State { get; set; } //1：已通过；2：正要审核；3：未审核；4：未通过

    }
}