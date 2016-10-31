
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.Sample.CommonService.Store;
  

namespace Senparc.Weixin.MP.Sample.CommonService.Store
{
    public abstract class BaseQueryService
    {
        protected IDbConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }
    }
}
