using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinHangWisdomParkWeb.Controllers
{
    public class BaseController : Controller
    {
        public Models.ajIIPdbEntities1 dal = new Models.ajIIPdbEntities1();
    }
}