using MinHangWisdomParkWeb.Filters;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinHangWisdomParkWeb.Controllers
{
    /// <summary>
    /// 授权审核管理
    /// </summary>
    public class AuthorizationAuditController : BaseController
    {
        #region 参数


        ApplyHelp applyhelp = new ApplyHelp();

        BusinessApplicationController business = new BusinessApplicationController();
        private int pagesize;

        #endregion

        #region 业务申请授权
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult Index(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            ViewBag.BuniessList1 = BuniessList("4", 0, 10, "", GlobalParameter.UserId);
            ViewBag.BuniessList2 = BuniessList("5", 0, 10, "", GlobalParameter.UserId);
            ViewBag.BuniessList3 = BuniessList("6", 0, 10, "", GlobalParameter.UserId);
            ViewBag.BuniessList4 = BuniessList("7", 0, 10, "", GlobalParameter.UserId);
            ViewBag.RepairList1 = RepairList(1, 0, 10, "", GlobalParameter.UserId);
            ViewBag.RepairList2 = RepairList(2, 0, 10, "", GlobalParameter.UserId);


            Decimal[] BuniessNumber = new Decimal[6];
            BuniessNumber[0] = Math.Ceiling(Convert.ToDecimal(BuniessList("4", 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            BuniessNumber[1] = Math.Ceiling(Convert.ToDecimal(BuniessList("5", 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            BuniessNumber[2] = Math.Ceiling(Convert.ToDecimal(RepairList(1, 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            BuniessNumber[3] = Math.Ceiling(Convert.ToDecimal(RepairList(2, 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            BuniessNumber[4] = Math.Ceiling(Convert.ToDecimal(BuniessList("6", 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            BuniessNumber[5] = Math.Ceiling(Convert.ToDecimal(BuniessList("7", 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            //BuniessNumber[0] = 12;
            //BuniessNumber[1] = 11;
            //BuniessNumber[2] = 3;
            //BuniessNumber[3] = 9;
            ViewBag.BuniessNumber = BuniessNumber;
            return View();
        }

        //Math.Ceiling(Convert.ToDecimal(SelectBuniessAll("", "0", 1000000000, "5", GlobalParameter.UserId).Count()) / 10);
        #endregion

        #region 信息申请授权
        [UserChkAttribute.IsUser]
        public ActionResult PeblishIndex(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            ViewBag.PeblishList1 = PeblishList("1", 0, 10, "", GlobalParameter.UserId);
            ViewBag.PeblishList2 = PeblishList("2", 0, 10, "", GlobalParameter.UserId);
            ViewBag.PeblishList3 = PeblishList("3", 0, 10, "", GlobalParameter.UserId);
            ViewBag.PeblishList4 = PeblishList("4", 0, 10, "", GlobalParameter.UserId);
            ViewBag.PeblishList5 = PeblishList("5", 0, 10, "", GlobalParameter.UserId);


            decimal[] n = new decimal[5];
            n[0] = Math.Ceiling(Convert.ToDecimal(PeblishList("1", 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            n[1] = Math.Ceiling(Convert.ToDecimal(PeblishList("2", 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            n[2] = Math.Ceiling(Convert.ToDecimal(PeblishList("3", 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            n[3] = Math.Ceiling(Convert.ToDecimal(PeblishList("4", 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            n[4] = Math.Ceiling(Convert.ToDecimal(PeblishList("5", 0, 1000000000, "", GlobalParameter.UserId).Count()) / 10);
            ViewBag.n = n;
            return View();
        }


        #endregion

        #region 出入园区授权

        /// <summary>
        /// 页面视图
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <param name="lujin"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult InOrOut(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }

            ViewBag.CheckList1 = InOrOutApi("1", GlobalParameter.UserId, 0, 1000000000, "");
            ViewBag.CheckList2 = InOrOutApi("2", GlobalParameter.UserId, 0, 1000000000, "");

            Decimal[] BuniessNumber = new Decimal[6];
            BuniessNumber[0] = (int)Math.Ceiling(Convert.ToDecimal(InOrOutApi("1", GlobalParameter.UserId, 0, 1000000000, "").Count()) / 10);
            BuniessNumber[1] = (int)Math.Ceiling(Convert.ToDecimal(InOrOutApi("2", GlobalParameter.UserId, 0, 1000000000, "").Count()) / 10);

            ViewBag.BuniessNumber = BuniessNumber;
            return View();
        }


        /// <summary>
        /// 提取出入园区列表数据--审核人
        /// </summary>
        /// <param name="InOrOutType"></param>
        /// <param name="useid"></param>
        /// <param name="pageindex"></param>
        /// <param name="number"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<ShenHe> InOrOutApi(string InOrOutType, string useid, int pageindex, int number, string str)
        {
            var aa = (from a in dal.tbApplyBill
                      from c in dal.tbConfirmState
                      from u in dal.mtUser
                      from o in dal.mtOwner
                      from ch in dal.tbCheckInOut
                      where a.StateType == 1 &&
                      a.ApplyType == "CheckInOut" &&
                      a.ObjectID == ch.CheckID &&
                      c.ConfirmerID == useid &&
                      a.ApplyID == c.ApplyID &&
                      a.Updater == u.UserId &&
                      ch.InOutType == InOrOutType &&
                      u.OwnerId == o.OwnerId
                      select new ShenHe
                      {
                          ApplyID = SqlFunctions.StringConvert((double)a.ApplyID),
                          Title = ch.CheckTitle,
                          Time = (DateTime)ch.CreateTime,
                          Updater = u.UserName,
                          Content = ch.CheckID,
                          typeid = ch.InOutType,
                          ownerid = o.OwnerName
                      }).ToList();

            aa = aa.OrderByDescending(item => item.Time).ToList();
            string[] strs = str.Split(' ');
            aa = aa.Where(item =>
            {
                var tmp = item.Content + " " + item.Title + " " + item.Time.ToString() + " " + item.Updater + " " + item.ownerid;
                foreach (var s in strs)
                {
                    if (tmp.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();
            return Funs.fenye<ShenHe>(aa, pageindex, number);

        }


        #endregion

        #region 已审信息状态管理
        [UserChkAttribute.IsUser]
        public ActionResult YiShenHe(string Type, string Title, string lujin)
        {
            ViewBag.Type = Type;
            ViewBag.Title = Title;
            ViewBag.lujin = lujin;
            ViewBag.yishenhe = YiSHPeblish(0, 10, "");
            ViewBag.count = Math.Ceiling(Convert.ToDecimal(pagesize) / 10);
            return View();
        }

        /// <summary>
        /// 已审核信息提取--分页
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagenumber"></param>
        /// <param name="str"></param>
        /// <param name="useid"></param>
        /// <returns></returns>
        public List<ShenHe> YiSHPeblish(int pageindex, int pagenumber, string str)
        {
            List<ShenHe> s = new List<ShenHe>();

            s = (from a in dal.tbApplyBill
                 from u in dal.mtUser
                 from u1 in dal.mtUniversalCode
                 from o in dal.mtOwner
                 from p in dal.tbPeblish
                 where p.PeblishID == a.ObjectID
                 && a.StateType == 2
                 && u.UserId == p.Updater
                 && u.OwnerId == o.OwnerId
                 && u1.UniversalType == "PeblishType"
                 && a.ApplyType== "Peblish"
                 && SqlFunctions.StringConvert((double)u1.CodeID).Trim() == p.PeblishType
                 select new ShenHe
                 {
                     ApplyID = p.PeblishID,
                     Title = p.PeblishTitle,
                     Time = (DateTime)p.CreateTime,
                     ownerid = o.OwnerName,
                     Updater = u.UserName,
                     typeid = u1.CodeName,
                     Content = SqlFunctions.StringConvert((double)a.ApplyID)
                 }).OrderByDescending(item => item.Time).ToList();
            pagesize = s.Count();
            string[] strs = str.Split(' ');
            s = s.Where(item =>
            {
                var tmp = item.Title + " " + item.Content + " " + item.Time.ToString() + " " + item.Updater + " " + item.ownerid + " " + item.typeid + " " + item.Time;
                foreach (var it in strs)
                {
                    if (tmp.Contains(it))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();

            s = Funs.fenye<ShenHe>(s, pageindex, pagenumber);

            return s;
        }



        #endregion

        #region 报修申请授权

        /// <summary>
        /// 报修申请
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult RepairIndex(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            ViewBag.RepairList1 = RepairList(1, 0, 10, "", GlobalParameter.UserId);
            ViewBag.RepairList2 = RepairList(2, 0, 10, "", GlobalParameter.UserId);
            return View();
        }

        #endregion

        #region JsonResult

        /// <summary>
        /// 审核功能
        /// </summary>
        /// <param name="applyid"></param>
        /// <param name="content"></param>
        /// <param name="bools">1通过，0不通过</param>
        /// <returns></returns>
        public JsonResult ShenHe(string applyid, string content, string bools)
        {
            if (ShenHewebservice(applyid, content, bools) == 1)
            {
                return Json(new { msg = "OK" });
            }
            else
            {
                return Json(new { msg = "NO" });
            }
        }

        public JsonResult quxiaoshenhe(string applyid, string content, string bools)
        {
            if (quxiaoshenhewebserver(applyid, content, bools) == 1)
            {
                return Json(new { msg = "操作成功！" });
            }
            else
            {
                return Json(new { msg = "操作失败！" });
            }
        }

        public int quxiaoshenhewebserver(string applyid, string content, string bools)
        {
            try
            {
                applyid = applyid.Trim();
                int numapplyid;
                int.TryParse(applyid, out numapplyid);
                var a = dal.tbApplyBill.FirstOrDefault(item => item.ApplyID == numapplyid);
                a.StateType = 3;
                dal.SaveChanges();
                return 1;

            }
            catch (Exception ee)
            {
                return 0;
            }
        }

        public int ShenHewebservice(string applyid, string content, string bools)
        {
            try
            {
                applyid = applyid.Trim();
                int numapplyid, numbools;
                int.TryParse(applyid, out numapplyid);
                int.TryParse(bools, out numbools);
                applyhelp.UpdateComfirmStart(numapplyid, numbools, content);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="str"></param>
        /// <param name="typename"></param>
        /// <param name="typenumber"></param>
        /// <returns></returns>
        public JsonResult ShenHeFenYe(int pageindex, string str, string typename, int typenumber, int number)
        {
            try
            {
                List<ShenHe> s = new List<ShenHe>();
                switch (typename)
                {
                    case "peblish":
                        s = PeblishList(typenumber.ToString(), pageindex, number, str, GlobalParameter.UserId);
                        break;
                    case "buniess":
                        s = BuniessList(typenumber.ToString(), pageindex, number, str, GlobalParameter.UserId);
                        break;
                    case "check":
                        s = business.InOrOutJieKou(typenumber.ToString(), GlobalParameter.UserId, pageindex.ToString(), number, str).Select(item => new ShenHe { ApplyID = item.CheckID, Title = item.Title, Updater = item.StatusName, Time = (DateTime)item.Time }).ToList();
                        break;
                    case "repair":
                        s = RepairList(typenumber, pageindex, number, str, GlobalParameter.UserId);
                        break;
                    case "yishenhe":
                        s = YiSHPeblish(pageindex, number, str);
                        break;
                }
                return Json(s);
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
            }
        }

        #endregion

        #region WebService



        #endregion

        #region 数据

        /// <summary>
        /// 返回所有审核
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="number"></param>
        /// <param name="str"></param>
        /// <param name="useid"></param>
        /// <returns></returns>
        public List<ShenHe> ShenHeAll(int pageindex, int number, string str, string useid)
        {
            List<ShenHe> shenhe = new List<ShenHe>();
            shenhe.AddRange(BuniessList("4", 0, 10, "", useid));
            shenhe.AddRange(BuniessList("5", 0, 10, "", useid));
            shenhe.AddRange(RepairList(1, 0, 10, "", useid));
            shenhe.AddRange(RepairList(2, 0, 10, "", useid));

            shenhe = shenhe.OrderByDescending(item => item.Time).ToList();
            string[] strs = str.Split(' ');
            shenhe = shenhe.Where(item =>
            {
                var tmp = item.Title + " " + item.Content + " " + item.Time.ToString() + " " + item.Updater;
                foreach (var s in strs)
                {
                    if (tmp.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();
            return Funs.fenye<ShenHe>(shenhe, pageindex, number);

        }


        /// <summary>
        /// 报修
        /// </summary> 
        /// <returns></returns>                                                              
        public List<ShenHe> RepairList(int RepairType, int pageindex, int number, string str, string useid)
        {
            var aa = (from c in dal.tbConfirmState
                      from r in dal.tbRepair
                      from a in dal.tbApplyBill
                      from u in dal.mtUser
                      from o in dal.mtOwner
                      where a.ApplyID == c.ApplyID
                      && a.ObjectID == r.RepairID
                      && c.ConfirmerID == useid
                      && a.Updater == u.UserId
                      && r.RepairType == RepairType
                      && a.StateType == 1
                      && o.OwnerId == u.OwnerId
                      select new ShenHe
                      {
                          ApplyID = SqlFunctions.StringConvert((double)a.ApplyID),
                          Title = r.RepairTitle,
                          Content = r.RepairID,
                          Updater = u.UserName,
                          Time = (DateTime)a.UpdateTime,
                          typeid = SqlFunctions.StringConvert((double)r.RepairType),
                          ownerid = o.OwnerName
                      }).ToList();
            aa = aa.OrderByDescending(item => item.Time).ToList();
            string[] strs = str.Split(' ');
            aa = aa.Where(item =>
            {
                var tmp = item.Title + " " + item.Content + " " + item.Time.ToString() + " " + item.Updater;
                foreach (var s in strs)
                {
                    if (tmp.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();
            return Funs.fenye<ShenHe>(aa, pageindex, number);
        }



        /// <summary>
        ///提取信息发布个类别数据
        /// </summary>
        /// <param name="PeblishType"></param>
        /// <returns></returns>
        public List<ShenHe> PeblishList(string PeblishType, int pageindex, int number, string str, string useid)
        {
            var sq = (from p in dal.tbPeblish
                      from a in dal.tbApplyBill
                      from c in dal.tbConfirmState
                      from u in dal.mtUniversalCode
                      from u1 in dal.mtUser
                      from o in dal.mtOwner
                      where p.PeblishID == a.ObjectID &&
                      u.UniversalType == "StateType" &&
                      a.StateType == u.CodeID &&
                      p.PeblishType == PeblishType &&
                      a.ApplyType == "Peblish" &&
                      a.ApplyID == c.ApplyID &&
                      p.Updater == u1.UserId &&
                      c.ConfirmerID == useid &&
                      u1.OwnerId == o.OwnerId &&
                      a.StateType == 1
                      select new ShenHe
                      {
                          ApplyID = SqlFunctions.StringConvert((double)a.ApplyID),
                          Title = p.PeblishTitle,
                          Time = (DateTime)p.CreateTime,
                          Updater = u1.UserName,
                          Content = p.PeblishID,
                          typeid = p.PeblishType,
                          ownerid = o.OwnerName
                      }).ToList();

            sq = sq.OrderByDescending(item => item.Time).ToList();
            string[] strs = str.Split(' ');
            sq = sq.Where(item =>
            {
                var tmp = item.Content + " " + item.Title + " " + item.Time.ToString() + " " + item.Updater;
                foreach (var s in strs)
                {
                    if (tmp.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();

            return Funs.fenye<ShenHe>(sq, pageindex, number);
        }

        /// <summary>
        /// 业务提取数据
        /// </summary>
        /// <param name="BuniessType"></param>
        /// <param name="pageindex"></param>
        /// <param name="number"></param>
        /// <param name="str"></param>
        /// <param name="useid"></param>
        /// <returns></returns>
        public List<ShenHe> BuniessList(string BuniessType, int pageindex, int number, string str, string useid)
        {
            var cc = (from b in dal.tbBuniess
                      from a in dal.tbApplyBill
                      from c in dal.tbConfirmState
                      from u in dal.mtUser
                      from o in dal.mtOwner
                      where a.ObjectID == b.BuniessID
                      && a.ApplyType == "Buniess"
                      && c.ConfirmerID == useid
                      && a.ApplyID == c.ApplyID
                      && b.Updater == u.UserId
                      && b.BuniessType == BuniessType
                      && a.StateType == 1
                      && u.OwnerId == o.OwnerId
                      select new ShenHe
                      {
                          ApplyID = SqlFunctions.StringConvert((double)a.ApplyID),
                          Title = b.BuniessContent,
                          Time = (DateTime)b.CreateTime,
                          Updater = u.UserName,
                          Content = b.BuniessID,
                          typeid = b.BuniessType,
                          ownerid = o.OwnerName
                      }).ToList();
            cc = cc.OrderByDescending(item => item.Time).ToList();
            string[] strs = str.Split(' ');
            cc = cc.Where(item =>
            {
                var tmp = item.Content + " " + item.Title + " " + item.Time.ToString() + " " + item.Updater;
                foreach (var s in strs)
                {
                    if (tmp.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();
            return Funs.fenye<ShenHe>(cc, pageindex, number);
        }

        #endregion


    }


    #region 模型

    public class ShenHe
    {
        public string ApplyID { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string Updater { get; set; }
        public string Content { get; set; }
        public string typeid { get; set; }
        public string ownerid { get; set; }
    }

    #endregion
}
