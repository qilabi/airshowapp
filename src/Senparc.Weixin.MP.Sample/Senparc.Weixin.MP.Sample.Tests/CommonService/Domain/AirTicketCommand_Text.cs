 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Sample.CommonService.Domain.AirTicket;
using Senparc.Weixin.MP.Sample.CommonService.Data.Models;
using Senparc.Weixin.MP.Sample.CommonService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Weixin.MP.Sample.CommonService.Store;

namespace Senparc.Weixin.MP.Sample.Tests.CommonService.Domain
{
    [TestClass]
    public class AirTicketCommand_Text
    {
        public AirTicketCommand serviceCommand = new AirTicketCommand();

         [TestMethod]
        public void CreateOrder_Test()
         {
             decimal ff = 0.1m;
             var fee = Convert.ToDouble(ff)*100;
             var str = fee.ToString();
             
            //reqHandler.setParameter("total_fee", (Convert.ToDouble(order_amount) * 100).ToString()); 
             return;
             ConfigSettings.Initialize();
           var result= serviceCommand.CreateOrder(new PostOrderDto()
            {
                OrderNo = Utils.GetOrderNumber(),
                Amount = 1100,
                Dtime = DateTime.Parse("2016-11-04"),
                OpenId = "oX1rFs-GrSV-R_GkGml4mP84fawI",
                Telephone = "13425081893",
                Title = "珠海航展电子成人票（11.4-11.6）",
                Lines = new List<PostOrderLineDto>()
                {
                    new PostOrderLineDto()
                    {
                        Name = "陈从睿",
                        IdentityCardNo = "360735199012151414",
                        UnitPrice = 550,
                    },
                    new PostOrderLineDto()
                    {
                        Name = "黃杰",
                        IdentityCardNo = "360735198911121401",
                        UnitPrice = 550
                    }
                }
            });
           Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void GetOrder()
        {
            ConfigSettings.Initialize();
            var orderEntity= serviceCommand.GetOrder("16103114080304");
            Assert.IsNotNull(orderEntity);
        }

    }
}
