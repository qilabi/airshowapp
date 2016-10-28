 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Senparc.Weixin.MP.Sample.RyanWebForms
{
    public partial class _Default : Page
    {

        public string DllVersion { get; set; }

        const string Token = "TokenWeiXin";

        protected void Page_Load(object sender, EventArgs e)
        {
            //WriteLog("調用過:"+DateTime.Now.ToString());
            string echoStr = Request.QueryString["echoStr"];
            if (CheckSignature() && !string.IsNullOrEmpty(echoStr))
            {
                try
                {
                    WriteLog(string.Format("echoStr:{0};signature:{1};timestamp:{2};nonce:{3};msg_signature:{4}", echoStr,
                        Request.QueryString["signature"], Request.QueryString["timestamp"],
                        Request.QueryString["nonce"], Request.QueryString["msg_signature"]));
                }
                catch (Exception)
                {
                    throw;
                }
                Response.Write(echoStr);
                Response.End();
               
            }
            //var fileVersionInfo = FileVersionInfo.GetVersionInfo(Server.MapPath("~/bin/Senparc.Weixin.MP.dll"));
            //DllVersion = string.Format("{0}.{1}", fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart);
        }

        private void WriteLog(string msg)
        {
            var logPath = Server.MapPath(string.Format("~/App_Data/Open/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
             
            try
            {
           
                //记录ResponseMessage日志（可选）
                using (TextWriter tw = new StreamWriter(Path.Combine(logPath, string.Format("Response.txt"))))
                {
                    tw.WriteLine(msg);
                    tw.Flush();
                    tw.Close();
                }

                //return Content(messageHandler.ResponseMessageText);
            }
            catch (Exception ex)
            {
                throw;
                //return Content("error：" + ex.Message);
            }
        }
        private bool CheckSignature()
        {
            string signature = Request.QueryString["signature"];
            string timestamp = Request.QueryString["timestamp"];
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp))
                return false;
            string nonce = Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}