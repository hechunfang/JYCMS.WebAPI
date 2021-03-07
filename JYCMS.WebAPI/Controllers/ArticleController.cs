
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YYCMS.Model.Model;
using JYCMS.WebAPI.DataSQLHelper;
using MySqlHelper = JYCMS.WebAPI.DataSQLHelper.MySqlHelper;
using Newtonsoft.Json;
using System.IO;
using JYCMS.WebAPI.Models;

namespace JYCMS.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        /// <summary>
        /// 根据文章ID获得文章实体 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]

        public string GetID(int id)
        {
            int code = 1;  int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();

            base.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");//允许跨域
            //throw new Exception("1234567");
            string idParam = base.HttpContext.Request.Query["id"];
            string sql = string.Format(@"select a.*, (select UserName from `User` where UserID = a.UserID) AS UserName,
   (SELECT ClassifyName FROM Classify WHERE ClassifyID = a.ClassifyID)as ClassifyName,
(select SiteName from Site where SiteID = a.SiteID) as SiteName
 from Article as a where ArticleID = {0}   ", idParam);
            MySqlParameter param = new MySqlParameter("ArticleID", MySqlDbType.Int32);
            DataTable dt = MySqlHelper.GetDataSet(CommandType.Text, sql, param).Tables[0];
            if (dt == null)
            {
                code = 1; message = "数据集为空";
                data.data = message;
            }
            else
            {
                data.data = JsonHelper.DataTableToJson(dt);
            }
         
            data.code = code; 

            responseInfo.status = status;
            responseInfo.data = data;
            responseInfo.message = message;

            string strJson = JsonConvert.SerializeObject(responseInfo);
            // responseInfo.data = List<ExtendMethod.ToDataList<ArticleInfo>(dt)>;
            return strJson;
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="jsons">实体封装成json格式</param>
        /// <returns></returns>
        [HttpPost]
        public string AddArticle(string jsons)
        {
            int code = 1; int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();
         
        
            if (string.IsNullOrEmpty(jsons))
            {
                code = 1; status = 200;  message = "传递参数不能为空";
               // data.data = "{\"Message\":\"传递参数不能为空！\"}";
                data.data = message;
                data.code = code; 
                responseInfo.status = status;
                responseInfo.data = data;
                responseInfo.message = message;
                // return "{\"status\":\"-1\",\"Message\":\"传递参数不能为空！\"}";
                return   JsonConvert.SerializeObject(responseInfo);
            }
            try
            {
                ArticleInfo info = JsonConvert.DeserializeObject<ArticleInfo>(jsons);
                string sql = string.Format(@"insert into Article(Title,Summary,Contents,Images,Keywords,Description,ClassifyID,SiteID,UserID,Status,Label,CreateTime)
VALUES ('{0}','{1}','{2}','{3}'.'{4}','{5}',{6},{7},{8},{9},'{10}',now())", info.Title, info.Summary, info.Contents, info.Images, info.Keywords, info.Description, info.ClassifyID, info.SiteID, info.ClassifyID, info.SiteID, 1, info.Label);
                MySqlParameter[] sqlparams ={ new MySqlParameter("Title", info.Title),new MySqlParameter("Summary",info.Summary),new MySqlParameter("Contents",info.Contents),
                                         new MySqlParameter("Images",info.Images),new MySqlParameter("Keywords",info.Keywords),new MySqlParameter("Description",info.Description),
                                         new MySqlParameter("ClassifyID",info.ClassifyID),new MySqlParameter("SiteID",info.SiteID),new MySqlParameter("UserID",info.UserID),
                                         new MySqlParameter("Status",info.Status),new MySqlParameter("Label",info.Label),new MySqlParameter("CreateTime",info.CreateTime)
             };
             
                int i = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql, sqlparams);
                if (i > 0)
                {
                    message = "成功";
                    //return "{\"status\":\"1\",\"Message\":\"添加成功！\"}";
                }
                else
                {
                    code = 0; status = 200; message = "添加失败";
                   // return "{\"status\":\"0\",\"Message\":\"添加失败！\"}";
                }
            }
            catch (Exception ex)
            {
                code = 0; status = 500; message = "服务器内部错误:" + ex.Message;
                //return "{\"status\":\"-2\",\"Message\":\"异常:\"" + ex.Message + "}";
                //throw;
            }

            data.code = code;
            data.data = message;
            responseInfo.status = status;
            responseInfo.data = data;
            responseInfo.message = message;

            return JsonConvert.SerializeObject(responseInfo);
        }

        /// <summary>
        /// 更改文章状态【1：已发布，0：存草稿，-1：已删除，2：已修改:】
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public string UpdateArticleStatus(int id, int status)
        {
            int code = 1; int statusStr = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();
            if (string.IsNullOrEmpty(id.ToString()))
            {
                code = 1; statusStr = 200; message = "传递参数不能为空";
                data.data = "{\"Message\":\"传递参数不能为空！\"}";
                data.code = code;
                responseInfo.status = statusStr;
                responseInfo.data = data;
                responseInfo.message = message;
                // return "{\"status\":\"-1\",\"Message\":\"传递参数不能为空！\"}";
                return JsonConvert.SerializeObject(responseInfo);
                //return "{\"status\":\"-1\",\"Message\":\"传递的参数不能为空！\"}";
            }
            
            base.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");//允许跨域 
            string idParam = base.HttpContext.Request.Query["id"];
            string sql = string.Format(@"update Article set Status={1} where ArticleID={0}", id, status);
            MySqlParameter[] sqlParameters = { new MySqlParameter("Status", status), new MySqlParameter("ArticleID", id) };
            int i = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql, sqlParameters);

            if (i > 0)
            {
                code = 1; statusStr = 200; message = "更改文章状态成功";
              //  return "{\"status\":\"1\",\"Message\":\"更改文章状态成功！\"}";
            }
            else
            {
                code = 0; statusStr = 200; message = "更改文章状态失败";
               // return "{\"status\":\"0\",\"Message\":\"更改文章状态失败！\"}";
            }
            data.code = code;
            data.data = message;
            responseInfo.status = statusStr;
            responseInfo.data = data;
            responseInfo.message = message;
            return JsonConvert.SerializeObject(responseInfo);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">文章ID</param> 
        /// <returns></returns>
        [HttpGet]
        public string Delete(int id)
        {
            int code = 1; int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();
            if (string.IsNullOrEmpty(id.ToString()))
            {
                code = 1; status = 200; message = "传递参数不能为空";
               // data.data = "{\"Message\":\"传递参数不能为空！\"}";
                data.data = message;
                data.code = code;
                responseInfo.status = status;
                responseInfo.data = data;
                responseInfo.message = message; 
                return JsonConvert.SerializeObject(responseInfo);
                //return "{\"status\":\"-1\",\"Message\":\"传递的参数不能为空！\"}";
            }
            base.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");//允许跨域 
            string idParam = base.HttpContext.Request.Query["id"];
            string sql = string.Format(@"delete Article  where ArticleID={0}", id);
            MySqlParameter[] sqlParameters = { new MySqlParameter("ArticleID", id) };
            int i = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql, sqlParameters);

            if (i > 0)
            {
                code = 1; status = 200; message = "删除文章成功";
                //return "{\"status\":\"1\",\"Message\":\"删除文章成功！\"}";
            }
            else
            {
                code = 0; status = 200; message = "删除文章失败";
               // return "{\"status\":\"0\",\"Message\":\"删除文章失败！\"}";
            }
            data.code = code;
            data.data = message;
            responseInfo.status = status;
            responseInfo.data = data;
            responseInfo.message = message;
            return JsonConvert.SerializeObject(responseInfo);

        }


        public string UploadImages(List<IFormFile> files)
        {
            int code = 1; int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();
            bool suffix = false; bool format = false; bool flen = false;
           // string message = "上传成功";
            if (files.Count < 1)
            {

                code = 0; status = 200; message = "上传图片不能为空";
                data.code = code;
                responseInfo.status = status; 
                data.data = "{\"Message\":\"上传图片不能为空！\"}"; 
                responseInfo.message = message;
                return JsonConvert.SerializeObject(responseInfo);
               // return "{\"status\":\"-1\",\"Message\":\"上传图片不能为空！\"}";
            }
            //返回的文件地址
            List<string> filenames = new List<string>();
            var now = DateTime.Now;
            //文件存储路径
            var filePath = string.Format("/Uploads/{0}/{1}/", now.ToString("yyyy"), now.ToString("yyyyMM"));
            //获取当前web目录
            var webRootPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(webRootPath + filePath))
            {
                Directory.CreateDirectory(webRootPath + filePath);
            }
            try
            {
                foreach (var item in files)
                {
                    if (item != null)
                    {
                        #region 图片判断
                        //文件后缀
                        var fileExtension = Path.GetExtension(item.FileName);
                        //判断后缀是否是图片
                        const string fileFilt = ".gif|.jpg|.jpeg|.png";
                        if (fileExtension == null)
                        {
                            //没有后缀  上传的文件没有后缀
                            suffix = true;
                            code = 1;
                            message = "上传的文件没有后缀";
                            break;
                        }
                        if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                        {
                            //请上传jpg、png、gif格式的图片
                            format = true;
                            code = 1;
                            message = "请上传jpg、png、gif格式的图片";
                            break;
                        }
                        long length = item.Length;
                        if (length > 1024 * 1024 * 2)
                        {
                            flen = true;
                            code = 1;
                            message = "上传的文件不能大于2M";
                            break;
                        }
                        #endregion

                        var strDateTime = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                        var strRan = Convert.ToString(new Random().Next(100, 999));
                        var saveName = strDateTime + strRan + fileExtension;

                        //插入图片
                        using (FileStream fs = System.IO.File.Create(webRootPath + filePath + saveName))
                        {
                            item.CopyToAsync(fs);
                            fs.Flush();
                        }
                        filenames.Add(filePath + saveName);
                    }
                }

              status = 200; message = "传递参数不能为空";
              
                //return Newtonsoft.Json.JsonConvert.SerializeObject(filenames);
            }
            catch (Exception ex)
            {
                code = 0; status = 500; message = "服务器内部错误："+ex.Message;
                //throw;
            }
            data.code = code;
            data.data = Newtonsoft.Json.JsonConvert.SerializeObject(filenames);
            responseInfo.status = status;
            responseInfo.data = data;
            responseInfo.message = message;
            return JsonConvert.SerializeObject(responseInfo);
        }
    }
}
