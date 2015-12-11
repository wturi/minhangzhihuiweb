using MinHangWisdomParkWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinHangWisdomParkWeb.Controllers
{
    public class JurisdictionController : BaseController
    {


        #region 参数

        Models.ajIIPdbEntities1 dal = new Models.ajIIPdbEntities1();

        ApplyHelp applyhelp = new ApplyHelp();

        #endregion

        #region 页面

        /// <summary>
        /// 角色信息管理
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult juese(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            ViewBag.ActorList = ActorSelect();
            return View();
        }


        /// <summary>
        /// 用户角色管理
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult yonghu(string Type, string Title, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title))
            {
                ViewBag.Type = Type.Replace("申请", "");
                ViewBag.Title = Title;
                ViewBag.lujin = lujin;
            }
            return View();
        }

        #endregion

        #region JsonReault

        /// <summary>
        /// 提取权限菜单
        /// </summary>
        /// <param name="actorid"></param>
        /// <returns></returns>
        public JsonResult ActorSelectJson(string actorid)
        {
            try
            {

                return Json(new { msg = "OK" });
            }
            catch (Exception ee)
            {
                return Json(new { msg = "NO" });
            }
        }


        /// <summary>
        /// 添加角色和关联网页
        /// </summary>
        /// <param name="actorname"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public JsonResult ActorAddJson(string actorname, string str)
        {
            try
            {
                ActorAdd(actorname, str);
                return Json(new { msg = "OK" });
            }
            catch (Exception ee)
            {
                return Json(new { msg = "NO" });
            }
        }

        /// <summary>
        /// 角色关联权限
        /// </summary>
        /// <param name="actorid"></param>
        /// <returns></returns>
        public JsonResult ActorPowerSelect(string actorid)
        {
            try
            {
                int id = int.Parse(actorid);
                return Json(ActorPower(id));
            }
            catch (Exception ee)
            {

                return Json(new { msg = "NO" });
            }
        }

        /// <summary>
        /// 修改角色权限关联
        /// </summary>
        /// <param name="actorid"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public JsonResult ActorPowerUpdateJson(int actorid, string str)
        {
            try
            {
                ActorPowerUpdate(actorid, str);
                return Json(new { msg = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { msg = "NO" });

            }
        }

        /// <summary>
        /// 添加人员和角色关联
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="actorid"></param>
        /// <returns></returns>
        public JsonResult UserActorAddJson(string userid, int actorid)
        {
            if (UserActorAdd(userid.Trim(), actorid))
            {
                return Json(new { msg = "OK" });
            }
            else
            {
                return Json(new { msg = "NO" });
            }
        }

        /// <summary>
        /// 删除人员和角色关联
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="actorid"></param>
        /// <returns></returns>
        public JsonResult UserActorDeleteJson(string userid, int actorid)
        {
            if (UserActorDelete(userid, actorid))
            {
                return Json(new { msg = "OK" });
            }
            else
            {
                return Json(new { msg = "NO" });
            }
        }

        /// <summary>
        /// 用户与角色关联
        /// </summary>
        /// <returns></returns>
        public JsonResult UserActorSelectJson()
        {
            try
            {
                return Json(UserActorSelect());
            }
            catch (Exception e)
            {
                return Json(new { msg = "NO" });
            }
        }


        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        public JsonResult OwnerSelect()
        {
            var a = dal.mtOwner.Where(item => item.OwnerParentId != 0 && item.IsDel == false).ToList();
            return Json(a);
        }

        /// <summary>
        /// 用户
        /// </summary>
        /// <param name="ownerid"></param>
        /// <returns></returns>
        public JsonResult UserByOwnerId(int ownerid)
        {
            return Json(dal.mtUser.Where(item => item.IsDel == false && item.OwnerId == ownerid).ToList());
        }

        /// <summary>
        /// 角色
        /// </summary>
        /// <returns></returns>
        public JsonResult ActorALLselectjson()
        {
            return Json(ActorSelect());
        }
        #endregion


        #region Webseive

        /// <summary>
        /// 提取网站角色
        /// </summary>
        /// <returns></returns>
        public List<Models.mtActor> ActorSelect()
        {
            return dal.mtActor.Where(item => item.ActorType == 1).ToList();
        }

        /// <summary>
        /// 添加角色和关联网页
        /// </summary>
        /// <param name="ActorName"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool ActorAdd(string ActorName, string str)
        {
            try
            {
                Models.mtActor actor = new Models.mtActor
                {
                    ActorName = ActorName,
                    ActorType = 1,
                };
                dal.mtActor.Add(actor);
                dal.SaveChanges();

                int actorid = dal.mtActor.Where(item => item.ActorName == ActorName).Max(item => item.ActorID);

                ActorPowerAdd(actorid, str);

                return true;

            }
            catch (Exception ee)
            {
                return false;

            }
        }

        /// <summary>
        /// 添加关联表
        /// </summary>
        /// <param name="actorid"></param>
        /// <param name="str"></param>
        public void ActorPowerAdd(int actorid, string str)
        {
            string[] strs = str.Split(' ');

            foreach (string s in strs)
            {
                int ss = int.Parse(s);
                Models.tbActorPower p = new Models.tbActorPower
                {
                    ActorId = actorid,
                    FuncNo = ss,
                    PowerId = 1,
                    FuncType = "WEBS"
                };
                dal.tbActorPower.Add(p);
            }
            dal.SaveChanges();
        }

        /// <summary>
        /// 角色关联
        /// </summary>
        /// <returns></returns>
        public List<ActorsLists> ActorPower(int ActorId)
        {

            List<ActorsLists> a = new List<ActorsLists>();
            try
            {
                var ftype = (from aa in dal.tbActorPower
                             from ff in dal.Functions
                             from f1 in dal.Functions
                             where aa.ActorId == ActorId
                             && aa.FuncNo == ff.FunctionID
                             && ff.IsDel == false
                             && ff.Superior == f1.FunctionID
                             select f1).ToList();
                var fname = (from aa in dal.tbActorPower
                             from ff in dal.Functions
                             from f1 in dal.Functions
                             where aa.ActorId == ActorId
                             && aa.FuncNo == ff.FunctionID
                             && ff.IsDel == false
                             && ff.Superior == f1.FunctionID
                             select ff).ToList();
                foreach (var i in dal.Functions.Where(item => item.MasterMenu == true && item.IsDel == false))
                {
                    a.Add(new ActorsLists
                    {
                        FunctionID = i.FunctionID,
                        FunctionName = i.FunctionName,
                        Superior = i.Superior,
                        MasterMenu = i.MasterMenu,
                        Type = i.Type,
                        ischecked = ftype.FirstOrDefault(item => item.FunctionID == i.FunctionID) != null,
                        List = (from mm in dal.Functions
                                where mm.Superior == i.FunctionID
                                select new Actors
                                {
                                    FunctionID = mm.FunctionID,
                                    FunctionName = mm.FunctionName,
                                    Superior = mm.Superior,
                                    MasterMenu = mm.MasterMenu,
                                    Type = mm.Type,
                                }).ToList()
                    });
                }

                foreach (var d in a)
                {
                    foreach (var dd in d.List)
                    {
                        if (fname.FirstOrDefault(item => item.FunctionID == dd.FunctionID) != null)
                        {
                            dd.ischecked = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }

            return a;
        }

        /// <summary>
        /// 修改角色和网页关联
        /// </summary>
        /// <param name="actorid"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool ActorPowerUpdate(int actorid, string str)
        {
            try
            {
                ActorPowerDelect(actorid);
                ActorPowerAdd(actorid, str);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        /// <summary>
        /// 删除角色和网页关联
        /// </summary>
        /// <param name="actorid"></param>
        public void ActorPowerDelect(int actorid)
        {
            foreach (var i in dal.tbActorPower.Where(item => item.ActorId == actorid && item.FuncType == "WEBS"))
            {
                dal.tbActorPower.Remove(i);
            }
            dal.SaveChanges();
        }

        /// <summary>
        /// 添加人员和角色关联
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="actorid"></param>
        /// <returns></returns>
        public bool UserActorAdd(string userid, int? actorid)
        {
            try
            {
                if (dal.mtUser.Where(item => item.UserId == userid && item.ActorId == actorid).ToList().Count != 0)
                {
                    return false;
                }

                var user = dal.mtUser.FirstOrDefault(item => item.UserId == userid);
                user.ActorId = actorid;
                dal.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除人员和角色关联
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="actorid"></param>
        /// <returns></returns>
        public bool UserActorDelete(string userid, int actorid)
        {
            try
            {
                Models.mtUser u = dal.mtUser.First(item => item.UserId == userid && item.ActorId == actorid);
                u.ActorId = null;
                dal.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 用户和角色关联
        /// </summary>
        /// <returns></returns>
        public List<UserActorList> UserActorSelect()
        {
            var d = (from o in dal.mtOwner
                     from u in dal.mtUser
                     from a in dal.mtActor
                     where u.OwnerId == o.OwnerId
                     && u.ActorId == a.ActorID
                     select new UserActorList
                     {
                         userid = u.UserId,
                         username = u.UserName,
                         ownername = o.OwnerName,
                         actorid = a.ActorID,
                         actorname = a.ActorName
                     }).ToList();
            return d;
        }

        #endregion

        #region 模型

        /// <summary>
        /// 角色权限
        /// </summary>
        public class Actors
        {
            public int FunctionID { get; set; }
            public string FunctionName { get; set; }
            public int Superior { get; set; }
            public bool MasterMenu { get; set; }
            public string Type { get; set; }
            public bool ischecked { get; set; }
            public bool? isphone { get; set; }
        }

        public class ActorsLists : Actors
        {
            public List<Actors> List { get; set; }
        }

        public class UserActorList
        {
            public string userid { get; set; }
            public string username { get; set; }
            public string ownername { get; set; }
            public int actorid { get; set; }
            public string actorname { get; set; }
        }

        #endregion

    }
}
