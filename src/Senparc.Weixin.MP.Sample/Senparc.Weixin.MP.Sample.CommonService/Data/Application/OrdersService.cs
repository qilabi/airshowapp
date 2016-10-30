using System;
using System.Linq;
using Senparc.Weixin.MP.AdvancedAPIs.MerChant;
using Senparc.Weixin.MP.Sample.CommonService.Data.Contracts;
using Senparc.Weixin.MP.Sample.CommonService.Data.Models;
using Senparc.Weixin.MP.Sample.CommonService.Data.Repository;
using ServiceStack.OrmLite;

namespace Senparc.Weixin.MP.Sample.CommonService.Data.Application
{
    public class OrdersService
    {
        public string CreateOrder(PostTicketOrder postTicketOrder)
        {
            if (postTicketOrder == null)
            {
                return "0";
            }
            Orders orders = new Orders
            {
                order_no = "AS" + Utils.GetOrderNumber(),
                remark = postTicketOrder.date,
                mobile = postTicketOrder.telephone,
                user_name = postTicketOrder.realNames,
                express_status = 1,
                status = 1,
                articleid = postTicketOrder.Quantity
            };
            IBaseRepository<Orders, int> baseRepository = new BaseRepository<Orders, int>();

            long aa = baseRepository.Insert(orders);
            if (aa > 0)
            {
                return orders.order_no;
            }
            return "0";
            
        }

        public Orders QueryByOrderNo(string orderNo)
        {
          Orders orders=  Utils.ExecuteSqlAction(db => db.Where<Orders>(new { orderNo = orderNo })).FirstOrDefault();
            return orders;
        }
    }
}