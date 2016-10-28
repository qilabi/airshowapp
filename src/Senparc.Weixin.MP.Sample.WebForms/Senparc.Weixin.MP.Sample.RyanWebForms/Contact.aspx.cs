using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Senparc.Weixin.MP.Sample.RyanWebForms
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var logPath = Server.MapPath(string.Format("~/App_Data/Open/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
            try
            {
                var fullPath = Path.Combine(logPath, string.Format("Response.txt"));
                var exists= File.Exists(fullPath);

                using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fs);
                    string line1 = sr.ReadLine();
                    Console.WriteLine(line1);       //输出 111111111111
                    this.txtContext.Text = line1;
                }

            }
            catch (Exception)
            { 
                throw;
            }
        }
    }
}