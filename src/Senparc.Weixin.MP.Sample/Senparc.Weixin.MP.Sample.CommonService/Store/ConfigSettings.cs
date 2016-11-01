using System.Configuration;

namespace Senparc.Weixin.MP.Sample.CommonService.Store
{
    public class ConfigSettings
    {
        public static string ConnectionString { get; set; }

        public static void Initialize()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["airshowConnection"].ConnectionString;
        }
    }
}
