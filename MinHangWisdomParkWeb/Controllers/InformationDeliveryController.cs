using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MinHangWisdomParkWeb.Filters;

namespace MinHangWisdomParkWeb.Controllers
{
    /// <summary>
    /// 信息发布管理
    /// </summary>
    public class InformationDeliveryController : BaseController
    {

        #region 参数

        Models.ajIIPdbEntities1 dal = new Models.ajIIPdbEntities1();

        ApplyHelp applyhelp = new ApplyHelp();

        #endregion

        #region v0.2

        /// <summary>
        /// 模板页面
        /// </summary>
        /// <param name="type">信息类型</param>
        /// <returns></returns>
        [UserChkAttribute.IsUser]
        public ActionResult Index(string Type, string Title, string CodeID, string lujin)
        {
            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(CodeID))
            {
                int codeid = int.Parse(CodeID);
                ViewBag.Type = Type.Substring(0, 2);
                ViewBag.CodeID = dal.mtUniversalCode.FirstOrDefault(m => m.UniversalType == "PeblishType" && m.CodeID == codeid).CodeID;
                ViewBag.Title = Title;
                var shenqin = ShenQins(CodeID);
                ViewBag.Count = (shenqin.Count % 10 != 0) ? (shenqin.Count / 10 + 1) : (shenqin.Count);
                var shenqinfeny = Funs.fenye<ShenQin>(shenqin, 0, 10);
                ViewBag.PeblishList = shenqinfeny;
                ViewBag.lujin = lujin;
            }

            return View();
        }



        #region 图片上传
        [HttpPost]
        public JsonResult UpLoadPhoto(HttpPostedFileBase file)
        {
            var res = CheckImg(file);
            string imgurl = "";
            string strerror = "";
            string imgname = "";
            if (res == "ok")
            {
                var fileName = file.FileName;//Path.GetExtension() 也许可以解决这个问题，先不管了。
                int i = fileName.LastIndexOf('.');//取得文件名中最后一个"."的索引    
                string fileextenName = fileName.Substring(i).ToLower();
                string newFileName = Guid.NewGuid().ToString() + fileextenName;
                var pathtemp = Path.Combine(Server.MapPath("~/Uploads/"), newFileName);//先存入临时文件夹
                var scrtemp = Path.Combine("~/Uploads/", newFileName);//图片展示的地址
                imgname = fileName.Replace(fileextenName, "");//图片名称

                var list = Session["Imgscr"] as List<string>;
                var slist = Session["ImgServerscr"] as List<string>;
                if (list != null)
                {
                    list.Add(scrtemp);
                }
                else
                {
                    list = new List<string> { scrtemp };
                    Session["Imgscr"] = list;
                }
                if (slist != null)
                {
                    slist.Add(pathtemp);
                }
                else
                {
                    slist = new List<string> { pathtemp };
                    Session["ImgServerscr"] = slist;
                }

                file.SaveAs(pathtemp);
                //Response.Write("");
                imgurl = "/Uploads/" + newFileName + "";
            }
            else
            {
                strerror = res;
            }
            var Result = new { ErrorInfo = strerror, imgUrl = imgurl, imgId = InsertFiles(imgurl, imgname).ToString() };


            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        private string CheckImg(HttpPostedFileBase file)
        {
            if (file == null) return "图片不能空！";
            if (file.ContentLength / 1024 > 8000)
            {
                return "图片太大";
            }
            if (file.ContentLength / 1024 < 10)
            {
                return "图片太小！";
            }
            var image = GetExtensionName(file.FileName).ToLower();
            if (image != ".bmp" && image != ".png" && image != ".gif" && image != ".jpg" && image != ".jpeg")// 这里你自己加入其他图片格式，最好全部转化为大写再判断，我就偷懒了
            {
                return "格式不对";
            }

            var scrtemp = Path.Combine("/Uploads/", file.FileName);//图片展示的地址
            var list = Session["Imgscr"] as List<string>;
            if (list != null && list.Find(n => n == scrtemp) != null)
            {
                return "同样的照片已经存在！";
            }

            return "ok";
        }
        public string GetExtensionName(string fileName)
        {
            if (fileName.LastIndexOf("\\", StringComparison.Ordinal) > -1)//在不同浏览器下，filename有时候指的是文件名，有时候指的是全路径，所有这里要要统一。
            {
                fileName = fileName.Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1);//IndexOf 有时候会受到特殊字符的影响而判断错误。加上这个就纠正了。
            }
            return Path.GetExtension(fileName.ToLower());
        }

        #endregion



        #region 功能

        /// <summary>
        /// 插入files表
        /// </summary>
        /// <param name="url"></param>
        public int InsertFiles(string url, string name)
        {
            try
            {
                Models.tbFiles file = new Models.tbFiles
                {
                    FileType = "img",
                    FilePath = url,
                    FileName = name
                };
                dal.tbFiles.Add(file);
                dal.SaveChanges();
                return file.FileID;
            }
            catch (Exception e)
            {
                throw;
            }

        }


        /// <summary>
        /// 删除files表数据及文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public void DeleteFiles(string url)
        {
            try
            {
                dal.tbFiles.Remove(dal.tbFiles.FirstOrDefault(m => m.FilePath == url));
                dal.SaveChanges();
                FileHelper.DeleteFile(Server.MapPath(url));
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="PeblishType"></param>
        /// <param name="PeblishTitle"></param>
        /// <param name="PublishContent"></param>
        /// <param name="PublishImg"></param>
        public JsonResult InsertPeblishApply(string useid, string PeblishType, string PeblishTitle, string PublishContent, string[] PublishImg, string DateTimeNew, string DateTimeOld)
        {
            try
            {
                useid = GlobalParameter.UserId;
                PublishContent = HttpUtility.UrlDecode(PublishContent, System.Text.Encoding.GetEncoding("UTF-8"));
                applyhelp.InsertApplyBill(useid, InsertPeblish(PeblishType, PeblishTitle, PublishContent, PublishImg), "Peblish", DateTime.Now.ToString(), 2, Helps.SearchConfirmIdInXmlByType("信息"));
                return Json(new { msg = "ok" });

            }
            catch (Exception ee)
            {
                return Json(new { msg = "no" });
                throw;
            }
        }


        /// <summary>
        /// 插入peblish数据并返回插入数据ID
        /// </summary>
        /// <param name="PeblishType"></param>
        /// <param name="PeblishTitle"></param>
        /// <param name="PublishContent"></param>
        /// <param name="PublishImg"></param>
        /// <returns></returns>
        private string InsertPeblish(string PeblishType, string PeblishTitle, string PublishContent, string[] PublishImg)
        {

            try
            {
                Models.tbPeblish peblish = new Models.tbPeblish
                {
                    PeblishID = (int.Parse((dal.tbPeblish.Max(m => m.PeblishID) == null ? "0" : dal.tbPeblish.Max(m => m.PeblishID))) + 1).ToString().PadLeft(12, '0'),
                    PeblishType = PeblishType,
                    PeblishTitle = PeblishTitle,
                    PeblishContent = PublishContent,
                    Updater = GlobalParameter.UserId
                };
                if (PublishImg != null)
                {
                    peblish.FileIDs = string.Join(",", PublishImg);
                }
                dal.tbPeblish.Add(peblish);
                dal.SaveChanges();
                return peblish.PeblishID;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 提取当前类别数据
        /// </summary>
        /// <param name="PeblishType"></param>
        /// <returns></returns>
        public List<ShenQin> ShenQins(string PeblishType)
        {
            var sq = (from p in dal.tbPeblish
                      from a in dal.tbApplyBill
                      from c in dal.tbConfirmState
                      from u in dal.mtUniversalCode
                      from u1 in dal.mtUser
                      where p.PeblishID == a.ObjectID &&
                      u.UniversalType == "StateType" &&
                      a.StateType == u.CodeID &&
                      p.Updater == GlobalParameter.UserId &&
                      p.PeblishType == PeblishType &&
                      a.ApplyType == "Peblish" &&
                      a.ApplyID == c.ApplyID &&
                      c.ConfirmerID == u1.UserId
                      select new ShenQin
                      {
                          PeblishID = p.PeblishID,
                          Title = p.PeblishTitle,
                          Time = p.CreateTime,
                          Content = p.PeblishContent,
                          StateName = u.CodeName,
                          ConfirmerName = u1.UserName,
                          ConfirmeMemo = c.ConfirmeMemo != null ? c.ConfirmeMemo : ""
                      }).ToList();
            sq = sq.OrderByDescending(item => item.Time).ToList();
            return sq;
        }


        /// <summary>
        /// 提取当前类别信息数据--已经审核通过的
        /// </summary>
        /// <param name="PeblishType">Peblish类别</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="pagenumber">每页显示数量</param>
        /// <returns></returns>
        public List<Models.tbPeblish> PeblishSelect(string PeblishType, string PageIndex, int pagenumber)
        {
            List<Models.tbPeblish> pp = new List<Models.tbPeblish>();
            pp = (from p in dal.tbPeblish
                  from a in dal.tbApplyBill
                  where p.PeblishType == PeblishType &&
                  a.ObjectID == p.PeblishID &&
                  a.StateType == 2
                  select p).OrderByDescending(item => item.CreateTime).ToList();
            pp = Funs.fenye<Models.tbPeblish>(pp, int.Parse(PageIndex) - 1, pagenumber);
            return pp;
        }

        #endregion


        #region AJAX 功能

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="CodeID"></param>
        /// <returns></returns>
        public JsonResult Ajaxshuju(string PageIndex, string CodeID)
        {
            var shenqinfeny = Funs.fenye<ShenQin>(ShenQins(CodeID), int.Parse(PageIndex) - 1, 10);

            return Json(shenqinfeny);
        }



        /// <summary>
        /// 多条件查询
        /// </summary>
        /// <returns></returns>
        public JsonResult DuoTiaoJianChaXun(string str, string CodeID, string PageIndex, int pagesize)
        {
            var shenqinfeny = ShenQins(CodeID);
            string[] strs = str.Split(' ');

            var sss = shenqinfeny.Where(item =>
             {
                 var tmp = item.Content + " " + item.Title + " " + item.StateName;
                 foreach (var s in strs)
                 {
                     if (tmp.Contains(s))
                     {
                         return true;
                     }
                 }
                 return false;
             }).ToList();
            sss = sss.OrderByDescending(item => item.Time).ToList();
            sss = Funs.fenye<ShenQin>(sss, int.Parse(PageIndex) - 1, pagesize);
            return Json(sss);
        }



        public JsonResult ChaByPeblishId(string peblishid)
        {
            var peblishs = dal.tbPeblish.First(m => m.PeblishID == peblishid);

            return Json(new { title = peblishs.PeblishTitle, Content = peblishs.PeblishContent });
        }




        #endregion


        #endregion



    }

    #region 模型
    public class ShenQin
    {
        public string PeblishID { get; set; }
        public string Title { get; set; }
        public DateTime? Time { get; set; }
        public string Content { get; set; }
        public string StateName { get; set; }
        public string ConfirmerName { get; set; }
        public string ConfirmeMemo { get; set; }
    }



    #endregion
}
