using Senparc.Weixin.MP.Sample.CommonService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Senparc.Weixin.MP.Sample.CommonService.Data.Models;

namespace Senparc.Weixin.MP.Sample.CommonService.Store
{
    public class AirTicketStore:BaseQueryService
    {

        public AirOrder GetOrder(string orderNo)
        {
            var sqlQuery = "select * from orders  where OrderNo=@OrderNo ";
            var entity = default(AirOrder);
            using (var conn = GetConnection())
            {
                entity = conn.Query<AirOrder>(sqlQuery, new {OrderNo = orderNo}).FirstOrDefault();
                if (entity != null)
                {
                    var lines = conn.Query<PostOrderLineDto>("select * from orderLine where OrderNo=@OrderNo ",
                        new {OrderNo = orderNo});
                    foreach (var line in lines)
                    {
                        entity.Lines.Add(line);
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// 根据用户(OpenId)获取订单列表
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public IEnumerable<AirOrder> GetOrderWithOpenId(string openId)
        {
            var sqlQuery = "select * from orders  where OpenId=@OpenId ";

            using (var conn = GetConnection())
            {
                return conn.Query<AirOrder>(sqlQuery, new {OpenId = openId});
            }
        }

    }
}
