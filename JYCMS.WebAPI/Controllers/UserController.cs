using JYCMS.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using JYCMS.WebAPI.DataSQLHelper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using MySqlHelper = JYCMS.WebAPI.DataSQLHelper.MySqlHelper;
using YYCMS.Model.Model;

namespace JYCMS.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 根据用户名id 返回一个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetID(int id)
        {
            int code = 1; int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();

            base.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");//允许跨域
            //throw new Exception("1234567");
            string idParam = base.HttpContext.Request.Query["id"];
            string sql = string.Format(@"select * from  User where UserID={0}",idParam);
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
            return strJson;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="json">用户名，密码</param>
        /// <returns></returns>
        [HttpPost]
        public string Login(string json)
        {
            int code = 1; int status = 200; string message = "成功";
            ResponseInfo responseInfo = new Models.ResponseInfo();
            Data data = new Data();


            if (string.IsNullOrEmpty(json))
            {
                code = 1; status = 200; message = "传递参数不能为空";
                // data.data = "{\"Message\":\"传递参数不能为空！\"}";
                data.data = message;
               
            }
            else
            {
                UserInfo info = JsonConvert.DeserializeObject<UserInfo>(json);
                string sql = string.Format(@"select * from User where UserName='{0}' and  Password='{1}'"); 
                MySqlParameter[] sqlparams={ new MySqlParameter("UserName", info.UserName), new MySqlParameter("Password", info.Password) };
                DataTable dt = MySqlHelper.GetDataSet(CommandType.Text, sql, sqlparams).Tables[0];
                if (dt.Columns.Count > 0)
                {
                    //登录成功！
                    code = 1; status = 200; message = "登录成功！";
                }
                else
                { 
                    code = 0; status = 200; message = "登录失败！";
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
