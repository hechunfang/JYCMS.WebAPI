using JYCMS.WebAPI.DataSQLHelper;
using JYCMS.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using YYCMS.Model.Model;
using MySqlHelper = JYCMS.WebAPI.DataSQLHelper.MySqlHelper;

namespace JYCMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        [HttpGet]
        public string GetAll()
        {
            int code = 1; int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();
            string sql = string.Format(@"select * from  Site  "); 
            DataTable dt = MySqlHelper.GetDataSetNoP(CommandType.Text, sql).Tables[0];
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
            return strJson;
        }

        /// <summary>
        /// 添加站点
        /// </summary>
        /// <param name="jsons">站点名称</param>
        /// <returns></returns>
        [HttpGet]
        public string Add(string jsons)
        {
            int code = 1; int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();


            if (string.IsNullOrEmpty(jsons))
            {
                code = 1; status = 200; message = "传递参数不能为空";
                // data.data = "{\"Message\":\"传递参数不能为空！\"}";
                data.data = message;
                data.code = code;
                responseInfo.status = status;
                responseInfo.data = data;
                responseInfo.message = message;
                // return "{\"status\":\"-1\",\"Message\":\"传递参数不能为空！\"}";
                return JsonConvert.SerializeObject(responseInfo);
            }
            try
            {
                SiteInfo info = JsonConvert.DeserializeObject<SiteInfo>(jsons);
                string sql = string.Format(@"insert into Site(SiteName,CreateTime)
VALUES ('{0}',now())", info.SiteName, info.CreateTime);
                MySqlParameter[] sqlparams ={ new MySqlParameter("SiteName", info.SiteName),new MySqlParameter("CreateTime",info.CreateTime) 
             };

                int i = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql, sqlparams);
                if (i > 0)
                {
                    message = "成功";
                  //  return "{\"status\":\"1\",\"Message\":\"添加成功！\"}";
                }
                else
                {
                    code =0; status = 200; message = "添加失败";
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
        /// 更新站点
        /// </summary>
        /// <param name="jsons"></param>
        /// <returns></returns>
        [HttpPost]
        public string Update(string jsons)
        {
            int code = 1; int statusStr = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();
            if (string.IsNullOrEmpty(jsons.ToString()))
            {
                code = 1; statusStr = 200; message = "传递参数不能为空";
                data.data = "{\"Message\":\"传递参数不能为空！\"}";
                data.code = code;
                responseInfo.status = statusStr;
                responseInfo.data = data;
                responseInfo.message = message; 
                return JsonConvert.SerializeObject(responseInfo); 
            }
            SiteInfo info = JsonConvert.DeserializeObject<SiteInfo>(jsons);
         
            string sql = string.Format(@"update Site set SiteName='{1}' where SiteID={0}", info.SiteID, info.SiteName);
            MySqlParameter[] sqlParameters = { new MySqlParameter("SiteID", info.SiteID), new MySqlParameter("SiteName", info.SiteName) };
            int i = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql, sqlParameters);

            if (i > 0)
            {
                code = 1; statusStr = 200; message = "更改站点成功";
                //  return "{\"status\":\"1\",\"Message\":\"更改文章状态成功！\"}";
            }
            else
            {
                code = 0; statusStr = 200; message = "更改站点失败";
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
        /// 删除
        /// </summary>
        /// <param name="jsons"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete(string jsons)
        {
            int code = 1; int statusStr = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();
            if (string.IsNullOrEmpty(jsons.ToString()))
            {
                code = 1; statusStr = 200; message = "传递参数不能为空";
                data.data = "{\"Message\":\"传递参数不能为空！\"}";
                data.code = code;
                responseInfo.status = statusStr;
                responseInfo.data = data;
                responseInfo.message = message;
                return JsonConvert.SerializeObject(responseInfo);
            }
            SiteInfo info = JsonConvert.DeserializeObject<SiteInfo>(jsons);

            string sql = string.Format(@"delete  Site   where SiteID={0}", info.SiteID);
            MySqlParameter[] sqlParameters = { new MySqlParameter("SiteID", info.SiteID) };
            int i = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql, sqlParameters);

            if (i > 0)
            {
                code = 1; statusStr = 200; message = "删除站点成功";
                //  return "{\"status\":\"1\",\"Message\":\"更改文章状态成功！\"}";
            }
            else
            {
                code = 0; statusStr = 200; message = "删除站点失败";
                // return "{\"status\":\"0\",\"Message\":\"更改文章状态失败！\"}";
            }
            data.code = code;
            data.data = message;
            responseInfo.status = statusStr;
            responseInfo.data = data;
            responseInfo.message = message;
            return JsonConvert.SerializeObject(responseInfo);
        }
    }
}
