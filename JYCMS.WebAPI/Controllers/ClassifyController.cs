using JYCMS.WebAPI.DataSQLHelper;
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
            string sql = string.Format(@"select * from Classify ");
            DataTable dt = MySqlHelper.GetDataSetNoP(CommandType.Text, sql).Tables[0];
            return JsonHelper.DataTableToJson(dt);
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="jsons">传递过来的是json</param>
        /// <returns></returns>
        [HttpPost]
        public string Add(string jsons)
        {
            if (string.IsNullOrEmpty(jsons))
            {
                return "{\"status\":\"-1\",\"Message\":\"传递的参数不能为空！\"}";
            }
            try
            {
                ClassifyInfo info = JsonConvert.DeserializeObject<ClassifyInfo>(jsons);
                string sql = string.Format(@"insert into Classify(ClassifyName,CreateTime)values('{0}',now())",info.ClassifyName);
                MySqlParameter  parameters =   new MySqlParameter("ClassifyName", info.ClassifyName) ;
                int i = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
                if (i > 0)
                {
                    return "{\"status\":\"1\",\"Message\":\"添加分类成功！\"}";
                }
                else
                {
                    return "{\"status\":\"0\",\"Message\":\"添加分类失败！\"}";
                }
            }
            catch (Exception ex)
            {
                return "{\"status\":\"-2\",\"Message\":\"异常:\""+ex.Message+"}";
                //throw;
            }
        }

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="jsons"></param>
        /// <returns></returns>
        [HttpPost]
        public string Edit(string jsons)
        {
            if (string.IsNullOrEmpty(jsons))
            {
                return "{\"status\":\"-1\",\"Message\":\"传递的参数不能为空！\"}";
            }
            try
            {
                ClassifyInfo info = JsonConvert.DeserializeObject<ClassifyInfo>(jsons);
                string sql = string.Format(@"update Classify set ClassifyName='{1}' where ClassifyID={0}", info.ClassifyID, info.ClassifyName);
                MySqlParameter[] mySqlParameters = { new MySqlParameter("ClassifyID", info.ClassifyID), new MySqlParameter("ClassifyName", info.ClassifyName) };
                int i = MySqlHelper.ExecuteNonQuery(CommandType.Text, sql, mySqlParameters);
                if (i > 0)
                {
                    return "{\"status\":\"1\",\"Message\":\"编辑分类成功！\"}";
                }
                else
                {
                    return "{\"status\":\"0\",\"Message\":\"编辑分类失败！\"}";
                }
            }
            catch (Exception ex)
            {
                return "{\"status\":\"-2\",\"Message\":\"异常:\"" + ex.Message + "}";
                //throw;
            }
          
        }

    }
}
