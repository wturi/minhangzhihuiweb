using MinHangWisdomParkWeb.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MinHangWisdomParkWeb.Controllers
{
    public class MainController : BaseController
    {
        #region 参数

        ApplyHelp applyhelp = new ApplyHelp();

        #endregion


        #region 页面

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 后台欢迎页面
        /// </summary>
        /// <returns></returns>
        public ActionResult huanyingshiyong()
        {

            return View();
        }


        public ActionResult Main1()
        {
            return View();
        }

        /// <summary>
        /// 官网门户
        /// </summary>
        /// <returns></returns>
        public ActionResult Main()
        {

            ViewBag.NewsList = TypeContentList("1", 1, 10);
            ViewBag.Title = GlobalParameter.ZongName;
            ViewBag.xinwen = (from a in dal.tbApplyBill
                              from pe in dal.tbPeblish
                              where pe.PeblishType == "1" &&
                              pe.PeblishID == a.ObjectID &&
                              a.StateType == 2
                              select pe).OrderByDescending(item => item.CreateTime).ToList();
            ViewBag.gonggao = (from a in dal.tbApplyBill
                               from pe in dal.tbPeblish
                               where pe.PeblishType == "2" &&
                               pe.PeblishID == a.ObjectID &&
                               a.StateType == 2
                               select pe).OrderByDescending(item => item.CreateTime).ToList();
            List<guanggao> Advert = new List<guanggao>();

            foreach (var i in TypeContentList("3", 1, 5) as List<Models.tbPeblish>)
            {
                Advert.Add(new guanggao
                {
                    httpurl = i.PeblishContent,
                    imgurl = FilesImgUrl(i.FileIDs)
                });
            }
            ViewBag.guanggao = Advert;
            ViewBag.zhengce = (from a in dal.tbApplyBill
                               from pe in dal.tbPeblish
                               where pe.PeblishType == "4" &&
                               pe.PeblishID == a.ObjectID &&
                               a.StateType == 2
                               select pe).OrderByDescending(item => item.CreateTime).ToList();
            ViewBag.qiye = (from a in dal.tbApplyBill
                            from pe in dal.tbPeblish
                            where pe.PeblishType == "5" &&
                            pe.PeblishID == a.ObjectID &&
                            a.StateType == 2
                            select pe).OrderByDescending(item => item.CreateTime).ToList();
            return View();
        }

        /// <summary>
        /// 类别页面模板
        /// </summary>
        /// <returns></returns>
        public ActionResult TypeContent(string TypeName)
        {
            if (!string.IsNullOrEmpty(TypeName))
            {
                string typeid = ((int)Enum.Parse(typeof(Helps.Peblish), TypeName)).ToString();
                ViewBag.Count = (from a in dal.tbApplyBill
                                 from pe in dal.tbPeblish
                                 where pe.PeblishType == typeid &&
                                 pe.PeblishID == a.ObjectID &&
                                 a.StateType == 2
                                 select pe).Count();
                ViewBag.Title = TypeName;
            }
            return View();
        }

        /// <summary>
        ///详细内容页面模板
        /// </summary>
        /// <returns></returns>
        public ActionResult TextContent(string TypeName, string PeblishID)
        {
            if (!string.IsNullOrEmpty(TypeName) && !string.IsNullOrEmpty(PeblishID))
            {

                var p = TextAjaxJieKou(TypeName, PeblishID);

                if (p.files != null)
                {

                    ViewBag.Files = p.files;
                }

                ViewBag.Peblish = p.peblish;
                ViewBag.Title = p.peblish.PeblishTitle;
                ViewBag.TypeName = TypeName;

            }
            return View();
        }


        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public JsonResult TypeLie(string typename, string pageindex)
        {
            try
            {
                int typeid = (int)Enum.Parse(typeof(Helps.Peblish), typename);


                return Json(TypeContentList(typeid.ToString(), int.Parse(pageindex), 10));
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
            }
        }

        /// <summary>
        /// 提取单个数据
        /// </summary>
        /// <param name="typename"></param>
        /// <param name="peblishid"></param>
        /// <returns></returns>
        public JsonResult TextAjax(string typename, string peblishid)
        {
            try
            {
                return Json(TextAjaxJieKou(typename, peblishid));
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
            }
        }

        /// <summary>
        /// 提取单个数据接口
        /// </summary>
        /// <param name="typename"></param>
        /// <param name="peblishid"></param>
        /// <returns></returns>
        public PeblishFiles TextAjaxJieKou(string typename, string peblishid)
        {
            try
            {
                peblishid = peblishid.Trim();
                PeblishFiles p = new PeblishFiles();
                p.peblish = dal.tbPeblish.First(item => item.PeblishID == peblishid);
                if (p.peblish.FileIDs != null)
                {
                    string[] ids = p.peblish.FileIDs.Split(',');
                    List<Models.tbFiles> f = new List<Models.tbFiles>();

                    foreach (string i in ids)
                    {
                        int a = int.Parse(i);
                        f.Add(dal.tbFiles.First(item => item.FileID == a));
                    }
                    p.files = f;
                }
                return p;
            }
            catch (Exception r)
            {
                return null;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        public ActionResult login(string name, string pwd, string type)
        {
            //if (type == "入驻企业申请")
            //{
            //    LoginSelect("00000000", "1234");
            //    JudgeJurApi(type);
            //}

            //Helps.SearchConfirmeFlowByApplyid(220);
            if (!(string.IsNullOrEmpty(name) && string.IsNullOrEmpty(pwd)))
            {
                if (!(string.IsNullOrEmpty(type)))
                {
                    if (LoginSelect(name, pwd) == 1)
                    {
                        if (!(JudgeJurApi(type)))
                        {
                            ViewBag.Msg = "权限不足!";
                        }
                    }
                    else
                    {
                        ViewBag.Msg = "账户信息输入错误,登录失败！";
                    }
                }
                else if (LoginSelect(name, pwd) == 1)
                {
                    Response.Redirect("huanyingshiyong");
                }
                else
                {
                    ViewBag.Msg = "账户信息输入错误,登录失败！";
                }
            }

            if (!(string.IsNullOrEmpty(type)))
            {
                ViewBag.Type = type;
            }
            return View();
        }



        /// <summary>
        /// 判断用户是否有指定功能权限
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="type"></param>
        public bool JudgeJurApi(string type)
        {
            try
            {
                int FuncNo = Helps.SearchFuncNoInXmlByType(type);
                var Ap = dal.tbActorPower.First(item => item.ActorId == GlobalParameter.Actorid && item.FuncNo == FuncNo);
                if (Ap != null)
                {
                    Models.Functions function = dal.Functions.First(item => item.FunctionID == FuncNo);
                    string url = string.Format("/{0}&Type={1}&Title={1}&lujin={1}", function.Type, function.FunctionName);
                    Response.Redirect(url);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }




        /// <summary>
        /// 修改用户资料Ajax
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <param name="Sexy"></param>
        /// <param name="Phone"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        public JsonResult MyPeopleUpdateAjax(string Id, string Name, string Sexy, string Phone, string Email)
        {
            try
            {
                string msg = MyPeopleUpdate(Id, Name, Sexy, Phone, Email, GlobalParameter.UserId)
                    ? "OK"
                    : "NO";
                return Json(new { msg = msg });
            }
            catch (Exception)
            {
                return Json(new { msg = "NO" });
            }
        }

        public JsonResult MyPwdUpdateAjax(string oldpwd, string newpwd)
        {
            return Json(new { msg = MyPwdUpdate(oldpwd, newpwd, GlobalParameter.UserId) });
        }
        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <param name="Sexy"></param>
        /// <param name="Phone"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        public bool MyPeopleUpdate(string Id, string Name, string Sexy, string Phone, string Email, string userid)
        {
            try
            {
                var peopleold = dal.tbPeople.Where(item => item.PeopleID == Id).OrderByDescending(item => item.Version).FirstOrDefault();
                var myuser = dal.mtUser.FirstOrDefault(item => item.UserId == userid);
                myuser.UserName = Name;

                Models.tbPeople peoplenew = new Models.tbPeople
                {
                    PeopleID = Id,
                    Version = peopleold.Version + 1,
                    PeopleName = Name,
                    Sexy = Sexy,
                    Position = peopleold.Position,
                    PhoneNum = Phone,
                    IdentityCardID = peopleold.IdentityCardID,
                    ExpDate = peopleold.ExpDate,
                    StateCode = peopleold.StateCode,
                    Age = peopleold.Age,
                    Native = peopleold.Native,
                    CurrProject = peopleold.CurrProject,
                    Address = peopleold.Address,
                    Birthday = peopleold.Birthday,
                    JobPartTime = peopleold.JobPartTime,
                    SkillItem = peopleold.SkillItem,
                    Email = Email,
                    QQ = peopleold.QQ,
                    WeiXin = peopleold.WeiXin,
                    CompanyName = peopleold.CompanyName,
                    HeadPhotoPic = peopleold.HeadPhotoPic,
                    FingerprintLeftPic = peopleold.FingerprintLeftPic,
                    FingerprintRightPic = peopleold.FingerprintRightPic
                };

                dal.tbPeople.Add(peoplenew);

                dal.SaveChanges();

                return true;

            }
            catch (Exception ee)
            {
                return false;
            }
        }


        public string MyPwdUpdate(string oldpwd, string newpwd, string useid)
        {
            oldpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(oldpwd, "MD5");
            newpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(newpwd, "MD5");
            var u = dal.mtUser.First(item => item.UserId == useid && item.IsDel == false);
            if (u.UserPWD != oldpwd)
            {
                return "原始密码输入错误！";
            }
            else
            {
                try
                {
                    u.UserPWD = newpwd;
                    dal.SaveChanges(); 
                    return "密码修改成功！";
                }
                catch (Exception)
                {
                    return "密码修改失败！";
                }
            }
        }

        /// <summary>
        /// 论坛管理
        /// </summary>
        /// <returns></returns>
        public ActionResult LunTan()
        {
            return View();
        }

        /// <summary>
        /// 临时入口
        /// </summary>
        /// <returns></returns>
        public ActionResult RuKou()
        {
            return View();
        }

        /// <summary>
        /// 一体机公告页面
        /// </summary>
        /// <returns></returns>
        public ActionResult YiTiJiGongGao()
        {
            return View();
        }

        #endregion

        #region  功能

        /// <summary>
        /// 登录功能
        /// </summary>
        /// <param name="name">UserId/手机号/邮箱地址</param>
        /// <param name="pwd">密码</param>
        /// <returns>0：失败，1：成功</returns>
        public int LoginSelect(string name, string pwd)
        {
            pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");

            int a = 0;


            try
            {
                if (Regex.IsMatch(name, "1[0-9]{10}"))//验证手机号
                {
                    var user = (from o in dal.tbOwnerRfid
                                from o1 in dal.tbOwnerRfid
                                from u in dal.mtUser
                                from p in dal.tbPeople
                                where o.ObjectID == u.UserId &&
                                o1.ObjectID == p.PeopleID &&
                                u.IsDel == false &&
                                p.PhoneNum == name &&
                                u.UserPWD == pwd &&
                                p.Version == (dal.tbPeople.Where(item => item.PeopleID == o1.ObjectID).Max(item => item.Version))
                                select u).FirstOrDefault();
                    if (user != null)
                    {
                        GlobalParameter.UserId = user.UserId;
                        GlobalParameter.UserName = user.UserName;
                        GlobalParameter.Actorid = user.ActorId;
                        a = 1;
                    }

                }
                else if (Regex.IsMatch(name, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))//验证邮箱
                {

                    var user = (from o in dal.tbOwnerRfid
                                from o1 in dal.tbOwnerRfid
                                from u in dal.mtUser
                                from p in dal.tbPeople
                                where o.ObjectID == u.UserId &&
                                o1.ObjectID == p.PeopleID &&
                                u.IsDel == false &&
                                p.Email == name &&
                                u.UserPWD == pwd &&
                                p.Version == (dal.tbPeople.Where(item => item.PeopleID == o1.ObjectID).Max(item => item.Version))
                                select u).FirstOrDefault();
                    if (user != null)
                    {
                        GlobalParameter.UserId = user.UserId;
                        GlobalParameter.UserName = user.UserName;
                        GlobalParameter.Actorid = user.ActorId;
                        a = 1;
                    }
                }
                else //UserId
                {
                    var use = dal.mtUser.First(m => m.UserId == name && m.UserPWD == pwd && m.IsDel == false);
                    if (use != null)
                    {
                        GlobalParameter.UserId = use.UserId;
                        GlobalParameter.UserName = use.UserName;
                        GlobalParameter.Actorid = use.ActorId;
                        a = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                a = 0;
            }


            return a;
        }

        public Models.mtUser LoginSelectOutUseId(string name, string pwd)
        {
            pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
            Models.mtUser modeluser = new Models.mtUser();
            try
            {
                if (Regex.IsMatch(name, "13[0-9]{9}"))//验证手机号
                {
                    var user = (from o in dal.tbOwnerRfid
                                from o1 in dal.tbOwnerRfid
                                from u in dal.mtUser
                                from p in dal.tbPeople
                                where o.ObjectID == u.UserId &&
                                o1.ObjectID == p.PeopleID &&
                                u.IsDel == false &&
                                p.PhoneNum == name &&
                                u.UserPWD == pwd &&
                                p.Version == (dal.tbPeople.Where(item => item.PeopleID == o1.ObjectID).Max(item => item.Version))
                                select u).FirstOrDefault();
                    if (user != null)
                    {
                        modeluser = user;
                    }

                }
                else if (Regex.IsMatch(name, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))//验证邮箱
                {

                    var user = (from o in dal.tbOwnerRfid
                                from o1 in dal.tbOwnerRfid
                                from u in dal.mtUser
                                from p in dal.tbPeople
                                where o.ObjectID == u.UserId &&
                                o1.ObjectID == p.PeopleID &&
                                u.IsDel == false &&
                                p.Email == name &&
                                u.UserPWD == pwd &&
                                p.Version == (dal.tbPeople.Where(item => item.PeopleID == o1.ObjectID).Max(item => item.Version))
                                select u).FirstOrDefault();
                    if (user != null)
                    {
                        modeluser = user;
                    }
                }
                else //UserId
                {
                    var use = dal.mtUser.First(m => m.UserId == name && m.UserPWD == pwd && m.IsDel == false);
                    if (use != null)
                    {
                        modeluser = use;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return modeluser;
        }

        /// <summary>
        /// 数据
        /// </summary>
        /// <param name="TypeId"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public List<Models.tbPeblish> TypeContentList(string TypeId, int PageIndex, int sex)
        {
            try
            {
                List<Models.tbPeblish> p = (from a in dal.tbApplyBill
                                            from pe in dal.tbPeblish
                                            where pe.PeblishType == TypeId &&
                                            pe.PeblishID == a.ObjectID &&
                                            a.StateType == 2
                                            select pe).ToList();

                p = p.OrderByDescending(item => item.CreateTime).ToList();

                p = Funs.fenye<Models.tbPeblish>(p, PageIndex - 1, sex);

                return p;
            }
            catch (Exception)
            {
                return null;
            }

        }


        /// <summary>
        /// 所有Peblish的前5条
        /// </summary>
        /// <returns></returns>
        public JsonResult AllPeblish()
        {

            AllPeblishList all = new AllPeblishList();

            all.News = TypeContentList("1", 1, 3);
            all.Notice = TypeContentList("2", 1, 3);
            all.Policy = TypeContentList("4", 1, 3);
            all.Enterprises = TypeContentList("5", 1, 3);
            all.Advert = new List<guanggao>();

            foreach (var i in TypeContentList("3", 1, 3) as List<Models.tbPeblish>)
            {
                all.Advert.Add(new guanggao
                {
                    httpurl = i.PeblishContent,
                    imgurl = FilesImgUrl(i.FileIDs)
                });
            }
            return Json(all);

        }



        /// <summary>
        /// 图片地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string FilesImgUrl(string id)
        {
            string[] b = id.Split(',');
            int a = int.Parse(b[0]);
            string url = dal.tbFiles.First(item => item.FileID == a).FilePath;
            return url;
        }





        #endregion

        #region 数据



        #endregion



    }

    #region 模型

    /// <summary>
    /// 菜单
    /// </summary>
    public class Menus : Models.Functions
    {
        public List<Models.Functions> FunctionList { get; set; }
    }


    /// <summary>
    /// 新闻
    /// </summary>
    public class News
    {
        public string NewsTitle { get; set; }
        public string NewsContent { get; set; }
    }

    /// <summary>
    /// 信息
    /// </summary>
    public class PeblishFiles
    {
        public Models.tbPeblish peblish { get; set; }
        public List<Models.tbFiles> files { get; set; }
    }


    /// <summary>
    /// 所有Peblish的前5条
    /// </summary>
    public class AllPeblishList
    {
        public List<Models.tbPeblish> News { get; set; }
        public List<Models.tbPeblish> Notice { get; set; }
        public List<guanggao> Advert { get; set; }
        public List<Models.tbPeblish> Policy { get; set; }
        public List<Models.tbPeblish> Enterprises { get; set; }

    }



    public class guanggao
    {
        public string imgurl { get; set; }
        public string httpurl { get; set; }
    }
    #endregion

}
