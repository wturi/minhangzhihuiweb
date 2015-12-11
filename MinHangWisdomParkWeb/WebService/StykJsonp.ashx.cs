using MinHangWisdomParkWeb.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinHangWisdomParkWeb.WebService
{
    /// <summary>
    /// StykJsonp 的摘要说明
    /// </summary>
    public class StykJsonp : IHttpHandler
    {

        MainController main = new MainController();
        BusinessApplicationController business = new BusinessApplicationController();
        AuthorizationAuditController aut = new AuthorizationAuditController();
        InformationDeliveryController inform = new InformationDeliveryController();
        BBSHelps bbs = new BBSHelps();

        /// <summary>
        ///接口文件
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            #region 参数

            //默认传参
            var type = context.Request["type"];
            var callbackFunName = context.Request["callback"];
            if (type == null || callbackFunName == null)
            {
                return;
            }

            //公用参数
            string jsonStr;
            var time = context.Request["time"]; //时间
            var title = context.Request["title"];//标题
            var content = context.Request["content"];//内容
            var client = context.Request["clientip"];//客户IP地址
            var pageindex = context.Request["pageindex"];//页码
            var pagenumber = context.Request["pagenumber"];//每页数量
            var statetype = context.Request["statetype"];//通用状态
            var suotext = context.Request["suotext"];//通用搜索条件
            var applyid = context.Request["applyid"];//审核表ID
            var applybool = context.Request["applybool"];//审核是否通过
            var actorid = context.Request["actorid"];//角色id
            var peblishid = context.Request["peblishid"];//信息表通用ID


            //用户传参
            var useid = context.Request["useid"];//用户ID
            var usename = context.Request["usename"];//用户名
            var usepwd = context.Request["usepwd"];//用户密码

            //业务传参
            var buniesstype = context.Request["buniesstype"];//我的申请中需要用到的业务申请类别ID
            var RenNumber = context.Request["rennumber"];//人员制卡数量
            var shenqingid = context.Request["shenqingid"];//申请ID


            //人员出入园区传参
            var pizhunrenid = context.Request["pizhunrenid"];//临时批准人上级部门ID
            //报修传参
            var repairtype = context.Request["repairtype"];//报修类别


            //出入园公共传参
            var outorin = context.Request["outorin"];//类别
            var Peopleid = context.Request["Peopleid"];//人
            var Carid = context.Request["Carid"];//车
            var Productid = context.Request["Productid"];//物
            var pizhunren = context.Request["pizhunren"];//批准人

            //peblish传参
            var peblishtype = context.Request["peblishtype"];//Peblish类别

            //移动社区
            var boardid = context.Request["boardid"];//分类id
            var topicid = context.Request["topicid"];//话题id


            #endregion

            switch (type)
            {
                #region 登陆
                case "Login":
                    if (string.IsNullOrEmpty(usename) || string.IsNullOrEmpty(usepwd))
                    {
                        return;
                    }
                    Models.mtUser modeluser = main.LoginSelectOutUseId(usename, usepwd);
                    //jsonStr = string.Format("{0}({useid:\"{1}\"})", callbackFunName, main.LoginSelectOutUseId(usename, usepwd));
                    jsonStr = callbackFunName + "({\"useid\":\"" + modeluser.UserId + "\",\"actorid\":\"" + modeluser.ActorId + "\",\"usename\":\"" + modeluser.UserName + "\"})";
                    break;
                #endregion

                #region 业务

                //人员制卡申请
                case "RenYuanAdd":
                    if (string.IsNullOrEmpty(useid) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, business.RenYuanShen(useid, title, content, RenNumber, client));
                    break;
                //证卡挂失
                case "zhengkaguashi":
                    if (string.IsNullOrEmpty(content))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, business.guashijiekou(useid, content, client));
                    break;
                //网上报修
                case "baoxiu":
                    if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(useid))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, business.InsertBaoxiu(useid, repairtype, title, content));
                    break;
                //正式出入园
                case "zhengshi":

                    jsonStr = "";
                    break;
                //临时出入园
                case "linshi":
                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(Peopleid))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, business.LinShiJieKou(useid, title, content, Peopleid, Carid, Productid, pizhunren));
                    break;

                //我的申请
                case "myshenqing":
                    if (string.IsNullOrEmpty(useid) || string.IsNullOrEmpty(buniesstype) || string.IsNullOrEmpty(pageindex))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(myshenqin(useid, buniesstype, pageindex)));
                    break;
                //申请详细
                case "myshenqingxs":
                    if (string.IsNullOrEmpty(shenqingid))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(business.InOrOutXSJieKou(shenqingid)));
                    break;
                //申请业务
                case "myshenqingbuniess":
                    if (string.IsNullOrEmpty(shenqingid))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(business.SelectBuniessById(shenqingid)));
                    break;
                //申请报修单个
                case "myshenqingrepair":
                    if (string.IsNullOrEmpty(shenqingid))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(business.SelectRepairById(shenqingid)));
                    break;
                //临时出入园批准人
                //case "pizhunren":
                //    if (string.IsNullOrEmpty(pizhunrenid))
                //    {
                //        return;
                //    }
                //    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(business.PiZhunrenjiekou(pizhunrenid)));
                //    break;
                //需要审核列表
                case "shenheAll":

                    if (string.IsNullOrEmpty(useid) && string.IsNullOrEmpty(statetype))
                    {
                        return;
                    }
                    suotext = string.IsNullOrEmpty(suotext) ? "" : suotext;
                    int pageindexnum = int.Parse(pageindex);
                    int pagenumbernum = int.Parse(pagenumber);
                    switch (statetype)
                    {
                        case "4":
                            jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(aut.BuniessList(statetype, pageindexnum, pagenumbernum, suotext, useid)));
                            break;
                        case "5":
                            jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(aut.BuniessList(statetype, pageindexnum, pagenumbernum, suotext, useid)));
                            break;
                        case "1":
                            int statetypelin1 = int.Parse(statetype);
                            jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(aut.RepairList(statetypelin1, pageindexnum, pagenumbernum, suotext, useid)));
                            break;
                        case "2":
                            int statetypelin2 = int.Parse(statetype);
                            jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(aut.RepairList(statetypelin2, pageindexnum, pagenumbernum, suotext, useid)));
                            break;
                        case "all":
                            jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(aut.ShenHeAll(pageindexnum, pagenumbernum, suotext, useid)));
                            break;
                        default:
                            jsonStr = string.Format("{0}({1})", callbackFunName, 1);
                            break;
                    }
                    break;
                //审批
                case "shenhetijiao":
                    if (string.IsNullOrEmpty(applyid) && string.IsNullOrEmpty(applybool))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, aut.ShenHewebservice(applyid, content, applybool));
                    break;
                //功能权限
                case "menusgongneng":
                    if (string.IsNullOrEmpty(actorid))
                    {
                        return;
                    }
                    int aid = int.Parse(actorid);
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(Helps.ActorPower(aid)));
                    break;
                //正式出入园区人车物数据信息
                case "OutOrInShuju":
                    if (string.IsNullOrEmpty(outorin) || string.IsNullOrEmpty(useid))
                    {
                        return;
                    }
                    if (string.IsNullOrEmpty(suotext))
                    {
                        suotext = "";
                    }
                    switch (outorin)
                    {
                        case "1": jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(business.SelectPeople(suotext, useid))); break;
                        case "2": jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(business.SelectCars(suotext, useid))); break;
                        case "3": jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(business.SelectProduct(suotext, useid))); break;
                        default: jsonStr = string.Format("{0}({1})", callbackFunName, 0); break;
                    }
                    break;
                //正式出入园区提交
                case "OutOrIninsert":
                    if (string.IsNullOrEmpty(useid))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, business.InsertMainJieKou(useid, outorin, title, content, Peopleid, Carid, Productid));
                    break;
                //正式出入园单个信息
                case "CheckInOutZs":
                    if (string.IsNullOrEmpty(shenqingid))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(business.InOrOutZsJieKou(shenqingid)));
                    break;
                //临时出入园区提交
                case "OutOrIninsertls":
                    if (string.IsNullOrEmpty(useid))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(business.LinShiJieKou(useid, title, content, Peopleid, Carid, Productid, "1")));
                    break;
                //公告全部
                case "PeblishAll":
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(inform.PeblishSelect(peblishtype, pageindex, int.Parse(pagenumber))));
                    break;

                //移动社区分类列表
                case "sehqulist":
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(bbs.BoardList()));
                    break;
                //移动社区各分类话题
                case "TopicList":
                    if (string.IsNullOrEmpty(boardid))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(bbs.TopicListByType(int.Parse(boardid))));
                    break;
                //返回话题详细内容及回复内容
                case "PostList":
                    if (string.IsNullOrEmpty(topicid))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(bbs.PostList(int.Parse(topicid))));
                    break;
                //关联用户
                case "BBSUser":
                    if (string.IsNullOrEmpty(usename) || string.IsNullOrEmpty(usepwd) || string.IsNullOrEmpty(client))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, bbs.BBSUser(usename, usepwd, client));
                    break;
                //发帖
                case "BBSTopic":
                    if (string.IsNullOrEmpty(boardid) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(usename) || string.IsNullOrEmpty(client) || string.IsNullOrEmpty(usepwd))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, bbs.BBSTopic(int.Parse(boardid), title, usename, usepwd, content, client));
                    break;
                //回帖
                case "BBSPost":
                    if (string.IsNullOrEmpty(topicid) || string.IsNullOrEmpty(usename) || string.IsNullOrEmpty(client) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(usepwd))
                    {
                        return;
                    }
                    jsonStr = string.Format("{0}({1})", callbackFunName, bbs.BBSPost(int.Parse(topicid), usename, client, usepwd, content));
                    break;
                //公告
                case "gonggao":
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(bbs.gonggao(int.Parse(pageindex), int.Parse(pagenumber))));
                    break;
                //单个公告
                case "gonggaodan":
                    jsonStr = string.Format("{0}({1})", callbackFunName, JsonConvert.SerializeObject(bbs.gonggaos(peblishid)));
                    break;
                //app、main页面概要信息
                case "zuiixin":
                    if (string.IsNullOrEmpty(useid))
                    {
                        return;
                    }
                    int aaa = business.SelectBuniessAll("", "0", 1000000000, "4", useid).Count() + business.SelectBuniessAll("", "0", 1000000000, "5", useid).Count() + business.SelectRepairAll("", "0", 1000000000, useid).Count() + business.InOrOutJieKou("1", useid, "0", 1000000000, "").Count() + business.InOrOutJieKou("2", useid, "0", 1000000000, "").Count();
                    int mashenhe = bbs.MaxShenhe(useid);
                    string strzuixin = bbs.zuixin();
                    jsonStr = callbackFunName + "({\"shenqing\":\"" + aaa + "\",\"shenhe\":\"" + mashenhe + "\",\"shequ\":\"" + strzuixin + "\"})";
                    break;
                #endregion
                default:
                    return;
            }
            context.Response.Write(jsonStr);
        }

        /// <summary>
        /// 我的申请列表
        /// </summary>
        /// <param name="useid"></param>
        /// <param name="mytype"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public List<Myshen> myshenqin(string useid, string mytype, string pageindex)
        {
            try
            {
                List<Myshen> my = new List<Myshen>();
                switch (mytype)
                {
                    //人员制卡
                    case "1":
                        my = business.SelectBuniessAll("", pageindex, 10, "4", useid).Select(item => new Myshen { id = item.BuniessID, title = item.BuniessContent.Split(new string[] { "|+|" }, StringSplitOptions.RemoveEmptyEntries)[0], content = item.BuniessContent.Split(new string[] { "|+|" }, StringSplitOptions.RemoveEmptyEntries)[1], status = item.StateName, statusname = item.ConfimeName }).ToList();
                        break;
                    //证卡挂失
                    case "2":
                        my = business.SelectBuniessAll("", pageindex, 10, "5", useid).Select(item => new Myshen { id = item.BuniessID, title = item.BuniessContent.Split(new string[] { "|+|" }, StringSplitOptions.RemoveEmptyEntries)[0], content = item.BuniessContent.Split(new string[] { "|+|" }, StringSplitOptions.RemoveEmptyEntries)[1], status = item.StateName, statusname = item.ConfimeName }).ToList();
                        break;
                    //网上报修
                    case "3":
                        my = business.SelectRepairAll("", pageindex, 10, useid).Select(m => new Myshen { id = m.RepairID, title = m.RepairTitle, content = m.RepairContent, status = m.StateName, statusname = "" }).ToList();
                        break;
                    //正式出入园
                    case "4":
                        my = business.InOrOutJieKou("1", useid, pageindex, 10, "").Select(m => new Myshen { id = m.CheckID, title = m.Title, content = "", status = m.StatusName, statusname = "" }).ToList();
                        break;
                    //临时出入园
                    case "5":
                        my = business.InOrOutJieKou("2", useid, pageindex, 10, "").Select(m => new Myshen { id = m.CheckID, title = m.Title, content = "", status = m.StatusName, statusname = "" }).ToList();
                        break;
                }
                return my;
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }




        public bool IsReusable
        {
            get
            {
                return false;
            }


        }

        public class Myshen
        {
            public string id { get; set; }
            public string title { get; set; }
            public string content { get; set; }
            public string status { get; set; }
            public string statusname { get; set; }
        }
    }
}