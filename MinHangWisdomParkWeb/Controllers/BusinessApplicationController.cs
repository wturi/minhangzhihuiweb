using MinHangWisdomParkWeb.Filters;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace MinHangWisdomParkWeb.Controllers
{
    /// <summary>
    /// 业务申请
    /// </summary>
    public class BusinessApplicationController : BaseController
    {

        #region 参数


        ApplyHelp applyhelp = new ApplyHelp();

        #endregion

        #region 模板页面

        /// <summary>
        /// 模板页面
        /// </summary>
        /// <param name="type">信息类型</param>
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
            return View();
        }

        /// <summary>
        /// 出入园区申请记录
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <param name="lujin"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult InOrOutAll(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            ViewBag.CheckList1 = InOrOutJieKou("1", GlobalParameter.UserId, "0", 10, "");
            ViewBag.CheckList2 = InOrOutJieKou("2", GlobalParameter.UserId, "0", 10, "");

            ViewBag.CheckNumber1 = Math.Ceiling(Convert.ToDecimal(InOrOutJieKou("1", GlobalParameter.UserId, "0", 1000000000, "").Count()) / 10);
            ViewBag.CheckNumber2 = Math.Ceiling(Convert.ToDecimal(InOrOutJieKou("2", GlobalParameter.UserId, "0", 1000000000, "").Count()) / 10);

            return View();
        }


        #endregion

        #region  入驻企业

        /// <summary>
        /// 入驻企业视图
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <param name="lujin"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult SettledEnterprises(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            return View();
        }


        public JsonResult SettleEnterprisesAjax(string title, string content, string clientip)
        {
            try
            {
                content = HttpUtility.UrlDecode(content, System.Text.Encoding.GetEncoding("UTF-8"));
                SettleEnterprisesApi(title, content, clientip, GlobalParameter.UserId);
                return Json(new { msg = "OK" });
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
            }
        }


        /// <summary>
        /// 入驻
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="clientip"></param>
        /// <param name="useid"></param>
        /// <returns></returns>
        public int SettleEnterprisesApi(string title, string content, string clientip, string useid)
        {
            try
            {
                applyhelp.InsertApplyBill(useid, InsertBuniess("6", title, content, null, clientip, useid), "Buniess", DateTime.Now.ToString(), 2, Helps.SearchConfirmIdInXmlByType("入驻"));
                dal.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
                throw;
            }
        }


        #endregion

        #region 建设施工

        /// <summary>
        /// 建设施工视图
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <param name="lujin"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult Construction(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            return View();
        }

        public JsonResult ConstructionAjax(string title, string content, string clientip)
        {
            try
            {
                content = HttpUtility.UrlDecode(content, System.Text.Encoding.GetEncoding("UTF-8"));
                ConstructionApi(title, content, clientip, GlobalParameter.UserId);
                return Json(new { msg = "OK" });
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
            }
        }

        public int ConstructionApi(string title, string content, string clientip, string useid)
        {
            try
            {
                applyhelp.InsertApplyBill(useid, InsertBuniess("7", title, content, null, clientip, useid), "Buniess", DateTime.Now.ToString(), 2, Helps.SearchConfirmIdInXmlByType("施工"));
                dal.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
                throw;
            }
        }

        #endregion

        #region 报修

        /// <summary>
        /// 报修申请界面
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult Repair(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.lujin = lujin;
                ViewBag.TypeList = Helps.UniversalCodeList("RepairType");
                ViewBag.Title = Title;
            }
            return View();
        }


        /// <summary>
        /// 插入报修
        /// </summary>
        /// <param name="PeblishType"></param>
        /// <param name="PeblishTitle"></param>
        /// <param name="PublishContent"></param>
        /// <returns></returns>
        public JsonResult Insert(string RepairType, string RepairTitle, string RepairContent, string DateTimeNew, string DateTimeOld)
        {
            try
            {
                InsertBaoxiu(GlobalParameter.UserId, RepairType, RepairTitle, RepairContent);
                return Json(new { msg = "ok" });
            }
            catch (Exception)
            {
                return Json(new { msg = "no" });
                throw;
            }
        }


        /// <summary>
        /// 网上报修公告接口
        /// </summary>
        /// <param name="RepairType"></param>
        /// <param name="RepairTitle"></param>
        /// <param name="RepairContent"></param>
        /// <param name="DateTimeNew"></param>
        /// <param name="DateTimeOld"></param>
        /// <returns></returns>
        public int InsertBaoxiu(string useid, string RepairType, string RepairTitle, string RepairContent)
        {
            try
            {
                Models.tbRepair repair = new Models.tbRepair
                {
                    RepairID = (int.Parse((dal.tbRepair.Max(m => m.RepairID) == null ? "0" : dal.tbRepair.Max(m => m.RepairID))) + 1).ToString().PadLeft(12, '0'),
                    RepairType = int.Parse(RepairType),
                    RepairTitle = RepairTitle,
                    RepairContent = RepairContent,
                    StateCode = 1,
                };

                applyhelp.InsertApplyBill(useid, InsertRepair(repair), "Repair", DateTime.Now.ToString(), 2, Helps.SearchConfirmIdInXmlByType("报修"));
                dal.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        /// <summary>
        /// 插入repair表数据
        /// </summary>
        /// <param name="repair"></param>
        /// <returns></returns>
        public string InsertRepair(Models.tbRepair repair)
        {
            dal.tbRepair.Add(repair);
            dal.SaveChanges();
            return repair.RepairID;
        }


        #endregion

        #region  状态

        /// <summary>
        /// 业务申请状态查询
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult ApplyState(string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                ViewBag.lujin = lujin;
                ViewBag.Title = Title;
            }
            ViewBag.RepairNumber = Math.Ceiling(Convert.ToDecimal(SelectRepairAll("", "0", 1000000000, GlobalParameter.UserId).Count()) / 10);
            ViewBag.BuniessNumber4 = Math.Ceiling(Convert.ToDecimal(SelectBuniessAll("", "0", 1000000000, "4", GlobalParameter.UserId).Count()) / 10);
            ViewBag.BuniessNumber5 = Math.Ceiling(Convert.ToDecimal(SelectBuniessAll("", "0", 1000000000, "5", GlobalParameter.UserId).Count()) / 10);
            ViewBag.BuniessNumber6 = Math.Ceiling(Convert.ToDecimal(SelectBuniessAll("", "0", 1000000000, "6", GlobalParameter.UserId).Count()) / 10);
            ViewBag.BuniessNumber7 = Math.Ceiling(Convert.ToDecimal(SelectBuniessAll("", "0", 1000000000, "7", GlobalParameter.UserId).Count()) / 10);
            //ViewBag.RepairNumber = 24;
            //ViewBag.BuniessNumber5 = 24;

            ViewBag.ApplyList = SelectRepairAll("", "0", 10, GlobalParameter.UserId);
            ViewBag.BuniessList4 = SelectBuniessAll("", "0", 10, "4", GlobalParameter.UserId);
            ViewBag.BuniessList5 = SelectBuniessAll("", "0", 10, "5", GlobalParameter.UserId);
            ViewBag.BuniessList6 = SelectBuniessAll("", "0", 10, "6", GlobalParameter.UserId);
            ViewBag.BuniessList7 = SelectBuniessAll("", "0", 10, "7", GlobalParameter.UserId);
            return View();
        }




        #endregion

        #region 出入园申请

        #region 视图

        /// <summary>
        /// 正式出入园
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult CarsAndItems(string Type, string Title, string Typeid, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Typeid))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            return View();
        }

        #endregion

        #region 功能块

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="checkout"></param>
        /// <returns></returns>
        public void InsertCheck(string InOutFlag, string InOutType, string CheckTitle, string CheckContent, string people, string car, string product)
        {
            try
            {
                Models.tbCheckInOut CheckInOut = new Models.tbCheckInOut
                {
                    CheckID = (int.Parse((dal.tbCheckInOut.Max(m => m.CheckID) == null ? "0" : dal.tbCheckInOut.Max(m => m.CheckID))) + 1).ToString().PadLeft(12, '0'),
                    Version = (int.Parse((dal.tbCheckInOut.Max(m => m.CheckID) == null ? "0" : dal.tbCheckInOut.Max(m => m.CheckID))) + 1),
                    InOutFlag = int.Parse(InOutFlag),
                    InOutType = InOutType,
                    CheckTitle = CheckTitle,
                    CheckContent = CheckContent,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                string CheckInOutID = InsertCheckInOut(CheckInOut);
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// 插入CheckInOut表
        /// </summary>
        /// <param name="CheckInOut"></param>
        /// <returns>返回id</returns>
        public string InsertCheckInOut(Models.tbCheckInOut CheckInOut)
        {
            try
            {
                dal.tbCheckInOut.Add(CheckInOut);
                dal.SaveChanges();
                return CheckInOut.CheckID;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region 正式出入园 AJAX

        /// <summary>
        /// 正式出入园
        /// </summary>
        /// <param name="InOutType"></param>
        /// <param name="InOutTime"></param>
        /// <param name="CheckTitle"></param>
        /// <param name="CheckContent"></param>
        /// <param name="people"></param>
        /// <param name="car"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public JsonResult InsertMain(string InOutType, string InOutTime, string CheckTitle, string CheckContent, string people, string car, string product)
        {
            string msg = "NO";
            try
            {
                InsertMainJieKou(GlobalParameter.UserId, InOutType, CheckTitle, CheckContent, people, car, product);
                msg = "OK";
            }
            catch (Exception ee)
            {
            }
            return Json(new { msg = msg });
        }



        /// <summary>
        /// 给人车物添加出入园记录
        /// </summary>
        /// <param name="people"></param>
        /// <param name="car"></param>
        /// <param name="product"></param>
        /// <param name="InOutType"></param>
        public void InsertPCP(string people, string car, string product, string InOutType)
        {
            string[] peoples = people.Split(' ');
            string[] cars = car.Split(' ');
            string[] products = product.Split(' ');


        }


        /// <summary>
        /// 添加正式出入园区数据接口
        /// </summary>
        /// <param name="useid"></param>
        /// <param name="InOutType"></param>
        /// <param name="CheckTitle"></param>
        /// <param name="CheckContent"></param>
        /// <param name="People"></param>
        /// <param name="Car"></param>
        /// <param name="Product"></param>
        public int InsertMainJieKou(string useid, string InOutType, string CheckTitle, string CheckContent, string People, string Car, string Product)
        {
            int a = 0;
            try
            {
                string[] peoples = (!string.IsNullOrEmpty(People)) ? (People.Split(' ')) : (null);
                string[] cars = (!string.IsNullOrEmpty(Car)) ? (Car.Split(' ')) : (null);
                string[] products = (!string.IsNullOrEmpty(Product)) ? (Product.Split(' ')) : (null);

                Models.tbCheckInOut check = new Models.tbCheckInOut()
                {
                    CheckID = (int.Parse((dal.tbCheckInOut.Max(item => item.CheckID) == null ? "0" : dal.tbCheckInOut.Max(m => m.CheckID))) + 1).ToString().PadLeft(12, '0'),
                    Version = 1,
                    InOutFlag = int.Parse(InOutType),
                    InOutType = "1",
                    CheckTitle = CheckTitle,
                    CheckContent = CheckContent,
                    StateCode = 1
                };
                dal.tbCheckInOut.Add(check);
                applyhelp.InsertApplyBill(useid, check.CheckID, "CheckInOut", DateTime.Now.ToString(), 2, Helps.SearchConfirmIdInXmlByType("入园"));
                int i = 0;
                if (peoples != null)
                {
                    foreach (string str in peoples)
                    {
                        CheckInOutsInsert(check.CheckID, i, "1", str, 0, "");
                        i++;
                    }
                }
                if (cars != null)
                {
                    foreach (string str in cars)
                    {
                        CheckInOutsInsert(check.CheckID, i, "2", str, 0, "");
                        i++;
                    }
                }
                if (products != null)
                {
                    foreach (string str in products)
                    {
                        CheckInOutsInsert(check.CheckID, i, "3", str, 0, "");
                        i++;
                    }
                }
                dal.SaveChanges();
                a = 1;
            }
            catch (Exception)
            {
                throw;
            }
            return a;
        }


        #endregion

        #region 临时出入园申请

        #region 视图
        [UserChkAttribute.IsUser]
        public ActionResult Linshi(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            ViewBag.RfidCodes = RfidCodeSelectBy2();
            return View();
        }

        #endregion

        #region 临时入园AJAX

        /// <summary>
        /// 临时入园AJAX    --新建
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="Peopleid">人员 人员</param>
        /// <param name="Carid">车辆 车辆</param>
        /// <param name="Productid">物品 物品</param>
        /// <param name="pizhunren">批准人ID </param>
        /// <returns></returns>
        public JsonResult LinShiAjax(string RfidCodeid, string title, string content, string Peopleid, string Carid, string Productid, string pizhunren)
        {
            try
            {
                LinShiJieKou(GlobalParameter.UserId, title, content, Peopleid, Carid, Productid, pizhunren);

                return Json(new { msg = "OK" });
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
                throw;
            }
        }


        /// 添加临时出入园审核
        /// </summary>
        /// <param name="useid"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="Peopleid"></param>
        /// <param name="Carid"></param>
        /// <param name="Productid"></param>
        /// <param name="pizhunren"></param>
        /// <returns></returns>
        public int LinShiJieKou(string useid, string title, string content, string Peopleid, string Carid, string Productid, string pizhunren)
        {
            try
            {
                string[] Peoples = string.IsNullOrEmpty(Peopleid) ? null : (Peopleid.Split(' '));
                string[] Carids = string.IsNullOrEmpty(Carid) ? null : (Carid.Split(' '));
                string[] Productids = string.IsNullOrEmpty(Productid) ? null : (Productid.Split(' '));
                Models.tbCheckInOut check = new Models.tbCheckInOut
                {
                    CheckID = (int.Parse((dal.tbCheckInOut.Max(item => item.CheckID) == null ? "0" : dal.tbCheckInOut.Max(m => m.CheckID))) + 1).ToString().PadLeft(12, '0'),
                    Version = 1,
                    InOutFlag = 1,
                    InOutType = "2",
                    CheckTitle = title,
                    CheckContent = content,
                    StateCode = 1
                };
                dal.tbCheckInOut.Add(check);
                applyhelp.InsertApplyBill(useid, check.CheckID, "CheckInOut", DateTime.Now.ToString(), 2, Helps.SearchConfirmIdInXmlByType("入园"));

                int i = 0;
                foreach (string str in Peoples)
                {
                    Models.tbTemp temps = new Models.tbTemp
                    {
                        TempID = (int.Parse((dal.tbTemp.Max(item => item.TempID) == null ? "0" : dal.tbTemp.Max(item => item.TempID))) + 1).ToString().PadLeft(12, '0'),
                        Version = 0,
                        UseType = "1",
                        TempName = check.CheckTitle,
                        TempDesc = str,
                        StateCode = 1
                    };
                    dal.tbTemp.Add(temps);
                    dal.SaveChanges();
                    CheckInOutsInsert(check.CheckID, i, temps.UseType, temps.TempID, temps.Version, temps.TempDesc);
                    i++;
                }
                if (Carids != null)
                {
                    foreach (string str in Carids)
                    {
                        Models.tbTemp temps = new Models.tbTemp
                        {
                            TempID = (int.Parse((dal.tbTemp.Max(item => item.TempID) == null ? "0" : dal.tbTemp.Max(item => item.TempID))) + 1).ToString().PadLeft(12, '0'),
                            Version = 0,
                            UseType = "2",
                            TempName = check.CheckTitle,
                            TempDesc = str,
                            StateCode = 1
                        };
                        dal.tbTemp.Add(temps);
                        dal.SaveChanges();
                        CheckInOutsInsert(check.CheckID, i, temps.UseType, temps.TempID, temps.Version, temps.TempDesc);

                        i++;
                    }
                }
                if (Productids != null)
                {
                    foreach (string str in Productids)
                    {
                        Models.tbTemp temps = new Models.tbTemp
                        {
                            TempID = (int.Parse((dal.tbTemp.Max(item => item.TempID) == null ? "0" : dal.tbTemp.Max(item => item.TempID))) + 1).ToString().PadLeft(12, '0'),
                            Version = 0,
                            UseType = "3",
                            TempName = check.CheckTitle,
                            TempDesc = str,
                            StateCode = 1
                        };
                        dal.tbTemp.Add(temps);
                        dal.SaveChanges();
                        CheckInOutsInsert(check.CheckID, i, temps.UseType, temps.TempID, temps.Version, temps.TempDesc);
                        i++;
                    }
                }
                dal.SaveChanges();

                return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }


        /// <summary>
        /// 添加出入园区申请详细表
        /// </summary>
        /// <param name="checkID"></param>
        /// <param name="SortNo"></param>
        /// <param name="UseType"></param>
        /// <param name="ObjectID"></param>
        /// <param name="ObjectVersion"></param>
        /// <param name="Memo"></param>
        public void CheckInOutsInsert(string checkID, int SortNo, string UseType, string ObjectID, int ObjectVersion, string Memo)
        {
            Models.tbCheckInOuts s = new Models.tbCheckInOuts
            {
                CheckID = checkID,
                Version = 0,
                SortNo = SortNo,
                UseType = UseType,
                ObjectId = ObjectID,
                ObjectVersion = ObjectVersion,
                Memo = Memo
            };
            dal.tbCheckInOuts.Add(s);
        }

        /// <summary>
        /// 查找下级批准人
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></ returns >
        public JsonResult PiZhunRensbumen(string id)
        {

            return Json(PiZhunrenjiekou(id));
        }



        public PiZhun PiZhunrenjiekou(string id)
        {
            try
            {
                int i = int.Parse(id);
                PiZhun p = new PiZhun();
                List<BuMen> b = new List<BuMen>();
                List<RenYuan> r = new List<RenYuan>();

                var a = dal.mtOwner.Where(item => item.OwnerParentId == i).ToList();
                foreach (var ii in a)
                {
                    b.Add(new BuMen
                    {
                        BuMenId = ii.OwnerId,
                        BuMenName = ii.OwnerName
                    });
                }

                var c = dal.mtUser.Where(item => item.OwnerId == i).ToList();
                foreach (var ii in c)
                {
                    r.Add(new RenYuan
                    {
                        RenYuanId = ii.UserId,
                        RenYuanName = ii.UserName
                    });
                }

                p.BuMens = b;
                p.RenYuans = r;
                return p;
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// 查找人
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public JsonResult PiZhunRenStr(string owner, string user)
        {
            int? ownerid = 0;
            //int a = int.Parse(owner);
            if (!string.IsNullOrEmpty(owner))//部门
            {
                ownerid = dal.mtOwner.First(item => item.OwnerName.Contains(owner)).OwnerParentId;
            }
            if (!string.IsNullOrEmpty(user))//人员
            {
                ownerid = dal.mtUser.First(item => item.UserName.Contains(user)).OwnerId;
            }
            return Json(new { ownerid = ownerid });
        }



        #endregion

        #endregion



        #region 出入园列表显示


        /// <summary>
        /// 出入园列表
        /// </summary>
        /// <param name="InOrOutType"></param>
        /// <returns></returns>
        public JsonResult InOrOutAjax(string InOrOutType, string str, string pageindex, int number)
        {
            try
            {
                string UserId = GlobalParameter.UserId;
                return Json(InOrOutJieKou(InOrOutType, UserId, pageindex, number, str));
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
                throw;
            }
        }


        /// <summary>
        /// 提取出入园列表数据--本人
        /// </summary>
        /// <param name="InOrOutType">正式/临时</param>
        /// <param name="useid"></param>
        /// <param name="pageindex"></param>
        /// <param name="number"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<CheckInOutList> InOrOutJieKou(string InOrOutType, string useid, string pageindex, int number, string str)
        {
            var aa = (from a in dal.tbApplyBill
                      from c in dal.tbCheckInOut
                      from u in dal.mtUniversalCode
                      where c.InOutType == InOrOutType &&
                      a.ObjectID == c.CheckID &&
                      a.ApplyType == "CheckInOut" &&
                      a.Updater == useid &&
                      a.StateType == u.CodeID &&
                      u.UniversalType == "StateType"
                      select new CheckInOutList
                      {
                          CheckID = c.CheckID,
                          Title = c.CheckTitle,
                          Time = c.CreateTime,
                          StatusName = u.CodeName
                      }).ToList();
            aa = aa.OrderByDescending(item => item.Time).ToList();

            string[] strs = str.Split(' ');
            aa = aa.Where(item =>
            {
                var temp = item.Title + " " + item.StatusName + " " + item.Time.ToString();
                foreach (var s in strs)
                {
                    if (temp.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();

            return Funs.fenye<CheckInOutList>(aa, int.Parse(pageindex), number);


        }





        /// <summary>
        /// 出入园数据
        /// </summary>
        /// <param name="checkid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult InOrOutXSAjax(string checkid, string type)
        {
            try
            {

                switch (type)
                {
                    case "zhengshi": return Json(InOrOutZsJieKou(checkid));
                    case "lingshi": return Json(InOrOutXSJieKou(checkid));
                    default: return Json(new { msg = "NO" });
                }
            }
            catch (Exception e)
            {
                return Json(new { msg = "NO" });
                throw;
            }
        }


        /// <summary>
        /// 提取临时出入园列表单个数据
        /// </summary>
        /// <param name="checkid"></param>
        /// <returns></returns>
        public CheckInOutXS InOrOutXSJieKou(string checkid)
        {
            try
            {
                Models.tbCheckInOut aa = dal.tbCheckInOut.First(item => item.CheckID == checkid);

                CheckInOutXS cc = new CheckInOutXS();

                cc.CheckID = aa.CheckID;
                cc.Title = aa.CheckTitle;
                cc.Content = aa.CheckContent;
                cc.Time = aa.CreateTime;
                cc.Temps = (from c in dal.tbCheckInOuts
                            from t in dal.tbTemp
                            where c.CheckID == checkid &&
                            c.ObjectId == t.TempID
                            select t).ToList();
                return cc;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        ///正式出入园区单个信息
        /// </summary>
        /// <param name="checkid"></param>
        /// <returns></returns>
        public CheckInOutZs InOrOutZsJieKou(string checkid)
        {
            try
            {
                Models.tbCheckInOut aa = dal.tbCheckInOut.FirstOrDefault(item => item.CheckID == checkid);
                CheckInOutZs cc = new CheckInOutZs();

                cc.CheckID = aa.CheckID;
                cc.Title = aa.CheckTitle;
                cc.Content = aa.CheckContent;
                cc.Time = aa.CreateTime;
                cc.Peoples = (from c in dal.tbCheckInOuts
                              from p in dal.tbPeople
                              where c.CheckID == aa.CheckID &&
                              c.UseType == "1" &&
                              c.ObjectId == p.PeopleID &&
                              p.Version == (dal.tbPeople.Where(item => item.PeopleID == p.PeopleID).Max(item => item.Version))
                              select p).ToList();
                cc.Cars = (from c in dal.tbCheckInOuts
                           from p in dal.tbCar
                           where c.CheckID == aa.CheckID &&
                           c.UseType == "2" &&
                           c.ObjectId == p.CarID &&
                           p.Version == (dal.tbCar.Where(item => item.CarID == p.CarID).Max(item => item.Version))
                           select p).ToList();
                cc.Products = (from c in dal.tbCheckInOuts
                               from p in dal.tbProduct
                               where c.CheckID == aa.CheckID &&
                               c.UseType == "2" &&
                               c.ObjectId == p.ProductID &&
                               p.Version == (dal.tbProduct.Where(item => item.ProductID == p.ProductID).Max(item => item.Version))
                               select p).ToList();

                return cc;
            }
            catch (Exception)
            {
                throw;
            }
        }




        #endregion


        #endregion

        #region 证卡挂失申请

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult guashi(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            return View();
        }

        /// <summary>
        /// 挂失AJAX
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public JsonResult guashiajax(string content, string clientip)
        {
            try
            {
                guashijiekou(GlobalParameter.UserId, content, clientip);
                return Json(new { msg = "OK" });
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
            }
        }

        /// <summary>
        /// 挂失提交
        /// </summary>
        /// <param name="useid">用户ID</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public int guashijiekou(string useid, string content, string clientip)
        {
            try
            {
                string title = "挂失申请";
                applyhelp.InsertApplyBill(useid, InsertBuniess("5", title, content, null, clientip, useid), "Buniess", DateTime.Now.ToString(), 2, Helps.SearchConfirmIdInXmlByType("挂失"));
                dal.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        #region  人员制卡申请


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult renyuan(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            return View();
        }


        /// <summary>
        /// 人员制卡ajax
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public JsonResult renyuanshenqin(string title, string content, string number, string clientip)
        {
            try
            {
                RenYuanShen(GlobalParameter.UserId, title, content, number, clientip);
                return Json(new { msg = "OK" });
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
            }
        }

        /// <summary>
        /// 提交人员制卡
        /// </summary>
        /// <param name="useid">用户ID</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="number">数量</param>
        /// <returns></returns>
        public int RenYuanShen(string useid, string title, string content, string number, string ClientIp)
        {
            try
            {
                applyhelp.InsertApplyBill(useid, InsertBuniess("4", title, content, number, ClientIp, useid), "Buniess", DateTime.Now.ToString(), 2, Helps.SearchConfirmIdInXmlByType("制卡"));
                dal.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }

        }


        /// <summary>
        /// 插入buniess表 返回id
        /// </summary>
        /// <param name="BuniessType"></param>
        /// <param name="content"></param>
        /// <param name="clientIP"></param>
        /// <param name="useid"></param>
        /// <returns></returns>
        public string InsertBuniess(string BuniessType, string title, string content, string number, string clientIP, string useid)
        {
            Models.tbBuniess b = new Models.tbBuniess
            {
                BuniessID = (int.Parse((dal.tbBuniess.Max(item => item.BuniessID) == null ? "0" : dal.tbBuniess.Max(m => m.BuniessID))) + 1).ToString().PadLeft(12, '0'),
                BuniessType = BuniessType,
                BuniessContent = title + "|+|" + content + ((string.IsNullOrEmpty(number)) ? null : ("|+|" + number)), //如果是人员制卡 有数量 
                IsCompleted = 1,
                ClientIP = clientIP,
                Updater = useid
            };

            return dal.tbBuniess.Add(b).BuniessID;
        }

        #endregion

        #region  根据userid 按照业主的关系进行人车物的选择

        /// <summary>
        /// 主入口
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        /// 
        public JsonResult PeopleCarItemMain(string typeid, string str)
        {

            switch (typeid)
            {
                case "1": return Json(SelectPeople(str, GlobalParameter.UserId));
                case "2": return Json(SelectCars(str, GlobalParameter.UserId));
                case "3": return Json(SelectProduct(str, GlobalParameter.UserId));
                default: return Json(new { msg = "NO" });
            }
        }

        /// <summary>
        /// 车
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public JsonResult Car(string typeid, string str)
        {
            return Json(SelectCars(str, GlobalParameter.UserId));
        }


        /// <summary>
        /// people表列表
        /// </summary>
        /// <param name="str">查询关键字</param>
        /// <returns></returns>
        public List<Models.tbPeople> SelectPeople(string str, string useid)
        {
            string[] strs = str.Split(' ');
            var Peoplelist = (from u in dal.mtUser
                              from r in dal.tbOwnerRfid
                              from o in dal.tbPeople
                              where u.UserId == useid &&
                              u.OwnerId == r.OwnerId &&
                              r.UseType == 1 &&
                              r.ObjectID == o.PeopleID
                              select o).ToList();
            Peoplelist = (from p in Peoplelist
                          group p by p.PeopleID into grp
                          let maxV = grp.Max(a => a.Version)
                          from row in grp
                          where row.Version == maxV
                          select row).ToList();
            Peoplelist = Peoplelist.Where(item =>
            {
                string tmp = item.PeopleName + ' ' +
                item.Sexy + ' ' +
                item.PhoneNum + ' ' +
                item.Position + ' ' +
                item.IdentityCardID + ' ' +
                item.Age.ToString() + ' ' +
                item.Native + ' ' +
                item.CurrProject + ' ' +
                item.Address + ' ' +
                item.Birthday.ToString() + ' ' +
                item.JobPartTime + ' ' +
                item.SkillItem;
                foreach (var s in strs)
                {
                    if (tmp.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();
            return Peoplelist;
        }

        /// <summary>
        /// Car表数据列表
        /// </summary>
        /// <param name="str">查询关键字</param>
        /// <returns></returns>
        public List<Models.tbCar> SelectCars(string str, string useid)
        {
            string[] strs = str.Split(' ');

            var CarList = (from u in dal.mtUser
                           from r in dal.tbOwnerRfid
                           from o in dal.tbCar
                           where u.UserId == useid &&
                           u.OwnerId == r.OwnerId &&
                           r.UseType == 2 &&
                           r.ObjectID == o.CarID
                           select o).ToList();
            CarList = (from c in CarList
                       group c by c.CarID into grp
                       let MaxV = grp.Max(x => x.Version)
                       from row in grp
                       where row.Version == MaxV
                       select row).ToList();

            CarList = CarList.Where(item =>
            {
                var tmp = item.CarLicence + ' ' +
                item.CarType + ' ' +
                item.CarVehicleNo + ' ' +
                item.MaxWeight.ToString() + ' ' +
                item.MinWeight.ToString() + ' ' +
                item.Drivers + ' ' +
                item.CurrProject + ' ' +
                item.ParkSpace + ' ' +
                item.JobPartTime + ' ' +
                item.SkillItem;
                foreach (var s in strs)
                {
                    if (tmp.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();
            return CarList;
        }

        /// <summary>
        /// Product表数据列表
        /// </summary>
        /// <param name="str">查询关键字</param>
        /// <returns></returns>
        public List<Models.tbProduct> SelectProduct(string str, string useid)
        {
            string[] strs = str.Split(' ');

            var ProductList = (from u in dal.mtUser
                               from r in dal.tbOwnerRfid
                               from o in dal.tbProduct
                               where u.UserId == useid &&
                               u.OwnerId == r.OwnerId &&
                               r.UseType == 3 &&
                               r.ObjectID == o.ProductID
                               select o).ToList();
            ProductList = (from p in ProductList
                           group p by p.ProductID into grp
                           let MaxV = grp.Max(x => x.Version)
                           from row in grp
                           where row.Version == MaxV
                           select row).ToList();


            ProductList = ProductList.Where(item =>
            {
                var tmp = item.ProductName + ' ' +
                item.ProductSpec + ' ' +
                item.ProductQuantity.ToString() + ' ' +
                item.ProductUnit + ' ' +
                item.CurrProject + ' ' +
                item.Space + ' ' +
                item.JobPartTime + ' ' +
                item.SkillItem;
                foreach (var s in strs)
                {
                    if (tmp.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();
            return ProductList;
        }


        #endregion

        #region JsonResult


        /// <summary>
        /// 业务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult SelectBuniessByIdAjax(string id)
        {
            try
            {
                return Json(SelectBuniessById(id));
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
                throw;
            }
        }




        /// <summary>
        /// 网上报修
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult SelectRepairByIdAjax(string id)
        {
            try
            {
                return Json(SelectRepairById(id));
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
                throw;
            }
        }

        /// <summary>
        /// 网上报修All
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pageindex"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public JsonResult SelectRepairAllAjax(string str, string pageindex, int number)
        {
            try
            {
                return Json(SelectRepairAll(str, pageindex, number, GlobalParameter.UserId));
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
                throw;
            }
        }

        /// <summary>
        /// 业务报修All
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pageindex"></param>
        /// <param name="number"></param>
        /// <param name="buniesstype">业务类别</param>
        /// <returns></returns>

        public JsonResult SelectBuniessAllAjax(string str, string pageindex, int number, string buniesstype)
        {
            return Json(SelectBuniessAll(str, pageindex, number, buniesstype, GlobalParameter.UserId));
        }


        /// <summary>
        /// 提取当前申请单的审核流程状态、审核意见
        /// </summary>
        /// <param name="applyid"></param>
        /// <returns></returns>
        public JsonResult SelectConfirmerMenoFlowByApplyid(string applyid)
        {
            try
            {
                int aid;
                int.TryParse(applyid, out aid);
                return Json(Helps.SearchConfirmeFlowByApplyid(aid));
            }
            catch (Exception e)
            {
                return Json(new { Msg = "NO" });
            }

        }

        #endregion

        #region WebService

        /// <summary>
        /// 网上报修接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.tbRepair SelectRepairById(string id)
        {
            try
            {
                return dal.tbRepair.First(item => item.RepairID == id);
            }
            catch (Exception)
            {
                return null;
            }
        }



        /// <summary>
        /// 业务接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.tbBuniess SelectBuniessById(string id)
        {
            try
            {
                return dal.tbBuniess.First(item => item.BuniessID == id);
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 业务表所有分页
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pageindex"></param>
        /// <param name="number"></param>
        /// <param name="buniesstype"></param>
        /// <param name="useid"></param>
        /// <returns></returns>
        public List<BuniessAll> SelectBuniessAll(string str, string pageindex, int number, string buniesstype, string useid)
        {
            var bb = (from b in dal.tbBuniess
                      from u in dal.mtUniversalCode
                      from a in dal.tbApplyBill
                      from c in dal.tbConfirmState
                      from mu in dal.mtUser
                      where a.ObjectID == b.BuniessID
                      && a.ApplyType == "Buniess"
                      && b.Updater == useid
                      && b.BuniessType == buniesstype
                      && u.UniversalType == "StateType"
                      && a.StateType == u.CodeID
                      && a.ApplyID == c.ApplyID
                      && c.ConfirmerID == mu.UserId
                      select new BuniessAll
                      {
                          BuniessID = b.BuniessID + "|" + SqlFunctions.StringConvert((double)c.ApplyID),
                          BuniessContent = b.BuniessContent,
                          BuniessTime = b.CreateTime,
                          StateName = u.CodeName,
                          ConfimeName = mu.UserName
                      }).ToList();
            bb = bb.OrderByDescending(item => item.BuniessTime).ToList();
            string[] strs = str.Split(' ');
            bb = bb.Where(item =>
              {
                  var tmp = item.BuniessTime + " " + item.BuniessContent + " " + item.ConfimeName + " " + item.StateName;
                  foreach (var s in strs)
                  {
                      if (tmp.Contains(s))
                      {
                          return true;
                      }
                  }
                  return false;
              }).ToList();

            return Funs.fenye(bb, int.Parse(pageindex), number);

        }


        /// <summary>
        /// 网上报修所有分页
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<ApplyStateListJian> SelectRepairAll(string str, string pageindex, int number, string useid)
        {
            var bb = (from r in dal.tbRepair
                      from u in dal.mtUniversalCode
                      from u1 in dal.mtUniversalCode
                      from b in dal.tbApplyBill
                      where b.StateType == u.CodeID
                      && u.UniversalType == "StateType"
                      && u1.UniversalType == "RepairType"
                      && b.ApplyType == "Repair"
                      && u1.CodeID == r.RepairType
                      && b.ObjectID == r.RepairID
                      && b.Updater == useid
                      select new ApplyStateListJian
                      {
                          RepairID = r.RepairID,
                          RepairTitle = r.RepairTitle,
                          Time = r.CreateTime,
                          StateName = u.CodeName,
                          TypeName = u1.CodeName,
                          RepairContent = r.RepairContent
                      }).ToList();

            var aa = bb.OrderByDescending(a => a.Time).ToList();
            string[] strs = str.Split(' ');
            aa = aa.Where(item =>
            {
                var temp = item.RepairTitle + " " + item.RepairContent + " " + item.RepairContent + " " + item.StateName + " " + item.TypeName;
                foreach (var s in strs)
                {
                    if (temp.Contains(s))
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();
            return Funs.fenye<ApplyStateListJian>(aa, int.Parse(pageindex), number);
        }


        public JsonResult RfidCodeJson()
        {
            return Json(RfidCodeSelectBy2());
        }

        /// <summary>
        /// 提取空闲中的临时标签
        /// </summary>
        /// <returns></returns>
        public List<Models.mtRfidCode> RfidCodeSelectBy2()
        {
            var a = (from r in dal.mtRfidCode
                     from o in dal.tbOwnerRfid
                     from t in dal.tbTemp
                     where o.RfidAutoId == r.AutoId
                     && o.UseType == 4
                     && t.Version == (dal.tbTemp.Where(item => item.TempID == o.ObjectID).Max(item => item.Version))
                     && o.ObjectID == t.TempID
                     select r).ToList();
            return a;
        }

        #endregion



    }

    #region 模型
    /// <summary>
    /// 状态用临时模型
    /// </summary>
    public class ApplyStateListJian
    {
        public string RepairID { get; set; }
        public string RepairTitle { get; set; }
        public DateTime? Time { get; set; }
        public string StateName { get; set; }
        public string TypeName { get; set; }
        public string RepairContent { get; set; }
    }


    public class PiZhun
    {
        public List<BuMen> BuMens { get; set; }
        public List<RenYuan> RenYuans { get; set; }
    }

    public class BuMen
    {
        public int BuMenId { get; set; }
        public string BuMenName { get; set; }

    }

    public class RenYuan
    {
        public string RenYuanId { get; set; }
        public string RenYuanName { get; set; }

    }


    public class BuniessAll
    {
        public string BuniessID { get; set; }
        public string BuniessContent { get; set; }
        public DateTime? BuniessTime { get; set; }
        public string StateName { get; set; }
        public string ConfimeName { get; set; }
    }



    public class CheckInOutList
    {
        public string CheckID { get; set; }
        public string Title { get; set; }
        public DateTime? Time { get; set; }
        public string StatusName { get; set; }
    }


    public class CheckInOutXS
    {
        public string CheckID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? Time { get; set; }
        public List<Models.tbTemp> Temps { get; set; }
    }

    public class CheckInOutZs
    {
        public string CheckID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? Time { get; set; }
        public List<Models.tbPeople> Peoples { get; set; }
        public List<Models.tbCar> Cars { get; set; }
        public List<Models.tbProduct> Products { get; set; }
    }

    #endregion

}
