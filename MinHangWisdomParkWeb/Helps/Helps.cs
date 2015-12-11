using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace MinHangWisdomParkWeb
{
    public class Helps
    {



        static Models.ajIIPdbEntities1 dal = new Models.ajIIPdbEntities1();

        /// <summary>
        /// 菜单显示
        /// </summary>
        /// <returns></returns>
        public static List<Controllers.Menus> MenuList()
        {
            using (Models.ajIIPdbEntities1 dal = new Models.ajIIPdbEntities1())
            {
                List<Controllers.Menus> menus = new List<Controllers.Menus>();

                foreach (var ii in dal.Functions.Where(n => n.MasterMenu == true))
                {
                    menus.Add(new Controllers.Menus
                    {
                        FunctionID = ii.FunctionID,
                        FunctionName = ii.FunctionName,
                        Superior = ii.Superior,
                        MasterMenu = ii.MasterMenu,
                        Type = ii.Type,
                        FunctionList = dal.Functions.Where(m => m.Superior == ii.FunctionID).ToList()
                    });
                }
                return menus;
            }
        }


        /// <summary>
        /// 角色关联
        /// </summary>
        /// <returns></returns>
        public static List<Controllers.JurisdictionController.ActorsLists> ActorPower(int? ActorId)
        {

            List<Controllers.JurisdictionController.ActorsLists> a = new List<Controllers.JurisdictionController.ActorsLists>();
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
                    a.Add(new Controllers.JurisdictionController.ActorsLists
                    {
                        FunctionID = i.FunctionID,
                        FunctionName = i.FunctionName,
                        Superior = i.Superior,
                        MasterMenu = i.MasterMenu,
                        Type = i.Type,
                        ischecked = ftype.FirstOrDefault(item => item.FunctionID == i.FunctionID) != null,
                        isphone = i.IsPhone,
                        List = (from mm in dal.Functions
                                where mm.Superior == i.FunctionID
                                select new Controllers.JurisdictionController.Actors
                                {
                                    FunctionID = mm.FunctionID,
                                    FunctionName = mm.FunctionName,
                                    Superior = mm.Superior,
                                    MasterMenu = mm.MasterMenu,
                                    Type = mm.Type,
                                    isphone = mm.IsPhone
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

            }

            return a;
        }

        /// <summary>
        /// 用户详细信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static Models.tbPeople MyPeopleSelect(string userid)
        {
            try
            {
                var people = (from o in dal.tbOwnerRfid
                              from u in dal.mtUser
                              from o1 in dal.tbOwnerRfid
                              from p in dal.tbPeople
                              where u.UserId == userid &&
                              o.ObjectID == u.UserId &&
                              o.UseType == 5 &&
                              o.RfidAutoId == o1.RfidAutoId &&
                              o1.ObjectID == p.PeopleID &&
                              p.Version == dal.tbPeople.Where(item => item.PeopleID == p.PeopleID).Max(item => item.Version)
                              select p).FirstOrDefault();
                return people;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        /// <summary>
        /// 提取通用代码list
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static List<Models.mtUniversalCode> UniversalCodeList(string Type)
        {
            return dal.mtUniversalCode.Where(m => m.UniversalType == Type).ToList();
        }



        /// <summary>
        /// 根据Type的数值查找审核流id
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static int SearchConfirmIdInXmlByType(string Type)
        {
            try
            {
                XElement xml = XElement.Load(HttpContext.Current.Request.PhysicalApplicationPath + @"XML\All.xml");

                var query = from p in xml.Elements("confirm")
                            where (string)p.Attribute("Type") == Type
                            select new
                            {
                                id = p.Element("Cid").Value
                            };

                int a = int.Parse(query.Max(item => item.id));
                return a;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据Type的数值查找功能对应ID
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static int SearchFuncNoInXmlByType(string Type)
        {
            try
            {
                XElement xml = XElement.Load(HttpContext.Current.Request.PhysicalApplicationPath + @"XML\All.xml");

                var query = from p in xml.Elements("function")
                            where (string)p.Attribute("Type") == Type
                            select new
                            {
                                id = p.Element("FuncNo").Value
                            };

                int a = int.Parse(query.Max(item => item.id));
                return a;
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        /// <summary>
        /// 公告发布类型枚举
        /// </summary>
        public enum Peblish
        {
            新闻 = 1,
            公告 = 2,
            广告 = 3,
            政策 = 4,
            企业 = 5,
        }

        /// <summary>
        /// 提取审核流及对应审核意见
        /// </summary>
        /// <param name="applyid"></param>
        /// <returns></returns>
        public static List<Models.ConfirmeMenos> SearchConfirmeFlowByApplyid(int applyid)
        {
            var a = (from cs in dal.tbConfirmState
                     join cf in (dal.mtConfirmFlow.Where(item => item.ConfirmerAutoID == (dal.tbConfirmState.FirstOrDefault(aa => aa.ApplyID == applyid).ConfirmerAutoID)))
                      on cs.ConfirmerAutoID equals cf.ConfirmerAutoID
                     from u in dal.mtUser
                     where cs.ApplyID == applyid && cf.UserId == u.UserId
                     select new Models.ConfirmeMenos
                     {
                         UserID = (cf.UserId == cs.ConfirmerID) ? (cf.UserId) : (""),
                         UserName = u.UserName,
                         ConfirmeMeno = (cf.UserId == cs.ConfirmerID) ? (cs.ConfirmeMemo != null ? (cs.ConfirmeMemo) : ("无")) : ("无"),
                         ConfirmeID = cs.ConfirmerID,
                         State = cs.ConfirmerID.CompareTo(cf.UserId) > 0 ? (1) : (cs.ConfirmerID.CompareTo(cf.UserId) == 0 ? (cs.CurrConfirmLevel == 0 ? (4) : (2)) : (3))
                     }).ToList();

            return a;
        }


    }

    /// <summary>
    /// 文件操作
    /// </summary>
    public class FileHelper
    {
        #region 检测指定目录是否存在
        /// <summary>   
        /// 检测指定目录是否存在   
        /// </summary>   
        /// <param name="directoryPath">目录的绝对路径</param>           
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        #endregion

        #region 检测指定文件是否存在
        /// <summary>   
        /// 检测指定文件是否存在,如果存在则返回true。   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region 检测指定目录是否为空
        /// <summary>   
        /// 检测指定目录是否为空   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>           
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件   
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹   
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {

                return true;
            }
        }
        #endregion

        #region 检测指定目录中是否存在指定的文件
        /// <summary>   
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>   
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。   
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>           
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表   
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判断指定文件是否存在   
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>   
        /// 检测指定目录中是否存在指定的文件   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>   
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。   
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>    
        /// <param name="isSearchChild">是否搜索子目录</param>   
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表   
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //判断指定文件是否存在   
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion

        #region 创建一个目录
        /// <summary>   
        /// 创建一个目录   
        /// </summary>   
        /// <param name="directoryPath">目录的绝对路径</param>   
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录   
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion

        #region 创建一个文件
        /// <summary>   
        /// 创建一个文件。   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        public static void CreateFile(string filePath)
        {
            try
            {
                //如果文件不存在则创建该文件   
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象   
                    FileInfo file = new FileInfo(filePath);

                    //创建文件   
                    FileStream fs = file.Create();

                    //关闭文件流   
                    fs.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>   
        /// 创建一个文件,并将字节流写入文件。   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        /// <param name="buffer">二进制流数据</param>   
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果文件不存在则创建该文件   
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象   
                    FileInfo file = new FileInfo(filePath);

                    //创建文件   
                    FileStream fs = file.Create();

                    //写入二进制流   
                    fs.Write(buffer, 0, buffer.Length);

                    //关闭文件流   
                    fs.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取文本文件的行数
        /// <summary>   
        /// 获取文本文件的行数   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static int GetLineCount(string filePath)
        {
            //将文本文件的各行读到一个字符串数组中   
            string[] rows = File.ReadAllLines(filePath);

            //返回行数   
            return rows.Length;
        }
        #endregion

        #region 获取一个文件的长度
        /// <summary>   
        /// 获取一个文件的长度,单位为Byte   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static int GetFileSize(string filePath)
        {
            //创建一个文件对象   
            FileInfo fi = new FileInfo(filePath);

            //获取文件的大小   
            return (int)fi.Length;
        }

        ///// <summary>   
        ///// 获取一个文件的长度,单位为KB   
        ///// </summary>   
        ///// <param name="filePath">文件的路径</param>           
        //public static double GetFileSizeByKB( string filePath )   
        //{   
        //    //创建一个文件对象   
        //    FileInfo fi = new FileInfo( filePath );               

        //    //获取文件的大小   
        //    return ConvertHelper.ToDouble( ConvertHelper.ToDouble( fi.Length ) / 1024 , 1 );   
        //}   

        ///// <summary>   
        ///// 获取一个文件的长度,单位为MB   
        ///// </summary>   
        ///// <param name="filePath">文件的路径</param>           
        //public static double GetFileSizeByMB( string filePath )   
        //{   
        //    //创建一个文件对象   
        //    FileInfo fi = new FileInfo( filePath );   

        //    //获取文件的大小   
        //    return ConvertHelper.ToDouble( ConvertHelper.ToDouble( fi.Length ) / 1024 / 1024 , 1 );   
        //}   
        #endregion

        #region 获取指定目录中的文件列表
        /// <summary>   
        /// 获取指定目录中所有文件列表   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>           
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常   
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表   
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>   
        /// 获取指定目录及子目录中所有文件列表   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>   
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。   
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>   
        /// <param name="isSearchChild">是否搜索子目录</param>   
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常   
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取指定目录中的子目录列表
        /// <summary>   
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>           
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>   
        /// 获取指定目录及子目录中所有子目录列表   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>   
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。   
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>   
        /// <param name="isSearchChild">是否搜索子目录</param>   
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 向文本文件写入内容
        /// <summary>   
        /// 向文本文件中写入内容   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        /// <param name="content">写入的内容</param>           
        public static void WriteText(string filePath, string content)
        {
            //向文件写入内容   
            File.WriteAllText(filePath, content);
        }
        #endregion

        #region 向文本文件的尾部追加内容
        /// <summary>   
        /// 向文本文件的尾部追加内容   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        /// <param name="content">写入的内容</param>   
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }
        #endregion

        #region 将现有文件的内容复制到新文件中
        /// <summary>   
        /// 将源文件的内容复制到目标文件中   
        /// </summary>   
        /// <param name="sourceFilePath">源文件的绝对路径</param>   
        /// <param name="destFilePath">目标文件的绝对路径</param>   
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }
        #endregion

        #region 将文件移动到指定目录
        /// <summary>   
        /// 将文件移动到指定目录   
        /// </summary>   
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>   
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>   
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            //获取源文件的名称   
            string sourceFileName = GetFileName(sourceFilePath);

            if (IsExistDirectory(descDirectoryPath))
            {
                //如果目标中存在同名文件,则删除   
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                //将文件移动到指定目录   
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }
        #endregion

        #region 将流读取到缓冲区中
        /// <summary>   
        /// 将流读取到缓冲区中   
        /// </summary>   
        /// <param name="stream">原始流</param>   
        //public static byte[] StreamToBytes( Stream stream )   
        //{   
        //    try   
        //    {   
        //        //创建缓冲区   
        //        byte[] buffer = new byte[stream.Length];   

        //        //读取流   
        //        stream.Read( buffer, 0, ConvertHelper.ToInt32( stream.Length ) );   

        //        //返回流   
        //        return buffer;   
        //    }   
        //    catch ( Exception ex )   
        //    {   
        //        LogHelper.WriteTraceLog( TraceLogLevel.Error, ex.Message );   
        //        throw ex;   
        //    }   
        //    finally   
        //    {   
        //        //关闭流   
        //        stream.Close();   
        //    }   
        //}   
        #endregion

        #region 将文件读取到缓冲区中
        /// <summary>   
        /// 将文件读取到缓冲区中   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        public static byte[] FileToBytes(string filePath)
        {
            //获取文件的大小    
            int fileSize = GetFileSize(filePath);

            //创建一个临时缓冲区   
            byte[] buffer = new byte[fileSize];

            //创建一个文件流   
            FileInfo fi = new FileInfo(filePath);
            FileStream fs = fi.Open(FileMode.Open);

            try
            {
                //将文件流读入缓冲区   
                fs.Read(buffer, 0, fileSize);

                return buffer;
            }
            catch (IOException ex)
            {

                throw ex;
            }
            finally
            {
                //关闭文件流   
                fs.Close();
            }
        }
        #endregion

        #region 将文件读取到字符串中
        /// <summary>   
        /// 将文件读取到字符串中   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        //public static string FileToString( string filePath )   
        //{   
        //    return FileToString( filePath, BaseInfo.DefaultEncoding );   
        //}   
        ///// <summary>   
        ///// 将文件读取到字符串中   
        ///// </summary>   
        ///// <param name="filePath">文件的绝对路径</param>   
        ///// <param name="encoding">字符编码</param>   
        //public static string FileToString( string filePath,Encoding encoding )   
        //{   
        //    //创建流读取器   
        //    StreamReader reader = new StreamReader( filePath, encoding );   
        //    try   
        //    {   
        //        //读取流   
        //        return reader.ReadToEnd();   
        //    }   
        //    catch ( Exception ex )   
        //    {   
        //        LogHelper.WriteTraceLog( TraceLogLevel.Error, ex.Message );   
        //        throw ex;   
        //    }   
        //    finally   
        //    {   
        //        //关闭流读取器   
        //        reader.Close();   
        //    }   
        //}   
        #endregion

        #region 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// <summary>   
        /// 从文件的绝对路径中获取文件名( 包含扩展名 )   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static string GetFileName(string filePath)
        {
            //获取文件的名称   
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }
        #endregion

        #region 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// <summary>   
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static string GetFileNameNoExtension(string filePath)
        {
            //获取文件的名称   
            FileInfo fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }
        #endregion

        #region 从文件的绝对路径中获取扩展名
        /// <summary>   
        /// 从文件的绝对路径中获取扩展名   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>           
        public static string GetExtension(string filePath)
        {
            //获取文件的名称   
            FileInfo fi = new FileInfo(filePath);
            return fi.Extension;
        }
        #endregion

        #region 清空指定目录
        /// <summary>   
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>   
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件   
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }

                //删除目录中所有的子目录   
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDirectory(directoryNames[i]);
                }
            }
        }
        #endregion

        #region 清空文件内容
        /// <summary>   
        /// 清空文件内容   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        public static void ClearFile(string filePath)
        {
            //删除文件   
            File.Delete(filePath);

            //重新创建该文件   
            CreateFile(filePath);
        }
        #endregion

        #region 删除指定文件
        /// <summary>  
        /// 删除指定文件   
        /// </summary>   
        /// <param name="filePath">文件的绝对路径</param>   
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }
        #endregion

        #region 删除指定目录
        /// <summary>   
        /// 删除指定目录及其所有子目录   
        /// </summary>   
        /// <param name="directoryPath">指定目录的绝对路径</param>   
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
        #endregion
    }


    /// <summary>
    /// 工具
    /// </summary>
    public class Funs
    {

        static Models.ajIIPdbEntities1 dal = new Models.ajIIPdbEntities1();

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="lists">数据</param>
        /// <param name="i">页数</param>
        /// <returns></returns>
        public static List<T> fenye<T>(List<T> lists, int CurrentPageIndex, int PageSize)
        {
            int count = 0;
            if (lists == null || lists.Count == 0)
            {
                return lists;
            }
            count = lists.Count;
            int startIndex = CurrentPageIndex * PageSize;
            if (startIndex + PageSize > count)
            {
                PageSize = count - startIndex;
            }
            return lists.GetRange(startIndex, PageSize);

        }


    }

    public class ConfirmModel
    {
        public string Type { get; set; }
        public int Cid { get; set; }
    }
}