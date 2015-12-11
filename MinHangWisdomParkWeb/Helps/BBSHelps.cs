using MinHangWisdomParkWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MinHangWisdomParkWeb
{
    public class BBSHelps
    {
        Models.MinHangBBSDataEntities1 bbsdal = new Models.MinHangBBSDataEntities1();
        Models.ajIIPdbEntities1 dal = new Models.ajIIPdbEntities1();
        Controllers.AuthorizationAuditController auth = new AuthorizationAuditController();

        /// <summary>
        /// 返回论坛分类
        /// </summary>
        /// <returns></returns>
        public List<Models.Jokul_Forum_Board> BoardList()
        {
            List<Models.Jokul_Forum_Board> b = new List<Models.Jokul_Forum_Board>();

            b = bbsdal.Jokul_Forum_Board.Where(item => item.ParentId == 1 && item.Show == 1).ToList();

            return b;
        }

        /// <summary>
        /// 返回分类下话题列表
        /// </summary>
        /// <param name="BoardId"></param>
        /// <returns></returns>
        public List<Models.Jokul_Forum_Topic> TopicListByType(int BoardId)
        {
            List<Models.Jokul_Forum_Topic> t = new List<Models.Jokul_Forum_Topic>();

            t = bbsdal.Jokul_Forum_Topic.Where(item => item.BoardId == BoardId).OrderByDescending(item => item.LastPostDatetime).ToList();

            return t;
        }


        /// <summary>
        /// 返回话题详细内容及回复内容
        /// </summary>
        /// <param name="TopicId"></param>
        /// <returns></returns>
        public PostList PostList(int TopicId)
        {
            PostList pl = new PostList();
            Models.Jokul_Forum_Topic top = bbsdal.Jokul_Forum_Topic.FirstOrDefault(item => item.Id == TopicId);
            top.ViewCount = top.ViewCount + 1;
            bbsdal.SaveChanges();
            pl.Post1 = bbsdal.Jokul_Forum_Post_1.FirstOrDefault(item => item.TopicId == TopicId && item.First == 1);
            pl.Posts = bbsdal.Jokul_Forum_Post_1.Where(item => item.TopicId == TopicId && item.First != 1).ToList();
            return pl;
        }

        /// <summary>
        /// 关联用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd">MD5后的密码</param>
        /// <returns></returns>
        public int BBSUser(string username, string pwd, string userclientip)
        {
            try
            {
                if (bbsdal.Jokul_Passport_Unique.Where(item => item.Data == username).Count() != 0)
                {
                    return 2;
                }
                else
                {

                    Models.Jokul_Passport_User puser = new Models.Jokul_Passport_User
                    {
                        Username = username,
                        Password = pwd,
                        GroupId = 3,
                        CreateIp = userclientip,
                        CreateDateTime = DateTime.Now,
                        LastIp = userclientip,
                        LastDatetime = DateTime.Now,
                        PMCount = 0,
                        PMNewCount = 0,
                        Question = "我的密码",
                        Answer = pwd,
                        Email = "",
                        UnevaluatedEmail = "",
                        Avatar = 0
                    };

                    bbsdal.Jokul_Passport_User.Add(puser);
                    bbsdal.SaveChanges();


                    Models.Jokul_Passport_Unique unique = new Models.Jokul_Passport_Unique
                    {
                        UserId = puser.Id,
                        Type = 1,
                        Data = puser.Username,
                        CreateTime = DateTime.Now
                    };
                    bbsdal.Jokul_Passport_Unique.Add(unique);
                    bbsdal.SaveChanges();

                    Models.Jokul_Forum_User uuser = new Models.Jokul_Forum_User
                    {
                        DataId = unique.DataId,
                        Id = puser.Id,
                        Username = puser.Username,
                        Email = "",
                        CreateIp = puser.CreateIp,
                        CreateDateTime = DateTime.Now,
                        LastIp = puser.CreateIp,
                        LastDatetime = DateTime.Now,
                        GroupId = 8,
                        TemplateId = 0,
                        LastPostDateTime = DateTime.Now,
                        Credit = 0,
                        Credit1 = 0,
                        Credit2 = 0,
                        Credit3 = 0,
                        Credit4 = 0,
                        Credit5 = 0,
                        Credit6 = 0,
                        Credit7 = 0,
                        Credit8 = 0,
                        Credit9 = 0,
                        Credit10 = 0,
                        LastActivity = DateTime.Now,
                        TopicCount = 0,
                        PostCount = 0,
                        PostDigestCount = 0,
                        Medal = "",
                        OnlineTime = 0,
                        OnlineUpdateTime = DateTime.Now,
                        Verify = 0
                    };

                    bbsdal.Jokul_Forum_User.Add(uuser);
                    bbsdal.SaveChanges();

                    Models.Jokul_Forum_UserExtended userex = new Models.Jokul_Forum_UserExtended
                    {
                        UserId = uuser.Id,
                        QQ = "",
                        MSN = "",
                        Gender = 0,
                        Birthday = "",
                        Bio = "",
                        Address = "",
                        Site = "",
                        Signature = "",
                        Nickname = ""
                    };
                    bbsdal.Jokul_Forum_UserExtended.Add(userex);
                    bbsdal.SaveChanges();
                    return 1;
                }
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        /// <summary>
        /// 发帖
        /// </summary>
        /// <param name="BoardId"></param>
        /// <param name="Title"></param>
        /// <param name="PostUserId"></param>
        /// <param name="PostUsername"></param>
        /// <param name="message"></param>
        /// <param name="userclientip"></param>
        /// <returns></returns>
        public int BBSTopic(int BoardId, string Title, string PostUsername, string usepwd, string message, string userclientip)
        {
            try
            {
                Models.Jokul_Forum_User users = bbsdal.Jokul_Forum_User.FirstOrDefault(item => item.Username == PostUsername);

                if (users != null)
                {
                    Models.Jokul_Forum_Topic topic = new Models.Jokul_Forum_Topic
                    {
                        BoardId = BoardId,
                        TopicTypeId = 0,
                        Title = Title,
                        ViewCount = 1,
                        ReplayCount = 0,
                        TodayReplayCount = 0,
                        Attachment = 0,
                        TagCount = 0,
                        PostUserId = users.Id,
                        PostUsername = users.Username,
                        PostDatetime = DateTime.Now,
                        LastPostId = bbsdal.Jokul_Forum_PostId.Max(item => item.Id) + 1,
                        LastPostDatetime = DateTime.Now,
                        LastPostUserId = users.Id,
                        LastPostUsername = users.Username,
                        Digest = 0,
                        Top = 0,
                        Audit = 0,
                        Invisible = 0,
                        PostSubTable = 0,
                        HighLight = "",
                        Close = 0,
                        FormId = 0,
                        Ban = 0,
                        LastModId = 0,
                        Cover = 0,
                        Rate = 0
                    };
                    bbsdal.Jokul_Forum_Topic.Add(topic);
                    bbsdal.SaveChanges();

                    Models.Jokul_Forum_PostId postid = new Models.Jokul_Forum_PostId
                    {
                        Id = topic.LastPostId,
                        TopicId = topic.Id,
                    };
                    bbsdal.Jokul_Forum_PostId.Add(postid);
                    bbsdal.SaveChanges();

                    Models.Jokul_Forum_Post_1 post = new Models.Jokul_Forum_Post_1
                    {
                        Id = postid.Id,
                        BoardId = topic.BoardId,
                        TopicId = topic.Id,
                        PostUserId = topic.PostUserId,
                        PostUsername = topic.PostUsername,
                        PostUserIp = userclientip,
                        PostDateTime = DateTime.Now,
                        Title = topic.Title,
                        Message = message,
                        HTML = 0,
                        Smile = 1,
                        UBB = 1,
                        Attachment = 0,
                        Signature = 1,
                        Url = 1,
                        Audit = 0,
                        First = 1,
                        Invisible = 0,
                        Ban = 0,
                        Grade = 0,
                        Hide = 0,
                        UpdateUserId = 0,
                        UpdateUsername = "",
                        UpdateTime = DateTime.Now
                    };
                    bbsdal.Jokul_Forum_Post_1.Add(post);
                    bbsdal.SaveChanges();
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception e)
            {
                return 0;
            }
        }


        /// <summary>
        /// 回帖
        /// </summary>
        /// <param name="topicid"></param>
        /// <param name="postuserid"></param>
        /// <param name="userclientip"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public int BBSPost(int topicid, string Username, string userclientip, string usepwd, string message)
        {
            try
            {
                Models.Jokul_Forum_User user = bbsdal.Jokul_Forum_User.FirstOrDefault(item => item.Username == Username);

                if (user == null)
                {
                    BBSUser(Username, FormsAuthentication.HashPasswordForStoringInConfigFile(usepwd, "MD5"), userclientip);
                    user = bbsdal.Jokul_Forum_User.FirstOrDefault(item => item.Username == Username);
                }

                Models.Jokul_Forum_Topic topic = bbsdal.Jokul_Forum_Topic.FirstOrDefault(item => item.Id == topicid);

                topic.LastPostId = bbsdal.Jokul_Forum_PostId.Max(item => item.Id) + 1;
                topic.LastPostDatetime = DateTime.Now;
                topic.LastPostUserId = user.Id;
                topic.LastPostUsername = user.Username;
                bbsdal.SaveChanges();

                Models.Jokul_Forum_PostId postid = new Models.Jokul_Forum_PostId
                {
                    Id = topic.LastPostId,
                    TopicId = topic.Id
                };

                bbsdal.Jokul_Forum_PostId.Add(postid);
                bbsdal.SaveChanges();

                Models.Jokul_Forum_Post_1 post_1 = new Models.Jokul_Forum_Post_1
                {
                    Id = postid.Id,
                    BoardId = topic.BoardId,
                    TopicId = topic.Id,
                    PostUserId = topic.LastPostUserId,
                    PostUsername = topic.LastPostUsername,
                    PostUserIp = userclientip,
                    PostDateTime = DateTime.Now,
                    Title = "回复：" + topic.Title,
                    Message = message,
                    HTML = 0,
                    Smile = 1,
                    UBB = 1,
                    Attachment = 1,
                    Signature = 1,
                    Url = 1,
                    Audit = 0,
                    First = 0,
                    Invisible = 0,
                    Ban = 0,
                    Grade = 0,
                    Hide = 0,
                    UpdateUserId = 0,
                    UpdateUsername = "",
                    UpdateTime = DateTime.Now
                };

                bbsdal.Jokul_Forum_Post_1.Add(post_1);
                bbsdal.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        /// <summary>
        /// 公告列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<gonggao> gonggao(int pageindex, int number)
        {
            List<gonggao> peblish = new List<gonggao>();
            List<Models.tbPeblish> pe = new List<Models.tbPeblish>();

            pe = (from p in dal.tbPeblish
                  from a in dal.tbApplyBill
                  where p.PeblishType == "2" &&
                  a.ObjectID == p.PeblishID &&
                  a.StateType == 2 && 
                  a.ApplyType== "Peblish"
                  select p).OrderByDescending(item => item.CreateTime).ToList();
            foreach (var i in pe)
            {
                if (i.FileIDs == null)
                {
                    continue;
                }
                peblish.Add(new gonggao
                {
                    pp = i,
                    fs = files(i.FileIDs)
                });
            }

            return Funs.fenye<gonggao>(peblish, pageindex, number);
        }


        public gonggao gonggaos(string id)
        {
            gonggao g = new gonggao();
            g.pp = dal.tbPeblish.FirstOrDefault(item => item.PeblishID == id);
            g.fs = files(g.pp.FileIDs);
            return g;
        }


        /// <summary>
        /// 附件
        /// </summary>
        /// <param name="filesid"></param>
        /// <returns></returns>
        public List<Models.tbFiles> files(string filesid)
        {
            List<Models.tbFiles> filess = new List<Models.tbFiles>();
            string[] id = filesid.Split(',');
            if (id == null)
            {
                return null;
            }
            else
            {
                foreach (string ii in id)
                {
                    int fileid = int.Parse(ii);
                    filess.Add(dal.tbFiles.FirstOrDefault(item => item.FileID == fileid));
                }
                return filess;
            }

        }

        /// <summary>
        /// 最新回复内容
        /// </summary>
        /// <returns></returns>
        public string zuixin()
        {
            string a = bbsdal.Jokul_Forum_Post_1.Where(item => item.First != 1).OrderByDescending(item => item.PostDateTime).FirstOrDefault().Message;
            return a;
        }

        /// <summary>
        /// 返回所有审核数量
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="number"></param>
        /// <param name="str"></param>
        /// <param name="useid"></param>
        /// <returns></returns>
        public int MaxShenhe(string useid)
        {
            List<ShenHe> shenhe = new List<ShenHe>();
            shenhe.AddRange(auth.BuniessList("4", 0, 1000000000, "", useid));
            shenhe.AddRange(auth.BuniessList("5", 0, 1000000000, "", useid));
            shenhe.AddRange(auth.RepairList(1, 0, 1000000000, "", useid));
            shenhe.AddRange(auth.RepairList(2, 0, 1000000000, "", useid));
            return shenhe.Count();
        }


    }



    public class PostList
    {
        public Models.Jokul_Forum_Post_1 Post1 { get; set; }
        public List<Models.Jokul_Forum_Post_1> Posts { get; set; }
    }

    public class gonggao
    {
        public Models.tbPeblish pp { get; set; }
        public List<Models.tbFiles> fs { get; set; }
    }
}