using Senparc.Weixin.MP.Sample.CommonService.Data.Models;
using Senparc.Weixin.MP.Sample.CommonService.Infrastructure;
using Senparc.Weixin.MP.Sample.CommonService.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Senparc.Weixin.MP.Sample.CommonService.Dapper;
using Senparc.Weixin.MP.Sample.CommonService.Domain.Models;

namespace Senparc.Weixin.MP.Sample.CommonService.Domain.AirTicket
{
    public class AirTicketCommand : BaseQueryService
    {
        public EffectResult<string> CreateOrder(PostOrderDto dto)
        {
            var result = new EffectResult<string>() {Message = "操作失败!"};
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var sqlText = @"insert into orders (OrderNo,OpenId,Telephone,Dtime,TicketCount,Amount,Title) 
                                        values(@OrderNo,@OpenId,@Telephone,@Dtime,@TicketCount,@Amount,@Title);select @@identity;";

                        var res = conn.ExecuteScalar(sqlText, dto, trans);
                        var orderId =   Int32.Parse(res.ToString());
                        var success = true;
                        foreach (var line in dto.Lines)
                        {
                            var sql = @" insert into orderLine (OrderId,OrderNo,RealName,IdentityCardNo,UnitPrice)
                                values(@OrderId,@OrderNo,@RealName,@IdentityCardNo,@UnitPrice)
                                        ";
                            var eff = conn.Execute(sql,
                                new
                                {
                                    OrderId = orderId,
                                    OrderNo = dto.OrderNo,
                                    RealName = line.Name,
                                    IdentityCardNo = line.IdentityCardNo,
                                    UnitPrice = line.UnitPrice
                                }, trans);
                            if (eff <= 0)
                            {
                                success = false;
                            }

                        }
                        if (success && orderId > 0)
                        {
                            trans.Commit();
                            result.Value = dto.OrderNo;
                            result.Success = true;
                        }
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        result.Message = e.Message;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 付款成功
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public EffectResult<string> PayMoneySuccess(string orderNo)
        {
            var result = new EffectResult<string>() {Message = "操作失败!"};
            using (var conn = GetConnection())
            {
                var sql = "update orders set Status=1 where OrderNo=@OrderNo ";
                var eff = conn.Execute(sql, new {OrderNo = orderNo});
                if (eff > 0)
                {
                    result.Value = orderNo;
                    result.Success = true;
                }
            }
            return result;
        }



    }
}
