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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClassifyController : ControllerBase
    {
        /// <summary>
        /// 获取所有的分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetAll()
        {
            int code = 1; int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();

            string sql = string.Format(@"select * from Classify ");
            DataTable dt = MySqlHelper.GetDataSetNoP(CommandType.Text, sql).Tables[0];
            if (dt == null)
            {
                code = 1; status = 200; message = "数据集合为空";
                data.data =message;
            }
            else
            {
                code = 1; status = 200; message = "成功";
                data.data = JsonHelper.DataTableToJson(dt);
            }
            data.code = code;
          
            responseInfo.status = status;
            responseInfo.data = data;
            responseInfo.message = message;
            return JsonConvert.SerializeObject(responseInfo);
           // return JsonHelper.DataTableToJson(dt);
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="jsons">传递过来的是json</param>
        /// <returns></returns>
        [HttpPost]
        public string Add(string jsons)
        {
            int code = 1; int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();
            if (string.IsNullOrEmpty(jsons))
            {
                code = 1; status = 200; message = "传递的参数不能为空";
                //return "{\"status\":\"-1\",\"Message\":\"传递的参数不能为空！\"}";
            }
            else
            {
                try
                {
                    ClassifyInfo info = JsonConvert.DeserializeObject<ClassifyInfo>(jsons);
                    string sql = string.Format(@"insert into Classify(ClassifyName,CreateTime)values('{0}',now())", info.ClassifyName);
                    MySqlParameter parameters = new MySqlParameter("ClassifyName", info.ClassifyName);
                    int i = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
                    if (i > 0)
                    {
                        code = 1; status = 200; message = "添加分类成功";
                        //  return "{\"status\":\"1\",\"Message\":\"添加分类成功！\"}";
                    }
                    else
                    {
                        code = 0; status = 200; message = "添加分类失败";
                       // return "{\"status\":\"0\",\"Message\":\"添加分类失败！\"}";
                    }
                }
                catch (Exception ex)
                {
                    code = 0; status = 500; message = "服务器内部错误："+ex.Message;
                    //return "{\"status\":\"-2\",\"Message\":\"异常:\"" + ex.Message + "}";
                    //throw;
                }
            }
            data.code = code;
            data.data = message;
            responseInfo.status = status;
            responseInfo.data = data;
            responseInfo.message = message;
            return JsonConvert.SerializeObject(responseInfo);
        }

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="jsons"></param>
        /// <returns></returns>
        [HttpPost]
        public string Edit(string jsons)
        {
            int code = 1; int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();

            if (string.IsNullOrEmpty(jsons))
            {
                message = "传递的参数不能为空";
                // return "{\"status\":\"-1\",\"Message\":\"传递的参数不能为空！\"}";
            }
            else
            {
                try
                {
                    ClassifyInfo info = JsonConvert.DeserializeObject<ClassifyInfo>(jsons);
                    string sql = string.Format(@"update Classify set ClassifyName='{1}' where ClassifyID={0}", info.ClassifyID, info.ClassifyName);
                    MySqlParameter[] mySqlParameters = { new MySqlParameter("ClassifyID", info.ClassifyID), new MySqlParameter("ClassifyName", info.ClassifyName) };
                    int i = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql, mySqlParameters);
                    if (i > 0)
                    {
                        code = 1; status = 200; message = "编辑分类成功";
                       // return "{\"status\":\"1\",\"Message\":\"编辑分类成功！\"}";
                    }
                    else
                    {
                        code = 0; status = 200; message = "编辑分类失败";
                       // return "{\"status\":\"0\",\"Message\":\"编辑分类失败！\"}";
                    }
                }
                catch (Exception ex)
                {
                    code = 0; status = 500; message = "服务器内部错误：" + ex.Message; ;
                    //return "{\"status\":\"-2\",\"Message\":\"异常:\"" + ex.Message + "}";
                    //throw;
                }
            }
            data.code = code;
            data.data = message;
            responseInfo.status = status;
            responseInfo.data = data;
            responseInfo.message = message;
            return JsonConvert.SerializeObject(responseInfo);

        }

    }
}
